using System;
using System.Collections.Generic;
using System.Text;

namespace DICOM_Router.Core
{
    public static class SettingsFromDB
    {
        // This class will load all DB settings into memory at startup.
        public static void LoadSettingsFromDB()
        {
        }

        public static void GetRulesFromDB()
        {
        }

        public static void GetServerSettingsFromDB()
        {

        }

        public static string GetDbPath()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string dbPath = $@"{workingDirectory}\DB\Database.db";
            return dbPath;
        }


    }
}
