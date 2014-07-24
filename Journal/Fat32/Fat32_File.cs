using Journal.Volume;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal
{
    public class Fat32_File : IFile
    {
        public FileInfo Entry { get; set; }
        private IFile _Parent;
        public IFile Parent { get { return _Parent; } set { _Parent = value; } }
        public List<IFile> Children { get { return new List<IFile>(); }}
        public bool IsFile() { return true; }
        public bool IsFolder() { return false; }
        public Fat32_File(FileInfo u, IFile parent)
        {

            Entry = u;
            _Parent = parent;
        }
        public string Name() { return Entry.Name; }
        public int FileCount()
        {
            return 1;
        }

        public int FolderCount()
        {
            return 0;
        }
        public override string ToString()
        {
            var t = "Entry: '" + Entry.Name + "'";
            if(Parent != null)
                t += " Parent: '" + Parent.Name() + "'";
            t += "\n\tFileCount: " + FileCount() + " FolderCount: " + FolderCount();
            return t;
        }
    }
}
