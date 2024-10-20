using GUI.Forms.AuthenticationForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms.MainForm
{
    public partial class MainForm : Form
    {
        private bool isLoading = false;
        private PictureBox pictureBoxLoad;

        public MainForm()
        {
            InitializeComponent();
            InitializeLoadingPictureBox();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                isLoading = true;

                panelMain.Visible = false;
                pictureBox.Visible = false;

                pictureBoxLoad.Visible = true;

                await LoadData();

                pictureBoxLoad.Visible = false;

                panelMain.Visible = true;
                pictureBox.Visible = true;

                isLoading = false;
            }
            // Hiển thị Form Login lên panelMain
            ShowForm(new LoginForm(this));

        }
        // Khởi tạo PictureBox để hiển thị ảnh loading
        private void InitializeLoadingPictureBox()
        {
            string imagePath = Path.Combine(Application.StartupPath, @"..\..\Resources\images", "loading1.gif");
            if (File.Exists(imagePath))
            {
                pictureBoxLoad = new PictureBox
                {
                    Image = Image.FromFile(imagePath),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Dock = DockStyle.Fill,
                    Visible = false
                };
                this.Controls.Add(pictureBoxLoad);
            }
            else
            {
                MessageBox.Show("Loading image not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Ngừng chương trình
                Environment.Exit(0);
            }
        }
        // Load dữ liệu
        public async Task LoadData()
        {
            // Lấy thời gian chạy
            string strartTime = DateTime.Now.ToString("HH:mm:ss");

            DataLoader dataLoader = new DataLoader();
            await dataLoader.LoadDataAsync();

            // Lấy thời gian kết thúc
            string endTime = DateTime.Now.ToString("HH:mm:ss");

            // Tính thời gian chạy
            TimeSpan time = DateTime.Parse(endTime).Subtract(DateTime.Parse(strartTime));
            Console.WriteLine("Time: " + time);
        }
        // Hiển thị Form lên panelMain
        public void ShowForm(Form form)
        {
            panelMain.Controls.Clear();
            form.TopLevel = false;
            panelMain.Controls.Add(form);
            form.Show();
        }
    }
}
