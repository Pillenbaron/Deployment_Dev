using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace awinta.Deployment_NET.Solution.Model
{
    internal abstract class BaseData : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion

        #region Member

        private readonly Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        #endregion

        #region Propertie

        public bool HasErrors => errors.Count > 0;

        #endregion

        #region Validation

        public virtual bool Validate(object value, [CallerMemberName] string propertyName = "")
        {

            throw new NotImplementedException();

        }

        public void AddError(string propertyName, string error, bool isWarning)
        {
            if (!errors.ContainsKey(propertyName))
                errors[propertyName] = new List<string>();

            if (errors[propertyName].Contains(error)) return;
            if (isWarning) errors[propertyName].Add(error);
            else errors[propertyName].Insert(0, error);
            OnErrorsChanged(propertyName);
        }

        public void RemoveError(string propertyName, string error)
        {
            if (!errors.ContainsKey(propertyName) || !errors[propertyName].Contains(error)) return;
            errors[propertyName].Remove(error);
            if (errors[propertyName].Count == 0) errors.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName) || !errors.ContainsKey(propertyName)) return null;
            return errors[propertyName];
        }

        #endregion

        #region OnMethoden

        internal virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        internal virtual void OnNotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Other

        public override string ToString()
        {

            var result = string.Empty;

            var Class = GetType();
            var propertyInfos = Class.GetProperties();

            foreach (var propertyInfo in propertyInfos)
            {
                if (!propertyInfo.CanRead) continue;
                if (result == string.Empty)
                {
                    result += propertyInfo.GetValue(this, null);
                }
                else
                {
                    result += "." + propertyInfo.GetValue(this, null);
                }
            }

            return result;

        }

        #endregion

    }
}
