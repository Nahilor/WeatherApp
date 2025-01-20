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
        internal static Control[] CreateInfoControls(string label, string value, string infoMessage, Point[] location)
        {
            var controls = new Control[3];

            Label titleLabel = new Label
            {
                Text = label,
                Location = location[0],
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };
            controls[0] = (titleLabel);

            Label valueLabel = new Label
            {
                Text = value,
                Location = location[1],
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };
            controls[1] = valueLabel;

            Picture infoIcon = new Picture
            {
                Size = new Size(15, 15),
                Location = location[2],
            };
            infoIcon.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\question-mark.png", 15, 15);
            controls[2] = infoIcon;

            // Info Icon Click Event
            infoIcon.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    MessageBox.Show(infoMessage);
                }
            };

            return controls;
        }
    }
}
