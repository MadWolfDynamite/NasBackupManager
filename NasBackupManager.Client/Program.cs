﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NasBackupManager.Client
{
    class Program
    { 
        private static async Task Main(string[] args)
        {
            LogManager logger = new LogManager();

            string jsonFileLocation;
            if (args.Length == 0)
            {
                jsonFileLocation = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\BackupSettings.json";
            }
            else
            {
                jsonFileLocation = args[0];
            }

            Console.WriteLine($"Loading {jsonFileLocation}...");
            Console.WriteLine(CreateDivider());

            var jsonData = await ExternalFileProcessor.LoadJsonData(jsonFileLocation);
            var backupSettings = JsonConvert.DeserializeObject<List<FileDetailsModel>>(jsonData);

            await logger.WriteLog($"Loaded '{jsonFileLocation}' for backup data");

            Console.WriteLine("Processing Files...");
            Console.WriteLine(CreateDivider());

            Parallel.ForEach(backupSettings, details =>
            {
                int copyCount = 0, overwriteCount = 0;

                logger.WriteLog($"Processing {details.Source}");

                System.Text.StringBuilder buffer = new System.Text.StringBuilder();
                var files = FileManager.GetFiles(details.Source, details.RecurseFolders);

                buffer.AppendLine($"[{details.Source}]");

                foreach (var file in files)
                {
                    if (details.ExcludedExtensions.Contains(file.Extension))
                        continue;

                    var destination = FileManager.GetFileDetails(file.FullName.Replace(details.Source, details.Destination));

                    if (destination != null)
                    {
                        if (destination.LastWriteTime >= file.LastWriteTime)
                            continue;

                        overwriteCount++;
                    }

                    logger.WriteLog($"Copying '{file.Name}' [{file.DirectoryName} --> {file.DirectoryName.Replace(details.Source, details.Destination)}]");

                    copyCount++;  
                }

                buffer.AppendLine($"Files Copied: {copyCount - overwriteCount:00}");
                buffer.AppendLine($"Files Updated: {overwriteCount:00}");

                if (details.SyncSource)
                {
                    int deleteCount = 0;
                    var previousFiles = FileManager.GetFiles(details.Destination, details.RecurseFolders);

                    foreach (var file in previousFiles)
                    {
                        var source = FileManager.GetFileDetails(file.FullName.Replace(details.Destination, details.Source));

                        if (source == null)
                        {
                            logger.WriteLog($"Deleting '{file.FullName}' to sync with {file.DirectoryName.Replace(details.Destination, details.Source)}");
                            deleteCount++;
                        }
                    }

                    buffer.AppendLine($"Files Deleted: {deleteCount:00}");
                }

                logger.WriteLog($"Processed {files.Count} files in {details.Source}");

                buffer.AppendLine();
                Console.Write(buffer);
            });
        }

        private static string CreateDivider()
        {
            string output = "";

            for (int i = 0; i < 75; i++)
                output += "-";

            return output;
        }
    }
}