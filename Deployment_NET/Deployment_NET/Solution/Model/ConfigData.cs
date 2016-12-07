using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace awinta.Deployment_NET.Solution.Model
{
    internal class ConfigData : BaseData
    {

        #region Member

        private const string defaultDeployPath = @"\\asys-smart500\ASYS_Installation";
        private const string uriErrorMessage = "Eingabe ist kein gültiger Pfad!";
        private const string versionErrorMessage = "Die Versionsnummer muss vierstellig sein!";
        private const string pflichtfeldErrorMessage = "Pflichtfeld darf nicht leer sein!";

        #endregion

        #region Properties

        private VersionData version;
        public VersionData Version
        {
            get { return version; }
            set
            {
                version = value;
                OnNotifyPropertyChanged();
            }
        }

        //#region testnumeric

        //private int hauptversion = 0;

        //public int Hauptversion
        //{
        //    get { return hauptversion; }
        //    set
        //    {
        //        hauptversion = value;
        //        OnNotifyPropertyChanged();
        //    }
        //}

        //private int nebenversion = 0;

        //public int Nebenversion
        //{
        //    get { return nebenversion; }
        //    set
        //    {
        //        nebenversion = value;
        //        OnNotifyPropertyChanged();
        //    }
        //}

        //private int buildnummer = 0;

        //public int Buildnummer
        //{
        //    get { return buildnummer; }
        //    set
        //    {
        //        buildnummer = value;
        //        OnNotifyPropertyChanged();
        //    }
        //}

        //private int revision = 0;

        //public int Revision
        //{
        //    get { return revision; }
        //    set
        //    {
        //        revision = value;
        //        OnNotifyPropertyChanged();
        //    }
        //}

        //#endregion

        private Uri deployPath = new Uri(defaultDeployPath);
        public Uri DeployPath
        {
            get { return deployPath; }
            set
            {
                if (Validate(value)) deployPath = value;
                OnNotifyPropertyChanged();
            }
        }

        public string FullDeployPath => Path.Combine(DeployPath.AbsolutePath, $"SMART PharmaComp Update 5.0.0.{DeployPathVersion.ToString()}");

        private string deployPathVersion;
        public string DeployPathVersion
        {
            get { return deployPathVersion; }
            set
            {
                if (Validate(value)) deployPathVersion = value;
                OnNotifyPropertyChanged();
            }
        }

        private bool isLocked;
        public bool Islocked
        {
            get { return isLocked; }
            set
            {
                isLocked = value;
                OnNotifyPropertyChanged();
            }
        }

        #endregion

        public override bool Validate(object value, [CallerMemberName] string propertyName = "")
        {

            var isValid = true;

            switch (propertyName)
            {
                case "DeployPath":
                    if (value == null)
                    {
                        AddError(propertyName, uriErrorMessage, true);
                        isValid = false;
                    }
                    else
                    {
                        RemoveError(propertyName, uriErrorMessage);
                    }
                    break;
                case "DeployPathVersion":

                    var NumericString = value as string;

                    if (string.IsNullOrWhiteSpace(NumericString))
                    {
                        AddError(propertyName, pflichtfeldErrorMessage, true);
                        isValid = false;
                    }
                    else
                    {
                        RemoveError(propertyName, pflichtfeldErrorMessage);
                    }

                    if (NumericString.Length != 4)
                    {
                        AddError(propertyName, versionErrorMessage, true);
                        isValid = false;
                    }
                    else
                    {
                        RemoveError(propertyName, versionErrorMessage);
                    }
                    break;
            }

            return isValid;

        }

    }
}