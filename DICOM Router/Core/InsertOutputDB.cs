using DICOM_Router.Models;
using FellowOakDicom;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DICOM_Router.Core
{
    public class InsertOutputDB
    {
        public string DateSent { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string ModalityType { get; set; }
        public int ImageCount { get; set; }
        public string RuleName { get; set; }
        public string DeviceName { get; set; }
        public string Status { get; set; }
        public string Suid { get; set; }
        public InsertOutputDB(RoutingRule rule, DicomFile dicomFile)
        {
            var dataSet = dicomFile.Dataset;

            this.PatientId = dataSet.GetString(DicomTag.PatientName);
            this.PatientId = dataSet.GetString(DicomTag.PatientID);
            this.ModalityType = dataSet.GetString(DicomTag.Modality);
            this.ImageCount += 1;
            this.RuleName = rule.Name;
            this.DeviceName = rule.DeviceName;
            this.Status = "Processing";
            this.Suid = dataSet.GetString(DicomTag.StudyInstanceUID);

        }

        // Overriding constructor in the case of just updating a record.
        public InsertOutputDB()
        {

        }

        // Insert the study info into the outgoing DB. 
        public int Insert()
        {
            DatabaseContext databaseContext = new DatabaseContext();

            var outgoingStudy = new OutgingStudy
            {
                DateSent = DateTime.Now.ToString(),
                PatientId = this.PatientId,
                PatientName = this.PatientName,
                ModalityType = this.ModalityType,
                ImageCount = this.ImageCount,
                RuleName = this.RuleName,
                DeviceName = this.DeviceName,
                Status = this.Status,
                Suid = this.Suid
            };
            try
            {
                databaseContext.OutgingStudies.Add(outgoingStudy);
                databaseContext.SaveChanges();
                int recordID = outgoingStudy.Id;

                return recordID;
            }
            catch(Exception e)
            {
                return 0;
            }
        }

        // Method to insert Sending into outgoing DB.
        public int InsertSendingStatus(int recordID)
        {
            try
            {
                using (var databaseContext = new DatabaseContext())
                {
                    databaseContext.OutgingStudies
                        .Where(e => e.Id == recordID)
                        .ExecuteUpdate(setters => setters
                            .SetProperty(e => e.Status, "Sending"));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return recordID;
        }
    }
}
