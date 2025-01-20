using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using WeatherApp.Views;


namespace WeatherApp
{
    public partial class WeatherGui : Form
    {
        private int GlobalXPoint = 20;
        private bool menubarpressed = false;
        public static bool isRenderingPaused = false;
        private int offsetX, offsetY;
        private bool isDragging = false;
        private String[] Days =  { "Sunday", "Monday",  "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};
        public static int currentday = (int) DateTime.Now.DayOfWeek;

        public WeatherGui()
        {
            InitializeComponent();
            this.BackColor = Color.CadetBlue;
            this.DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Size = new System.Drawing.Size(300, 500);
            InitGui();
        }

        public void InitGui()
        {
            // Main panel
            MainPanel Mainpnl = new MainPanel();
            Mainpnl.Width = this.Width;
            Mainpnl.Height = this.Height;
            Mainpnl.BackColor = Color.CadetBlue;


            // Menu Panel
            MenuPanel Menupnl = new MenuPanel();
            Menupnl.Location = new Point(0, 0);
            Menupnl.Height = this.Height;
            Menupnl.Width = 0;
            Menupnl.BackColor = Color.CornflowerBlue;



            // Location Label
            Label locationlbl = new Label();
            locationlbl.Text = "Addis Ababa";
            locationlbl.Height = 40;
            locationlbl.Width = 300;
            locationlbl.ForeColor = Color.White;
            locationlbl.BackColor = Color.CadetBlue;
            locationlbl.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            locationlbl.Location = new Point(GlobalXPoint + 45, 16);




            // Hamburger menu button
            Picture sidebarbtn = new Picture();
            sidebarbtn.BackColor = Color.CadetBlue;
            sidebarbtn.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\Hamburger Menu.png", 30, 30);
            ComboBox CityList = new ComboBox()
            {
                Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0),
                BackColor = Color.CornflowerBlue,
                ForeColor = Color.White,
                Location = new Point(GlobalXPoint, 20),
                Width = 200
            };
            CityList.Visible = false;
            Object[] capitalCities = new Object[]
            {
                "Addis Ababa", "Tokyo", "Washington, D.C.", "London", "Paris",
                "Berlin", "Ottawa", "Canberra", "Beijing", "Moscow",
                "New Delhi", "Brasília", "Buenos Aires", "Cairo", "Nairobi",
                "Riyadh", "Bangkok", "Seoul", "Hanoi", "Jakarta",
                "Kuala Lumpur", "Manila", "Athens", "Rome", "Madrid",
                "Lisbon", "Helsinki", "Oslo", "Stockholm", "Warsaw",
                "Vienna", "Dublin", "Amsterdam", "Brussels", "Copenhagen"
            };
            AutoCompleteStringCollection Cities = new AutoCompleteStringCollection();
            CityList.AutoCompleteSource = AutoCompleteSource.CustomSource;
            CityList.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            foreach (var city in capitalCities)
            {
                Cities.Add((String)city);
            }

            CityList.AutoCompleteCustomSource = Cities;

            CityList.Items.AddRange(capitalCities);
            CityList.SelectedIndex = 0;
            CityList.SelectedIndexChanged += new EventHandler((sender, e) =>
            {
                locationlbl.Text = CityList.Text;
                CityList.Visible = false;
                menubarpressed = false;
                Mainpnl.Location = new Point(0, 0);
                Menupnl.Width = 0;
            });
            sidebarbtn.MouseClick += new MouseEventHandler((sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (!menubarpressed)
                    {
                        SuspendLayout();
                        Functions.ToggleRendering();
                        Menupnl.Width = 235;
                        CityList.Visible = true;
                        Mainpnl.Location = new Point(235, 0);
                        menubarpressed = true;
                        ResumeLayout();
                    }
                    else if (menubarpressed)
                    {
                        SuspendLayout();
                        Functions.ToggleRendering();
                        CityList.Visible = false;
                        menubarpressed = false;
                        Mainpnl.Location = new Point(0, 0);
                        Menupnl.Width = 0;
                        ResumeLayout();
                    }
                }
                Functions.ToggleRendering();
            });
            sidebarbtn.Location = new Point(GlobalXPoint, 20);
            

            // Temperature Label
            Label templbl = new Label()
            {
                Text = "10\u00B0",
                Font = new Font("Roboto", 35, FontStyle.Bold),
                Location = new Point(80, 70),
                Height = 50,
                ForeColor = Color.White
            };

            // Calvin/Fahrenheit
            Button TempScaleToggle = new Button()
            {
                Width = 35,
                Height = 35,
                Location = new Point(225, 80),
                Text = "°C",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.CadetBlue,
                ForeColor = Color.White,
                Font = new Font("Calibri", 11, FontStyle.Bold),
            };
            TempScaleToggle.FlatAppearance.BorderColor = Color.White;
            TempScaleToggle.FlatAppearance.BorderSize = 2;

            TempScaleToggle.Click += (sender, e) =>
            {
                if (TempScaleToggle.Text == "°C")
                {
                    templbl.Text = Convert.ToString(Functions.CtoF(Convert.ToInt32(templbl.Text.Replace('°', ' ')))) + "°";
                    TempScaleToggle.Text = "°F";
                }

                else
                {
                    templbl.Text = Convert.ToString(Functions.FtoC(Convert.ToInt32(templbl.Text.Replace('°', ' ')))) + "°";
                    TempScaleToggle.Text = "°C";
                }
            };



            // Weather type icons
            List<PictureBox> WeatherIcons = new List<PictureBox>();

            // Icon data for initialization
            var iconData = new List<(string FilePath, string IconName)>
            {
                ("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\sun.png", "Sunny"),
                ("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\cloudy.png", "Cloudy"),
                ("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\windy.png", "Windy"),
                ("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\snowy.png", "Snowy"),
                ("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\rainy-day.png", "Rainy")
            };

            // Initialize weather icons
            foreach (var (filePath, iconName) in iconData)
            {
                var icon = new PictureBox
                {
                    Size = new Size(60, 60),
                    Location = new Point(18, 64),
                    Image = Image.FromFile(filePath),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Tag = iconName // I may use this for api selections
                };
                WeatherIcons.Add(icon);
            }
            Mainpnl.Controls.Add(WeatherIcons[0]);

            // Update the icon dynamically based on API data
            void UpdateWeatherIcon(int weatherIndex)
            {
                if (weatherIndex >= 0 && weatherIndex < WeatherIcons.Count)
                {
                    // Clear current icons but this might change
                    Mainpnl.Controls.Remove(WeatherIcons[0]);

                    // Add the selected icon
                    Mainpnl.Controls.Add(WeatherIcons[weatherIndex]);
                }
            }



            // Minimize and close 
            Picture minimizebtn = new Picture();
            minimizebtn.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\minus-sign.png", 17, 17);
            minimizebtn.Location = new Point(245, 15);
            minimizebtn.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.WindowState = FormWindowState.Minimized;
                }
            };
            Picture closebtn = new Picture();
            closebtn.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\close.png", 17, 17);
            closebtn.Location = new Point(270, 15);
            closebtn.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.Close();
                }
            };


            // Moving of window 
            Mainpnl.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    // init relative mouse position
                    offsetX = MousePosition.X - this.Location.X;
                    offsetY = MousePosition.Y - this.Location.Y;

                    // ToggleRendering();

                    isDragging = true;
                }
            };

            // Handling MouseMove event to drag the form
            Mainpnl.MouseMove += (sender, e) =>
            {
                // ToggleRendering();
                if (isDragging)
                {
                    this.Location = new Point(Cursor.Position.X - offsetX, Cursor.Position.Y - offsetY);
                }
            };

            // Handling MouseUp event to stop dragging
            Mainpnl.MouseUp += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = false;
                    // ToggleRendering();
                }
            };

            // Feels Like weather attribute
            Label Feelslike = new Label()
            {
                Text = "Fair",
                Location = new Point(30, 120),
                Font = new Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                ForeColor = Color.White,
                Height = 50
            };



            Mainpnl.Controls.AddRange(Functions.CreateInfoControls("Air Quality", "100",
                "Air quality is a measure of how clean or polluted the air is. A value of 100 is considered to be the best air quality.",
                new Point[3] { new Point (20, 160), new Point(20, 185), new Point(105, 162) }));
           
            Mainpnl.Controls.AddRange(Functions.CreateInfoControls("Wind", "N 10 km/h",
                "Wind speed is the speed of the wind in km/h. Wind direction is the direction from which the wind is coming from.",
                new Point[3] {new Point (160, 160), new Point(160, 185), new Point(230, 162)}));
            
           
           Mainpnl.Controls.AddRange(Functions.CreateInfoControls("Pressure", "1000 hPa",
               "Pressure is the force exerted by the air above a given point. It is measured in hectopascals (hPa).",
                new Point[3] { new Point(160, 200), new Point(160, 225), new Point(230, 202) }));
            
             Mainpnl.Controls.AddRange (Functions.CreateInfoControls("Dew Point", "10°",
               "Dew point is the temperature at which air becomes saturated with water vapor and dew can form.",
                new Point[3] { new Point(20, 200), new Point(20, 225), new Point(105, 202) }));
            // Days Panel

            DaysPanel TempDaysPnl = new DaysPanel();
            int[] dayprecedence = Functions.ManageDays();
            Point tmp =  new Point(10, 10);
            for (int i = 0; i < 7; i++)
            {
                Label dayLabel = new Label()
                {
                    Text = i == 0 ? "Today" : Days[dayprecedence[i]],
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    BackColor = ColorTranslator.FromHtml("#3D5A6C"),
                };
                dayLabel.Location = tmp;
                tmp.Y += 25;

                Label highLowLabel = new Label()
                {
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Text = "10/27",
                    ForeColor = Color.White,
                    BackColor = ColorTranslator.FromHtml("#3D5A6C"),
                };
                highLowLabel.Location = new Point (dayLabel.Location.X + 180, dayLabel.Location.Y);

                
                TempDaysPnl.Controls.Add(highLowLabel);
                TempDaysPnl.Controls.Add(dayLabel);
            }

            Mainpnl.Controls.Add(closebtn);
            Mainpnl.Controls.Add(minimizebtn);
            Mainpnl.Controls.Add(sidebarbtn);
            Mainpnl.Controls.Add(templbl);
            Mainpnl.Controls.Add(TempScaleToggle);
            Mainpnl.Controls.Add(Feelslike);
            Mainpnl.Controls.Add(locationlbl);
            Mainpnl.Controls.Add(TempDaysPnl);

            Menupnl.Controls.Add(CityList);

            Controls.Add(Mainpnl);
            Controls.Add(Menupnl);
        }

        // Gradient Background On Form
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (isRenderingPaused) return;
            /*
            Rectangle gradientRect = new Rectangle(0, 0, 300, 500);
            LinearGradientBrush brush = new LinearGradientBrush
            (
                gradientRect,
                ColorTranslator.FromHtml("#0061ff"),
                ColorTranslator.FromHtml("#60efff"),
                65f
            );

            e.Graphics.FillRectangle(brush, gradientRect);
            */
            GraphicsPath path = new GraphicsPath();
            int radius = 20;

            // Radius for the rounded corners
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();
            this.Region = new Region(path);
        }
    }
}
