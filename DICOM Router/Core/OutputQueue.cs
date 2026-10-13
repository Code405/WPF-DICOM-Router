using FellowOakDicom;
using System;
using System.Collections.Generic;
using System.Text;

namespace DICOM_Router.Core
{
    public class OutputQueue
    {

        // THIS IS ALL NONSENSE FOR TESTING. NOT RELEVANT WHATSOEVER. 
        DicomStudy study = new DicomStudy();

        DicomFile dicomFile = new DicomFile();

        Queue<DicomStudy> outputQueue = new Queue<DicomStudy>();

        public void Main() 
        {
            study.AddDicomFile(dicomFile);

            outputQueue.Enqueue(study);
        }  

    }
}  
