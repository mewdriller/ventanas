using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Text;
using ManagedWinapi;
using Hardcodet.Wpf.TaskbarNotification;

namespace Ventanas
{
    // <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width, int height, uint flags);

        private List<Hotkey> hotkeys;

        protected override void OnStartup(StartupEventArgs e)
        {
            TaskbarIcon tbi = (TaskbarIcon)FindResource("TrayIcon");

            hotkeys = new List<Hotkey>(9);

            // Configure all the hotkeys:
            registerHotkey(System.Windows.Forms.Keys.NumPad1);
            registerHotkey(System.Windows.Forms.Keys.NumPad2);
            registerHotkey(System.Windows.Forms.Keys.NumPad3);
            registerHotkey(System.Windows.Forms.Keys.NumPad4);
            registerHotkey(System.Windows.Forms.Keys.NumPad5);
            registerHotkey(System.Windows.Forms.Keys.NumPad6);
            registerHotkey(System.Windows.Forms.Keys.NumPad7);
            registerHotkey(System.Windows.Forms.Keys.NumPad8);
            registerHotkey(System.Windows.Forms.Keys.NumPad9);

            base.OnStartup(e);
        }

        private void registerHotkey(System.Windows.Forms.Keys keyCode)
        {
            Hotkey hotkey = new Hotkey();
            hotkey.Ctrl = true;
            hotkey.Alt = true;
            hotkey.KeyCode = keyCode;
            hotkey.HotkeyPressed += new EventHandler(hotkey_HotkeyPressedHandler);
            try
            {
                hotkey.Enabled = true;
            }
            catch (ManagedWinapi.HotkeyAlreadyInUseException)
            {
                System.Windows.MessageBox.Show("Could not register hotkey, it is already in use.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                hotkeys.Add(hotkey);
            }
        }

        private void hotkey_HotkeyPressedHandler(object sender, EventArgs e)
        {
            ManagedWinapi.Hotkey hotkey = sender as ManagedWinapi.Hotkey;

            // Get the handle for the foreground window.
            IntPtr handle = GetForegroundWindow();

            // Get the working area of the screen:
            Rect area = System.Windows.SystemParameters.WorkArea;
            int fullWidth = (int)area.Width;
            int fullHeight = (int)area.Height;

            // Determine what dimmensions to change to based on the triggering hotkey:
            int targetX = 0, targetY = 0, targetWidth = 0, targetHeight = 0;

            switch (hotkey.KeyCode)
            {
                // Left-side sizes:

                case System.Windows.Forms.Keys.NumPad1:
                    targetWidth = fullWidth / 3;
                    targetHeight = fullHeight;
                    break;

                case System.Windows.Forms.Keys.NumPad4:
                    targetWidth = fullWidth / 2;
                    targetHeight = fullHeight;
                    break;

                case System.Windows.Forms.Keys.NumPad7:
                    targetWidth = 2 * fullWidth / 3;
                    targetHeight = fullHeight;
                    break;

                // Right-side sizes:

                case System.Windows.Forms.Keys.NumPad3:
                    targetX = 2 * fullWidth / 3;
                    targetWidth = fullWidth / 3;
                    targetHeight = fullHeight;
                    break;

                case System.Windows.Forms.Keys.NumPad6:
                    targetX = fullWidth / 2;
                    targetWidth = targetX;
                    targetHeight = fullHeight;
                    break;

                case System.Windows.Forms.Keys.NumPad9:
                    targetX = fullWidth / 3;
                    targetWidth = 2 * fullWidth / 3;
                    targetHeight = fullHeight;
                    break;

                // Vertical sizes:

                case System.Windows.Forms.Keys.NumPad2:
                    targetY = fullHeight / 2;
                    targetWidth = fullWidth;
                    targetHeight = fullHeight / 2;
                    break;

                case System.Windows.Forms.Keys.NumPad8:
                    targetWidth = fullWidth;
                    targetHeight = fullHeight / 2;
                    break;

                // Centering:

                case System.Windows.Forms.Keys.NumPad5:
                    targetX = fullWidth / 6;
                    targetY = fullHeight / 6;
                    targetWidth = 2 * fullWidth / 3;
                    targetHeight = 2 * fullHeight / 3;
                    break;
            }

            // Adjust the size and position of the window.
            SetWindowPos(handle, IntPtr.Zero, targetX, targetY, targetWidth, targetHeight, 0);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Clean up all the hotkeys:
            foreach (Hotkey hotkey in hotkeys)
            {
                hotkey.Dispose();
            }

            base.OnExit(e);
        }
    }
}
