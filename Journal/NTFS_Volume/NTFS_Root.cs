using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal
{
    public class NTFS_Root: Journal.Volume.IFile
    {
        private Dictionary<UInt64, Journal.Volume.IFile> Lookup;

        private List<Journal.Volume.IFile> _TopLevel;
        public List<Journal.Volume.IFile> Children { get { return _TopLevel; } set{ _TopLevel = value;} }
        public Journal.Volume.IFile Parent { get { return null; } set {  } }
        private string _Name;
        public string Name() { return _Name; } 
        public bool IsFile(){ return false; } 
        public bool IsFolder(){ return true; }

        public NTFS_Root(string name)
        {
            Lookup = new Dictionary<ulong, Journal.Volume.IFile>();
            _TopLevel = new List<Volume.IFile>();
            _Name = name;

        }
        public void Add(Win32Api.UsnEntry u)
        {
            Lookup.Add(u.FileReferenceNumber, new Journal.Volume.NTFS_File(u));
        }
        public int FileCount()
        {
            var i = 0;
            foreach(var item in Children)
            {
                i += item.FileCount();
            }
            return i;
        }
        public int FolderCount()
        {
            var i = 0;
            foreach(var item in Children)
            {
                i += item.FolderCount();
            }
            return i;
        }
        public void Build()
        {
            Debug.WriteLine("Starting Lookup");
            var start = DateTime.Now;
            int foldercounter = 0;
            int filecounter = 0;
            foreach(var item in Lookup)
            {
                var file = (Journal.Volume.NTFS_File)item.Value;
                if(file.Entry.IsFile)
                    filecounter += 1;
                else
                    foldercounter += 1;
                Journal.Volume.IFile parent = null;
                if(Lookup.TryGetValue(file.Entry.ParentFileReferenceNumber, out parent))
                {
                    parent.Children.Add(item.Value);
                    item.Value.Parent = parent;
                }
            }
            Debug.WriteLine("Time took: " + (DateTime.Now - start).TotalMilliseconds + "ms");
            _TopLevel = Lookup.Where(a => a.Value.Parent == null).Select(a => a.Value).ToList();
            Debug.WriteLine("Root Nodes Found");
            foreach(var item in _TopLevel)  Debug.WriteLine(item);
            
            Debug.WriteLine("Total Files Found: " + filecounter);
            Debug.WriteLine("Total Folders Found: " + foldercounter);

        }
    }
}
