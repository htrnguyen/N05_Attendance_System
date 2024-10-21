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
            labelRole.Text = _user.Role;
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
            // Sử dụng SaveFileDialog để yêu cầu người dùng chọn đường dẫn và tên file
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx";
                saveFileDialog.Title = "Lưu báo cáo điểm danh";
                saveFileDialog.FileName = "AttendanceReport.xlsx"; // Tên file mặc định

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Người dùng đã chọn đường dẫn và tên file
                    string filePath = saveFileDialog.FileName;

                    // Kiểm tra xem file có đang được sử dụng bởi một quá trình khác không
                    if (IsFileInUse(filePath))
                    {
                        MessageBox.Show("File đang được mở trong một ứng dụng khác. Vui lòng đóng file và thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage())
                    {
                        foreach (var courseData in attendanceDataByCourses)
                        {
                            string courseKey = courseData.Key;
                            List<AttendanceDTO> attendanceList = courseData.Value;

                            var worksheet = package.Workbook.Worksheets.Add(courseKey);

                            // Tạo header cho sheet
                            worksheet.Cells[1, 1].Value = "Tên sinh viên";
                            worksheet.Cells[1, 2].Value = "Tuần 1";
                            worksheet.Cells[1, 3].Value = "Tuần 2";
                            worksheet.Cells[1, 4].Value = "Tuần 3";
                            worksheet.Cells[1, 5].Value = "Tuần 4";
                            worksheet.Cells[1, 6].Value = "Tuần 5";
                            worksheet.Cells[1, 7].Value = "Tuần 6";
                            worksheet.Cells[1, 8].Value = "Tuần 7";
                            worksheet.Cells[1, 9].Value = "Tuần 8";
                            worksheet.Cells[1, 10].Value = "Tuần 9";
                            worksheet.Cells[1, 11].Value = "Tuần 10";
                            worksheet.Cells[1, 12].Value = "Tuần 11";
                            worksheet.Cells[1, 13].Value = "Tuần 12";
                            worksheet.Cells[1, 14].Value = "Tuần 13";
                            worksheet.Cells[1, 15].Value = "Tuần 14";
                            worksheet.Cells[1, 16].Value = "Tuần 15";

                            // Điền dữ liệu điểm danh vào sheet
                            int row = 2;
                            foreach (var attendance in attendanceList)
                            {
                                worksheet.Cells[row, 1].Value = attendance.StudentName;
                                worksheet.Cells[row, 2].Value = attendance.Week1;
                                worksheet.Cells[row, 3].Value = attendance.Week2;
                                worksheet.Cells[row, 4].Value = attendance.Week3;
                                worksheet.Cells[row, 5].Value = attendance.Week4;
                                worksheet.Cells[row, 6].Value = attendance.Week5;
                                worksheet.Cells[row, 7].Value = attendance.Week6;
                                worksheet.Cells[row, 8].Value = attendance.Week7;
                                worksheet.Cells[row, 9].Value = attendance.Week8;
                                worksheet.Cells[row, 10].Value = attendance.Week9;
                                worksheet.Cells[row, 11].Value = attendance.Week10;
                                worksheet.Cells[row, 12].Value = attendance.Week11;
                                worksheet.Cells[row, 13].Value = attendance.Week12;
                                worksheet.Cells[row, 14].Value = attendance.Week13;
                                worksheet.Cells[row, 15].Value = attendance.Week14;
                                worksheet.Cells[row, 16].Value = attendance.Week15;
                                row++;
                            }
                        }

                        // Lưu file Excel ra đường dẫn filePath đã chọn
                        FileInfo file = new FileInfo(filePath);
                        package.SaveAs(file);
                    }

                    //MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        // Hàm kiểm tra file có đang được mở bởi một quá trình khác hay không
        private bool IsFileInUse(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    fs.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }

        private void lbAttendance_Click(object sender, EventArgs e)
        {
            Console.WriteLine(_termID);
            ShowForm(new StudentAttendanceForm(this, _user.UserID, _termID));
        }
    }
}
