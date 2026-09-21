using System;
using System.Collections.Generic;
using System.Text;

namespace DICOM_Router
{
    public class IncomingDicomData
    {
        public DateTime date { get; set; }
        public int patientID { get; set; }
        public string name { get; set; }
        public string modality { get; set; }
        public int images { get; set; }

        public IncomingDicomData(DateTime date, int patientID, string name, string modality, int images)
        {
            this.date = date;
            this.patientID = patientID;
            this.name = name;
            this.modality = modality;
            this.images = images;
        }
    }
}
