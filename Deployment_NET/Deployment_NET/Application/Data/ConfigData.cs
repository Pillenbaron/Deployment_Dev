using System.IO;
using System.Runtime.CompilerServices;

namespace awinta.Deployment_NET.Application.Data
{
    public class ConfigData : BaseData
    {

        public int ConfigDataId { get; set; }

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
                case "BinPath":
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

        public bool IsFeature { get; set; }

        private VersionData version;

        public VersionData Version
        {
            get { return version; }
            set
            {
                if (version == value) return;
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
                if (deployPath == value) return;
                Validate(value);
                deployPath = value;
                OnNotifyPropertyChanged();
            }
        }

        private string binPath;

        public string BinPath
        {
            get { return binPath; }
            set
            {
                if (binPath == value) return;
                Validate(value);
                binPath = value;
                OnNotifyPropertyChanged();
            }
        }

        public string DynamicBinPath
            => IsFeature ? binPath.Replace("\\Dev\\", "\\Dev-Feature\\") : binPath.Replace("\\Dev-Feature\\", "\\Dev\\")
        ;

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
                if (isLocked == value) return;
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
                if (doCheckout == value) return;
                doCheckout = value;
                OnNotifyPropertyChanged();
            }
        }

        private bool copyToUpdate;

        public bool CopyToUpdate
        {
            get { return copyToUpdate; }
            set
            {
                if (copyToUpdate == value) return;
                copyToUpdate = value;
                OnNotifyPropertyChanged();
            }
        }

        private bool copyToSmart;

        public bool CopyToSmart
        {
            get { return copyToSmart; }
            set
            {
                if (copyToSmart == value) return;
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
                if (copyToBin == value) return;
                copyToBin = value;
                OnNotifyPropertyChanged();
            }
        }

        public DeployData DeployData { get; set; }

        #endregion
    }
}