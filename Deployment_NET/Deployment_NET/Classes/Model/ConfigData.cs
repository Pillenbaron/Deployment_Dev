using System.ComponentModel.DataAnnotations;
using System.IO;

namespace awinta.Deployment_NET.Solution.Model
{
    internal class ConfigData : Base.Model.BaseData
    {

        public override string this[string columnName]
        {
            get
            {
                string errorMessage = string.Empty;
                switch (columnName)
                {
                    case "DeployPath":
                        errorMessage = Service.ValidationService.ValidateUri(DeployPath);
                        break;
                    default:
                        break;
                }

                return base[columnName];
            }
        }

        private const string defaultDeployPath = "\\\\asys-smart500\\ASYS_Installation";

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

        private string deployPath = defaultDeployPath;
        public string DeployPath
        {
            get { return deployPath; }
            set
            {
                deployPath = value;
                OnNotifyPropertyChanged();
            }
        }

        public string FullDeployPath
        {
            get { return Path.Combine(DeployPath, $"SMART PharmaComp Update 5.0.0.{DeployPathVersion}"); }
            private set { }
        }

        private string deployPathVersion;
        [MaxLength(4), MinLength(4)]
        public string DeployPathVersion
        {
            get { return deployPathVersion; }
            set
            {
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

    }
}
