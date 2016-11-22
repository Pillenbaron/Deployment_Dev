using System.Collections.ObjectModel;
using EnvDTE;
using System.Linq;
using awinta.Deployment_NET.Extensions;
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

        private DTE service = null;

        private Solution.Model.VersionData version;

        public Solution.Model.VersionData Version
        {
            get { return version; }
            set
            {
                version = value;
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
        public ICommand DeployCommand { get; set; }
        #endregion

        public MainViewModel()
        {

            LoadCommand = new Commands.DefaultCommand(Load);
            SaveCommand = new Commands.DefaultCommand(Save);
            DeployCommand = new Commands.DefaultCommand(Deploy);

            service = Service.ServiceLocator.GetInstance<DTE>();
            data = new ObservableCollection<Solution.Model.ProjectData>();

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
                                projects.Item(i).Properties.Item(i2).Value = Project.AssemblyInfo.Dateiversion.ToString();
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

            for (int i = 0; i < Data.Count - 1; i++)
            {



            }

        }

        public void BuildSolution()
        {

            SolutionBuild solBuild = service.Solution.SolutionBuild;
            service.Events.BuildEvents.OnBuildDone += _BuildDone;
            solBuild.ActiveConfiguration.Activate();
            solBuild.Build();
            solBuild.ActiveConfiguration.Collection
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

        #endregion

    }
}