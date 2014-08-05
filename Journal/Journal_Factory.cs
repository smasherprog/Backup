using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Journal
{
    public static class Journal_Factory
    {
        public static NTFSVolume Create(string fullpath)
        {
            return new NTFSVolume(fullpath.Substring(0, 1));
  
        }
    }
}
