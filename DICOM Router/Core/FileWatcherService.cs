using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using FellowOakDicom;

namespace DICOM_Router.Core
{

/*
**********************************************************************************************************
**********************************************************************************************************
*                            
*               
*               File Watcher Service to monitor incoming folder.
*               
*               
*               Documentation and example that was used to create this can be found at:
*               https://learn.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?view=net-10.0
*               
**********************************************************************************************************
**********************************************************************************************************
*/

    public class FileWatcherService : IDisposable
    {
        private FileSystemWatcher fsw;

        // Create an event that can be subscribed to.
        public event Action<string> FileCreated;

        public void Start()
        {
            // Get the current directory and the main directory above it. 
            string currentDirectory = Directory.GetCurrentDirectory();
            string baseDirectory = Directory.GetParent(currentDirectory).FullName;

            if (fsw != null) return; // Check if running.

            fsw = new FileSystemWatcher
            {
                Path = $@"{currentDirectory}/Incoming", // Map to the ScpReceiveService.cs Incoming folder.
                Filter = "*.*",
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            // Add Events
            fsw.Created += OnCreated;
            fsw.Error += OnError;

            // Start listening
            fsw.EnableRaisingEvents = true;
        }

        private async void OnCreated(object sender, FileSystemEventArgs e)
        {
            // Send event to subscribers.
            FileCreated?.Invoke(e.FullPath);// Are we even going to use this? 
            Debug.WriteLine($"File created: {e.FullPath}");

            // Add a delay to avoid the File Lock error.
            Thread.Sleep(1000);

            // Created a file from the file path.
            DicomFile dicomFile = DicomFile.Open(e.FullPath);

            // Add dicom file to the output queue. 
            OutputQueue.AddDicomFile(dicomFile);

            // This should be done in the Rules Engine... Not here. For Testing Only. 
            ScuSendService scu = new ScuSendService("localhost", 4104, "SCU", "SCP"); 

            await scu.Send(dicomFile);
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            // TODO: Log and handle error.
        }

        public void Dispose()
        {
            if (fsw != null)
            {
                fsw.EnableRaisingEvents = false;
                fsw.Created -= OnCreated;
                fsw.Error -= OnError;
                fsw.Dispose();
            }
        }
    }
}
    