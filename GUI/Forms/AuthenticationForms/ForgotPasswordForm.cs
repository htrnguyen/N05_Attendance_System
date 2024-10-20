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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;


namespace GUI.Forms.AuthenticationForms
{
    public partial class ForgotPasswordForm : Form
    {
        private readonly MainForm.MainForm _mainForm;
        private readonly AuthService _authService;
        private string code;
        public ForgotPasswordForm(MainForm.MainForm mainForm)
        {
            InitializeComponent();
            this._mainForm = mainForm;
            this._authService = new AuthService();

            // Enter để xác nhận
            tbEmail.KeyDown += new KeyEventHandler(tb_KeyDown);
            tbCode.KeyDown += new KeyEventHandler(tb_KeyDown);
        }
        private void ForgotPasswordForm_Load(object sender, EventArgs e)
        {
            tbEmail.Focus();
            HideError();

            // Chỉ được nhập số
            tbCode.KeyPress += new KeyPressEventHandler(tbCode_KeyPress);

            // Đặt thời gian hết hạn là 5 phút kể từ bây giờ
            DateTime expirationTime = DateTime.Now.AddMinutes(2);

            Timer timer = new Timer();
            timer.Interval = 1000; // 1 giây
            timer.Tick += new EventHandler((object timerSender, EventArgs timerEventArgs) =>
            {
                if (code != null)
                {
                    TimeSpan remainingTime = expirationTime - DateTime.Now;

                    if (remainingTime.TotalSeconds > 0)
                    {
                        // Hiển thị thời gian còn lại theo định dạng phút:giây
                        ShowError($"Mã xác nhận sẽ hết hạn sau {remainingTime.Minutes:D2}:{remainingTime.Seconds:D2} phút");
                    }
                    else
                    {
                        // Khi thời gian hết hạn, dừng Timer và thông báo mã đã hết hạn
                        timer.Stop();
                        ShowError("Mã xác nhận đã hết hạn.");
                        code = null;

                        picbReceiveCode.Enabled = true;
                        lbNameReceiveCode.Enabled = true;
                    }
                }
            });
            timer.Start();

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
        // Nhấn Enter để xác nhận
        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnConfirm_Click(sender, e);
            }
        }
        // Xác nhận
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            HideError();
            string email = tbEmail.Text;

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowError("Email không được để trống");
                return;
            }

            if (!_authService.IsEmailExist(email))
            {
                ShowError("Email không tồn tại");
                return;
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                ShowError("Vui lòng nhấn gửi mã xác nhận");
                return;
            }

            // Kiểm tra mã xác nhận
            if (tbCode.Text != code)
            {
                ShowError("Mã xác nhận không đúng");
                return;
            }

            HideError();
            ShowError("Đang chuyển hướng đến trang đổi mật khẩu...");
        }
        private void picbReceiveCode_Click(object sender, EventArgs e)
        {
            // Tắt nhấn nút gửi mã xác nhận
            picbReceiveCode.Enabled = false;
            lbNameReceiveCode.Enabled = false;

            if (string.IsNullOrEmpty(code))
            {
                code = _authService.GenerateCode();
            }
            _authService.SendVerificationEmail(tbEmail.Text, code);
            ShowError("Mã xác nhận đã được gửi đến email của bạn");
        }
        private void lbNameReceiveCode_Click(object sender, EventArgs e)
        {
            picbReceiveCode_Click(sender, e);
        }
        // Chỉ được nhập số và tối đa 6 ký tự
        private void tbCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) || tbCode.Text.Length >= 6)
            {
                e.Handled = true;
            }
        }
    }
}
