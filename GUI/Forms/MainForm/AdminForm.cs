using DTO;
using GUI.Forms.AdminForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms.MainForm
{
    public partial class AdminForm : Form
    {
        private UserDTO _user;
        public AdminForm(UserDTO user)
        {
            InitializeComponent();
            this._user = user;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            ShowUserInfo();
            Showtime();
            ShowForm(new AdminDataForm());
        }
        private void ShowUserInfo()
        {
            labelFullName.Text = _user.FullName;
            labelRole.Text = _user.Role;
        }
        private void Showtime()
        {
            CultureInfo viCulture = new CultureInfo("vi-VN");
            labelDateTime.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy", viCulture);
        }
        public void ShowForm(Form form)
        {
            form.TopLevel = false;
            form.AutoScroll = true;
            panelMain.Controls.Clear();
            panelMain.Controls.Add(form);
            form.Show();
        }

        private void lbLogout_Click(object sender, EventArgs e)
        {
            // Quay lại form đăng nhập
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
            this.Close();
        }
    }
}
