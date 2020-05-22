using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace RzChromaLEDs
{
    class ChromaSDK
    {
        // The exported DLL's name
        private const string DllName = "rzledapp.dll";

        [DllImport(DllName)]
        public static extern bool initialize();

        [DllImport(DllName)]
        public static extern bool unitialize();

        [DllImport(DllName)]
        public static extern void setKeyboardColor(ulong color);

        [DllImport(DllName)]
        public static extern void setMouseColor(ulong color);
    }
}
