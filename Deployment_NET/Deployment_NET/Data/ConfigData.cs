using System.IO;
using System.Runtime.CompilerServices;

namespace awinta.Deployment_NET.Data
{
    public class ConfigData : BaseData
    {
        public ConfigData()
        {
            Version = new VersionData();
            DeployPath = DefaultDeployPath;
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
                        AddError(propertyName, UriErrorMessage, true);
                        isValid = false;
                    }
                    else
                    {
                        RemoveError(propertyName, UriErrorMessage);
                    }

                    if (!Directory.Exists(path))
                    {
                        AddError(propertyName, UriExistsErrorMessage, true);
                        isValid = false;
                    }
                    else
                    {
                        RemoveError(propertyName, UriExistsErrorMessage);
                    }

                    break;
                case "DeployPathVersion":

                    var numericString = value as string;

                    if (string.IsNullOrWhiteSpace(numericString))
                    {
                        AddError(propertyName, PflichtfeldErrorMessage, true);
                        isValid = false;
                    }
                    else
                    {
                        RemoveError(propertyName, PflichtfeldErrorMessage);
                    }

                    if (numericString != null && numericString.Length != 4)
                    {
                        AddError(propertyName, VersionErrorMessage, true);
                        isValid = false;
                    }
                    else
                    {
                        RemoveError(propertyName, VersionErrorMessage);
                    }
                    break;
            }

            return isValid;
        }

        #region Member

        //private const string defaultDeployPath = @"\\asys-smart500\ASYS_Installationen\SMART PharmaComp Update";

        private const string DefaultDeployPath = @"D:\TestUpdate";
        private const string UriErrorMessage = "Eingabe ist kein gültiger Pfad!";
        private const string VersionErrorMessage = "Die Versionsnummer muss vierstellig sein!";
        private const string PflichtfeldErrorMessage = "Pflichtfeld darf nicht leer sein!";
        private const string UriExistsErrorMessage = "Der Pfad existiert nicht!";

        #endregion

        #region Properties

        public string SolutionPath { get; set; }

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

        public string FullDeployPath => Path.Combine(DeployPath, $"SMART PharmaComp Update 5.0.0.{DeployPathVersion}");

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

        private bool doCheckout;

        public bool DoCheckout
        {
            get { return doCheckout; }
            set
            {
                doCheckout = value;
                OnNotifyPropertyChanged();
            }
        }

        private bool copyToSmart;

        public bool CopyToSmart
        {
            get { return copyToSmart; }
            set
            {
                copyToSmart = value;
                OnNotifyPropertyChanged();
            }
        }

        private bool copyToBin;

        public bool CopyToBin
        {
            get { return copyToBin; }
            set
            {
                copyToBin = value;
                OnNotifyPropertyChanged();
            }
        }

        #endregion
    }
}