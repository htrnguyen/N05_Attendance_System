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
using System.Globalization;
using BLL.Services;
using GUI.Forms.StudentForms;
using GUI.Forms.TeacherForms;
using System.IO;
using OfficeOpenXml;

namespace GUI.Forms.MainForm
{
    public partial class MainDashboardForm : Form
    {
        private DataLoader _dataLoader;
        private TermService _termService;
        private TeacherService _teacherService;
        private AttendanceService _attendanceService;
        private UserDTO _user;
        private int _termID;

        public MainDashboardForm(UserDTO user)
        {
            InitializeComponent();
            this._user = user;
            this._dataLoader = new DataLoader();
            this._termService = new TermService();
            this._teacherService = new TeacherService();
            this._attendanceService = new AttendanceService();
        }
        private void MainDashboardForm_Load(object sender, EventArgs e)
        {
            ShowUserInfo();
            Showtime();
            LoadTerms();
        }
        // Hiển thị thông tin 
        private void ShowUserInfo()
        {
            labelFullName.Text = _user.FullName;
            //labelRole.Text = _user.Role;
            if (_user.Role == "Student")
            {
                labelRole.Text = "Sinh viên";
            }
            else if (_user.Role == "Teacher")
            {
                labelRole.Text = "Giáo viên";
            }
        }
        // Hiển thị thời gian hiện tại  
        private void Showtime()
        {
            CultureInfo viCulture = new CultureInfo("vi-VN");
            labelDateTime.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy", viCulture);
        }
        // Lấy thông tin kỳ học 
        private void LoadTerms()
        {
            List<TermDTO> terms = _termService.GetAllTermsByUserID(_user.UserID);
            foreach (TermDTO term in terms)
            {
                cbbTerms.Items.Add(term.TermName);
            }
            cbbTerms.SelectedIndex = 0;
        }
        // Hiển thị form
        public void ShowForm(Form form)
        {
            panelMain.Controls.Clear();
            form.TopLevel = false;
            panelMain.Controls.Add(form);
            form.Show();
        }
        // Đăng xuất
        private void lbLogout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đăng xuất thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void cbbTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy giá trị của kỳ học
            var termName = cbbTerms.SelectedItem.ToString();
            // Lấy ID của kỳ học
            this._termID = _termService.GetTermIDByTermName(termName);

            if (_user.Role == "Student")
            {
                Console.WriteLine(_termID);  
                ShowForm(new StudentCoursesForm(this, _user.UserID, _termID));
            }
            else if (_user.Role == "Teacher")
            {
                Console.WriteLine(_user.UserID);
                ShowForm(new TeacherCoursesForm(this, _user.UserID, _termID));
            }
        }
        private void lbDashboard_Click(object sender, EventArgs e)
        {
            cbbTerms_SelectedIndexChanged(sender, e);
        }
        public void HideDownloadButton()
        {
            panelDownload.Visible = false;
        }
        public void HideAttendanceButton()
        {
            panelAttendance.Visible = false;
        }
        private void lbLogout_Click_1(object sender, EventArgs e)
        {
            // Quay lại form đăng nhập
            this.Hide();
            MainForm mainForm = new MainForm();
            mainForm.ShowDialog();
            this.Close();
        }
        // Đổi mật khẩu
        private void lbChangePassword_Click(object sender, EventArgs e)
        {
            ShowForm(new AuthenticationForms.ChangePasswordDashBoardForm(this, _user.Email));
        }
        // Reload dữ liệu
        private async void pictureBoxReload_Click(object sender, EventArgs e)
        {
            // Hiệu ứng reload, đổi ảnh reload
            pictureBoxReload.Image = Properties.Resources.icons8_reload_50;
            await _dataLoader.LoadClassesAndWeeksAsync();
            Console.WriteLine("Reloaded");
            pictureBoxReload.Image = Properties.Resources.icons8_rotate_right_50;
        }
        // Xuất file điểm danh  
        private void lbDownload_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu điểm danh theo từng môn học
            var attendanceDataByCourses = GetAttendanceDataByCourses(_user.UserID, _termID);
            ExportAttendanceToExcel(attendanceDataByCourses);
        }
        // Lấy dữ liệu điểm danh theo từng môn học
        public Dictionary<string, List<AttendanceDTO>> GetAttendanceDataByCourses(int teacherID, int termID)
        {
            Dictionary<string, List<AttendanceDTO>> attendanceDataByCourses = new Dictionary<string, List<AttendanceDTO>>();

            var courses = _teacherService.GetCourses(teacherID, termID);

            foreach (var course in courses)
            {
                var attendanceList = _attendanceService.GetAttendances(teacherID, course.CourseID, course.ClassID);

                // Tạo một khóa duy nhất dựa trên CourseID và ClassID
                string uniqueKey = $"{course.CourseName}-{course.CourseCode}|{course.GroupName}-{course.ClassName}";

                // Thêm dữ liệu vào từ điển với khóa mới
                attendanceDataByCourses[uniqueKey] = attendanceList;
            }
            Console.WriteLine("Count: " + attendanceDataByCourses.Count);

            return attendanceDataByCourses;
        }
        public void ExportAttendanceToExcel(Dictionary<string, List<AttendanceDTO>> attendanceDataByCourses)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Lưu báo cáo điểm danh";
                saveFileDialog.FileName = "Danh sách điểm danh.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage())
                    {
                        foreach (var courseData in attendanceDataByCourses)
                        {
                            string courseKey = courseData.Key;
                            List<AttendanceDTO> attendanceList = courseData.Value;

                            var worksheet = package.Workbook.Worksheets.Add(courseKey);

                            // Tạo header cho sheet và làm cho tiêu đề dễ nhìn hơn
                            worksheet.Cells[1, 1].Value = "Tên sinh viên";
                            for (int i = 1; i <= 15; i++)
                            {
                                worksheet.Cells[1, i + 1].Value = $"Tuần {i}";
                            }
                            worksheet.Cells[1, 17].Value = "Tổng số vắng";

                            using (var headerRange = worksheet.Cells[1, 1, 1, 17])
                            {
                                headerRange.Style.Font.Bold = true;
                                headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                                headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                headerRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                headerRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            }

                            int row = 2;
                            foreach (var attendance in attendanceList)
                            {
                                worksheet.Cells[row, 1].Value = attendance.StudentName;

                                int totalAbsences = 0;

                                for (int i = 1; i <= 15; i++)
                                {
                                    var weekValue = attendance.GetType().GetProperty($"Week{i}").GetValue(attendance);
                                    int weekAttendance;

                                    if (int.TryParse(weekValue?.ToString(), out weekAttendance))
                                    {
                                        string status = weekAttendance == 0 ? "Vắng" : "Có mặt";
                                        worksheet.Cells[row, i + 1].Value = status;

                                        // Tô vàng cho các ngày vắng
                                        if (weekAttendance == 0)
                                        {
                                            worksheet.Cells[row, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                            worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                            totalAbsences++;
                                        }
                                    }
                                    else
                                    {
                                        worksheet.Cells[row, i + 1].Value = "";
                                    }
                                }

                                // Ghi tổng số lần vắng vào cột sau tuần 15
                                worksheet.Cells[row, 17].Value = totalAbsences;

                                // Nếu sinh viên vắng đủ 3 ngày trở lên thì tô đỏ cả hàng
                                if (totalAbsences >= 3)
                                {
                                    worksheet.Cells[row, 1, row, 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[row, 1, row, 17].Style.Fill.BackgroundColor.SetColor(Color.LightCoral);
                                }

                                row++;
                            }

                            // Cố định cột tên sinh viên và dòng tiêu đề
                            worksheet.View.FreezePanes(2, 2);

                            // Căn chỉnh format cho các ô
                            using (var range = worksheet.Cells[1, 1, row - 1, 17])
                            {
                                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                range.AutoFitColumns(); // Tự động chỉnh độ rộng cột
                            }
                        }

                        FileInfo file = new FileInfo(filePath);
                        package.SaveAs(file);
                    }
                }
            }
        }
        private void lbAttendance_Click(object sender, EventArgs e)
        {
            Console.WriteLine(_termID);
            ShowForm(new StudentAttendanceForm(this, _user.UserID, _termID));
        }
        // Cập nhật breadcrumb khi click vào course
        public void UpdateBreadcrumb(string courseName)
        {
            lbBreadcrumb.Text = $"Trang Chủ {courseName}";
        }
    }
}
