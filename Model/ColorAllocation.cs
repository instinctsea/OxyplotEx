using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.Model
{
    class ColorAllocation
    {
        Color _color;
        private int _step_r = 40;
        private int _step_g = 40;
        private int _step_b = 40;
        private int _a;
        private int _r;
        private int _g;
        private int _b;
        public ColorAllocation(Color color)
        {
            _color = color;
            _a = _color.A;
            _r = _color.R;
            _g = _color.G;
            _b = _color.B;
   
        }

        public Color AllocateColor()
        {
            _r = GetNext(_r,ref _step_r);
            _g = GetNext(_g,ref _step_g);
            _b = GetNext(_b,ref _step_b);

            return Color.FromArgb(_a, _r, _g, _b);
        }

        int GetNext(int num,ref int step)
        {
            if (num + step <= 0 || num + step >= 255)
            {
                step = -step;
            }

            return num + step;
        }
    }
}
