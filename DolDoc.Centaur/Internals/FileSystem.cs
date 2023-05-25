using System;
using System.IO;

namespace DolDoc.Centaur.Internals
{
    public class CDirEntry
    {
        public CDirEntry next, parent, sub;
        public string full_name;
        public string name;
        public long user_data, user_data2;

        public ushort attr;
        public long clus, size;
        public DateTime datetime;
    }
    
    [Flags]
    public enum FUF_FLAG
    {
        FUF_RECURSE = 0x0000001, //r
        FUF_DIFF = 0x0000002, //d
        // FUF_DEL = 0x0000002, //d
        FUF_IGNORE = 0x0000004, //i
        FUF_ALL = 0x0000008, //a
        FUF_CANCEL = 0x0000010, //c
        FUF_REPLACE = 0x0000020, //R
        // FUF_RISKY = 0x0000020, //R
        FUF_PUBLIC = 0x0000040, //p
        FUF_MAP = 0x0000080, //m
        FUF_EXPAND = 0x0000100, //x
        FUF_SINGLE = 0x0000200, //s
        FUF_JUST_DIRS = 0x0000400, //D
        FUF_JUST_FILES = 0x0000800, //F
        FUF_JUST_TXT = 0x0001000, //T
        FUF_JUST_DD = 0x0002000, //$
        FUF_JUST_SRC = 0x0004000, //S
        FUF_JUST_AOT = 0x0008000, //A
        FUF_JUST_JIT = 0x0010000, //J
        FUF_JUST_GR = 0x0020000, //G
        FUF_Z_OR_NOT_Z = 0x0040000, //Z
        FUF_CLUS_ORDER = 0x0080000, //O Move disk head one direction
        FUF_SCAN_PARENTS = 0x0100000, //P
        FUF_FLATTEN_TREE = 0x0200000, //f
        FUF_WHOLE_LABELS = 0x0400000, //l
        FUF_WHOLE_LABELS_BEFORE = 0x0800000, //lb
        FUF_WHOLE_LABELS_AFTER = 0x1000000, //la
    }

    public class FileSystem
    {
        public static CDirEntry FilesFind(string ff_mask, long flags)
        {
            Console.WriteLine("Flags: {0}", flags);
            Console.WriteLine("Flags: {0}", (FUF_FLAG)flags);
            
            var path = Path.GetDirectoryName(ff_mask);
            var mask = Path.GetFileName(ff_mask);
            if (mask == "*")
                mask = "*.*";

            var dirInfo = new DirectoryInfo(path);
            var items = dirInfo.GetFileSystemInfos(mask);

            if (items.Length == 0)
                return null;

            var idx = 0;
            CDirEntry current = CreateDirEntry(items[idx++], null);
            CDirEntry result = current;
            while (idx < items.Length)
            {
                current = CreateDirEntry(items[idx++], current);
            }
            
            return result;
        }

        private static CDirEntry CreateDirEntry(FileSystemInfo fsi, CDirEntry current)
        {
            var res = new CDirEntry
            {
                full_name = fsi.FullName,
                name = fsi.Name,
                datetime = fsi.CreationTime,
            };

            if (current != null)
                current.next = res;
            return res;
        }
        
    }
}