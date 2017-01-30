using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace awinta.Deployment_NET.Logging
{
    public abstract class LoggingBase
    {

        protected static Logger logger = LogManager.GetCurrentClassLogger();

    }
}
