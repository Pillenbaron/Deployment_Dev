using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace awinta.Deployment_NET.Solution.Model
{
    internal class ConfigData : Base.Model.BaseData
    {
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
            }
        }


    }
}
