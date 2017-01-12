﻿using awinta.Deployment_NET.Extensions;
using awinta.Deployment_NET.Solution.Model;
using awinta.Deployment_NET.UICommands;
using EnvDTE;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace awinta.Deployment_NET.ViewModel
{
    internal class MainViewModel : BaseData
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
        private const string targetRegDir = "DotNetReg";
        private const string targetAppDir = @"AppPath\DotNet";
        private static readonly string[] UsedProperties = { assemblyVersion,
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
        private Interfaces.ITeamFoundationServerService tfsServer;

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

        private ObservableCollection<ProjectData> data;
        public ObservableCollection<ProjectData> Data
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

            LoadCommand = new DefaultCommand(Load);
            SaveCommand = new DefaultCommand(Save);
            DeploayCommand = new DefaultCommand(Deploy);
            BuildSolutionCommand = new DefaultCommand(BuildSolution);
            DirCommand = new DefaultCommand(SetDeployPath);

            Configuration = new ConfigData();
            data = new ObservableCollection<ProjectData>();

            service = IoC.ApplicationContainer.GetInstance<DTE>();
            tfsServer = IoC.ApplicationContainer.GetInstance<Interfaces.ITeamFoundationServerService>();

        }

        #region Methode

        public void Load()
        {

            Service.OutputService.WriteOutput("Load Projectinformation");

            Projects projects = service.Solution.Projects;

            for (int i = 1; i <= projects.Count; i++)
            {

                var project = projects.Item(i);

                if (project != null && project.Properties != null)
                {

                    Service.OutputService.WriteOutput($"Load Project: {project.FullName}");

                    Service.OutputService.WriteOutput($"Start Collecting ProjectProperties: {project.FullName}");
                    var queryProperties = from projectProperty in project.Properties.ToDictionary().AsEnumerable()
                                          where UsedProperties.Contains(projectProperty.Key)
                                          select projectProperty;

                    var dictionaryProperties = queryProperties.ToDictionary(x => x.Key, x => x.Value);
                    Service.OutputService.WriteOutput($"Finished Collecting ProjectProperties: {project.FullName}");

                    string[] assemblyVersionTemp = dictionaryProperties[assemblyVersion].Split('.');
                    string[] dateiVersionTemp = dictionaryProperties[assemblyFileVersion].Split('.');

                    Service.OutputService.WriteOutput($"Start Collecting ProjectBuildconfiguration: {project.FullName}");
                    var queryBuildConfig = from projectBuildConfig in project.ConfigurationManager.ActiveConfiguration.Properties.ToDictionary().AsEnumerable()
                                           where projectBuildConfig.Key == codeAnalysisInputAssembly
                                           select projectBuildConfig;
                    Service.OutputService.WriteOutput($"Finished Collecting ProjectBuildconfiguration: {project.FullName}");

                    var dictionaryBuildConfig = queryBuildConfig.ToDictionary(x => x.Key, x => x.Value);

                    Service.OutputService.WriteOutput($"Start Build ProjectContainer: {project.FullName}");

                    var currentProject = new ProjectData
                    {
                        Name = projects.Item(i).Name,
                        AssemblyPath = dictionaryBuildConfig[codeAnalysisInputAssembly],
                        AssemblyName = dictionaryProperties[assemblyName],
                        FullPath = dictionaryProperties[fullPath],
                        AssemblyInfo = new AssemblyData
                        {
                            Titel = dictionaryProperties[title],
                            Beschreibung = dictionaryProperties[description],
                            Firma = dictionaryProperties[company],
                            Produkt = dictionaryProperties[product],
                            Copyright = dictionaryProperties[copyright],
                            Marke = dictionaryProperties[trademark],
                            AssemblyVersion = new VersionData(assemblyVersionTemp[0], assemblyVersionTemp[1], assemblyVersionTemp[2], assemblyVersionTemp[3]),
                            Dateiversion = new VersionData(dateiVersionTemp[0], dateiVersionTemp[1], dateiVersionTemp[2], dateiVersionTemp[3])
                        }
                    };
                    Service.OutputService.WriteOutput($"Finished Build ProjectContainer: {project.FullName}");

                    Data.Add(currentProject);

                }

            }

            data = new ObservableCollection<ProjectData>(data.Distinct());

        }

        private void LoadLatestVersion()
        {

            try
            {
                foreach (ProjectData currentProject in data)
                {

                    Service.OutputService.WriteOutput($"<TFS>Get latest Version of: {currentProject.Name}");
                    tfsServer.UpdateProject(currentProject.FullPath);

                    if (tfsServer.Failures != null && tfsServer.Failures.Length > 0)
                    {
                        Service.OutputService.WriteOutput($"<TFS>Sync failed: {currentProject.Name}");

                        foreach (Failure fail in tfsServer.Failures)
                        {

                            Service.OutputService.WriteOutput($"<TFS>{fail.GetFormattedMessage()}");

                        }

                    }

                }
            }
            catch (Exception ex)
            {

                Service.OutputService.WriteOutput($"Fehler: {ex.Message}");
                throw;

            }


        }

        public void Save()
        {

            Projects projects = service.Solution.Projects;

            for (int i = 1; i <= projects.Count; i++)
            {

                var project = Data.FirstOrDefault(x => x.Name == projects.Item(i).Name);

                if (project != null)
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
                                projects.Item(i).Properties.Item(i2).Value = project.AssemblyInfo.Titel;
                                break;
                            case description:
                                projects.Item(i).Properties.Item(i2).Value = project.AssemblyInfo.Beschreibung;
                                break;
                            case company:
                                projects.Item(i).Properties.Item(i2).Value = project.AssemblyInfo.Firma;
                                break;
                            case product:
                                projects.Item(i).Properties.Item(i2).Value = project.AssemblyInfo.Produkt;
                                break;
                            case copyright:
                                projects.Item(i).Properties.Item(i2).Value = project.AssemblyInfo.Copyright;
                                break;
                            case trademark:
                                projects.Item(i).Properties.Item(i2).Value = project.AssemblyInfo.Marke;
                                break;
                            case assemblyVersion:
                                projects.Item(i).Properties.Item(i2).Value = project.AssemblyInfo.AssemblyVersion.ToString();
                                break;
                            case assemblyFileVersion:
                                projects.Item(i).Properties.Item(i2).Value = configuration.Version.ToString();
                                break;
                        }

                    }

                }

            }

        }

        public void Deploy()
        {

            data.ForEach(CopyAssembly);

        }

        public void BuildSolution()
        {
            LoadLatestVersion();
            setVersionNumber();

            SolutionBuild solBuild = service.Solution.SolutionBuild;
            service.Events.BuildEvents.OnBuildDone += _BuildDone;
            solBuild.ActiveConfiguration.Activate();
            solBuild.Build();

        }

        private void CopyAssembly(ProjectData project)
        {

            try
            {

                Service.OutputService.WriteOutput($"Deploy: {project.Name}");

                var dynamicPart = string.Empty;

                if (project.HasToRegister)
                {
                    dynamicPart = targetRegDir;
                }
                else
                {
                    dynamicPart = targetAppDir;
                }

                FileInfo assembly = new FileInfo(project.FullAssemblyPath);
                DirectoryInfo targetPath = new DirectoryInfo(Path.Combine(configuration.FullDeployPath, dynamicPart));
                FileInfo targetAssembly = new FileInfo(Path.Combine(targetPath.FullName, assembly.Name));

                if (assembly.Exists && targetPath.Exists)
                {

                    assembly.CopyTo(targetAssembly.FullName, true);

                    if (assembly.ToFileHash() == targetAssembly.ToFileHash()) return;

                    Service.OutputService.WriteOutput($"Fehler: die Datei {assembly.Name} stimmt nicht mit ihrer Quelle über ein.");
                    Service.OutputService.WriteOutput($"Hash: Quelle {assembly.ToFileHash()} <> Ziel {targetAssembly.ToFileHash()}");

                }
                else
                {

                    if (!assembly.Exists) Service.OutputService.WriteOutput($"Build nicht vorhanden: {assembly.FullName}");
                    if (!targetPath.Exists) Service.OutputService.WriteOutput($"Zielpfad nicht vorhanden: {targetPath.FullName}");

                }

            }
            catch (Exception ex)
            {
                Service.OutputService.WriteOutput($"Fehler: {ex.Message}");
                throw;
            }

        }

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

                        Service.OutputService.WriteOutput("StartDeploy");
                        Deploy();

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
            finally
            {

                service.Events.BuildEvents.OnBuildDone -= _BuildDone;

            }

        }

        private void SetDeployPath()
        {

            var dialog = new FolderBrowserDialog
            {

                Description = "Wählen sie den Root-Ordner der Updates aus",
                //RootFolder = Environment.SpecialFolder.NetworkShortcuts,
                SelectedPath = configuration.DeployPath
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {

                configuration.DeployPath = dialog.SelectedPath;

            }

        }

        private void setVersionNumber()
        {

            for (int i = 0; i < Data.Count; i++)
            {
                Data[i].AssemblyInfo.Dateiversion = Configuration.Version;
            }

            Projects projects = service.Solution.Projects;

            for (int i = 1; i <= projects.Count; i++)
            {

                var project = projects.Item(i);

                if (project != null && project.Properties != null)
                {

                    foreach (var item in project.Properties)
                    {

                        var property = item as Property;

                        if (property != null && property.Name.Equals(assemblyFileVersion))
                        {
                            //property.let_Value(Configuration.Version);
                            property.Value = Configuration.Version.ToString();
                            break;
                        }

                    }

                }

            }

        }

        #endregion

    }

}