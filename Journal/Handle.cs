using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal
{
    public class Raw_File_Handle :IDisposable
    { 
        private IntPtr _Handle;
        public IntPtr Handle { get { return _Handle; } }
        public Raw_File_Handle()
        {
            _Handle = IntPtr.Zero;
        }
        public Raw_File_Handle(IntPtr h)
        {
            _Handle = h;
        }
        public void Dispose()
        {
            if (Handle != IntPtr.Zero) Win32Api.CloseHandle(Handle);
        }
    }
}
