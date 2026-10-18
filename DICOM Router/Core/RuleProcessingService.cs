using DICOM_Router.Models;
using FellowOakDicom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace DICOM_Router.Core
{
    public class RuleProcessingService
    {
        DicomFile sourceDicomFile;
        List<RoutingRule> routingRules = new List<RoutingRule>();
        Device device = new Device();

        public RuleProcessingService(DicomFile dicomFile)
        {
            this.sourceDicomFile = dicomFile;
        }

        public void Start()
        {
            // Get All Rules from DB as List.
            routingRules = SettingsFromDB.GetRulesFromDB();

            // Loop through list and process each rule to the dicomFile.
            foreach (RoutingRule rule in routingRules)
            {
                // Get Device Name from current rule, if rule matches we send to this device.
                string deviceName = rule.DeviceName.ToString();

                // Get the correct device by name from DB. 
                device = SettingsFromDB.GetDeviceByName(deviceName);
 
                // Add rule and device to RuleEngine call, the this.dicomFile can be pulled in from the instance.
                RuleEngine(rule, device);

            }
        }

        public void RuleEngine(RoutingRule rule, Device device)
        {
            // Determine which type of Rule this is:
            // 0 = AutoRoute all studies no conditions.
            // 1 = Conditional on Tag string match.
            // 2 = Replace Tag String with new string;
            // 3 = Copy Tag to new Tag.

            switch (rule.Type)
            {
                case "0":
                    SendFile(device, this.sourceDicomFile);
                    break;
            }
        }

        private void CopyTag()
        {

        }

        private void ConditionalTag()
        {

        }

        private async void SendFile(Device device, DicomFile dicomFile)
        {
            ScuSendService scu = new ScuSendService(device);

            await scu.Send(dicomFile);
        }
    }
}
