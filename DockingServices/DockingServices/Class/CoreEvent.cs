using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace DockingServices
{
    public class CoreEvent
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern SafeWaitHandle CreateEvent(IntPtr lpSecurityAttributes, bool isManualReset,bool initialState, string name);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetEvent(SafeWaitHandle handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CloseHandle(SafeWaitHandle handle);
    }

}
