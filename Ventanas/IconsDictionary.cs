using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Hardcodet.Wpf.TaskbarNotification;

namespace Base2io.Ventanas
{
    partial class IconsDictionary : ResourceDictionary
    {
        public IconsDictionary() 
        {
            InitializeComponent();
        }

        private void OnExitClicked(object sender, RoutedEventArgs e)
        {
            // If we have a legit reference to the menu-item:
            MenuItem item = e.OriginalSource as MenuItem;
            if (item != null)
            {
                // Shutdown the application.
                Application.Current.Shutdown();
            }
        }

        private void OnPreferencesClicked(object sender, RoutedEventArgs e)
        {
            Window preferencesWindow = new Preferences();
            preferencesWindow.Show();
        }
    }
}
