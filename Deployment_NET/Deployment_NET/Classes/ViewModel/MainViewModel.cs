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

        private awinta.Deployment_NET.Solution.Model.VersionData version;

        public awinta.Deployment_NET.Solution.Model.VersionData Version
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

                var Query = from ProjectProperty in projects.Item(i).Properties.ToDictionary().AsEnumerable()
                            where usedProperties.Contains(ProjectProperty.Key)
                            select ProjectProperty;

                var Dictionary = Query.ToDictionary(x => x.Key, x => x.Value);

                string[] assemblyVersionTemp = Dictionary[assemblyVersion].Split('.');
                string[] dateiVersionTemp = Dictionary[assemblyFileVersion].Split('.');

                var CurrentProject = new Solution.Model.ProjectData()
                {
                    Name = projects.Item(i).Name,
                    AssemblyName = Dictionary[assemblyName],
                    FullPath = Dictionary[fullPath],
                    AssemblyInfo = new Solution.Model.AssemblyData()
                    {
                        Titel = Dictionary[title],
                        Beschreibung = Dictionary[description],
                        Firma = Dictionary[company],
                        Produkt = Dictionary[product],
                        Copyright = Dictionary[copyright],
                        Marke = Dictionary[trademark],
                        AssemblyVersion = new Solution.Model.VersionData(assemblyVersionTemp[0], assemblyVersionTemp[1], assemblyVersionTemp[2], assemblyVersionTemp[3]),
                        Dateiversion = new Solution.Model.VersionData(dateiVersionTemp[0], dateiVersionTemp[1], dateiVersionTemp[2], dateiVersionTemp[3])
                    }
                };

                Data.Add(CurrentProject);

            }

            //return Data.Count > 0;

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



        }

        #endregion

    }
}
