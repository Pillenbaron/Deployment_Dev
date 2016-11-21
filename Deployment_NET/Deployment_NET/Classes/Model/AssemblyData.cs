namespace awinta.Deployment_NET.Solution.Model
{
    internal class AssemblyData : Base.Model.BaseData
    {

        private string titel = string.Empty;

        public string Titel
        {
            get { return titel; }
            set
            {
                titel = value;
                OnNotifyPropertyChanged();
            }
        }

        private string beschreibung = string.Empty;

        public string Beschreibung
        {
            get { return beschreibung; }
            set
            {
                beschreibung = value;
                OnNotifyPropertyChanged();
            }
        }

        private string firma = string.Empty;

        public string Firma
        {
            get { return firma; }
            set
            {
                firma = value;
                OnNotifyPropertyChanged();
            }
        }

        private string produkt = string.Empty;

        public string Produkt
        {
            get { return produkt; }
            set
            {
                produkt = value;
                OnNotifyPropertyChanged();
            }
        }

        private string copyright = string.Empty;

        public string Copyright
        {
            get { return copyright; }
            set
            {
                copyright = value;
                OnNotifyPropertyChanged();
            }
        }

        private string marke = string.Empty;

        public string Marke
        {
            get { return marke; }
            set
            {
                marke = value;
                OnNotifyPropertyChanged();
            }
        }

        private VersionData assemblyVersion = null;

        public VersionData AssemblyVersion
        {
            get { return assemblyVersion; }
            set
            {
                assemblyVersion = value;
                OnNotifyPropertyChanged();
            }
        }

        private VersionData dateiversion = null;

        public VersionData Dateiversion
        {
            get { return dateiversion; }
            set
            {
                dateiversion = value;
                OnNotifyPropertyChanged();
            }
        }

        public AssemblyData()
        {

            assemblyVersion = new VersionData();
            dateiversion = new VersionData();

        }

    }
}