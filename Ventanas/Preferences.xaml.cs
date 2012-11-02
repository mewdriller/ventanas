using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Base2io.Ventanas.Logic;
using ManagedWinapi;

namespace Base2io.Ventanas
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences : Window
    {
        #region Constructor

        public Preferences()
        {
            CustomizedHotkeys = new List<Hotkey>(Hotkeys.Instance.RegisteredHotkeys);
            InitializeComponent();
        } 

        #endregion

        #region Properties

        public List<Hotkey> CustomizedHotkeys { get; set; }

        #endregion

        #region Event Handlers

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            // TODO: Apply new hotkeys.
        }

        #endregion
    }
}
