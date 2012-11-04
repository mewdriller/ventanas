using System.Collections.Generic;
using System.Windows;
using Base2io.Ventanas.Logic;
using Base2io.Ventanas.Model;

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
            CustomizedHotkeys = new List<PositionHotkey>(WindowPlacement.Instance.PositionHotkeys);
            InitializeComponent();
        } 

        #endregion

        #region Properties

        public List<PositionHotkey> CustomizedHotkeys { get; set; }

        #endregion

        #region Event Handlers

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            WindowPlacement.Instance.RegisterHotKeys(CustomizedHotkeys);
            Close();
        }

        #endregion
    }
}
