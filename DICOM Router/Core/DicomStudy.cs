using System;
using System.Collections.Generic;
using System.Text;
using FellowOakDicom;

namespace DICOM_Router.Core
{
/*
**********************************************************************************************************
**********************************************************************************************************
*                            
*               
*               A class designed as a container of all Dicom Files for a single Study.
*               
*               This container will be used to add an entire study to the output queue
*               so that is can be sent with a single association.
*               
*               
**********************************************************************************************************
**********************************************************************************************************
*/

    public class DicomStudy
    {
        List<DicomFile> dicomFiles = new List<DicomFile>();


        public void AddDicomFile(DicomFile dicomFile)
        {
            dicomFiles.Add(dicomFile);
        }   


    }
}
