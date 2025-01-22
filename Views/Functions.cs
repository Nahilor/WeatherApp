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
        public static string ToggleTemperatureCel(string tempLabel)
        {
            string[] temps = tempLabel.Split('/');
            if (temps.Length == 2)
            {
                // Parse the values
                int firstValue = Convert.ToInt32(temps[0]);
                int secondValue = Convert.ToInt32(temps[1]);

                // Convert both to Fahrenheit
                int fahrenheitFirst = CtoF(firstValue);
                int fahrenheitSecond = CtoF(secondValue);
                return fahrenheitFirst + "/" + fahrenheitSecond;
            }
            return tempLabel; // Return unchanged if the format is invalid or unrecognized
        }
        public static string ToggleTemperatureFeh(string tempLabel)
        {
            string[] temps = tempLabel.Split('/');
            if (temps.Length == 2)
            {
                // Parse the values
                int firstValue = Convert.ToInt32(temps[0]);
                int secondValue = Convert.ToInt32(temps[1]);

                // Convert both to Celsius
                int celsiusFirst = FtoC(firstValue);
                int celsiusSecond = FtoC(secondValue);
                return celsiusFirst + "/" + celsiusSecond;
            }
            return tempLabel; // Return unchanged if the format is invalid or unrecognized
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
        internal static int KtoC(double K)
        {
            return Convert.ToInt32(K - 273);
        }

        // Render Controller
        internal static void ToggleRendering()
        {
            WeatherGui.isRenderingPaused = !WeatherGui.isRenderingPaused;
        }

        // Method to Create Panels with Common Functionality
        internal static Control[] CreateInfoControls(string label, string infoMessage, Point[] location)
        {
            var controls = new Control[2];

            Label titleLabel = new Label
            {
                Text = label,
                Location = location[0],
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };
            controls[0] = (titleLabel);

            Picture infoIcon = new Picture
            {
                Size = new Size(15, 15),
                Location = location[1],
            };
            infoIcon.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\question-mark.png", 15, 15);
            controls[1] = infoIcon;

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
