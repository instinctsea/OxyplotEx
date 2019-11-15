using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    static class Pan
    {
        public static ScreenPoint PanPoint(ScreenPoint source, ScreenVector vector)
        {
            return new ScreenPoint(source.X+vector.X,source.Y+vector.Y);
        }

        public static OxyRect PanRect(OxyRect rect, ScreenVector vector)
        {
            return new OxyRect(rect.Left+vector.X,rect.Top+vector.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// 返回平移向量
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="bound"></param>
        /// <returns></returns>
        public static ScreenVector AjustBound(OxyRect rect, OxyRect bound)
        {
            int padding = 5;
            bool is_top_outside = rect.Top <= bound.Top;
            bool is_right_outside = rect.Right >= bound.Right;
            bool is_left_outside = rect.Left <= bound.Left;
            bool is_bottom_outside = rect.Bottom >= bound.Bottom;
            bool is_right_outleft = rect.Right < bound.Left;
            bool is_left_outright = rect.Left > bound.Right;
            bool is_top_outbottom = rect.Top > bound.Bottom;
            bool is_bottom_outtop = rect.Bottom < bound.Top;
            if ((is_top_outside && is_bottom_outside) ||
                (is_left_outside && is_right_outside)||is_left_outright||is_right_outleft||is_top_outbottom||is_bottom_outtop)
                return new ScreenVector(0, 0);

            double x = 0, y = 0;
            if (is_top_outside)
                y += bound.Top - rect.Top + padding;
            if (is_bottom_outside)
                y -= rect.Bottom - bound.Bottom + padding;
            if (is_left_outside)
                x += bound.Left - rect.Left + padding;
            if (is_right_outside)
                x -= rect.Right - bound.Right + padding;

            return new ScreenVector(x, y); 
        }

        public static bool ContainsRect(OxyRect origin, OxyRect target)
        {
            return target.Left > origin.Left && target.Right < origin.Right &&
                target.Top > origin.Top && target.Bottom < origin.Bottom;
        }
    }
}
