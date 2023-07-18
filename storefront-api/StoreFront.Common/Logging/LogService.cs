using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using StoreFront.Common.Interfaces.Logging;

namespace StoreFront.Common.Logging
{
    public class LogService : ILogService
    {
        #region Constructors
        public LogService() 
        {
            this._logger = LogManager.GetCurrentClassLogger();
        }
        #endregion

        #region Private Properties
        private Logger _logger { get; }
        #endregion

        #region Public Methods
        public void Fatal(string message, params object[] args)
        {
            if (args.Length > 0 )
            {
                message = string.Format(message, args);
            }

            this._logger.Fatal(message);
        }

        public void Error(string message, params object[] args)
        {
            if (args.Length > 0)
            {
                message = string.Format(message, args);
            }

            this._logger.Error(message);
        }

        public void Warn(string message, params object[] args)
        {
            if (args.Length > 0)
            {
                message = string.Format(message, args);
            }

            this._logger.Warn(message);
        }

        public void Info(string message, params object[] args)
        {
            if (args.Length > 0)
            {
                message = string.Format(message, args);
            }

            this._logger.Info(message);
        }

        public void Debug(string message, params object[] args)
        {
            if (args.Length > 0)
            {
                message = string.Format(message, args);
            }

            this._logger.Debug(message);
        }

        public void Trace(string message, params object[] args)
        {
            if (args.Length > 0)
            {
                message = string.Format(message, args);
            }

            this._logger.Trace(message);
        }


        #endregion
    }
}
