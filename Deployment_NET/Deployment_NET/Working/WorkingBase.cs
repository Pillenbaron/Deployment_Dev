using awinta.Deployment_NET.Solution.Model;

namespace awinta.Deployment_NET.Working
{
    public class WorkingBase : BaseData
    {

        protected int progressCount;

        public int ProgressCount
        {
            get { return progressCount; }
            set
            {
                progressCount = value;
                OnNotifyPropertyChanged();
            }
        }

        protected bool working;

        public bool Working
        {
            get { return working; }
            set
            {
                working = value;
                OnNotifyPropertyChanged();
            }
        }

        protected bool isEnabled;
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
