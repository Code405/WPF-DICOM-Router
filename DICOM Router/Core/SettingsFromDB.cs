using DICOM_Router.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DICOM_Router.Core
{
    public static class SettingsFromDB
    {
        // This class will load all DB settings into memory at startup.
        private static List<RoutingRule> routingRules = new List<RoutingRule>();
        private static List<Device> devices = new List<Device>();
        private static Server ?server;
        public static void LoadSettingsFromDB()
        {
            // TODO:




        }

        public static List<RoutingRule> GetRulesFromDB()
        {
            RoutingRule rule = new RoutingRule();
            RoutingRule rule2 = new RoutingRule();
            RoutingRule rule3 = new RoutingRule();

            rule.Name = "AutoForward";
            rule.Type = "0";
            rule.SourceTag = "";
            rule.DestTag = "";
            rule.ReplaceValue = "";
            rule.MatchValue = "";
            rule.DeviceName = "Auto1";

            routingRules.Add(rule);

            rule2.Name = "DX Match";
            rule2.Type = "1";
            rule2.SourceTag = "00080060";
            rule2.DestTag = "";
            rule2.ReplaceValue = "";
            rule2.MatchValue = "DX";
            rule2.DeviceName = "Auto2";

            routingRules.Add(rule2);

            rule3.Name = "MR Match";
            rule3.Type = "1";
            rule3.SourceTag = "00080060";
            rule3.DestTag = "";
            rule3.ReplaceValue = "";
            rule3.MatchValue = "DX";
            rule3.DeviceName = "Auto3";

            routingRules.Add(rule3);

            return routingRules;
        }

        public static void GetServerSettingsFromDB()
        {
            // Simulated load of data from DB.. TESTING
            server.AeTitle = "StoreSCP";
            server.Port = 11112;
            server.LoggingMode = 0;

        }

        public static Device GetDeviceByName(string deviceName)
        {
            // Cheating this for TESTING.
            if(deviceName == "Auto1")
            {
                Device device = new Device();

                device.DeviceName = deviceName;
                device.IpAddress = "127.0.0.1";
                device.Port = 4104;
                device.CalledAe = "StorageSCP";
                device.CallingAe = "SendingSCU";
                Debug.WriteLine("AUTO1 SELECTED!!!!");
                return device;

            }
            else if(deviceName == "Auto2")
            {
                Device device = new Device();

                device.DeviceName = deviceName;
                device.IpAddress = "127.0.0.1";
                device.Port = 4105;
                device.CalledAe = "StorageSCP";
                device.CallingAe = "SendingSCU2";
                Debug.WriteLine("DX Match SELECTED!!!!");
                return device;
            }
            else
            {
                Device device = new Device();

                device.DeviceName = deviceName;
                device.IpAddress = "127.0.0.1";
                device.Port = 4106;
                device.CalledAe = "StorageSCP";
                device.CallingAe = "SendingSCU2";
                Debug.WriteLine("MR Match SELECTED!!!!");
                return device;
            }



        }

        public static string GetDbPath()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string dbPath = $@"{workingDirectory}\DB\Database.db";
            return dbPath;
        }


    }
}
