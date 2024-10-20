using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DAL.Repositories;
using DTO;

namespace BLL.Services
{
    public class AuthService
    {
        private UserDAL _userDAL;

        public AuthService()
        {
            this._userDAL = new UserDAL();
        }
        // Xử lý đăng nhập
        public bool Login(string username, string password)
        {
            return _userDAL.isLogin(username, password);
        }
        // Kiểm tra email có  tồn tại không
        public bool IsEmailExist(string email)
        {
            return _userDAL.isEmailExist(email);
        }
        // Random mã xác nhận
        public string GenerateCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        // Xử lý gửi mã xác nhận
        public bool SendVerificationEmail(string recipientEmail, string verificationCode)
        {
            var fromAddress = new MailAddress("hatrongnguyen04@gmail.com", "Hà Trọng Nguyễn");
            var toAddress = new MailAddress(recipientEmail);
            const string fromPassword = "vgws uaqj zgxj wywv";
            const string subject = "Mã xác nhận của bạn";

            // Định dạng lại nội dung email với HTML
            string body = $@"
            <html>
              <body>
                <h2 style='color: #333;'>Xin chào,</h2>
                <p style='font-size: 14px;'>Mã xác nhận của bạn là:</p>
                <p style='font-size: 20px; font-weight: bold; color: #4CAF50;'>{verificationCode}</p>
                <p>Nếu bạn không yêu cầu mã này, hãy bỏ qua email này.</p>
                <br />  
                <p>Trân trọng,</p>
                <p>Đội ngũ hỗ trợ</p>
              </body>
            </html>";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(message);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }
        // Thay đổi mật khẩu
        public void ChangePassword(string email, string newPassword)
        {
            _userDAL.ChangePassword(email, newPassword);
        }
        // Thay đổi mật khẩu lên Google Sheet
        public async Task ChangePasswordGoogleSheet()
        {
            await _userDAL.ChangePasswordGoogleSheet();
        }
        // Lấy thông tin User
        public UserDTO GetUser(string username)
        {
            return _userDAL.GetUser(username);
        }

    }
}
