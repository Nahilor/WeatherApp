namespace WeatherApp.Views
{
    internal class Functions
    {
        internal static int[] ManageDays()
        {
            int[] tabledayscontent = new int[7];
            tabledayscontent[0] = WeatherGui.currentday;
            for (int i = 0; i < 7; i++)
            {
                if (WeatherGui.currentday + i < 7)
                    tabledayscontent[i] = WeatherGui.currentday + i;
                else
                {
                    tabledayscontent[i] = WeatherGui.currentday + i - 7;
                }
            }
            return tabledayscontent;
        }

        // Temperature conversion methods
        internal static int FtoC(int F)
        {
            return Convert.ToInt32(0.556 * (F - 32));
        }
        internal static int CtoF(int C)
        {
            return Convert.ToInt32((1.8 * C) + 32);
        }

        // Render Controller
        internal static void ToggleRendering()
        {
            WeatherGui.isRenderingPaused = !WeatherGui.isRenderingPaused;
        }

        // Method to Create Panels with Common Functionality
        public static Panel CreateInfoPanel(string label, string value, string infoMessage, Point location)
        {
            Panel panel = new Panel
            {
                Location = location,
                BackColor = Color.Transparent,
                Size = new Size(100, 35),
                Padding = new Padding(5)
            };

            Label titleLabel = new Label
            {
                Text = label,
                Location = new Point(0, 0),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };

            Label valueLabel = new Label
            {
                Text = value,
                Location = new Point(0, 20),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };

            PictureBox infoIcon = new PictureBox
            {
                Size = new Size(15, 15),
                Location = new Point(82, 2),
                Image = Image.FromFile("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\question-mark.png"),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Cursor = Cursors.Hand
            };

            // Info Icon Click Event
            infoIcon.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    MessageBox.Show(infoMessage);
                }
            };

            // Add controls to panel
            panel.Controls.Add(titleLabel);
            panel.Controls.Add(valueLabel);
            panel.Controls.Add(infoIcon);

            // Enable Double Buffering for Smooth Rendering
            panel.GetType().GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance)
                ?.SetValue(panel, true, null);

            return panel;
        }
    }
}
