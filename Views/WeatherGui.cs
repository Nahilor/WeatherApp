using System.Collections;
using System.Drawing.Drawing2D;
using Microsoft.VisualBasic.ApplicationServices;


namespace WeatherApp
{
    public partial class WeatherGui : Form
    {
        private int GlobalXPoint = 20;
        private bool menubarpressed = false;
        private static bool isRenderingPaused = false;
        private int offsetX, offsetY;
        private bool isDragging = false;

        public WeatherGui()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
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
            Mainpnl.BackColor = Color.Transparent;


            // Menu Panel
            MenuPanel Menupnl = new MenuPanel();
            Menupnl.Location = new Point(0, 0);
            Menupnl.Height = this.Height;
            Menupnl.Width = 0;
            Menupnl.BackColor = Color.FromArgb(110, 120, 255);


            // Location Label
            Label locationlbl = new Label();
            locationlbl.Text = "Addis Ababa";
            locationlbl.Height = 40;
            locationlbl.Width = 300;
            locationlbl.ForeColor = Color.White;
            locationlbl.BackColor = Color.Transparent;
            locationlbl.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            locationlbl.Location = new Point(GlobalXPoint + 45, 16);
    



            // Hamburger menu button
            Picture sidebarbtn = new Picture();
            sidebarbtn.BackColor = Color.Transparent;
            sidebarbtn.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\Hamburger Menu.png", 30, 30);
            ComboBox CityList = new ComboBox()
            {
                Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0),
                BackColor = Color.FromArgb(110, 170, 255),
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
                        Menupnl.Width = 235;
                        CityList.Visible = true;
                        Mainpnl.Location = new Point(235, 0);
                        menubarpressed = true;
                    }
                    else if (menubarpressed)
                    {
                        CityList.Visible = false;
                        menubarpressed = false;
                        Mainpnl.Location = new Point(0, 0);
                        Menupnl.Width = 0;
                    }
                }
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
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                Font = new Font("Calibri", 11, FontStyle.Bold),
            };
            TempScaleToggle.FlatAppearance.BorderColor = Color.White;
            TempScaleToggle.FlatAppearance.BorderSize = 2;

            TempScaleToggle.Click += (sender, e) =>
            {
                if (TempScaleToggle.Text == "°C")
                {
                    templbl.Text = Convert.ToString(CtoF(Convert.ToInt32(templbl.Text.Replace('°', ' ')))) + "°";
                    TempScaleToggle.Text = "°F";
                }

                else
                {
                    templbl.Text = Convert.ToString(FtoC(Convert.ToInt32(templbl.Text.Replace('°', ' ')))) + "°";
                    TempScaleToggle.Text = "°C";
                }
            };

            

            //Weather type icon
            List<Picture> WeatherIcons = new List<Picture>();
            // 1. Sunny
            WeatherIcons.Add
            (
                new Picture().CreateIcon
                (
                    60, 60, "C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\sun.png", new Point(18, 64)
                )
            );
            // 2. cloudy
            WeatherIcons.Add
            (
                new Picture().CreateIcon
                (
                    60, 60, "C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\cloudy.png", new Point(18, 64)
                )
            );
            // 3. Windy
            WeatherIcons.Add
            (
                new Picture().CreateIcon
                (
                    60, 60, "C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\windy.png", new Point(18, 64)
                )
            );
            // 4. Snowy
            WeatherIcons.Add
            (
                new Picture().CreateIcon
                (
                    60, 60, "C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\snowy.png", new Point(18, 64)
                )
            );
            // 5. Rainy
            WeatherIcons.Add
            (
                new Picture().CreateIcon
                (
                    60, 60, "C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\rainy-day.png", new Point(18, 64)
                )
            );
            // this is dependent on api data but for now i am using 0 index for sunny 
            Mainpnl.Controls.Add(WeatherIcons.ElementAt(0));


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
            Picture maximizebtn = new Picture();
            maximizebtn.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\close.png", 17, 17);
            maximizebtn.Location = new Point(270, 15);
            maximizebtn.MouseClick += (sender, e) =>
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

            // Handle MouseMove event to drag the form
            Mainpnl.MouseMove += (sender, e) =>
            {
                // ToggleRendering();
                if (isDragging)
                {
                    this.Location = new Point(Cursor.Position.X - offsetX, Cursor.Position.Y - offsetY);
                }
            };

            // Handle MouseUp event to stop dragging
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




            // Airquality Panel
            Panel airqualitypnl = new Panel();
            airqualitypnl.Location = new Point(20, 180);
            airqualitypnl.BackColor = Color.Transparent;
            airqualitypnl.Size = new Size(100, 35);


            Label airqualitylbl = new Label()
            {
                Text = "Air Quality",
                Location = new Point(0, 0),
                Font = new Font("Segoe UI", 10F,FontStyle.Bold),
                ForeColor = Color.White
            };
            Label airquality = new Label()
            {
                Text = "100",
                Location = new Point(0, 20),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White
            };
            Picture airqualityinfo = new Picture();
            airqualityinfo.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\question-mark.png", 15, 15);
            airqualityinfo.Location = new Point(80, 2);
            airqualityinfo.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    MessageBox.Show("Air quality is a measure of how clean or polluted the air is. A value of 100 is considered to be the best air quality.");
                }
            };
            airqualitypnl.Controls.Add(airqualityinfo);
            airqualitypnl.Controls.Add(airquality);
            airqualitypnl.Controls.Add(airqualitylbl);

            // Wind Panel
            Panel windpnl = new Panel();
            windpnl.Location = new Point(160, 180);
            windpnl.BackColor = Color.Transparent;
            windpnl.Size = new Size(100, 35);
            Label windlbl = new Label()
            {
                Text = "Wind",
                Location = new Point(0, 0),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White
            };
            int windSpeed = 10;
            char windDirection = 'N';
            Label wind = new Label()
            {
                
                Text = $"{windDirection} {windSpeed} km/h",
                Location = new Point(0, 20),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White
            };
            Picture windinfo = new Picture();
            windinfo.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\question-mark.png", 15, 15);
            windinfo.Location = new Point(80, 2);
            windinfo.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    MessageBox.Show("Wind speed is the speed of the wind in km/h. Wind direction is the direction from which the wind is coming from.");
                }
            };
            windpnl.Controls.Add(windinfo);


            windpnl.Controls.Add(wind);
            windpnl.Controls.Add(windlbl);

            // Pressure
            Panel pressurepnl = new Panel();
            pressurepnl.Location = new Point(20, 220);
            pressurepnl.BackColor = Color.Transparent;
            pressurepnl.Size = new Size(100, 35);
            Label pressurelbl = new Label()
            {
                Text = "Pressure",
                Location = new Point(0, 0),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White
            };
            Label pressure = new Label() {
                Text = "1000 hPa",
                Location = new Point(0, 20),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White
            };
            Picture pressureinfo = new Picture();
            pressureinfo.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\question-mark.png", 15, 15);
            pressureinfo.Location = new Point(80, 2);
            pressureinfo.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    MessageBox.Show("Pressure is the force exerted by the air above a given point. It is measured in hectopascals (hPa).");
                }
            };
            pressurepnl.Controls.Add(pressureinfo);
            pressurepnl.Controls.Add(pressurelbl);
            pressurepnl.Controls.Add(pressure);

            // Dew Point Panel
            Panel dewpointpnl = new Panel();
            dewpointpnl.Location = new Point(160, 220);
            dewpointpnl.BackColor = Color.Transparent;
            dewpointpnl.Size = new Size(100, 35);
            Label dewpointlbl = new Label()
            {
                Text = "Dew Point",
                Location = new Point(0, 0),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White
            };
            int dewPoint = 10;
            Label dewpoint = new Label()
            {
                Text = $"{dewPoint}°",
                Location = new Point(0, 20),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White
            };
            Picture dewpointinfo = new Picture();
            dewpointinfo.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\question-mark.png", 15, 15);
            dewpointinfo.Location = new Point(80, 2);
            dewpointinfo.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    MessageBox.Show("Dew point is the temperature at which air becomes saturated with water vapor and dew can form.");
                }
            };
            
            dewpointpnl.Controls.Add(dewpointinfo);
            dewpointpnl.Controls.Add(dewpoint);
            dewpointpnl.Controls.Add(dewpointlbl);

            Controls.Add(Mainpnl);
            Mainpnl.Controls.Add(pressurepnl);
            Mainpnl.Controls.Add(windpnl);
            Mainpnl.Controls.Add(airqualitypnl);
            Mainpnl.Controls.Add(dewpointpnl);
            Mainpnl.Controls.Add(maximizebtn);
            Mainpnl.Controls.Add(minimizebtn);
            Mainpnl.Controls.Add(sidebarbtn);
            Mainpnl.Controls.Add(TempScaleToggle);

            Mainpnl.Controls.Add(Feelslike);
            Mainpnl.Controls.Add(locationlbl);
            Controls.Add(Menupnl);
            Menupnl.Controls.Add(CityList);
            Mainpnl.Controls.Add(templbl);

        }

       

        // Temperature conversion methods
        public int FtoC(int F)
        {
            return Convert.ToInt32(0.556 * (F - 32));
        }
        public int CtoF(int C)
        {
            return Convert.ToInt32((1.8 * C) + 32);
        }


        // Hamburger menu
        class Picture : PictureBox
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
        // Render Controller
        private void ToggleRendering()
        {
            isRenderingPaused = !isRenderingPaused;
        }

        class MainPanel : Panel
        {
            public MainPanel()
            {
                DoubleBuffered = true;
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                if (isRenderingPaused) return;
            }
        }


        // Menu panel
        class MenuPanel : Panel
        {
            public MenuPanel()
            {
                DoubleBuffered = true;
            }
            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                GraphicsPath path = new GraphicsPath();
                float radius = 15;
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
                path.StartFigure();
                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
                path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
                path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
                path.CloseFigure();
                this.Region = new Region(path);
                if (isRenderingPaused) return;
            }
        }



        // Gradient Background On Form
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Rectangle gradientRect = new Rectangle(0, 0, 300, 500);
            LinearGradientBrush brush = new LinearGradientBrush
                (
                    gradientRect, 
                    ColorTranslator.FromHtml("#0061ff"), 
                    ColorTranslator.FromHtml("#60efff"), 
                    65f
                );

            e.Graphics.FillRectangle(brush, gradientRect);

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
            if (isRenderingPaused) return;
        }
    }
}
