using DTO;
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
    }
}
