using System;

namespace awinta.Deployment_NET.Solution.Model
{
    internal class VersionData : BaseData
    {
        private int buildnummer;

        private int hauptversion;

        private int nebenversion;

        private int revision;

        public VersionData(int vHauptversion, int vNebenversion, int vBuildnummer, int vRevision)
        {
            Hauptversion = vHauptversion;
            Nebenversion = vNebenversion;
            Buildnummer = vBuildnummer;
            Revision = vRevision;
        }

        public VersionData(string vHauptversion, string vNebenversion, string vBuildnummer, string vRevision)
        {
            Hauptversion = Convert.ToInt16(vHauptversion);
            Nebenversion = Convert.ToInt16(vNebenversion);
            Buildnummer = Convert.ToInt16(vBuildnummer);
            Revision = Convert.ToInt16(vRevision);
        }

        public VersionData()
        {
        }

        public int Hauptversion
        {
            get { return hauptversion; }
            set
            {
                hauptversion = value;
                OnNotifyPropertyChanged();
            }
        }

        public int Nebenversion
        {
            get { return nebenversion; }
            set
            {
                nebenversion = value;
                OnNotifyPropertyChanged();
            }
        }

        public int Buildnummer
        {
            get { return buildnummer; }
            set
            {
                buildnummer = value;
                OnNotifyPropertyChanged();
            }
        }

        public int Revision
        {
            get { return revision; }
            set
            {
                revision = value;
                OnNotifyPropertyChanged();
            }
        }

        public override string ToString()
        {
            return $"{Hauptversion}.{Nebenversion}.{Buildnummer}.{Revision}";
        }
    }
}