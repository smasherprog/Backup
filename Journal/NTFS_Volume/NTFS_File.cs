using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal.Volume
{

    public class NTFS_File : IFile
    {
        public Win32Api.UsnEntry Entry { get; set; }
        private IFile _Parent;
        public IFile Parent { get { return _Parent; } set { _Parent = value; } }
        private List<IFile> _Children;
        public List<IFile> Children { get { return _Children; } }
        private int _FileCount = -1;
        private int _FolderCount = -1;
        public bool IsFile() { return Entry.IsFile; }
        public bool IsFolder() { return Entry.IsFolder; }
        public NTFS_File(Win32Api.UsnEntry u)
        {
            Entry = u;
            _Children = new List<IFile>();
            Parent = null;
        }
        public string Name(){ return Entry.Name; } 
        public int FileCount()
        {
            if(_FileCount != -1)
                return _FileCount;
            _FileCount = 0;
            if(Entry.IsFile)
                _FileCount += 1;
            foreach(var item in Children)
                _FileCount += item.FileCount();
            return _FileCount;
        }

        public int FolderCount()
        {
            if(_FolderCount != -1)
                return _FolderCount;
            _FolderCount = 0;
            if(Entry.IsFolder)
                _FolderCount += 1;
            foreach(var item in Children)
                _FolderCount += item.FolderCount();
            return _FolderCount;
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
