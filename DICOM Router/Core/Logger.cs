using DICOM_Router.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DICOM_Router.Core
{
    public static class Logger
    {
        public static void Log(int LogLevel,string message)
        {
            Server server = new Server();
            server = SettingsFromDB.GetServerSettingsFromDB();

            if(LogLevel <= server.LoggingMode)
            {
                // If the Server Loggin Level greater than the log level of the message, Log it.
                // 3 Information, 2 Warning, 1 Error.
                string baseDir = AppContext.BaseDirectory;
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                string logDirectory = $@"{baseDir}\log\";

                Directory.CreateDirectory(logDirectory);

                string logPath = $@"{logDirectory}\{date}.log";

                using (StreamWriter writer = new StreamWriter(logPath, true))
                {
                    writer.WriteLine(message);
                }

            }


        }
    }
}
