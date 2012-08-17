using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Ventanas
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
    }
}
