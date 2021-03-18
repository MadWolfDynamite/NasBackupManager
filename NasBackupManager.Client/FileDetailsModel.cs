using System;
using System.Collections.Generic;
using System.Text;

namespace NasBackupManager.Client
{
    public class FileDetailsModel
    {
        public string Source { get; set; }
        public string Destination { get; set; }

        public IEnumerable<string> ExcludedExtensions { get; set; }

        public bool RecurseFolders { get; set; } = true;
        public bool SyncSource { get; set; } = false;

        public FileDetailsModel() : this(Environment.CurrentDirectory, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)) { }
        public FileDetailsModel(string source, string destination)
        {
            Source = source;
            Destination = destination;

            ExcludedExtensions = new List<string>();
        }
    }
}
