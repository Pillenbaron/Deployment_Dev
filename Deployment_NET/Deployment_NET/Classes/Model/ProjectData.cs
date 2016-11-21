namespace awinta.Deployment_NET.Solution.Model
{
    class ProjectData : Base.Model.BaseData
    {

        private string name;

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

        private AssemblyData assemblyInfo = null;

        public AssemblyData AssemblyInfo
        {
            get { return assemblyInfo; }
            set
            {
                assemblyInfo = value;
                OnNotifyPropertyChanged();
            }
        }

        public ProjectData()
        {

            assemblyInfo = new AssemblyData();

        }

    }
}
