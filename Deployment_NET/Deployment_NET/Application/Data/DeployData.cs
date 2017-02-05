using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace awinta.Deployment_NET.Application.Data
{
    public class DeployData : BaseData
    {
        private ConfigData config;
        private ObservableCollection<ProjectData> data;

        public DeployData(string solution)
        {
            UserName = Environment.UserName;
            Solution = solution;
            Config = new ConfigData();
            Data = new ObservableCollection<ProjectData>();
        }

        [Key, Column(Order = 0)]
        public string UserName { get; set; }

        [Key, Column(Order = 1)]
        public string Solution { get; set; }

        public ConfigData Config
        {
            get { return config; }
            set
            {
                config = value;
                OnNotifyPropertyChanged();
            }
        }

        public ObservableCollection<ProjectData> Data
        {
            get { return data; }
            set
            {
                data = value;
                OnNotifyPropertyChanged();
            }
        }
    }
}