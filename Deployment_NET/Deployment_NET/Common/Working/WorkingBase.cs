using awinta.Deployment_NET.Application.Data;

namespace awinta.Deployment_NET.Common.Working
{
    public class WorkingBase : BaseData
    {
        protected bool isEnabled;

        protected int progressCount;

        protected bool working;

        public int ProgressCount
        {
            get { return progressCount; }
            set
            {
                progressCount = value;
                OnNotifyPropertyChanged();
            }
        }

        public bool Working
        {
            get { return working; }
            set
            {
                working = value;
                OnNotifyPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnNotifyPropertyChanged();
            }
        }

        protected void WorkingStart()
        {
            Working = true;
            IsEnabled = false;
            ProgressCount = 0;
        }

        protected void WorkingStop()
        {
            ProgressCount = 100;
            Working = false;
            IsEnabled = true;
        }

        protected void WorkingLocked()
        {
            ProgressCount = 0;
            Working = false;
            IsEnabled = false;
        }

        protected void WorkingProgress(int state)
        {
            ProgressCount = state;
            Working = true;
            IsEnabled = false;
        }
    }
}