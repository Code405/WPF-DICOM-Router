using System;
using System.Collections.Generic;
using System.Text;

namespace DICOM_Router.Dummy
{
    class OutgoingDicomData
    {
        public DateTime dateSent { get; set; }
        public int patientID { get; set; }
        public string name { get; set; }
        public string modality { get; set; }
        public int images { get; set; }
        public string rule { get; set; }
        public string device { get; set; }
        public string status { get; set; }
        public string suid { get; set; }

        public OutgoingDicomData(DateTime dateSent, int patientID, string name, string modality, int images,
                                                string rule, string device, string status, string suid)
        {
            this.dateSent = dateSent;
            this.patientID = patientID;
            this.name = name;
            this.modality = modality;
            this.images = images;
            this.rule = rule;
            this.device = device;
            this.status = status;
            this.suid = suid;
        }
    }
}
