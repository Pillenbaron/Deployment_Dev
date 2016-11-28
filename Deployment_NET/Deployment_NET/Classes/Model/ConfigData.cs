using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.CompilerServices;

namespace awinta.Deployment_NET.Solution.Model
{
    internal class ConfigData : BaseData
    {

        #region Member

        private const string defaultDeployPath = "\\\\asys-smart500\\ASYS_Installation";
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

        public string FullDeployPath
        {
            get { return Path.Combine(DeployPath.AbsolutePath, $"SMART PharmaComp Update 5.0.0.{DeployPathVersion.ToString()}"); }
            private set { }
        }

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

            bool isValid = true;

            switch (propertyName)
            {
                case "DeployPath":
                    if (value == null)
                    {
                        AddError(propertyName, uriErrorMessage, true);
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
                    }
                    else
                    {
                        RemoveError(propertyName, pflichtfeldErrorMessage);
                    }

                    if (NumericString.Length != 4)
                    {
                        AddError(propertyName, versionErrorMessage, true);
                    }
                    else
                    {
                        RemoveError(propertyName, versionErrorMessage);
                    }
                    break;
                default:
                    break;
            }

            return isValid;

        }

    }
}