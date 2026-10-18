using FellowOakDicom;
using System;
using System.Collections.Generic;
using System.Text;

namespace DICOM_Router.Dummy
{
    public static class OutputQueue
    {
        // Create a queue to store dicomFiles coming from FileWatcherService.
        public static Queue<DicomFile> outputQueue = new Queue<DicomFile>();

        public static void AddDicomFile(DicomFile dicomFile) 
        {
            outputQueue.Enqueue(dicomFile);
        }  

        public static DicomFile GetNext()
        {
            DicomFile dicomFile;

            dicomFile = outputQueue.Dequeue();

            return dicomFile;
        }
    }
}  
