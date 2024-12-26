using System.Collections;
using System.Drawing.Drawing2D;


namespace WeatherApp
{
    public partial class WeatherGui : Form
    {
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private int GlobalXPoint = 20;
        private bool menubarpressed = false;
        private static bool isRenderingPaused = false;
        private int offsetX, offsetY;
        private bool isDragging = false;

        public WeatherGui()
        {
            timer.Interval = 1;
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            Size = new System.Drawing.Size(300, 500);
            InitGui();
            this.DoubleBuffered = false;
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
            Menupnl.BringToFront();


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
                Text = "25\u00B0C",
                Font = new Font("Roboto", 35, FontStyle.Bold),
                Location = new Point(80, 70),
                Height = 50,
                ForeColor = Color.White
            };

            // Calvin/Fahrenheit

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
            Mainpnl.Controls.Add(WeatherIcons.ElementAt(1));


            // Minimize and close 
            Picture minimizebtn = new Picture();
            minimizebtn.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\delete.png", 17, 17);
            minimizebtn.Location = new Point(250, 15);
            minimizebtn.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.WindowState = FormWindowState.Minimized;
                }
            };
            Picture maximizebtn = new Picture();
            maximizebtn.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\remove.png", 17, 17);
            maximizebtn.Location = new Point(270, 15);
            maximizebtn.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.Close();
                }
            };
            Mainpnl.Controls.Add(maximizebtn);
            Mainpnl.Controls.Add(minimizebtn);

            // Moving of window 
            Mainpnl.MouseDown += (sender, e) =>
            {
                timer.Tick += Timer_Tick;
                if (e.Button == MouseButtons.Left)
                {
                    // Capture initial mouse position and set drag flag
                    offsetX = MousePosition.X - this.Location.X;
                    offsetY = MousePosition.Y - this.Location.Y;

                    // Toggle rendering or perform any other actions needed
                    ToggleRendering();

                    // Start the timer for dragging
                    isDragging = true;
                    timer.Start();
                }
            };

            // Handle MouseUp event to stop dragging
            Mainpnl.MouseUp += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    // Stop dragging and the timer
                    isDragging = false;
                    timer.Stop();

                    // Toggle rendering back
                    ToggleRendering();
                }
            };



            Controls.Add(Mainpnl);
            Mainpnl.Controls.Add(sidebarbtn);
            Mainpnl.Controls.Add(locationlbl);
            
            Controls.Add(Menupnl);
            Menupnl.Controls.Add(CityList);
            Mainpnl.Controls.Add(templbl);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isDragging)
            {
                // Update the form's location based on the cursor's position
                this.Location = new Point(Cursor.Position.X - offsetX, Cursor.Position.Y - offsetY);
            }
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
