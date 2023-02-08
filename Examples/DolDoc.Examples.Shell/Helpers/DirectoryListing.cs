using System;
using System.Collections.Generic;
using System.IO;

namespace DolDoc.Examples.Shell.Helpers
{
    public static class DirectoryListing
    {
        public class Entry
        {
            internal Entry(string fullPath, string name, bool isDirectory, DateTime? lastModified, long? size = null)
            {
                FullPath = fullPath;
                Name = name;
                Size = size ?? 0;
                IsDirectory = isDirectory;
                LastModified = lastModified ?? DateTime.MinValue;
            }

            public bool IsDirectory { get; set; }

            public string Name { get; set; }

            public long Size { get; set; }

            public string FullPath { get; set; }

            public DateTime LastModified { get; set; }
        }

        public static Entry[] List(string path)
        {
            var dir = new DirectoryInfo(path);
            var result = new List<Entry>();

            result.Add(new Entry(dir.FullName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), ".", true, null));
            if (dir.Parent != null)
                result.Add(new Entry(dir.Parent.FullName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).TrimEnd('\\'), "..", true, null));

            foreach (var directory in dir.EnumerateDirectories())
                result.Add(new Entry(directory.FullName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), directory.Name, true, directory.LastWriteTimeUtc));
            foreach (var file in dir.EnumerateFiles())
                result.Add(new Entry(file.FullName.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), file.Name, false, file.LastWriteTimeUtc, file.Length));

            return result.ToArray();
        }
    }
}
