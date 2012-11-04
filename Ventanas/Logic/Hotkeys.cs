using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using ManagedWinapi;

namespace Base2io.Ventanas.Logic
{
    public class Hotkeys : IDisposable
    {
        #region Constructor

        private Hotkeys()
        {
            _registeredHotkeys = new List<Hotkey>();
        }

        private static Hotkeys _instance;
        public static Hotkeys Instance
        {
            get { return _instance ?? (_instance = new Hotkeys()); }
        }

        #endregion

        #region Properties

        List<Hotkey> _registeredHotkeys;
        /// <summary>
        /// List of currently registered hot keys
        /// </summary>
        public List<Hotkey> RegisteredHotkeys { get { return new List<Hotkey>(_registeredHotkeys); } }

        #endregion

        #region Public Logic

        public static string HotkeyName(Hotkey hotkey)
        {
            return (hotkey.Ctrl ? "ctrl+" : "")
                        + (hotkey.Alt ? "alt+" : "")
                        + (hotkey.Shift ? "shift+" : "")
                        + (hotkey.WindowsKey ? "win+" : "")
                        + hotkey.KeyCode;
        }

        /// <summary>
        /// Register the hotkey specified in the parameters.
        /// </summary>
        /// <param name="keyCode">The key to register</param>
        /// <param name="hotkeyHandler">The event handler to call when the hotkey occurs</param>
        /// <param name="isCtrlKeyUsed">True if the Control key modifier should be used</param>
        /// <param name="isAltKeyUsed">True if the Alt key modifier should be used</param>
        /// <param name="isShiftKeyUsed">True if the Shift key modifier should be used</param>
        /// <param name="isWinKeyUsed">True if the Windows key modifier should be used</param>
        /// <returns>True if the hotkey registration succeeds</returns>
        public bool RegisterHotkey(Keys keyCode, EventHandler hotkeyHandler, bool isCtrlKeyUsed = false, bool isAltKeyUsed = false, bool isShiftKeyUsed = false, bool isWinKeyUsed = false)
        {
            bool isSuccessful = false;

            if (hotkeyHandler != null)
            {
                Hotkey hotkey = new Hotkey();
                hotkey.KeyCode = keyCode;
                hotkey.Ctrl = isCtrlKeyUsed;
                hotkey.Alt = isAltKeyUsed;
                hotkey.Shift = isShiftKeyUsed;
                hotkey.WindowsKey = isWinKeyUsed;
                hotkey.HotkeyPressed += hotkeyHandler;                

                try
                {
                    hotkey.Enabled = true;
                    _registeredHotkeys.Add(hotkey);
                    isSuccessful = true;
                }
                catch (HotkeyAlreadyInUseException)
                {
                    System.Windows.MessageBox.Show("The following hotkey is already in use: " + HotkeyName(hotkey), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch
                {
                    System.Windows.MessageBox.Show("Failed to register the following hotkey: " + HotkeyName(hotkey), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return isSuccessful;
        }

        /// <summary>
        /// Registers a hotkey with the Ctrl and Alt key modifiers
        /// </summary>
        /// <param name="keyCode">The key to assocation with Ctrl+Alt</param>
        /// <param name="hotkeyHandler">The event handler to call when the hotkey occurs</param>
        /// <returns>True if the hotkey registration succeeds</returns>
        public bool RegisterCtrlAltHotkey(Keys keyCode, EventHandler hotkeyHandler)
        {
            return RegisterHotkey(keyCode, hotkeyHandler, true, true);
        }

        public void ClearRegisteredHotkeys()
        {
            DisposeRegisteredHotkeys();
            _registeredHotkeys = new List<Hotkey>();
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
                    DisposeRegisteredHotkeys();
                }

                _registeredHotkeys = null;
                _disposed = true;
            }
        }

        private void DisposeRegisteredHotkeys()
        {
            if (_registeredHotkeys != null)
            {
                foreach (Hotkey hotkey in _registeredHotkeys)
                {
                    hotkey.Dispose();
                }
            }
        }

        #endregion
    }
}
