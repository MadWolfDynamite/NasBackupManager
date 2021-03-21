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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[ERROR] Skipped {directory} due to the following:");

                        Console.ResetColor();
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

            try 
            {
                var filename = Path.GetFileName(filePath);
                var resolvedPath = Path.Combine(destinationPath, filename);

                if (!Directory.Exists(destinationPath))
                    Directory.CreateDirectory(destinationPath);

                File.Copy(filePath, resolvedPath, true);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR] Unable to copy '{filePath}' to '{destinationPath}':");

                Console.ResetColor();
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine("----------------------------------------------------------------");

                isCompleted = false;
            }
            
            return isCompleted;
        }
    }
}
