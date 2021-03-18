using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace NasBackupManager.Client
{
    public class LogManager
    {
        private string _folder, _file, _resolvedFilePath;

        public string Folder 
        { 
            get { return _folder; } 
            set
            {
                _folder = value;

                if (FileName != null)
                    _resolvedFilePath = Path.Combine(value, FileName);
            }
        }
        public string FileName 
        { 
            get { return _file; } 
            set 
            {
                _file = value;

                if (Folder != null)
                    _resolvedFilePath = Path.Combine(Folder, value);
            }
        }

        public LogManager() : this(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "output.txt") { }
        public LogManager(string folder, string file)
        {
            Folder = folder;
            FileName = file;

            if (File.Exists(_resolvedFilePath))
                File.Delete(_resolvedFilePath);
        }

        public async Task WriteLog(string message)
        {
            using var writer = new StreamWriter(_resolvedFilePath, true);
            await writer.WriteLineAsync($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.ffff}|{message}");
        }
    }
}
