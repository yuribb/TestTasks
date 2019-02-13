using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class Logger
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Info(string message)
        {
            Log.Info(message);
        }

        public static void Warn(string message)
        {
            Log.Warn(message);
        }

        public static void Error(string message)
        {
            Log.Error(message);
        }

        public static void Error(string message, Exception ex)
        {
            Log.Error(message, ex);
        }

        public static void Error(Exception ex)
        {
            Log.Error(ex);
        }
    }
}
