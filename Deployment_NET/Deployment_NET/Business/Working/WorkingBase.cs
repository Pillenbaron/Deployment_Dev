using awinta.Deployment_NET.Data;

namespace awinta.Deployment_NET.Business.Working
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

        protected void WorkingProgress(int state)
        {
            ProgressCount = state;
            Working = true;
            IsEnabled = false;
        }
    }
}