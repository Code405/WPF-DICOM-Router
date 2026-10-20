using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using FellowOakDicom;
using DICOM_Router.Dummy;
//using Microsoft.Extensions.Caching.Memory;
using System.Runtime.Caching;

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

        // Variables for Cache to avoid duplicate processing issues:
        private readonly MemoryCache _memCache = MemoryCache.Default;
        private readonly TimeSpan _cooldownPeriod = TimeSpan.FromMilliseconds(500);

        // Create an event that can be subscribed to.
        public event Action<string> FileCreated;

        public FileWatcherService()
        {
            string UniqueCacheName = $"FileWatcherCache_{Guid.NewGuid()}";
            _memCache = new MemoryCache(UniqueCacheName);
        }

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
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite };

            // Add Events
            fsw.Created += OnCreated;
            fsw.Error += OnError;

            // Start listening
            fsw.EnableRaisingEvents = true;

        }

        private void OnCreated(object sender, FileSystemEventArgs e) // This probably shouldnt be async now.
        {
            // Check if the file is already in the Cache. If so avoid it.
            // This was added because irratic behavior where the same file
            // was handled several times.      BUG was actually in RulesProcessor... not here. This maybe can go away?
            if (_memCache.Contains(e.FullPath))
            {
                return;
            }

            // If not, add the file to the cache to avoid double processing.
            var cachePolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMilliseconds(500)
            };

            _memCache.Add(e.FullPath, true, cachePolicy);

            // Send event to subscribers.
            FileCreated?.Invoke(e.FullPath);// Are we even going to use this? 
            Debug.WriteLine($"File created: {e.FullPath}");


            // Add a delay to avoid the File Lock error.
            Thread.Sleep(500);

            // Created a file from the file path.
            DicomFile dicomFile = DicomFile.Open(e.FullPath, FileReadOption.ReadAll);

            // Get just the filename from the full path of the dicomFile
            string fileName = Path.GetFileName(e.FullPath);

            Logger.Log(3, $"FileWatcher is processing file: {fileName}");

            // Send dicomFile into Rules Engine for processing. 
            RuleProcessingService ruleProcessor = new RuleProcessingService(dicomFile);

            // Start processing the dicomFile. Pass the fileName so that it can be removed from Incoming directory.
            ruleProcessor.Start(fileName);

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
    