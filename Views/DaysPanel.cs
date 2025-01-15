﻿using System.Drawing.Drawing2D;

namespace WeatherApp.Views
{
    internal class DaysPanel : TableLayoutPanel
    {
        public DaysPanel()
        {
            DoubleBuffered = true;
            Size = new Size(260, 190);
            Location = new Point(20, 275);
            //BackColor = ColorTranslator.FromHtml("#3D5A6C");
            BackColor = Color.FromArgb(128, 69, 90, 108);
            Padding = new Padding(5);
            ColumnCount = 1;
            RowCount = 7;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRoundedCorners();
        }

        private void UpdateRoundedCorners()
        {
            GraphicsPath path = new GraphicsPath();
            int radius = 20;
            Rectangle rect = new Rectangle(0, 0, Width, Height);

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();

            Region = new Region(path);
        }
    }
}
