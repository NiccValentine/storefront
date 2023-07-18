using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFront.Common.Interfaces.Logging
{
    public interface ILogService
    {
        void Fatal(string message, params object[] args);

        void Error(string message, params object[] args);

        void Warn(string message, params object[] args);

        void Info(string message, params object[] args);

        void Debug(string message, params object[] args);

        void Trace(string message, params object[] args);
    }
}
