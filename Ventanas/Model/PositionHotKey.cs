using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Base2io.Ventanas.Enums;

namespace Base2io.Ventanas.Model
{
    public class PositionHotkey
    {
        public WindowPosition WindowPosition { get; set; }
        public Keys KeyCode { get; set; }
        public bool IsCtrlKeyUsed { get; set; }
        public bool IsAltKeyUsed { get; set; }
        public bool IsShiftKeyUsed { get; set; }
        public bool IsWinKeyUsed { get; set; }
    }
}
