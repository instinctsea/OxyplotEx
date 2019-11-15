using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
    public class ViewChangedEventArgs : EventArgs
    {
        public ViewChangedEventArgs()
        {
        }
    }
    public delegate void ViewChangedEventHandler(object sender, ViewChangedEventArgs e);
}
