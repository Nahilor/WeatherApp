using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Views
{
    internal class Picture : PictureBox
    {
            Bitmap? icon;
            public Picture()
            {
                DoubleBuffered = true;
            }
            public void setImage(String fileDestination, int xSize, int ySize)
            {
                if (icon != null)
                {
                    icon.Dispose();
                }

                SizeMode = PictureBoxSizeMode.StretchImage;
                icon = new Bitmap(fileDestination);
                ClientSize = new Size(xSize, ySize);
                Image = (Image)icon;
            }

            public void setImage(String fileDestination)
            {
                if (icon != null)
                {
                    icon.Dispose();
                }

                SizeMode = PictureBoxSizeMode.AutoSize;
                icon = new Bitmap(fileDestination);
                Image = (Image)icon;
            }

            public Picture CreateIcon(int xSize, int ySize, String fileDestination, Point point)
            {
                Picture icon = new Picture();
                icon.Location = point;
                icon.setImage(fileDestination, xSize, ySize);
                return icon;
            }
        }
}