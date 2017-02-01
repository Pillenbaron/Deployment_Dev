using NLog;

namespace awinta.Deployment_NET.Common.Logging
{
    public abstract class LoggingBase
    {

        protected static Logger logger = LogManager.GetCurrentClassLogger();

    }
}
