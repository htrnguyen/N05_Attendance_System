using BLL.Services;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms.AuthenticationForms
{
    public partial class LoginForm : Form
    {
        private MainForm.MainForm _mainForm;
        private AuthService _authService;
        public LoginForm(MainForm.MainForm mainForm)
        {
            InitializeComponent();
            this._mainForm = mainForm;
            this._authService = new AuthService();

            // Enter để đăng nhập
            tbUsername.KeyDown += new KeyEventHandler(tb_KeyDown);
            tbPassword.KeyDown += new KeyEventHandler(tb_KeyDown);
        }
        private void LoginForm_Load(object sender, EventArgs e)
        {
            tbUsername.Focus();
            HideError();
        }
        // Hiển thị thông báo lỗi
        private void ShowError(string message)
        {
            pnError.Visible = true;
            pictureBoxError.Visible = true;
            lbMessageError.Text = message;
        }
        // Ẩn thông báo lỗi 
        private void HideError()
        {
            pnError.Visible = false;
            pictureBoxError.Visible = false;
            lbMessageError.Text = "";
        }
        // Hiển thị mật khẩu
        private void ShowPassword(object sender, EventArgs e)
        {
            picbHidePassword.Visible = true;
            picbShowPassword.Visible = false;
            tbPassword.UseSystemPasswordChar = false;
        }
        // Ẩn mật khẩu
        private void HidePassword(object sender, EventArgs e)
        {
            picbHidePassword.Visible = false;
            picbShowPassword.Visible = true;
            tbPassword.UseSystemPasswordChar = true;
        }
        // Nhấn Enter để đăng nhập
        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
        // Đăng nhập
        private void btnLogin_Click(object sender, EventArgs e)
        {
            HideError();

            string username = tbUsername.Text;
            string password = tbPassword.Text;

            if (string.IsNullOrEmpty(username))
            {
                ShowError("Vui lòng nhập tên đăng nhập.");
                return;
            }
            if (string.IsNullOrEmpty(password)) 
            { 
                ShowError("Vui lòng nhập mật khẩu.");
                return;
            }
            if (!_authService.Login(username, password))
            {
                ShowError("Tên đăng nhập hoặc mật khẩu không đúng");
                return;
            }
            HideError();

            UserDTO user = _authService.GetUser(username);

            _mainForm.ShowForm(new ChangePasswordForm(_mainForm, user.Email));


            if (user.Role == "Admin")
            {
                this.Hide();
                _mainForm.Hide();
                MainForm.AdminForm adminForm = new MainForm.AdminForm(user);
                adminForm.ShowDialog();

                // Tắt chương trình nếu MainDashboardForm đóng
                Environment.Exit(0);
            }
            else
            {
                this.Hide();
                _mainForm.Hide();
                MainForm.MainDashboardForm mainDashboardForm = new MainForm.MainDashboardForm(user);
                mainDashboardForm.ShowDialog();

                // Tắt chương trình nếu MainDashboardForm đóng
                Environment.Exit(0);
            } 
                
        }
        // Quên mật khẩu
        private void lbForgotPassword_Click(object sender, EventArgs e)
        {
            this.Hide();
            _mainForm.ShowForm(new ForgotPasswordForm(_mainForm));
        }
    }
}
