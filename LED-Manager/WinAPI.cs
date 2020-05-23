using System;
using System.Runtime.InteropServices;

namespace RzChromaLEDs
{
    class WinAPI
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern long GetWindowLong(IntPtr hWnd, int index);

        [DllImport("user32.dll")]
        public static extern long SetWindowLong(IntPtr hWnd, int index, long dwNewLong);

        public static readonly int SW_HIDE  = 0;
        public static readonly int SW_SHOW  = 5;
        private const int WS_EX_APPWINDOW   = 0x40000;
        private const int WS_EX_TOOLWINDOW  = 0x0080;
        private const int GWL_EXSTYLE       = -0x14;

        public static void ShowConsoleWindow(bool showing)
        {
            var windowHandle = GetConsoleWindow();

            //Hides or shows the window
            ShowWindow(windowHandle, showing ? SW_SHOW : SW_HIDE);

            //Removes the application's icon from the taskbar
            SetWindowLong(windowHandle, GWL_EXSTYLE, GetWindowLong(windowHandle, GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
        }
    }
}
