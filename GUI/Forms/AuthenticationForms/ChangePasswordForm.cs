using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL.Services; 

namespace GUI.Forms.AuthenticationForms
{
    public partial class ChangePasswordForm : Form
    {
        private readonly MainForm.MainForm _mainForm;
        private AuthService _authService;
        private readonly string _email;
        public ChangePasswordForm(MainForm.MainForm mainForm, string email)
        {
            InitializeComponent();
            this._mainForm = mainForm;
            this._authService = new AuthService();
            this._email = email;
        }
        private void ChangePasswordForm_Load(object sender, EventArgs e)
        {
            tbNewPassword.Focus();
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

        private async void btnChangePassword_Click(object sender, EventArgs e)
        {
            string newPassword = tbNewPassword.Text;
            string confirmPassword = tbConfirmPassword.Text;

            if (string.IsNullOrEmpty(newPassword))
            {
                ShowError("Vui lòng nhập mật khẩu mới");
                return;
            }

            if (newPassword.Length < 6)
            {
                ShowError("Mật khẩu phải chứa ít nhất 6 ký tự");
                return;
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                ShowError("Vui lòng xác nhận mật khẩu");
                return;
            }

            if (newPassword != confirmPassword)
            {
                ShowError("Mật khẩu xác nhận không khớp");
                return;
            }

            // Gọi hàm thay đổi mật khẩu 
            _authService.ChangePassword(_email, newPassword);
            await _authService.ChangePasswordGoogleSheet();

            // Hiển thị thông báo thay đổi mật khẩu thành công
            MessageBox.Show("Thay đổi mật khẩu thành công. Hệ thống sẽ tự động chuyển về trang đăng nhập sau 2 giây.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Timer timer = new Timer();
            timer.Interval = 2000;
            timer.Tick += new EventHandler((object timerSender, EventArgs timerEventArgs) =>
            {
                timer.Stop();
                this.Hide();
                _mainForm.ShowForm(new LoginForm(_mainForm));

            });
            timer.Start();
        }
    }
}
