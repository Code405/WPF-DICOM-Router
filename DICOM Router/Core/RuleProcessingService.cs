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

        public void Start(string fileName)
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
            // 1 = Conditional Tag Matches a value.
            // 2 = Always replace Tag with new string;
            // 3 = Copy Tag to new Tag.

            switch (rule.Type)
            {
                case "0":
                    SendFile(device, rule, this.sourceDicomFile);
                    break;
                case "1":
                    TagMatchesValue(rule, device, this.sourceDicomFile);
                    break;
                case "2":
                    break;
                case "3":
                    break;
                default:
                    break;
            }
        }

        private void CopyTag()
        {
            // TODO:

        }

        private void ReplaceTagValue()
        {
            //TODO:

        }

        private void TagMatchesValue(RoutingRule rule, Device device, DicomFile dicomFile)
        {
            // Create a dataset from Dicom File and get the string value of the Tag specified.
            var dataSet = dicomFile.Dataset;
            string tagNumber = rule.SourceTag; // TODO: Add null handling.
            string dicomTagToCheck = dataSet.GetString(DicomTag.Parse(tagNumber));

            // Simple string match for a tag value. IE: Modailty = "MR"
            if (dicomTagToCheck == rule.MatchValue)
            {
                SendFile(device, rule, dicomFile);
            }
            // No match? Do nothing.

        }

        // Method to send dicom file for all rules to use.
        private async void SendFile(Device device, RoutingRule rule, DicomFile dicomFile)
        {
            InsertOutputDB insertOutputDB = new InsertOutputDB(rule, dicomFile);
            int recordID = insertOutputDB.Insert();

            ScuSendService scu = new ScuSendService(device);

            await scu.Send(dicomFile, recordID);
        }
    }
}
