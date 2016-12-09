using System.IO;
using System.Runtime.CompilerServices;

namespace awinta.Deployment_NET.Solution.Model
{
    internal class ConfigData : BaseData
    {

        #region Member

        private const string defaultDeployPath = @"\\asys-smart500\ASYS_Installationen\SMART PharmaComp Update";
        private const string uriErrorMessage = "Eingabe ist kein gültiger Pfad!";
        private const string versionErrorMessage = "Die Versionsnummer muss vierstellig sein!";
        private const string pflichtfeldErrorMessage = "Pflichtfeld darf nicht leer sein!";
        private const string uriExistsErrorMessage = "Der Pfad existiert nicht!";

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

        private string deployPath;
        public string DeployPath
        {
            get { return deployPath; }
            set
            {
                Validate(value);
                deployPath = value;
                OnNotifyPropertyChanged();

            }
        }

        public string FullDeployPath => Path.Combine(DeployPath, $"SMART PharmaComp Update 5.0.0.{DeployPathVersion.ToString()}");

        private string deployPathVersion = "0";
        public string DeployPathVersion
        {
            get { return deployPathVersion; }
            set
            {
                Validate(value);
                deployPathVersion = value;
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

        public ConfigData()
        {

            Version = new VersionData();
            DeployPath = defaultDeployPath;

        }

        public override bool Validate(object value, [CallerMemberName] string propertyName = "")
        {

            var isValid = true;

            switch (propertyName)
            {
                case "DeployPath":

                    var path = value as string;

                    if (string.IsNullOrWhiteSpace(path))
                    {
                        AddError(propertyName, uriErrorMessage, true);
                        isValid = false;
                    }
                    else
                    {
                        RemoveError(propertyName, uriErrorMessage);
                    }

                    if (!Directory.Exists(path))
                    {
                        AddError(propertyName, uriExistsErrorMessage, true);
                        isValid = false;
                    }
                    else
                    {
                        RemoveError(propertyName, uriExistsErrorMessage);
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