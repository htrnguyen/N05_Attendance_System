using BLL.Services;
using DTO;
using GUI.Forms.AdminForms;
using GUI.Forms.AuthenticationForms;
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
        private AdminService _adminService;
        public AdminForm(UserDTO user)
        {
            InitializeComponent();
            this._adminService = new AdminService();
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
        // Đăng xuất
        private void lbLogout_Click(object sender, EventArgs e)
        {
            // Quay lại form đăng nhập
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
            this.Close();
        }
        // Tải lên danh sách
        private async void labelUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";
                openFileDialog.Title = "Chọn file Excel để tải lên";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;

                    // Kiểm tra định dạng file Excel
                    if (!_adminService.ValidateExcelFormat(filePath))
                    {
                        MessageBox.Show("File Excel không đúng định dạng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Hiển thị màn hình đang tải
                    LoadingForm loadingForm = new LoadingForm();
                    loadingForm.Show();

                    // Cấu hình ánh xạ giữa các sheet trong Excel và Google Sheets
                    Dictionary<string, string> sheetMappings = new Dictionary<string, string>
                        {
                            { "Roles", "Roles!A1:B" },
                            { "Users", "Users!A1:F" },
                            { "Terms", "Terms!A1:D" },
                            { "Courses", "Courses!A1:D" },
                            { "CourseAssignments", "CourseAssignments!A1:C" },
                            { "Classes", "Classes!A1:E" },
                            { "Groups", "Groups!A1:E" },
                            { "Enrollments", "Enrollments!A1:C" },
                            { "Weeks", "Weeks!A1:G" },
                            { "Announcements", "Announcements!A1:C" },
                            { "Attendances", "Attendances!A1:I" }
                        };

                    try
                    {
                        // Thực hiện đồng bộ dữ liệu lên Google Sheets
                        await _adminService.SyncAllSheetsFromExcelToGoogleSheets(filePath, sheetMappings);
                        MessageBox.Show("Dữ liệu đã được tải lên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Đã xảy ra lỗi khi đồng bộ: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        // Ẩn màn hình đang tải
                        loadingForm.Close();
                    }
                }
            }
        }
    }
}
