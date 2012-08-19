using System;
using System.Collections.Generic;
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
        public Preferences()
        {
            InitializeComponent();
        }

        public List<Hotkey> RegisteredHotkeys
        {
            get
            {
                return Hotkeys.Instance.RegisteredHotkeys;
            }
        }
    }
}
