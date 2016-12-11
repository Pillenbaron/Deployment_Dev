namespace awinta.Deployment_NET.Solution.Model
{
    internal class ProjectData : BaseData
    {

        private string name = string.Empty;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnNotifyPropertyChanged();
            }
        }

        private string assemblyName = string.Empty;

        public string AssemblyName
        {
            get { return assemblyName; }
            set
            {
                assemblyName = value;
                OnNotifyPropertyChanged();
            }
        }

        private string fullPath = string.Empty;

        public string FullPath
        {
            get { return fullPath; }
            set
            {
                fullPath = value;
                OnNotifyPropertyChanged();
            }
        }

        private AssemblyData assemblyInfo;

        public AssemblyData AssemblyInfo
        {
            get { return assemblyInfo; }
            set
            {
                assemblyInfo = value;
                OnNotifyPropertyChanged();
            }
        }

        private string assemblyPath = string.Empty;

        public string AssemblyPath
        {
            get { return assemblyPath; }
            set
            {
                assemblyPath = value;
                OnNotifyPropertyChanged();
            }
        }

        public string FullAssemblyPath
        {
            get
            {
                return FullPath != null ? $"{FullPath}\\{AssemblyPath}" : null;
            }
        }

        private bool hasToRegister;

        public bool HasToRegister
        {
            get { return hasToRegister; }
            set
            {
                hasToRegister = value;
                OnNotifyPropertyChanged();
            }
        }

        public ProjectData()
        {

            assemblyInfo = new AssemblyData();

        }

    }
}