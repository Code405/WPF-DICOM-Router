using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace DICOM_Router.Core
{
/*
**********************************************************************************************************
**********************************************************************************************************
*                            
*               
*               Class used to move file from Incoming Directory to Images directory after 
*               FileWatcher converts to "In Memory".
*               
*               
**********************************************************************************************************
**********************************************************************************************************
*/
    public static class FileMover
    {
        public static void Move(string fileName)
        {
            string baseDir = AppContext.BaseDirectory;
            string incomingFolder = @"\Incoming\";
            string storageFolder = @"\Images\";

            Directory.CreateDirectory($"{baseDir}{storageFolder}");

            try
            {
                File.Move($"{baseDir}{incomingFolder}{fileName}", $"{baseDir}{storageFolder}{fileName}");
            }
            catch (FileNotFoundException)
            {
                Logger.Log(1, "[ERROR] The source file was not found.");
            }
            catch (IOException ex)
            {
                Logger.Log(1,$"[ERROR] An I/O error occurred on file {fileName}: {ex.Message}");
            }

        }
    }
}
