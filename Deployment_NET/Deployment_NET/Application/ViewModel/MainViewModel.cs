using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using awinta.Deployment_NET.Application.Data;
using awinta.Deployment_NET.Application.Factory;
using awinta.Deployment_NET.Application.Interfaces;
using awinta.Deployment_NET.Application.Service;
using awinta.Deployment_NET.Common;
using awinta.Deployment_NET.Common.Extensions;
using awinta.Deployment_NET.Common.Working;
using awinta.Deployment_NET.Presentation.UICommands;
using EnvDTE;

namespace awinta.Deployment_NET.Application.ViewModel
{
    public class MainViewModel : WorkingBase
    {
        public MainViewModel()
        {
            LoadCommand = new AsyncCommand(LoadAsync);
            SaveCommand = new DefaultCommand(Save);
            DeploayCommand = new AsyncCommand(DeployAsync);
            BuildSolutionCommand = new AsyncCommand(BuildSolutionAsync);
            DirCommand = new ParamCommand<string>(SetDeployPath);

            service = ApplicationProvider.GetInstance<DTE>();
            tfsServer = ApplicationProvider.GetInstance<ITeamFoundationServerService>();
            deployDataFactory = ApplicationProvider.GetInstance<IDeployDataFactory>();

            solutionPath = new FileInfo(service.Solution.FullName);

            dbKey = $"{Environment.UserDomainName}||{Environment.UserName}||{solutionPath.Name}";

            deployDataFactory.Add(new DeployData(solutionPath.Name));
            deployDataFactory.SaveChanges();

            service.Events.SolutionEvents.Opened += _Opened;
            service.Events.SolutionEvents.BeforeClosing += _BeforeClosing;

            deployData = deployDataFactory.GetInstance(x => x.DeployDataId == dbKey);

            if (deployData == null)
            {
                deployData = new DeployData(solutionPath.Name);
                deployDataFactory.Add(deployData);
            }

        }

        #region Member

        private const string CodeAnalysisInputAssembly = "CodeAnalysisInputAssembly";

        private const string AssemblyName = "AssemblyName";
        private const string FullPath = "FullPath";
        private const string AssemblyVersion = "AssemblyVersion";
        private const string AssemblyFileVersion = "AssemblyFileVersion";
        private const string Title = "Title";
        private const string Description = "Description";
        private const string Company = "Company";
        private const string Product = "Product";
        private const string Copyright = "Copyright";
        private const string Trademark = "Trademark";
        private const string TargetRegDir = "DotNetReg";
        private const string TargetAppDir = @"AppPath\DotNet";
        private const string Release = "Release";
        private const string SmartDirectory = @"C:\Program Files\ASYS\SMART PharmaComp\dotnet";

        private static readonly string[] usedProperties =
        {
            AssemblyVersion,
            AssemblyFileVersion,
            Description,
            Product,
            Title,
            Copyright,
            Trademark,
            FullPath,
            Company,
            AssemblyName
        };

        private readonly DTE service;
        private readonly ITeamFoundationServerService tfsServer;
        private readonly FileInfo solutionPath;
        private readonly IDeployDataFactory deployDataFactory;
        private readonly string dbKey;

        #endregion

        #region Properties

        private DeployData deployData;

        public DeployData DeployData
        {
            get { return deployData; }
            set
            {
                deployData = value;
                OnNotifyPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand LoadCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand DeploayCommand { get; set; }
        public ICommand BuildSolutionCommand { get; set; }
        public ICommand DirCommand { get; set; }

        #endregion

        #region Methode

        #region Load

        public async Task LoadAsync()
        {
            try
            {
                WorkingStart();

                OutputService.WriteOutputWithContext("Start load Projectinformation.");

                if (solutionPath.FullName.Contains("Dev-Feature")) DeployData.ConfigData.IsFeature = true;

                var task = Task.Run(() => Load());

                await task;

                var result = task.Result;

                DeployData.ProjectDatas =
                    new ObservableCollection<ProjectData>(DeployData.ProjectDatas.Concat(result.Select(item => item).Where(item => !DeployData.ProjectDatas.Contains(item))));

                OutputService.WriteOutputWithContext("Finshed load Projectinformation.");
            }
            catch (Exception exception)
            {
                OutputService.WriteOutputWithContext($"Error: {exception.Message}");
                logger.Error(exception, exception.Message);
            }
            finally
            {
                WorkingStop();
            }
        }

        private ObservableCollection<ProjectData> Load()
        {
            OutputService.WriteOutputWithContext("Start: Load");

            var result = new ObservableCollection<ProjectData>();
            var projects = service.Solution.Projects;

            for (var i = 1; i <= projects.Count; i++)
            {
                var project = projects.Item(i);

                if (project?.Properties != null && !project.Name.ToLower().Contains(".test") && DeployData.ProjectDatas.FirstOrDefault(x => x.ProjectDataId == project.Name) == null)
                {
                    var queryProperties = from projectProperty in project.Properties.ToDictionary().AsEnumerable()
                                          where usedProperties.Contains(projectProperty.Key)
                                          select projectProperty;

                    var dictionaryProperties = queryProperties.ToDictionary(x => x.Key, x => x.Value);

                    var queryBuildConfig =
                        from projectBuildConfig in
                        project.ConfigurationManager.ActiveConfiguration.Properties.ToDictionary().AsEnumerable()
                        where projectBuildConfig.Key == CodeAnalysisInputAssembly
                        select projectBuildConfig;

                    var dictionaryBuildConfig = queryBuildConfig.ToDictionary(x => x.Key, x => x.Value);

                    if (dictionaryBuildConfig.Count == 0 || dictionaryProperties.Count == 0) continue;
                    var assemblyVersionTemp = dictionaryProperties[AssemblyVersion].Split('.');
                    var dateiVersionTemp = dictionaryProperties[AssemblyFileVersion].Split('.');

                    var currentProject = new ProjectData
                    {
                        ProjectDataId = projects.Item(i).Name,
                        AssemblyPath = dictionaryBuildConfig[CodeAnalysisInputAssembly],
                        AssemblyName = dictionaryProperties[AssemblyName],
                        FullPath = dictionaryProperties[FullPath],
                        AssemblyInfo = new AssemblyData
                        {
                            Titel = dictionaryProperties[Title],
                            Beschreibung = dictionaryProperties[Description],
                            Firma = dictionaryProperties[Company],
                            Produkt = dictionaryProperties[Product],
                            Copyright = dictionaryProperties[Copyright],
                            Marke = dictionaryProperties[Trademark],
                            AssemblyVersion =
                                new VersionData(assemblyVersionTemp[0], assemblyVersionTemp[1],
                                    assemblyVersionTemp[2],
                                    assemblyVersionTemp[3]),
                            Dateiversion =
                                new VersionData(dateiVersionTemp[0], dateiVersionTemp[1], dateiVersionTemp[2],
                                    dateiVersionTemp[3])
                        }
                    };

                    result.Add(currentProject);
                }
            }

            OutputService.WriteOutputWithContext("Finished: Load");

            return new ObservableCollection<ProjectData>(result.Distinct());
        }

        private async Task LoadLatestVersionAsync()
        {
            await Task.Run(() => LoadLatestVersion());
        }

        private void LoadLatestVersion()
        {
            if (!DeployData.ConfigData.DoCheckout) return;
            try
            {
                var solution = service.Solution;
                var solutionFile = solution.FullName;

                solution.Close(true);

                OutputService.WriteOutputWithContext($"<TFS>Get latest Version of: {solutionPath.DirectoryName}");
                tfsServer.UpdateProject(solutionPath.DirectoryName);

                if (tfsServer.Failures?.Length > 0)
                {
                    OutputService.WriteOutputWithContext($"<TFS>Sync failed: {solutionPath.DirectoryName}");

                    foreach (var fail in tfsServer.Failures)
                        OutputService.WriteOutputWithContext($"<TFS>{fail.GetFormattedMessage()}");
                }

                solution.Open(solutionFile);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                OutputService.WriteOutputWithContext($"Error: {ex.Message}");
            }
        }

        #endregion

        #region Save

        public void Save()
        {
            var projects = service.Solution.Projects;

            for (var i = 1; i <= projects.Count; i++)
            {
                var project = DeployData.ProjectDatas.FirstOrDefault(x => x.ProjectDataId == projects.Item(i).Name);

                if (project != null)
                    for (var i2 = 1; i2 <= projects.Item(i).Properties.Count; i++)
                        switch (projects.Item(i).Properties.Item(i2).Name)
                        {
                            //case fullPath:
                            //    projects.Item(i).Properties.Item(i2).Value = Project.FullPath;
                            //    break;
                            //case assemblyName:
                            //    projects.Item(i).Properties.Item(i2).Value = Project.AssemblyName;
                            //    break;

                            case Title:
                                projects.Item(i).Properties.Item(i2).Value = project.AssemblyInfo.Titel;
                                break;
                            case Description:
                                projects.Item(i).Properties.Item(i2).Value = project.AssemblyInfo.Beschreibung;
                                break;
                            case Company:
                                projects.Item(i).Properties.Item(i2).Value = project.AssemblyInfo.Firma;
                                break;
                            case Product:
                                projects.Item(i).Properties.Item(i2).Value = project.AssemblyInfo.Produkt;
                                break;
                            case Copyright:
                                projects.Item(i).Properties.Item(i2).Value = project.AssemblyInfo.Copyright;
                                break;
                            case Trademark:
                                projects.Item(i).Properties.Item(i2).Value = project.AssemblyInfo.Marke;
                                break;
                            case AssemblyVersion:
                                projects.Item(i).Properties.Item(i2).Value =
                                    project.AssemblyInfo.AssemblyVersion.ToString();
                                break;
                            case AssemblyFileVersion:
                                projects.Item(i).Properties.Item(i2).Value = DeployData.ConfigData.Version.ToString();
                                break;
                        }
            }
        }

        private void SaveConfigToDataBase()
        {
            OutputService.WriteOutputWithContext("Start: Saving ConfigData to Database.");

            deployDataFactory.SaveChanges();

            OutputService.WriteOutputWithContext("Finished: Saving ConfigData to Database.");
        }

        #endregion

        #region Deploy

        public async Task DeployAsync()
        {
            try
            {
                await Task.Run(() => SaveConfigToDataBase());

                if (DeployData.ConfigData.CopyToUpdate)
                    await DeployData.ProjectDatas.ForEachAsync(CopyAssembly);

                if (DeployData.ConfigData.CopyToBin)
                    await DeployData.ProjectDatas.ForEachAsync(CopyBin);

                if (DeployData.ConfigData.CopyToSmart)
                    await DeployData.ProjectDatas.ForEachAsync(CopySmart);
            }
            catch (Exception ex)
            {
                OutputService.WriteOutputWithContext($"Error: {ex.Message}");
            }
        }

        #endregion

        #region Build

        public async Task BuildSolutionAsync()
        {
            WorkingStart();

            await LoadLatestVersionAsync();
            await SetVersionNumberAsync();

            var solBuild = service.Solution.SolutionBuild;
            service.Events.BuildEvents.OnBuildDone += _BuildDone;

            //solBuild.ActiveConfiguration.Activate();

            //foreach (SolutionConfiguration variable in solBuild.SolutionConfigurations)
            //{
            //    OutputService.WriteOutputWithContext(variable.ProjectDataId);
            //}

            solBuild.SolutionConfigurations.Item(Release).Activate();

            solBuild.Build();
        }

        #endregion

        #region Copy

        private void CopyAssembly(ProjectData project)
        {
            try
            {
                if (!project.Include) return;
                OutputService.WriteOutputWithContext($"Start copy to Update: {project.ProjectDataId}");
                var dynamicPart = project.HasToRegister ? TargetRegDir : TargetAppDir;

                var assembly = new FileInfo(project.FullAssemblyPath);
                var targetPath = new DirectoryInfo(Path.Combine(DeployData.ConfigData.FullDeployPath, dynamicPart));
                var targetAssembly = new FileInfo(Path.Combine(targetPath.FullName, assembly.Name));

                if (assembly.Exists && targetPath.Exists)
                {
                    assembly.CopyTo(targetAssembly.FullName, true);

                    if (assembly.ToFileHash() == targetAssembly.ToFileHash()) return;

                    OutputService.WriteOutputWithContext($"Error: the File {assembly.Name} doesn't match the Source.");
                    OutputService.WriteOutputWithContext(
                        $"Hash: Source {assembly.ToFileHash()} <> Target {targetAssembly.ToFileHash()}");
                }
                else
                {
                    if (!assembly.Exists)
                        OutputService.WriteOutputWithContext($"Build is missing: {assembly.FullName}");
                    if (!targetPath.Exists)
                        OutputService.WriteOutputWithContext($"targetPath is missing: {targetPath.FullName}");
                }

                OutputService.WriteOutputWithContext($"Finished copy to Update: {project.ProjectDataId}");
            }
            catch (Exception ex)
            {
                OutputService.WriteOutputWithContext($"Error: {ex.Message}");
            }
        }

        private void CopyBin(ProjectData project)
        {
            if (!project.Include) return;
            OutputService.WriteOutputWithContext($"Start copy to Bin: {project.ProjectDataId}");
            var binDirectory = new DirectoryInfo(DeployData.ConfigData.DynamicBinPath);

            if (!binDirectory.Exists)
            {
                OutputService.WriteOutputWithContext(
                    $"Copy to Bin failed: Path does not Exists -> {binDirectory.FullName}");
            }
            else
            {
                var assembly = new FileInfo(project.FullAssemblyPath);
                var targetAssembly = new FileInfo(Path.Combine(binDirectory.FullName, assembly.Name));

                assembly.CopyTo(targetAssembly.FullName, true);

                if (assembly.ToFileHash() != targetAssembly.ToFileHash())
                {

                    OutputService.WriteOutputWithContext($"Error: the File {assembly.Name} doesn't match the Source.");
                    OutputService.WriteOutputWithContext(
                        $"Hash: Source {assembly.ToFileHash()} <> Target {targetAssembly.ToFileHash()}");

                }


            }

            OutputService.WriteOutputWithContext($"Finished copy to Bin: {project.ProjectDataId}");
        }

        private void CopySmart(ProjectData project)
        {
            if (!project.Include) return;
            OutputService.WriteOutputWithContext($"Start copy to Smart: {project.ProjectDataId}");
            var smartDirectory = new DirectoryInfo(SmartDirectory);

            if (!smartDirectory.Exists)
            {
                OutputService.WriteOutputWithContext($"Copy to Smart failed: Path does not Exists -> {smartDirectory.FullName}");
            }
            else
            {
                var assembly = new FileInfo(project.FullAssemblyPath);
                var targetAssembly = new FileInfo(Path.Combine(smartDirectory.FullName, assembly.Name));

                assembly.CopyTo(targetAssembly.FullName, true);

                if (assembly.ToFileHash() != targetAssembly.ToFileHash())
                {

                    OutputService.WriteOutputWithContext($"Error: the File {assembly.Name} doesn't match the Source.");
                    OutputService.WriteOutputWithContext(
                        $"Hash: Source {assembly.ToFileHash()} <> Target {targetAssembly.ToFileHash()}");

                }

            }

            OutputService.WriteOutputWithContext($"Finished copy to Smart: {project.ProjectDataId}");
        }

        #endregion

        #endregion

        #region _Events

        public void _BuildDone(vsBuildScope scope, vsBuildAction action)
        {
            try
            {
                if (scope != vsBuildScope.vsBuildScopeSolution) return;
                switch (action)
                {
                    case vsBuildAction.vsBuildActionBuild:

                        OutputService.WriteOutputWithContext("StartDeploy");
                        Task.Run(DeployAsync);

                        break;
                    case vsBuildAction.vsBuildActionRebuildAll:
                        break;
                    case vsBuildAction.vsBuildActionClean:
                        break;
                    case vsBuildAction.vsBuildActionDeploy:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(action), action, null);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                OutputService.WriteOutputWithContext($"Error: {ex.Message}");
            }
            finally
            {
                WorkingStop();
                service.Events.BuildEvents.OnBuildDone -= _BuildDone;
            }
        }

        public void _Opened()
        {
            LoadCommand.Execute(null);
        }

        public void _BeforeClosing()
        {
            WorkingLocked();
            DeployData.ConfigData = null;
            DeployData.ProjectDatas = null;

        }

        private void SetDeployPath(string caller)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = "Wählen sie den Root-Ordner der Updates aus",
                //RootFolder = Environment.SpecialFolder.NetworkShortcuts,
                SelectedPath = caller == "DeployPath" ? DeployData.ConfigData.DeployPath : DeployData.ConfigData.BinPath
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;
            if (caller == "DeployPath")
                DeployData.ConfigData.DeployPath = dialog.SelectedPath;
            else
                DeployData.ConfigData.BinPath = dialog.SelectedPath;
        }

        private async Task SetVersionNumberAsync()
        {
            await Task.Run(() => SetVersionNumber());
        }

        private void SetVersionNumber()
        {
            foreach (var project in DeployData.ProjectDatas)
                project.AssemblyInfo.Dateiversion = DeployData.ConfigData.Version;

            var projects = service.Solution.Projects;

            for (var i = 1; i <= projects.Count; i++)
            {
                var project = projects.Item(i);

                if (project?.Properties != null)
                    foreach (var item in project.Properties)
                    {
                        var property = item as Property;

                        if (property?.Name.Equals(AssemblyFileVersion) == true)
                        {
                            property.Value = DeployData.ConfigData.Version.ToString();
                            break;
                        }
                    }
            }
        }

        #endregion
    }
}