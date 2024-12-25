using System.Collections;
using System.Drawing.Drawing2D;

namespace WeatherApp
{
    public partial class WeatherGui : Form
    {
        int GlobalXPoint = 20;
        static System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        bool menubarpressed = false;

        public WeatherGui()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            Size = new System.Drawing.Size(300, 500);
            InitGui();
            this.DoubleBuffered = true;
        }

        public void InitGui()
        {
            // Main panel
            Panel Mainpnl = new Panel();
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
            Icon sidebarbtn = new Icon();
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
                Location = new Point(18, 70),
                Height = 50,
                ForeColor = Color.White
            };

            //Weather type icon
            List<Icon> WeatherIcons = new List<Icon>();
            // 1. Sunny
            WeatherIcons.Add
            (
                new Icon().CreateIcon
                (
                    50, 50, "C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\sun.png", new Point(200, 68)
                )
            );
            // 2. cloudy
            WeatherIcons.Add
            (
                new Icon().CreateIcon
                (
                    50, 50, "C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\cloudy.png", new Point(200, 68)
                )
            );
            // 3. Windy
            WeatherIcons.Add
            (
                new Icon().CreateIcon
                (
                    50, 50, "C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\windy.png", new Point(200, 68)
                )
            );
            // 4. Snowy
            WeatherIcons.Add
            (
                new Icon().CreateIcon
                (
                    50, 50, "C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\snowy.png", new Point(200, 68)
                )
            );
            // 5. Rainy
            WeatherIcons.Add
            (
                new Icon().CreateIcon
                (
                    50, 50, "C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\rainy-day.png", new Point(200, 68)
                )
            );

            Controls.Add(Mainpnl);
            Mainpnl.Controls.Add(sidebarbtn);
            Mainpnl.Controls.Add(locationlbl);
            Mainpnl.Controls.Add(WeatherIcons.ElementAt(4));
            Controls.Add(Menupnl);
            Menupnl.Controls.Add(CityList);
            Mainpnl.Controls.Add(templbl);
        }

        // Hamburger menu
        class Icon : PictureBox
        {
            Bitmap? icon;
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
            
            public Icon CreateIcon(int xSize, int ySize, String fileDestination, Point point)
            {
                Icon icon = new Icon();
                icon.Location = point;
                icon.setImage(fileDestination, xSize, ySize);
                return icon;
            }
        }

        // Menu panel
        class MenuPanel : Panel
        {
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
            }
        }

        // Gradient Background
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Rectangle gradientRect = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
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
        }
    }
}
