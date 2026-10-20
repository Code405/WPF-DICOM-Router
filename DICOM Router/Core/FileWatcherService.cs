using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using FellowOakDicom;
using DICOM_Router.Dummy;

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

        private async void OnCreated(object sender, FileSystemEventArgs e) // This probably shouldnt be async now.
        {
            // Send event to subscribers.
            FileCreated?.Invoke(e.FullPath);// Are we even going to use this? 
            Debug.WriteLine($"File created: {e.FullPath}");

            // Add a delay to avoid the File Lock error.
            Thread.Sleep(1000);

            // Created a file from the file path.
            DicomFile dicomFile = DicomFile.Open(e.FullPath, FileReadOption.ReadAll);

            // Get just the filename from the full path of the dicomFile
            string fileName = Path.GetFileName(e.FullPath);

            // Send dicomFile into Rules Engine for processing. 
            RuleProcessingService ruleProcessor = new RuleProcessingService(dicomFile);

            // Start processing the dicomFile. Pass the fileName so that it can be removed from Incoming directory.
            ruleProcessor.Start(fileName);

            // Move the file out of Incoming Directory after loading into memory. 
            FileMover.Move(fileName);
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
    