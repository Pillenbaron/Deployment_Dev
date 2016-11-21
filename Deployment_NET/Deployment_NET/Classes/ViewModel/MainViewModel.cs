using System.Collections.ObjectModel;
using EnvDTE;
using System.Linq;
using awinta.Deployment_NET.Extensions;

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
        
        public MainViewModel(DTE Service)
        {

            service = Service;
            data = new ObservableCollection<Solution.Model.ProjectData>();

        }

        public bool Load()
        {

            Projects projects = service.Solution.Projects;

            for (int i = 1; i <= projects.Count; i++)
            {

                var Query = from ProjectProperty in projects.Item(i).Properties.ToDictionary().AsEnumerable()
                            where usedProperties.Contains(ProjectProperty.Key)
                            select ProjectProperty;
                
                var Dictionary = Query.ToDictionary(x => x.Key, x => x.Value);

                string[] AssemblyVersion = Dictionary[MainViewModel.assemblyVersion].Split('.');
                string[] DateiVersion = Dictionary[assemblyFileVersion].Split('.');

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
                        Copyright = copyright,
                        Marke = trademark,
                        AssemblyVersion = new Solution.Model.VersionData(AssemblyVersion[0], AssemblyVersion[1], AssemblyVersion[2], AssemblyVersion[3]),
                        Dateiversion = new Solution.Model.VersionData(AssemblyVersion[0], AssemblyVersion[1], AssemblyVersion[2], AssemblyVersion[3])
                    }
                };

                Data.Add(CurrentProject);

            }

            return Data.Count > 0;

        }

        public void Save()
        {

            Projects projects = service.Solution.Projects;

            for (int i = 1; i <= projects.Count; i++)
            {



            }

        }

        public void Deploy()
        {



        }

    }
}
