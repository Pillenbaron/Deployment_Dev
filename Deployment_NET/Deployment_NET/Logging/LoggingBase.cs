using NLog;

namespace awinta.Deployment_NET.Logging
{
    public abstract class LoggingBase
    {

        protected static Logger logger = LogManager.GetCurrentClassLogger();

    }
}
