using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RzChromaLEDs
{
    class TrayIcon
    {
        private NotifyIcon _trayIcon;

        public TrayIcon(string appName, List<(string, EventHandler)> menuItems)
        {
            _trayIcon = new NotifyIcon();
            _trayIcon.Text = appName;

            //Load icon (uses executable icon)
            Icon icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            _trayIcon.Icon = icon;

            //Create context menu
            ContextMenu trayMenu = new ContextMenu();
            foreach (var menuItem in menuItems)
                trayMenu.MenuItems.Add(menuItem.Item1, menuItem.Item2);

            _trayIcon.ContextMenu = trayMenu;
            Show(true);

            Application.Run(); // This is needed for the tray-menu items to show up
        }

        public void Show(bool showing) => _trayIcon.Visible = showing;

        private void ContextMenu_ConnectToServer(object sender, EventArgs e)
        {

        }
    }
}
