using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using Base2io.Ventanas.Enums;
using Base2io.Ventanas.Model;

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

        private Hotkeys _hotkeyService;

        #endregion

        #region Constructors

        private WindowPlacement()
        {
        }

        private static WindowPlacement _instance;
        public static WindowPlacement Instance
        {
            get { return _instance ?? (_instance = new WindowPlacement()); }
        }

        #endregion

        #region Public Logic

        public void RegisterHotkeys()
        {
            _hotkeyService = Hotkeys.Instance;
            SetHotkeys();
        }

        public static void PositionActiveWindowByRatio(DockStyle position, float sizePercentage)
        {
            // Bound the size in [0, 1].
            sizePercentage = Math.Max(0, Math.Min(1, sizePercentage));

            // Get the handle for the foreground window.
            IntPtr handle = GetForegroundWindow();

            // If the intent is to maximize: use the native command.
            if (Math.Abs(sizePercentage - 1) < float.Epsilon)
            {
                ShowWindow(handle, SW_MAXIMIZE);
            }
            // If the intent is to minimize: use the native command.
            else if (Math.Abs(sizePercentage - 0) < float.Epsilon)
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

        #region Private Logic

        private void SetHotkeys()
        {
            // TODO: Set based on stored user preferences.
            
            IEnumerable<PositionHotKey> hotKeys = GetDefaultHotKeys();

            foreach (PositionHotKey hotKey in hotKeys)
            {
                _hotkeyService.RegisterHotkey(hotKey.KeyCode,
                                              GetWindowPositionEventHandler(hotKey.WindowPosition),
                                              hotKey.IsCtrlKeyUsed,
                                              hotKey.IsAltKeyUsed,
                                              hotKey.IsShiftKeyUsed,
                                              hotKey.IsWinKeyUsed);
            }
        }

        private IEnumerable<PositionHotKey> GetDefaultHotKeys()
        {
            return new List<PositionHotKey>
                       {
                           new PositionHotKey
                               {
                                   WindowPosition = WindowPosition.LeftOneThird,
                                   KeyCode = Keys.NumPad1,
                                   IsCtrlKeyUsed = true,
                                   IsAltKeyUsed = true
                               },
                           new PositionHotKey
                               {
                                   WindowPosition = WindowPosition.LeftHalf,
                                   KeyCode = Keys.NumPad4,
                                   IsCtrlKeyUsed = true,
                                   IsAltKeyUsed = true
                               },
                           new PositionHotKey
                               {
                                   WindowPosition = WindowPosition.LeftTwoThirds,
                                   KeyCode = Keys.NumPad7,
                                   IsCtrlKeyUsed = true,
                                   IsAltKeyUsed = true
                               },
                           new PositionHotKey
                               {
                                   WindowPosition = WindowPosition.RightOneThird,
                                   KeyCode = Keys.NumPad3,
                                   IsCtrlKeyUsed = true,
                                   IsAltKeyUsed = true
                               },
                           new PositionHotKey
                               {
                                   WindowPosition = WindowPosition.RightHalf,
                                   KeyCode = Keys.NumPad6,
                                   IsCtrlKeyUsed = true,
                                   IsAltKeyUsed = true
                               },
                           new PositionHotKey
                               {
                                   WindowPosition = WindowPosition.RightTwoThirds,
                                   KeyCode = Keys.NumPad9,
                                   IsCtrlKeyUsed = true,
                                   IsAltKeyUsed = true
                               },
                           new PositionHotKey
                               {
                                   WindowPosition = WindowPosition.TopHalf,
                                   KeyCode = Keys.NumPad8,
                                   IsCtrlKeyUsed = true,
                                   IsAltKeyUsed = true
                               },
                           new PositionHotKey
                               {
                                   WindowPosition = WindowPosition.BottomHalf,
                                   KeyCode = Keys.NumPad2,
                                   IsCtrlKeyUsed = true,
                                   IsAltKeyUsed = true
                               },
                           new PositionHotKey
                               {
                                   WindowPosition = WindowPosition.Center,
                                   KeyCode = Keys.NumPad5,
                                   IsCtrlKeyUsed = true,
                                   IsAltKeyUsed = true
                               },
                           new PositionHotKey
                               {
                                   WindowPosition = WindowPosition.Fill,
                                   KeyCode = Keys.NumPad0,
                                   IsCtrlKeyUsed = true,
                                   IsAltKeyUsed = true
                               }
                       };
        }

        private EventHandler GetWindowPositionEventHandler(WindowPosition position)
        {
            switch (position)
            {
                case WindowPosition.LeftOneThird:
                    return LeftThird_EventHandler;
                case WindowPosition.LeftHalf:
                    return LeftHalf_EventHandler;
                case WindowPosition.LeftTwoThirds:
                    return LeftTwoThirds_EventHandler;

                case WindowPosition.RightOneThird:
                    return RightThird_EventHandler;
                case WindowPosition.RightHalf:
                    return RightHalf_EventHandler;
                case WindowPosition.RightTwoThirds:
                    return RightTwoThirds_EventHandler;

                case WindowPosition.TopHalf:
                    return TopHalf_EventHandler;
                case WindowPosition.BottomHalf:
                    return BottomHalf_EventHandler;

                case WindowPosition.Center:
                    return Center_EventHandler;
                case WindowPosition.Fill:
                    return Fill_EventHandler;

                default:
                    return Fill_EventHandler;
            }
        }

        #region Window Placement Event Handlers

        private void LeftThird_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Left, (float)1 / 3);
        }

        private void BottomHalf_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Bottom, (float)1 / 2);
        }

        private void RightThird_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Right, (float)1 / 3);
        }

        private void LeftHalf_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Left, (float)1 / 2);
        }

        private void Center_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Fill, (float)2 / 3);
        }

        private void RightHalf_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Right, (float)1 / 2);
        }

        private void LeftTwoThirds_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Left, (float)2 / 3);
        }

        private void TopHalf_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Top, (float)1 / 2);
        }

        private void RightTwoThirds_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Right, (float)2 / 3);
        }

        private void Fill_EventHandler(object sender, EventArgs e)
        {
            PositionActiveWindowByRatio(DockStyle.Fill, 1);
        }

        #endregion 

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
                    if (_hotkeyService != null)
                    {
                        _hotkeyService.Dispose();
                    }
                }

                _hotkeyService = null;
                _disposed = true;
            }
        }

        #endregion
    }
}
