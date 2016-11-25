using awinta.Deployment_NET.Extensions;
using awinta.Deployment_NET.Solution.Model;
using EnvDTE;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace awinta.Deployment_NET.ViewModel
{
    internal class MainViewModel : Base.Model.BaseData
    {

        #region Member

        private const string codeAnalysisInputAssembly = "CodeAnalysisInputAssembly";

        private const string assemblyName = "AssemblyName";
        private const string fullPath = "FullPath";
        private const string assemblyVersion = "AssemblyVersion";
        private const string assemblyFileVersion = "AssemblyFileVersion";
        private const string title = "Title";
        private const string description = "Description";
        private const string company = "Company";
        private const string product = "Product";
        private const string copyright = "Copyright";
        private const string trademark = "Trademark";

        private static readonly string[] usedProperties = { assemblyVersion,
                                                            assemblyFileVersion,
                                                            description,
                                                            product,
                                                            title,
                                                            copyright,
                                                            trademark,
                                                            fullPath,
                                                            company,
                                                            assemblyName };

        private DTE service;
        public DTE Service
        {
            get { return service; }
            set
            {
                service = value;
            }
        }
        #endregion

        #region Properties

        private ConfigData configuration;
        public ConfigData Configuration
        {
            get { return configuration; }
            set
            {
                configuration = value;
                OnNotifyPropertyChanged();
            }
        }

        private ObservableCollection<Solution.Model.ProjectData> data;
        public ObservableCollection<Solution.Model.ProjectData> Data
        {
            get { return data; }
            set
            {
                data = value;
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

        public MainViewModel()
        {

            LoadCommand = new UICommands.DefaultCommand(Load);
            SaveCommand = new UICommands.DefaultCommand(Save);
            DeploayCommand = new UICommands.DefaultCommand(Deploy);
            BuildSolutionCommand = new UICommands.DefaultCommand(BuildSolution);
            DirCommand = new UICommands.DefaultCommand(setDeployPath);

            //service = Service.ServiceLocator.GetInstance<DTE>();
            data = new ObservableCollection<ProjectData>();

        }

        #region Methode

        public void Load()
        {

            Projects projects = service.Solution.Projects;

            for (int i = 1; i <= projects.Count; i++)
            {

                var QueryProperties = from ProjectProperty in projects.Item(i).Properties.ToDictionary().AsEnumerable()
                                      where usedProperties.Contains(ProjectProperty.Key)
                                      select ProjectProperty;

                var DictionaryProperties = QueryProperties.ToDictionary(x => x.Key, x => x.Value);

                string[] assemblyVersionTemp = DictionaryProperties[assemblyVersion].Split('.');
                string[] dateiVersionTemp = DictionaryProperties[assemblyFileVersion].Split('.');

                var QueryBuildConfig = from ProjectBuildConfig in projects.Item(i).ConfigurationManager.ActiveConfiguration.Properties.ToDictionary().AsEnumerable()
                                       where ProjectBuildConfig.Key == codeAnalysisInputAssembly
                                       select ProjectBuildConfig;

                var DictionaryBuildConfig = QueryProperties.ToDictionary(x => x.Key, x => x.Value);

                var CurrentProject = new Solution.Model.ProjectData()
                {
                    Name = projects.Item(i).Name,
                    AssemblyPath = DictionaryBuildConfig[codeAnalysisInputAssembly],
                    AssemblyName = DictionaryProperties[assemblyName],
                    FullPath = DictionaryProperties[fullPath],
                    AssemblyInfo = new Solution.Model.AssemblyData()
                    {
                        Titel = DictionaryProperties[title],
                        Beschreibung = DictionaryProperties[description],
                        Firma = DictionaryProperties[company],
                        Produkt = DictionaryProperties[product],
                        Copyright = DictionaryProperties[copyright],
                        Marke = DictionaryProperties[trademark],
                        AssemblyVersion = new Solution.Model.VersionData(assemblyVersionTemp[0], assemblyVersionTemp[1], assemblyVersionTemp[2], assemblyVersionTemp[3]),
                        Dateiversion = new Solution.Model.VersionData(dateiVersionTemp[0], dateiVersionTemp[1], dateiVersionTemp[2], dateiVersionTemp[3])
                    }
                };

                Data.Add(CurrentProject);

            }

        }

        public void Save()
        {

            Projects projects = service.Solution.Projects;

            for (int i = 1; i <= projects.Count; i++)
            {

                var Project = this.Data.FirstOrDefault(x => x.Name == projects.Item(i).Name);

                if (Project != null)
                {

                    for (int i2 = 1; i2 <= projects.Item(i).Properties.Count; i++)
                    {

                        switch (projects.Item(i).Properties.Item(i2).Name)
                        {

                            //case fullPath:
                            //    projects.Item(i).Properties.Item(i2).Value = Project.FullPath;
                            //    break;
                            //case assemblyName:
                            //    projects.Item(i).Properties.Item(i2).Value = Project.AssemblyName;
                            //    break;

                            case title:
                                projects.Item(i).Properties.Item(i2).Value = Project.AssemblyInfo.Titel;
                                break;
                            case description:
                                projects.Item(i).Properties.Item(i2).Value = Project.AssemblyInfo.Beschreibung;
                                break;
                            case company:
                                projects.Item(i).Properties.Item(i2).Value = Project.AssemblyInfo.Firma;
                                break;
                            case product:
                                projects.Item(i).Properties.Item(i2).Value = Project.AssemblyInfo.Produkt;
                                break;
                            case copyright:
                                projects.Item(i).Properties.Item(i2).Value = Project.AssemblyInfo.Copyright;
                                break;
                            case trademark:
                                projects.Item(i).Properties.Item(i2).Value = Project.AssemblyInfo.Marke;
                                break;
                            case assemblyVersion:
                                projects.Item(i).Properties.Item(i2).Value = Project.AssemblyInfo.AssemblyVersion.ToString();
                                break;
                            case assemblyFileVersion:
                                projects.Item(i).Properties.Item(i2).Value = configuration.Version.ToString();
                                break;
                            default:
                                break;
                        }

                    }

                }

            }

        }

        public void Deploy()
        {

            data.ForEach(x => copyAssembly(x));

        }

        public void BuildSolution()
        {

            SolutionBuild solBuild = service.Solution.SolutionBuild;
            service.Events.BuildEvents.OnBuildDone += _BuildDone;
            solBuild.ActiveConfiguration.Activate();
            solBuild.Build();

        }

        private void copyAssembly(ProjectData project)
        {

            System.Diagnostics.Debug.Print($"Deploy: {project.Name}");

            FileInfo assembly = new FileInfo(project.FullAssemblyPath);
            DirectoryInfo targetPath = new DirectoryInfo(configuration.FullDeployPath);
            FileInfo targetAssembly = new FileInfo(Path.Combine(configuration.FullDeployPath, assembly.Name));

            if (assembly.Exists && targetPath.Exists)
            {

                assembly.CopyTo(targetPath.FullName, true);

                if (assembly.ToFileHash() != targetAssembly.ToFileHash())
                {

                    System.Diagnostics.Debug.Print($"Fehler: die Datei {assembly.Name} stimmt nicht mit ihrer Quelle über ein.");
                    System.Diagnostics.Debug.Print($"Hash: Quelle {assembly.ToFileHash()} <> Ziel {targetAssembly.ToFileHash()}");

                }

            }
            else
            {

                if (!assembly.Exists) System.Diagnostics.Debug.Print($"Build nicht vorhanden: {assembly.FullName}");
                if (!targetPath.Exists) System.Diagnostics.Debug.Print($"Zielpfad nicht vorhanden: {targetPath.FullName}");

            }

        }

        #endregion

        #region _Events

        public void _BuildDone(vsBuildScope Scope, vsBuildAction Action)
        {

            try
            {

                if (Scope == vsBuildScope.vsBuildScopeSolution)
                {

                    switch (Action)
                    {
                        case vsBuildAction.vsBuildActionBuild:

                            System.Diagnostics.Debug.Print("StartDeploy");
                            Deploy();

                            break;
                        case vsBuildAction.vsBuildActionRebuildAll:
                            break;
                        case vsBuildAction.vsBuildActionClean:
                            break;
                        case vsBuildAction.vsBuildActionDeploy:
                            break;
                        default:
                            break;
                    }
                }

            }
            catch (System.Exception)
            {

                throw;

            }
            finally
            {

                service.Events.BuildEvents.OnBuildDone -= _BuildDone;

            }

        }

        private void setDeployPath()
        {

            var dialog = new System.Windows.Forms.FolderBrowserDialog()
            {

                Description = "Wählen sie den Root-Ordner der Updates aus",
                RootFolder = System.Environment.SpecialFolder.NetworkShortcuts,
                SelectedPath = configuration.DeployPath
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                configuration.DeployPath = dialog.SelectedPath;

            }

        }

        #endregion

    }
}