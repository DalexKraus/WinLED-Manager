using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RzChromaLEDs
{
    class WinAPI
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static readonly int SW_HIDE = 0;
        public static readonly int SW_SHOW = 5;

        public static void ShowConsoleWindow(bool showing)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, showing ? SW_SHOW : SW_HIDE);
        }
    }
}
