using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    interface IMouseInputHandler
    {
        void OnMouseDoubleClick(System.Windows.Forms.MouseEventArgs e);
        void OnMouseClick(System.Windows.Forms.MouseEventArgs e);
        bool OnMouseHover(System.Windows.Forms.MouseEventArgs e);
    }
}
