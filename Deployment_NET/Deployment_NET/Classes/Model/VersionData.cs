namespace awinta.Deployment_NET.Solution.Model
{
    internal class VersionData : Base.Model.BaseData
    {

        private int hauptversion = 0;

        public int Hauptversion
        {
            get { return hauptversion; }
            set
            {
                hauptversion = value;
                OnNotifyPropertyChanged();
            }
        }

        private int nebenversion = 0;

        public int Nebenversion
        {
            get { return nebenversion; }
            set
            {
                nebenversion = value;
                OnNotifyPropertyChanged();
            }
        }

        private int buildnummer = 0;

        public int Buildnummer
        {
            get { return buildnummer; }
            set
            {
                buildnummer = value;
                OnNotifyPropertyChanged();
            }
        }

        private int revision = 0;

        public int Revision
        {
            get { return revision; }
            set
            {
                revision = value;
                OnNotifyPropertyChanged();
            }
        }

        public VersionData(int vHauptversion, int vNebenversion, int vBuildnummer, int vRevision)
        {

            Hauptversion = vHauptversion;
            Nebenversion = vNebenversion;
            Buildnummer = vBuildnummer;
            Revision = vRevision;

        }

        public VersionData(string vHauptversion, string vNebenversion, string vBuildnummer, string vRevision)
        {

            Hauptversion = System.Convert.ToInt16(vHauptversion);
            Nebenversion = System.Convert.ToInt16(vNebenversion);
            Buildnummer = System.Convert.ToInt16(vBuildnummer);
            Revision = System.Convert.ToInt16(vRevision);

        }

        public VersionData() { }

    }

}
