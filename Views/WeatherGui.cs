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
            Controls.Add(Mainpnl);

            // Menu Panel
            MenuPanel Menupnl = new MenuPanel();
            Menupnl.Location = new Point(0, 0);
            Menupnl.Height = this.Height;
            Menupnl.Width = 0;
            Menupnl.BackColor = Color.FromArgb(110, 158, 255);
            Menupnl.BringToFront();
            Controls.Add(Menupnl);

            // Location Label
            Label locationlbl = new Label();
            locationlbl.Text = "Addis Ababa";
            locationlbl.Height = 40;
            locationlbl.Width = 300;
            locationlbl.ForeColor = Color.White;
            locationlbl.BackColor = Color.Transparent;
            locationlbl.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            locationlbl.Location = new Point(GlobalXPoint + 45, 19);
            Mainpnl.Controls.Add(locationlbl);

            // Hamburger menu button
            ImageBtn sidebarbtn = new ImageBtn();
            sidebarbtn.BackColor = Color.Transparent;
            sidebarbtn.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\Hamburger Menu.png", 30, 30);
            ComboBox CityList = new ComboBox()
            {
                Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0),
                BackColor = Color.FromArgb(110, 158, 255),
                ForeColor = Color.White,
                Location = new Point(GlobalXPoint, 20),
                Width = 200,

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
            Menupnl.Controls.Add(CityList);
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
            Mainpnl.Controls.Add(sidebarbtn);
        }

        // Hamburger menu
        class ImageBtn : PictureBox
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
        }

        // Menu panel
        class MenuPanel : Panel
        {
            public MenuPanel()
            {
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
            }
        }

        // Gradient Background
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Rectangle gradientRect = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            LinearGradientBrush brush = new LinearGradientBrush(gradientRect, Color.Empty, Color.Empty, 90f);

            Color[] colors = {
                Color.FromArgb(89, 154, 221), 
                Color.FromArgb(113, 168, 207), 
                Color.FromArgb(154, 186, 230)  
            };

            float[] positions = { 0.0f, 0.5f, 1.0f };

            ColorBlend colorBlend = new ColorBlend();
            colorBlend.Colors = colors;
            colorBlend.Positions = positions;

            brush.InterpolationColors = colorBlend;

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
