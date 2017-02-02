namespace awinta.Deployment_NET.Application.Data
{
    public class AssemblyData : BaseData
    {
        private VersionData assemblyVersion;

        private string beschreibung = string.Empty;

        private string copyright = string.Empty;

        private VersionData dateiversion;

        private string firma = string.Empty;

        private string marke = string.Empty;

        private string produkt = string.Empty;

        private string titel = string.Empty;

        public AssemblyData()
        {
            assemblyVersion = new VersionData();
            dateiversion = new VersionData();
        }

        public string Titel
        {
            get { return titel; }
            set
            {
                titel = value;
                OnNotifyPropertyChanged();
            }
        }

        public string Beschreibung
        {
            get { return beschreibung; }
            set
            {
                beschreibung = value;
                OnNotifyPropertyChanged();
            }
        }

        public string Firma
        {
            get { return firma; }
            set
            {
                firma = value;
                OnNotifyPropertyChanged();
            }
        }

        public string Produkt
        {
            get { return produkt; }
            set
            {
                produkt = value;
                OnNotifyPropertyChanged();
            }
        }

        public string Copyright
        {
            get { return copyright; }
            set
            {
                copyright = value;
                OnNotifyPropertyChanged();
            }
        }

        public string Marke
        {
            get { return marke; }
            set
            {
                marke = value;
                OnNotifyPropertyChanged();
            }
        }

        public VersionData AssemblyVersion
        {
            get { return assemblyVersion; }
            set
            {
                assemblyVersion = value;
                OnNotifyPropertyChanged();
            }
        }

        public VersionData Dateiversion
        {
            get { return dateiversion; }
            set
            {
                dateiversion = value;
                OnNotifyPropertyChanged();
            }
        }
    }
}