using Journal.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal
{
    public class Fat32Volume: IVolume
    {
        
        System.IO.DriveInfo _DriveInfo;
        public DriveInfo Drive { get { return _DriveInfo; } }

        private Journal.Volume.IFile _Root;
        public Journal.Volume.IFile Root { get { return _Root; } }

        public Fat32Volume(DriveInfo rootpath)
        {
            _DriveInfo = rootpath;
            
        }
        public void Map_Volume() {
            Debug.WriteLine("Starting Lookup");
            var start = DateTime.Now;
            _Root = new Fat32_Directory(Drive.RootDirectory, null);

            Debug.WriteLine("Time took: " + (DateTime.Now - start).TotalMilliseconds + "ms");

            Debug.WriteLine("Total Files Found: " + _Root.FileCount());
            Debug.WriteLine("Total Folders Found: " + _Root.FolderCount());
        }
        public void Dispose()
        {
        }
    }
}
