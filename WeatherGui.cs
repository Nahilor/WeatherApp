namespace WeatherApp
{
    public partial class WeatherGui : Form
    {
        public WeatherGui()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            Size = new System.Drawing.Size(300, 500);
            InitGui();
            Console.WriteLine("HI");
        }

        public void InitGui()
        {
            // Hamburger menu bag
            ImageBtn sidebarbtn = new ImageBtn();
            sidebarbtn.setImage("C:\\Users\\Nahilor\\Downloads\\menu.png", 30, 30);
            sidebarbtn.MouseClick += new MouseEventHandler((sender, e) =>
            {
                if (e.Button== MouseButtons.Left)
                   MessageBox.Show("Hamburger pressed");
            });
            sidebarbtn.Location = new Point(20, 20);
            this.Controls.Add(sidebarbtn);
        }

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
                Image = (Image) icon;
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
    }
}
