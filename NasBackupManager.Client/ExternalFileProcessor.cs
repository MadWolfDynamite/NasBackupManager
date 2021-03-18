using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NasBackupManager.Client
{
    public static class ExternalFileProcessor
    {
        public static async Task<string> LoadJsonData(string filePath)
        {
            return await File.ReadAllTextAsync(filePath);
        }
    }
}
