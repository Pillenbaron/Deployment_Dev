using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awinta.Deployment_NET.Application.Data
{
    public class DeployData : BaseData
    {
        private ConfigData configData;
        private ObservableCollection<ProjectData> projectDatas;

        public DeployData(string solution)
        {
            DeployDataId = $"{Environment.UserDomainName}||{Environment.UserName}||{solution}";
            ConfigData = new ConfigData();
            ProjectDatas = new ObservableCollection<ProjectData>();
        }

        public string DeployDataId { get; set; }

        public ConfigData ConfigData
        {
            get { return configData; }
            set
            {
                configData = value;
                OnNotifyPropertyChanged();
            }
        }

        public ObservableCollection<ProjectData> ProjectDatas
        {
            get { return projectDatas; }
            set
            {
                projectDatas = value;
                OnNotifyPropertyChanged();
            }
        }
    }
}