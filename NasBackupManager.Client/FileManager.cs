using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NasBackupManager.Client
{
    public static class FileManager
    {
        public static List<FileInfo> GetFiles(string location, bool recurse)
        {
            var results = new List<FileInfo>();

            if (!Directory.Exists(location))
                return results;

            if (recurse)
            {
                foreach (var directory in Directory.EnumerateDirectories(location))
                {
                    try
                    {
                        results.AddRange(GetFiles(directory, recurse));
                    }
                    catch (Exception ex)
                    {
                        var orginalTextColour = Console.ForegroundColor;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[ERROR] Skipped {directory} due to the following:");

                        Console.ForegroundColor = orginalTextColour;
                        Console.WriteLine($"{ex.Message}");
                        Console.WriteLine("----------------------------------------------------------------");
                    }
                }           
            }

            foreach (var file in Directory.EnumerateFiles(location))
                results.Add(new FileInfo(file));

            return results;
        }

        public static FileInfo GetFileDetails(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            return new FileInfo(filePath);
        }

        public static bool CopyFile(string filePath, string destinationPath)
        {
            bool isCompleted = true;

            File.Copy(filePath, destinationPath, true);

            return isCompleted;
        }
    }
}
