using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace OxyplotEx.Service
{
   public class FontsManager
    {
        public static Font SurfaceFont
        {
            get;
            private set;
        }
        public static Font WindFont
        {
            get;
            private set;
        }

      
        public static System.Drawing.Text.PrivateFontCollection Fonts
        {
            get;
            private set;
        }
        static bool _load = false;
        public static  void LoadFonts()
        {
            if (_load)
                return;

            _load = true;
            Fonts = new System.Drawing.Text.PrivateFontCollection();
        }
    }
}
