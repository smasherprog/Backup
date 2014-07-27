using Journal.Volume;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal
{
    public class Fat32_Directory : IFile
    {
        public DirectoryInfo Entry { get; set; }
        private IFile _Parent;
        public IFile Parent { get { return _Parent; } set { _Parent = value; } }
        private List<IFile> _Children;
        public List<IFile> Children { get { return _Children; } }

        private int _FileCount = -1;
        private int _FolderCount = -1;
        public bool IsFile() { return false; }
        public bool IsFolder() { return true; }
        public UInt32 Changes { get { return 0; } }
        public Fat32_Directory(DirectoryInfo u, IFile parent)
        {
            _FileCount = _FolderCount = -1;
            Entry = u;
            _Parent = parent;
            _Children = new List<IFile>();
            var fs = u.EnumerateDirectories();
            var iterator = fs.GetEnumerator();
            bool go = true;
            while(go)
            {
                try
                {
                    go = iterator.MoveNext();
                    var dir = iterator.Current;
                    if(dir == null)
                        break;
                    _Children.Add(new Fat32_Directory(dir, this));
          
                } catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
            var enumfiles = u.EnumerateFiles();
            var fileiterator = enumfiles.GetEnumerator();
            go = true;
            while(go)
            {
                try
                {
                    go = fileiterator.MoveNext();
                    var dir = fileiterator.Current;
                    if(dir == null)
                        break;
                    _Children.Add(new Fat32_File(dir, this));
                    
                } catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
   
        }
        public string Name() { return Entry.Name; }
        public int FileCount()
        {
            if(_FileCount != -1)
                return _FileCount;
            _FileCount = 1;
            foreach(var item in Children)
                _FileCount += item.FileCount();
            return _FileCount;
        }

        public int FolderCount()
        {
            if(_FolderCount != -1)
                return _FolderCount;
            _FolderCount = 1;
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
