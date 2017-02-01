using System;

namespace awinta.Deployment_NET.Data
{
    public class ProjectData : BaseData, IEquatable<ProjectData>
    {
        private AssemblyData assemblyInfo;

        private string assemblyName = string.Empty;

        private string assemblyPath = string.Empty;

        private string fullPath = string.Empty;

        private bool hasToRegister;

        private string name = string.Empty;

        public ProjectData()
        {
            assemblyInfo = new AssemblyData();
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnNotifyPropertyChanged();
            }
        }

        public string AssemblyName
        {
            get { return assemblyName; }
            set
            {
                assemblyName = value;
                OnNotifyPropertyChanged();
            }
        }

        public string FullPath
        {
            get { return fullPath; }
            set
            {
                fullPath = value;
                OnNotifyPropertyChanged();
            }
        }

        public AssemblyData AssemblyInfo
        {
            get { return assemblyInfo; }
            set
            {
                assemblyInfo = value;
                OnNotifyPropertyChanged();
            }
        }

        public string AssemblyPath
        {
            get { return assemblyPath; }
            set
            {
                assemblyPath = value;
                OnNotifyPropertyChanged();
            }
        }

        public string FullAssemblyPath => FullPath != null ? $"{FullPath}\\{AssemblyPath}" : null;

        public bool HasToRegister
        {
            get { return hasToRegister; }
            set
            {
                hasToRegister = value;
                OnNotifyPropertyChanged();
            }
        }

        public bool Equals(ProjectData other)
        {
            return other != null && Name.Equals(other.Name) && AssemblyName.Equals(other.AssemblyName) &&
                   FullPath.Equals(other.FullPath) && AssemblyPath.Equals(other.AssemblyPath) &&
                   FullAssemblyPath.Equals(other.FullAssemblyPath) /*&& HasToRegister.Equals(other.HasToRegister)*/;
        }

        public override bool Equals(object obj)
        {
            var project = obj as ProjectData;

            return Equals(project);
        }

        public override int GetHashCode()
        {
            //return $"{Name}{AssemblyName}{FullPath}{AssemblyPath}{FullAssemblyPath}{HasToRegister}".GetHashCode();
            return $"{Name}{AssemblyName}{FullPath}{AssemblyPath}{FullAssemblyPath}".GetHashCode();
        }

    }
}