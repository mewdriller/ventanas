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
using Base2io.Ventanas.Logic;
using System.Windows.Forms;

namespace Base2io.Ventanas
{
    // <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        WindowPlacement _windowPlacement;
        TaskbarIcon _tbi;

        protected override void OnStartup(StartupEventArgs e)
        {
            _tbi = (TaskbarIcon)FindResource("TrayIcon");

            _windowPlacement = new WindowPlacement();
            _windowPlacement.RegisterNumberPadHotkeys();

            base.OnStartup(e);
        }



        protected override void OnExit(ExitEventArgs e)
        {
            _windowPlacement.Dispose();
            _windowPlacement = null;

            _tbi.Dispose();
            _tbi = null;

            base.OnExit(e);
        }
    }
}
