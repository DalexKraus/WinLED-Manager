using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace RzChromaLEDs
{
    class Program
    {
        private const string    APP_NAME    = "LED-Manager";
        private const string    SERVER      = "192.168.1.7";
        private const int       PORT        = 7765;

        private static TrayIcon _trayIcon;
        private static Client _client;

        public Program()
        {
            _client = new Client();

            List<(string, EventHandler)> trayMenuItems = new List<(string, EventHandler)>()
            {
                (APP_NAME, null),
                ("-", null),
                ("Connect to Server ...", delegate { ConnectToServer(); }),
                ("-", null),
                ("Exit", delegate { Shutdown(); })
            };

            Thread.Sleep(250);
            WinAPI.ShowConsoleWindow(false);
            _trayIcon = new TrayIcon(APP_NAME, trayMenuItems);
        }

        private void ConnectToServer()
        {
            if (!_client.IsConnected())
            {
                WinAPI.ShowConsoleWindow(true);
                _client.Connect(SERVER, PORT);
                Thread updateThread = new Thread(new ThreadStart(UpdateColor));
                updateThread.Start();

                Thread.Sleep(1000);
                WinAPI.ShowConsoleWindow(false);
            }
        }

        private void UpdateColor()
        {
            while (_client.IsConnected())
            {
                uint color = _client.RequestCurrentColor();
                // -----
                // We need to bit-shift the color components,
                // as the Razer ChromaSDK uses the COLORREF from Gdi32.dll
                // which uses a different arrangement for the color components.
                //
                //LEDController:
                // 0xff000000 = red
                // 0x00ff0000 = green
                // 0x0000ff00 = blue
                //
                //GDI:
                // 0x000000ff = red
                // 0x0000ff00 = green
                // 0x00ff0000 = blue
                // -----
                byte r = (byte)(color >> 24);
                byte g = (byte)(color >> 16);
                byte b = (byte)(color >> 8);

                ulong gdiColor = (ulong) r | (ulong) g << 8 | (ulong) b << 16;

                ChromaSDK.setKeyboardColor(gdiColor);
                ChromaSDK.setMouseColor(gdiColor);
                Thread.Sleep(40);
            }
        }

        private void Shutdown()
        {
            WinAPI.ShowConsoleWindow(true);
            Console.WriteLine("\n\n--- Shutting down --- ");
            
            _client.Disconnect();
            ChromaSDK.unitialize();
            Application.Exit();

            Console.WriteLine("Shutdown procedure finished, terminating.");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            ChromaSDK.initialize();
            new Program();
        }
    }
}
