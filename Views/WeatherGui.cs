using System.Diagnostics;
using System.Drawing.Drawing2D;
using WeatherApp.Models;
using WeatherApp.Views;


namespace WeatherApp
{
    public partial class WeatherGui : Form
    {
        Button TempScaleToggle;
        Label seventhvalue;
        Label sixthvalue;
        Label fifthvalue;
        Label fourthvalue;
        Label thirdvalue;
        Label secondvalue;
        Label firstvalue;
        Label humidity;
        Label pressure;
        Label wind;
        Label Cloud;
        Label Feelslike;
        string icontype = "04d";
        Picture icon;
        Label locationlbl;
        ComboBox CityList;
        MainPanel Mainpnl;
        Label templbl;
        MenuPanel Menupnl;
        private int GlobalXPoint = 20;
        private bool menubarpressed = false;
        public static bool isRenderingPaused = false;
        private int offsetX, offsetY;
        private bool isDragging = false;
        private String[] Days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
        public static int currentday = (int)DateTime.Now.DayOfWeek;

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
            Mainpnl = new MainPanel();
            Mainpnl.Width = this.Width;
            Mainpnl.Height = this.Height;
            Mainpnl.BackColor = Color.CadetBlue;


            // Menu Panel
            Menupnl = new MenuPanel();
            Menupnl.Location = new Point(0, 0);
            Menupnl.Height = this.Height;
            Menupnl.Width = 0;
            Menupnl.BackColor = Color.CornflowerBlue;


            // Hamburger menu button
            Picture sidebarbtn = new Picture();
            sidebarbtn.BackColor = Color.Transparent;
            sidebarbtn.setImage("C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\Hamburger Menu.png", 30, 30);
            CityList = new ComboBox()
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
              "Accra", "Addis Ababa", "Algiers", "Amman", "Andorra la Vella", "Ankara", "Asmara", "Athens", "Baghdad", "Baku",
              "Bamako", "Bandar Seri Begawan", "Bangkok", "Bangui", "Banjul", "Beijing", "Beirut", "Belgrade", "Belmopan", "Berlin",
              "Bern", "Bishkek", "Bissau", "Bogotá", "Brasília", "Bratislava", "Brazzaville", "Bridgetown", "Brussels", "Bucharest",
              "Budapest", "Buenos Aires", "Cairo", "Canberra", "Caracas", "Castries", "Chisinau", "Colombo", "Conakry", "Copenhagen",
              "Dakar", "Damascus", "Dhaka", "Dili", "Djibouti", "Dodoma", "Doha", "Dublin", "Dushanbe", "Freetown", "Funafuti", "Gaborone",
              "Georgetown", "Gitega", "Guatemala City", "Hanoi", "Harare", "Havana", "Helsinki", "Honiara", "Islamabad", "Jakarta",
              "Jerusalem", "Juba", "Kabul", "Kampala", "Kathmandu", "Khartoum", "Kigali", "Kingston", "Kinshasa", "Kuala Lumpur",
              "Kuwait City", "Kyiv", "La Paz", "Libreville", "Lilongwe", "Lima", "Lisbon", "Ljubljana", "Lomé", "London", "Luanda",
              "Lusaka", "Luxembourg", "Madrid", "Majuro", "Malabo", "Male", "Manama", "Manila", "Maputo", "Maseru", "Mbabane", "Mexico City",
              "Minsk", "Mogadishu", "Monaco", "Monrovia", "Montevideo", "Moroni", "Moscow", "Muscat", "Nairobi", "Nassau", "Naypyidaw",
              "N'Djamena", "New Delhi", "Niamey", "Nicosia", "Nouakchott", "Nur-Sultan", "Oslo", "Ottawa", "Ouagadougou", "Palikir",
              "Panama City", "Paramaribo", "Paris", "Phnom Penh", "Podgorica", "Port Louis", "Port Moresby", "Port Vila", "Port-au-Prince",
              "Porto-Novo", "Prague", "Praia", "Pretoria", "Pyongyang", "Quito", "Rabat", "Reykjavik", "Riga", "Riyadh", "Rome",
              "Roseau", "San José", "San Marino", "San Salvador", "Santiago", "Santo Domingo", "Sarajevo", "Seoul", "Singapore",
              "Skopje", "Sofia", "Stockholm", "Suva", "Taipei", "Tallinn", "Tarawa", "Tashkent", "Tbilisi", "Tegucigalpa", "Tehran",
              "Thimphu", "Tirana", "Tokyo", "Tripoli", "Tunis", "Ulaanbaatar", "Vaduz", "Valletta", "Vatican City", "Victoria", "Vienna",
              "Vientiane", "Vilnius", "Warsaw", "Washington, D.C.", "Wellington", "Windhoek", "Yaoundé", "Yerevan", "Zagreb"
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
            CityList.SelectedIndex = 1;
            CityList.SelectedIndexChanged += OnCityChanged;
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
            templbl = new Label()
            {
                Font = new Font("Roboto", 35, FontStyle.Bold),
                Location = new Point(90, 70),
                Height = 50,
                ForeColor = Color.White
            };

            // Location Label
            locationlbl = new Label();
            locationlbl.Text = CityList.SelectedItem.ToString();
            locationlbl.Height = 40;
            locationlbl.Width = 300;
            locationlbl.ForeColor = Color.White;
            locationlbl.BackColor = Color.Transparent;
            locationlbl.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            locationlbl.Location = new Point(GlobalXPoint + 45, 16);

            // Calvin/Fahrenheit
            TempScaleToggle = new Button()
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
                    templbl.Text = Convert.ToString(Functions.CtoF(Convert.ToInt32(templbl.Text.Replace('°', ' ')))) + "°";
                    firstvalue.Text = Functions.ToggleTemperatureCel(firstvalue.Text);
                    secondvalue.Text = Functions.ToggleTemperatureCel(secondvalue.Text);
                    thirdvalue.Text = Functions.ToggleTemperatureCel(thirdvalue.Text);
                    fourthvalue.Text = Functions.ToggleTemperatureCel(fourthvalue.Text);
                    fifthvalue.Text = Functions.ToggleTemperatureCel(fifthvalue.Text);
                    sixthvalue.Text = Functions.ToggleTemperatureCel(sixthvalue.Text);
                    seventhvalue.Text = Functions.ToggleTemperatureCel(seventhvalue.Text);
                    TempScaleToggle.Text = "°F";
                }
                else
                {
                    templbl.Text = Convert.ToString(Functions.FtoC(Convert.ToInt32(templbl.Text.Replace('°', ' ')))) + "°";
                    firstvalue.Text = Functions.ToggleTemperatureFeh(firstvalue.Text);
                    secondvalue.Text = Functions.ToggleTemperatureFeh(secondvalue.Text);
                    thirdvalue.Text = Functions.ToggleTemperatureFeh(thirdvalue.Text);
                    fourthvalue.Text = Functions.ToggleTemperatureFeh(fourthvalue.Text);
                    fifthvalue.Text = Functions.ToggleTemperatureFeh(fifthvalue.Text);
                    sixthvalue.Text = Functions.ToggleTemperatureFeh(sixthvalue.Text);
                    seventhvalue.Text = Functions.ToggleTemperatureFeh(seventhvalue.Text);
                    TempScaleToggle.Text = "°C";
                }
            };



            // Weather type icons
            icon = new Picture()
            {
                Location = new Point(10, 55),
                SizeMode = PictureBoxSizeMode.StretchImage,
            };
            Mainpnl.Controls.Add(icon);

            // Minimize and close 
            Picture minimizebtn = new Picture();
            minimizebtn.BackColor = Color.Transparent;
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
            closebtn.BackColor = Color.Transparent;
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
            Feelslike = new Label()
            {
                Location = new Point(20, 140),
                Font = new Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                ForeColor = Color.White,
                Height = 50,
                Width = 150,
            };



            Cloud = new Label()
            {
                Text = "10000 km",
                Location = new Point(22, 205),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };
            Mainpnl.Controls.Add(Cloud);
            Mainpnl.Controls.AddRange(Functions.CreateInfoControls("Cloud",
                "Cloud cover is the amount of sky covered by clouds at a specific time and location. It's expressed as a percentage, ranging from 0% to 100%.",
                new Point[2] { new Point(20, 180), new Point(105, 182) }));


            wind = new Label()
            {
                Text = "10 m/s N",
                Location = new Point(160, 205),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };
            Mainpnl.Controls.Add(wind);
            Mainpnl.Controls.AddRange(Functions.CreateInfoControls("Wind",
                "Wind speed is the speed of the wind in km/h. Wind direction is the direction from which the wind is coming from.",
                new Point[2] { new Point(160, 180), new Point(230, 182) }));

            pressure = new Label()
            {
                Text = "90 hPa",
                Location = new Point(160, 245),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };
            Mainpnl.Controls.Add(pressure);
            Mainpnl.Controls.AddRange(Functions.CreateInfoControls("Pressure",
                "Pressure is the force exerted by the air above a given point. It is measured in hectopascals (hPa).",
                 new Point[2] { new Point(160, 220), new Point(230, 222) }));
            
            humidity = new Label()
            {
                Text = "10%",
                Location = new Point(22, 245),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true
            };
            Mainpnl.Controls.Add(humidity);
            Mainpnl.Controls.AddRange(Functions.CreateInfoControls("Humidity",
              "Humidity is the amount of water vapor in the air.",
               new Point[2] { new Point(20, 220), new Point(105, 222) }));
            // Days Panel

            DaysPanel TempDaysPnl = new DaysPanel();
            int[] dayprecedence = Functions.ManageDays();
            Point tmp = new Point(10, 10);
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
                TempDaysPnl.Controls.Add(dayLabel);
            }

            firstvalue = new Label()
            {
                Location = new Point(180, 10),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#3D5A6C"),
            };


            secondvalue = new Label()
            {
                Location = new Point(180, 35),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#3D5A6C"),
            };


            thirdvalue = new Label()
            {
                Location = new Point(180, 60),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#3D5A6C"),
            };


            fourthvalue = new Label()
            {
                Location = new Point(180, 85),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#3D5A6C"),
            };


            fifthvalue = new Label()
            {
                Location = new Point(180, 110),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#3D5A6C"),
            };


            sixthvalue = new Label()
            {
                Location = new Point(180, 135),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#3D5A6C"),
            };


            seventhvalue = new Label()
            {
                Location = new Point(180, 160),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#3D5A6C"),
            };
            TempDaysPnl.Controls.AddRange(new Control[7] {
                firstvalue, secondvalue, thirdvalue, fourthvalue, fifthvalue, seventhvalue, sixthvalue
            });

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

        private async void OnCityChanged(Object sender, EventArgs e)
        {
            locationlbl.Text = CityList.Text;
            CityList.Visible = false;
            menubarpressed = false;
            Mainpnl.Location = new Point(0, 0);
            Menupnl.Width = 0;
            Root api = await WeatherApiService.Get7DayForecastAsync($"{locationlbl.Text}", "ec41ae60ff582e38f9d23b71b591d7a0", "forecast");
            if (api != null)
            {
                TempScaleToggle.Text = "°C";
                templbl.Text = Convert.ToString(Functions.KtoC(api.list[0].main.temp)) + "°";
                icontype = $"{api.list[0].weather[0].icon}";
                icon.Image = Image.FromFile($"C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\{icontype}@2x.png");
                icon.ClientSize = new Size(80, 80);


                Cloud.Text = $"{api.list[0].clouds.all} %";
                double degree = api.list[0].wind.deg;
                wind.Text = $"{api.list[0].wind.speed} m/s {(
                      degree >= 337.5 || degree < 22.5 ? "N" :
                      degree >= 22.5 && degree < 67.5 ? "NE" :
                      degree >= 67.5 && degree < 112.5 ? "E" :
                      degree >= 112.5 && degree < 157.5 ? "SE" :
                      degree >= 157.5 && degree < 202.5 ? "S" :
                      degree >= 202.5 && degree < 247.5 ? "SW" :
                      degree >= 247.5 && degree < 292.5 ? "W" :
                      "NW")}";
                humidity.Text = $"{api.list[0].main.humidity} %";
                pressure.Text = $"{api.list[0].main.pressure} hPa";


                Feelslike.Text = $"Feels Like: {Functions.KtoC(api.list[0].main.feels_like)} °";
                Mainpnl.BackColor = api.list[0].sys.pod == "n" ? Color.SteelBlue : Color.CadetBlue;

                firstvalue.Text = $"{Functions.KtoC(api.list[0].main.temp_min)}/{Functions.KtoC(api.list[0].main.temp_max)}";
                secondvalue.Text = $"{Functions.KtoC(api.list[1].main.temp_min)}/{Functions.KtoC(api.list[1].main.temp_max)}";
                thirdvalue.Text = $"{Functions.KtoC(api.list[2].main.temp_min)}/{Functions.KtoC(api.list[2].main.temp_max)}";
                fourthvalue.Text = $"{Functions.KtoC(api.list[3].main.temp_min)}/{Functions.KtoC(api.list[3].main.temp_max)}";
                fifthvalue.Text = $"{Functions.KtoC(api.list[4].main.temp_min)}/{Functions.KtoC(api.list[4].main.temp_max)}";
                sixthvalue.Text = $"{Functions.KtoC(api.list[5].main.temp_min)}/{Functions.KtoC(api.list[5].main.temp_max)}";
                seventhvalue.Text = $"{Functions.KtoC(api.list[6].main.temp_min)}/{Functions.KtoC(api.list[6].main.temp_max)}";
            }
        }
        private async void WeatherGui_Load(object sender, EventArgs e)
        {
            Root api = await WeatherApiService.Get7DayForecastAsync($"{locationlbl.Text}", "ec41ae60ff582e38f9d23b71b591d7a0", "forecast");
            if (api != null)
            {
                // Change Temp with api data
                TempScaleToggle.Text = "°C";
                templbl.Text = Convert.ToString(Functions.KtoC(api.list[0].main.temp)) + "°";
                // Change icon with api data
                icontype = $"{api.list[0].weather[0].icon}";
                icon.Image = Image.FromFile($"C:\\Users\\Nahilor\\source\\repos\\Nahilor\\WeatherApp\\Assets\\{icontype}@2x.png");
                icon.ClientSize = new Size(80, 80);

                Cloud.Text = $"{api.list[0].clouds.all} %";
                double degree = api.list[0].wind.deg;
                wind.Text = $"{api.list[0].wind.speed} m/s {(
                      degree >= 337.5 || degree < 22.5 ? "N" :
                      degree >= 22.5 && degree < 67.5 ? "NE" :
                      degree >= 67.5 && degree < 112.5 ? "E" :
                      degree >= 112.5 && degree < 157.5 ? "SE" :
                      degree >= 157.5 && degree < 202.5 ? "S" :
                      degree >= 202.5 && degree < 247.5 ? "SW" :
                      degree >= 247.5 && degree < 292.5 ? "W" :
                      "NW")
                }";
                humidity.Text = $"{api.list[0].main.humidity} %";
                pressure.Text = $"{api.list[0].main.pressure} hPa";

                Feelslike.Text = $"Feels Like: {Functions.KtoC(api.list[0].main.feels_like)} °";
                Mainpnl.BackColor = api.list[0].sys.pod == "n" ? Color.SteelBlue : Color.CadetBlue;

                firstvalue.Text = $"{Functions.KtoC(api.list[0].main.temp_min)}/{Functions.KtoC(api.list[0].main.temp_max)}";
                secondvalue.Text = $"{Functions.KtoC(api.list[1].main.temp_min)}/{Functions.KtoC(api.list[1].main.temp_max)}";
                thirdvalue.Text = $"{Functions.KtoC(api.list[2].main.temp_min)}/{Functions.KtoC(api.list[2].main.temp_max)}";
                fourthvalue.Text = $"{Functions.KtoC(api.list[3].main.temp_min)}/{Functions.KtoC(api.list[3].main.temp_max)}";
                fifthvalue.Text = $"{Functions.KtoC(api.list[4].main.temp_min)}/{Functions.KtoC(api.list[4].main.temp_max)}";
                sixthvalue.Text = $"{Functions.KtoC(api.list[5].main.temp_min)}/{Functions.KtoC(api.list[5].main.temp_max)}";
                seventhvalue.Text = $"{Functions.KtoC(api.list[6].main.temp_min)}/{Functions.KtoC(api.list[6].main.temp_max)}";
            }
        }
    }
}
