using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace awinta.Deployment_NET.Base.Model
{
    internal abstract class BaseData : INotifyPropertyChanged, IDataErrorInfo
    {
        public virtual string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual string this[string columnName]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void OnNotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString()
        {

            string Result = string.Empty;

            Type Class = this.GetType();
            PropertyInfo[] propertyInfos = Class.GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {

                if (propertyInfo.CanRead)
                {

                    if (Result == string.Empty)
                    {
                        Result += propertyInfo.GetValue(this, null);
                    }
                    else
                    {
                        Result += "." + propertyInfo.GetValue(this, null);
                    }

                }

            }

            return Result;

        }

    }
}
