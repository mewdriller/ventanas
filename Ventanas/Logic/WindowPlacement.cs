using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using ManagedWinapi;

namespace Base2io.Ventanas.Logic
{
    public class WindowPlacement : IDisposable
    {
        #region Private Variables

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int width, int height, uint flags);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_MAXIMIZE = 3;
        private const int SW_MINIMIZE = 6;
        private const int SW_RESTORE = 9;

        private Hotkeys _hotkeys;

        #endregion

        #region Public Logic

        public void RegisterNumberPadHotkeys()
        {
            _hotkeys = Hotkeys.Instance;
            _hotkeys.RegisterCtrlAltHotkey(Keys.NumPad1, NumPad1_EventHandler);
            _hotkeys.RegisterCtrlAltHotkey(Keys.NumPad2, NumPad2_EventHandler);
            _hotkeys.RegisterCtrlAltHotkey(Keys.NumPad3, NumPad3_EventHandler);
            _hotkeys.RegisterCtrlAltHotkey(Keys.NumPad4, NumPad4_EventHandler);
            _hotkeys.RegisterCtrlAltHotkey(Keys.NumPad5, NumPad5_EventHandler);
            _hotkeys.RegisterCtrlAltHotkey(Keys.NumPad6, NumPad6_EventHandler);
            _hotkeys.RegisterCtrlAltHotkey(Keys.NumPad7, NumPad7_EventHandler);
            _hotkeys.RegisterCtrlAltHotkey(Keys.NumPad8, NumPad8_EventHandler);
            _hotkeys.RegisterCtrlAltHotkey(Keys.NumPad9, NumPad9_EventHandler);
            _hotkeys.RegisterCtrlAltHotkey(Keys.NumPad0, NumPad0_EventHandler);
        }

        public static void PositionActiveWindowByRatio(DockStyle position, float sizePercentage)
        {
            // Bound the size in [0, 1].
            sizePercentage = Math.Max(0, Math.Min(1, sizePercentage));

            // Get the handle for the foreground window.
            IntPtr handle = GetForegroundWindow();

            // If the intent is to maximize: use the native command.
            if (sizePercentage == 1)
            {
                ShowWindow(handle, SW_MAXIMIZE);
            }
            // If the intent is to minimize: use the native command.
            else if (sizePercentage == 0)
            {
                ShowWindow(handle, SW_MINIMIZE);
            }
            // Otherwise, we're looking to resize the window:
            else
            {
                // Restore it to snap out of minimized/maximized states.
                ShowWindow(handle, SW_RESTORE);

                int targetX = 0, targetY = 0, targetWidth = 0, targetHeight = 0;

                // Get the working area of the screen:
                Rect area = SystemParameters.WorkArea;
                int fullWidth = (int)area.Width;
                int fullHeight = (int)area.Height;

                switch (position)
                {
                    case DockStyle.Left:
                        targetHeight = fullHeight;
                        targetWidth = (int)(fullWidth * sizePercentage);
                        targetX = 0;
                        targetY = 0;
                        break;
                    case DockStyle.Right:
                        targetHeight = fullHeight;
                        targetWidth = (int)(fullWidth * sizePercentage);
                        targetX = fullWidth - targetWidth;
                        targetY = 0;
                        break;
                    case DockStyle.Top:
                        targetHeight = (int)(fullHeight * sizePercentage);
                        targetWidth = fullWidth;
                        targetX = 0;
                        targetY = 0;
                        break;
                    case DockStyle.Bottom:
                        targetHeight = (int)(fullHeight * sizePercentage);
                        targetWidth = fullWidth;
                        targetX = 0;
                        targetY = fullHeight - targetHeight;
                        break;
                    case DockStyle.Fill:
                    case DockStyle.None:
                        targetHeight = (int)(fullHeight * sizePercentage);
                        targetWidth = (int)(fullWidth * sizePercentage);
                        targetX = (fullWidth - targetWidth) / 2;
                        targetY = (fullHeight - targetHeight) / 2;
                        break;
                }

                // Adjust the size and position of the window.
                SetWindowPos(handle, IntPtr.Zero, targetX, targetY, targetWidth, targetHeight, 0);
            }
        }

        #endregion

        #region Window Placement Event Handlers

        private void NumPad1_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Left, (float)1 / 3);
        }

        private void NumPad2_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Bottom, (float)1 / 2);
        }

        private void NumPad3_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Right, (float)1 / 3);
        }

        private void NumPad4_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Left, (float)1 / 2);
        }

        private void NumPad5_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Fill, (float)2 / 3);
        }

        private void NumPad6_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Right, (float)1 / 2);
        }

        private void NumPad7_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Left, (float)2 / 3);
        }

        private void NumPad8_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Top, (float)1 / 2);
        }

        private void NumPad9_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Right, (float)2 / 3);
        }

        private void NumPad0_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Fill, (float)1);
        }

        #endregion
        
        #region IDisposable Implementation

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_hotkeys != null)
                    {
                        _hotkeys.Dispose();
                    }
                }

                _hotkeys = null;
                _disposed = true;
            }
        }

        #endregion
    }
}
