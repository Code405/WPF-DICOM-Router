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
            string baseDir = AppContext.BaseDirectory;
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string logDirectory = $@"{baseDir}\log\";

            Directory.CreateDirectory(logDirectory);

            string logPath = $@"{logDirectory}\{date}.log";

            /*
            if (!File.Exists(logPath))
            {
                File.WriteAllText(logPath, $"Log Created: {date}");
            }
            */

            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine(message);
            }
        }
    }
}
