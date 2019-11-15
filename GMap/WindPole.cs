using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OxyplotEx.GMap
{
    class WindPole
    {
        public WindPole()
        {

        }
        /// <summary>
        /// 获取风向杆点数组
        /// </summary>
        /// <param name="position">杆位置</param>
        /// <param name="fSpeed">风速</param>
        /// <param name="ang">风向</param>
        /// <param name="factor">比例系数>0</param>
        /// <param name="type">类型0,1</param>
        /// <returns></returns>
        public List<PointF[]> GetWindPolePolygons(PointF position, float fSpeed, float ang, float factor, int type)
        {
            List<PointF[]> polygons = new List<PointF[]>();

            ang = 360.0f - ang + 90.0f;
            if (ang >= 360)
                ang = ang - 360.0f;
            ang = ang * 0.0174532925F;//转化为弧度

            int i;

            int a, b, c, d;
            int xs, ys;

            fSpeed += 1.0f;
            double dir;

            a = (int)(fSpeed / 50);
            a = 0;
            b = ((int)fSpeed - a * 50) / 20;//旗子个数
            c = ((int)fSpeed - a * 50 - b * 20) / 4;//4个数
            d = ((int)fSpeed - a * 50 - b * 20 - 4 * c) / 2;//2个数

            //转化为顺时针
            dir = -ang; //* DEG_TO_RAD;
            xs = (int)(position.X);
            ys = (int)(position.Y);

            PointF pt, pt1, pt2;
            pt = new PointF();
            pt1 = new PointF();
            pt2 = new PointF();

            pt.X = 14.0f;
            pt.Y = 0.0f;
            List<PointF> wind_pts = new List<PointF>();
            if (fSpeed > 0.0)
            {
                RotateCoord(pt, out pt1, position, dir, factor);
                polygons.Add(new PointF[] { new PointF(xs, ys), new PointF((int)pt1.X, (int)pt1.Y) });

            }
            if (a > 0)
            {
                for (i = 0; i < a; i++)
                {
                    wind_pts.Clear();
                    pt.X = pt.X - 3.0f;
                    PointF[] pxy = new PointF[3];
                    FindCoord(position, pt, pxy, dir, factor, type);
                    wind_pts.AddRange(pxy);
                    wind_pts.Add(pxy[0]);
                    polygons.Add(wind_pts.ToArray());
                }
            }
            if (b > 0)
            {
                for (i = 0; i < b; i++)
                {
                    wind_pts.Clear();
                    pt.X = pt.X - 3.0f;
                    PointF[] pxy = new PointF[3];
                    FindCoord(position, pt, pxy, dir, factor, type);
                    wind_pts.AddRange(pxy);
                    wind_pts.Add(pxy[0]);
                    polygons.Add(wind_pts.ToArray());
                }
            }
            if (c > 0)
            {
                if ((a > 0) || (b > 0))
                    pt.X = pt.X - 2.0f;
                for (i = 0; i < c; i++)
                {
                    pt1.X = pt.X + 4.0f;
                    if (type == 1)
                        pt1.Y = pt.Y - 6.0f;
                    else
                        pt1.Y = pt.Y + 6.0f;

                    RotateCoord(pt1, out pt2, position, dir, factor);
                    RotateCoord(pt, out pt1, position, dir, factor);
                    polygons.Add(new PointF[] { new PointF(pt1.X, pt1.Y), new PointF(pt2.X, pt2.Y) });
                    pt.X = pt.X - 2.0f;
                }
            }
            if (d > 0)
            {
                if (((a > 0) || (b > 0)) && (c == 0))
                    pt.X = pt.X - 2.0f;
                if (type == 1)
                    pt1.Y = pt.Y - 3.0f;
                else
                    pt1.Y = pt.Y + 3.0f;
                pt1.X = pt.X + 2.0f;
                RotateCoord(pt1, out pt2, position, dir, factor);
                RotateCoord(pt, out pt1, position, dir, factor);
                polygons.Add(new PointF[] { new PointF(pt1.X, pt1.Y), new PointF(pt2.X, pt2.Y) });
            }

            return polygons;
        }
        private void RotateCoord(PointF pt, out PointF ptr, PointF base0, double ang, float factor)
        {
            PointF pt2 = new PointF();
            pt2.X = pt.X * factor;
            pt2.Y = pt.Y * factor;
            ptr = new PointF();
            ptr.X = base0.X + (float)(pt2.X * Math.Cos(ang) - pt2.Y * Math.Sin(ang));
            ptr.Y = base0.Y + (float)(pt2.X * Math.Sin(ang) + pt2.Y * Math.Cos(ang));
        }
        private void DrawWindspeed(Graphics canvas, PointF[] pt, int style_w, Color color, float lWidth)
        {
            PointF[] ptx = new PointF[4];
            int i;


            Pen pen = new Pen(color, lWidth);

            for (i = 0; i < 3; i++)
            {
                ptx[i] = pt[i];
            }
            ptx[3] = ptx[0];

            if (style_w == 0)
            {
                canvas.DrawPolygon(pen, ptx);
            }
            else
                canvas.FillPolygon(new SolidBrush(color), ptx);

        }
        private void FindCoord(PointF base0, PointF first, PointF[] ptxy, double ang, float factor, int type)
        {
            int i;
            PointF pts = new PointF();
            PointF[] pt = new PointF[3];

            pt[0].X = 0.0f;
            pt[0].Y = 0.0f;

            pt[1].X = 3.0f;
            pt[1].Y = 0.0f;

            pt[2].X = 5.0f;
            if (type == 0)
                pt[2].Y = 6.5f;
            else
                pt[2].Y = -6.5f;


            for (i = 0; i < 3; i++)
            {
                pts.X = pt[i].X + first.X;
                pts.Y = pt[i].Y + first.Y;
                ptxy[i].X = base0.X + (float)((pts.X * Math.Cos(ang) - pts.Y * Math.Sin(ang)) * factor);
                ptxy[i].Y = base0.Y + (float)((pts.X * Math.Sin(ang) + pts.Y * Math.Cos(ang)) * factor);
            }

        }
    }
}
