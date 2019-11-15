using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OxyplotEx.GMap
{
    public class GroupVisibleChangedEventArgs : EventArgs
    {
        public GroupVisibleChangedEventArgs(string id, bool visible)
        {
            ID = id;
            Visible = visible;
        }

        public string ID { get; private set; }
        public bool Visible { get; private set; }
    }

    public class SeriesVisibleChangedEventArgs : EventArgs
    {
        public SeriesVisibleChangedEventArgs(string id, bool visible)
        {
            ID = id;
            Visible = visible;
        }

        public string ID { get; private set; }
        public bool Visible { get; private set; }
    }

    public class LegendClickEventArgs : EventArgs
    {
        public LegendClickEventArgs(ISeries series,MouseButtons mouseButton,int x,int y,int clicks)
        {
            this.Series = series;
            this.MouseButton = mouseButton;
            this.X = x;
            this.Y = y;
            this.Clicks = clicks;
        }
        public ISeries Series { get; private set; }
        public MouseButtons MouseButton { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Clicks { get; private set; }
    }

    public class SelectedLocationEventArgs : EventArgs
    {
        public SelectedLocationEventArgs(IEnumerable<Tuple<double, double>> locations)
        {
            Locations = locations;
        }

        public IEnumerable<Tuple<double, double>> Locations
        {
            get; private set;
        }
    }

    public class DeleteLayerEventArgs : EventArgs
    {
        public DeleteLayerEventArgs(string id)
        {
            this.ID = id;
        }

        public string ID
        {
            get; private set;
        }
    }

    public class SelectedColorChangedEventArgs : EventArgs
    {
        public SelectedColorChangedEventArgs(Color color)
        {
            this.SelectedColor = color;
        }

        public Color SelectedColor
        {
            get;
            private set;
        }
    }
}
