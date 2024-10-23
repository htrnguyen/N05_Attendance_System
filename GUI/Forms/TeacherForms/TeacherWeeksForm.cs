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
using Microsoft.VisualBasic;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System.IO;

namespace GUI.Forms.TeacherForms
{
    public partial class TeacherWeeksForm : Form
    {
        private readonly TeacherService _teacherService;
        private readonly AttendanceService _attendanceService;
        private MainForm.MainDashboardForm _mainDashboardForm;
        private int _userID;
        private CourseDTO _course;
        private List<WeekDTO> _weeks;

        private int weekIDTemp;
        private WebView2 webView;

        private string receivedLat;
        private string receivedLon;
        public TeacherWeeksForm(MainForm.MainDashboardForm MainDashboardForm, int userID, CourseDTO courseDTO)
        {
            InitializeComponent();
            InitializeWebView();
            this._teacherService = new TeacherService();
            this._attendanceService = new AttendanceService();
            this._mainDashboardForm = MainDashboardForm;
            this._userID = userID;
            this._course = courseDTO;

            this.receivedLat = "-1";
            this.receivedLon = "-1";
        }
        private async void InitializeWebView()
        {
            webView = new WebView2();
            webView.Size = new System.Drawing.Size(400, 200); // Thiết lập kích thước cho WebView2
            webView.Location = new System.Drawing.Point(10, 60); // Vị trí của WebView2
            this.Controls.Add(webView);
            await webView.EnsureCoreWebView2Async(null);

            // Lắng nghe sự kiện WebMessageReceived
            webView.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;
        }
        private async void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            // Lấy thông điệp từ JavaScript
            string message = e.TryGetWebMessageAsString();

            // Phân tích cú pháp JSON
            try
            {
                // Chuyển đổi chuỗi JSON thành đối tượng Dictionary
                var jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, double>>(message);

                // Lấy tọa độ từ đối tượng đã phân tích
                this.receivedLat = jsonData["latitude"].ToString();
                this.receivedLon = jsonData["longitude"].ToString();

                // Cập nhật tọa độ lên Google Sheets trong bảng Class 
                if (receivedLat != "-1" && receivedLon != "-1")
                {
                    _attendanceService.UpdateCoordinates(_course.ClassID, _course.CourseID, receivedLat, receivedLon);

                    if (_attendanceService.UpdateAttendanceLinkStatus(weekIDTemp, _course.CourseID, _userID))
                    {
                        await _attendanceService.UpdateClassesToGoogleSheet();
                        await _attendanceService.UpdateWeeksToGoogleSheet();

                        Console.WriteLine(_userID + " " + _course.CourseID + " " + _course.ClassID);
                        // Lấy danh sách sinh viên trong lớp học
                        var students = _attendanceService.GetStudentInClass(_userID, _course.CourseID, _course.ClassID);

                        foreach (var student in students)
                        {
                            _attendanceService.UpdateAttendance(student.UserID, weekIDTemp, _course.GroupID, 0, "-1", "-1", "-1");
                        }
                        MessageBox.Show("Tạo điểm danh thành công!");

                        await _attendanceService.SyncAttendancesDataToGoogleSheet();

                        GetWeeks();
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật trạng thái điểm danh thất bại!");
                    }
                }
                else
                {
                    MessageBox.Show("Tạo điểm danh thất bại!");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu không phân tích được JSON
                MessageBox.Show($"Lỗi phân tích JSON: {ex.Message}");
            }
        }
        private void TeacherWeeksForm_Load(object sender, EventArgs e)
        {
            GetWeeks();
            _mainDashboardForm.HideAttendanceButton();
            string courseName = "/ " + _course.CourseName + " - " + _course.CourseCode;
            _mainDashboardForm.UpdateBreadcrumb(courseName);
        }
        // Lấy toàn bộ tuần học của môn học
        private void GetWeeks()
        {
            this._weeks = _teacherService.GetWeeks(_course.CourseID, _course.GroupID);

            tlpMain.Controls.Clear();
            tlpMain.AutoScroll = true;
            tlpMain.AutoSize = false;
            tlpMain.ColumnStyles.Clear();
            tlpMain.RowStyles.Clear();

            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            Color panelColor = Color.FromArgb(242, 244, 250);

            for (int i = 0; i < _weeks.Count; i++)
            {
                var week = _weeks[i];
                var announcements = _teacherService.GetAnnouncements(week.WeekID);

                var weekPanel = new TableLayoutPanel
                {
                    ColumnCount = 3,  // Thêm 3 cột để chứa các nút
                    RowCount = 1 + announcements.Count + 1,
                    Dock = DockStyle.Top,
                    AutoSize = true,
                    Margin = new Padding(0, 0, 0, 15),
                    BackColor = panelColor,
                    CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                    Tag = week
                };

                var weekLabel = CreateLabel($"Tuần {week.WeekNumber}", true, Color.FromArgb(0, 102, 204));
                weekPanel.Controls.Add(weekLabel, 0, 0);

                for (int j = 0; j < announcements.Count; j++)
                {
                    var announcement = announcements[j];
                    var announcementLabel = CreateLabel(announcement.Content, false, Color.Black);
                    weekPanel.Controls.Add(announcementLabel, 0, j + 1);

                    // Thêm nút chỉnh sửa
                    var editButton = new Button
                    {
                        Text = "Chỉnh sửa",
                        Font = new Font("Arial", 10, FontStyle.Regular),
                        AutoSize = true,
                        Margin = new Padding(5),
                        Padding = new Padding(5),
                        MinimumSize = new Size(80, 25)
                    };
                    editButton.Click += (s, e) => EditAnnouncement_Click(announcement.AnnouncementID, announcement.Content);

                    // Thêm nút xóa
                    var deleteButton = new Button
                    {
                        Text = "Xóa",
                        Font = new Font("Arial", 10, FontStyle.Regular),
                        AutoSize = true,
                        Margin = new Padding(5),
                        Padding = new Padding(5),
                        MinimumSize = new Size(80, 25)
                    };
                    deleteButton.Click += (s, e) => DeleteAnnouncement_Click(announcement.AnnouncementID);

                    // Thêm các nút vào tuần học
                    weekPanel.Controls.Add(editButton, 1, j + 1);
                    weekPanel.Controls.Add(deleteButton, 2, j + 1);
                }

                // Tạo nút "Tạo điểm danh"
                var checkInButton = new Button
                {
                    Text = "Tạo điểm danh",
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding(5),
                    Padding = new Padding(5),
                    MinimumSize = new Size(80, 25)
                };
                checkInButton.Click += (s, e) => CheckInButton_Click(week.WeekID);

                if (_teacherService.CheckAttendanceLink(week.WeekID, _course.CourseID, _course.TeacherID, _course.ClassID))
                {
                    checkInButton.Enabled = true;
                }
                else
                {
                    checkInButton.Text = "Đã tạo điểm danh";
                    checkInButton.Enabled = false;
                }

                weekPanel.Controls.Add(checkInButton, 0, announcements.Count + 1);

                // Tạo nút "Thêm thông báo"
                var addAnnouncementButton = new Button
                {
                    Text = "Thêm thông báo",
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding(5),
                    Padding = new Padding(5),
                    MinimumSize = new Size(100, 25)
                };
                addAnnouncementButton.Click += (s, e) => AddAnnouncementButton_Click(week.WeekID);

                weekPanel.Controls.Add(addAnnouncementButton, 1, announcements.Count + 1);

                tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tlpMain.Controls.Add(weekPanel, 0, i);
            }
        }
        // Tạo label
        private Label CreateLabel(string text, bool isTitle = false, Color textColor = default)
        {
            var label = new Label
            {
                Text = text,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Arial", isTitle ? 14 : 10, isTitle ? FontStyle.Bold : FontStyle.Regular),
                ForeColor = textColor,
                AutoEllipsis = true,
                Margin = new Padding(2),
                Padding = new Padding(5)
            };

            return label;
        }
        // Tạo nút
        private void CheckInButton_Click(int weekID)
        {
            this.weekIDTemp = weekID;
            GetLocation();
        }
        // Thêm thông báo
        private async void AddAnnouncementButton_Click(int weekID)
        {
            string content = Interaction.InputBox("Nhập nội dung thông báo:", "Thêm thông báo", "");

            if (!string.IsNullOrEmpty(content))
            {
                var result = _teacherService.AddAnnouncement(weekID, content);
                if (result)
                {
                    MessageBox.Show("Thêm thông báo thành công!");

                    // Đồng bộ dữ liệu thông báo lên Google Sheets
                    await _teacherService.SyncAnnouncementsDataToGoogleSheet();

                    GetWeeks(); // Tải lại danh sách tuần để hiển thị thông báo mới
                }
                else
                {
                    MessageBox.Show("Thêm thông báo thất bại!");
                }
            }
        }
        // Chỉnh sửa thông báo
        private async void EditAnnouncement_Click(int announcementID, string currentContent)
        {
            string newContent = Interaction.InputBox("Chỉnh sửa nội dung thông báo:", "Chỉnh sửa thông báo", currentContent);

            if (!string.IsNullOrEmpty(newContent))
            {
                var result = _teacherService.UpdateAnnouncement(announcementID, newContent);
                if (result)
                {
                    MessageBox.Show("Chỉnh sửa thông báo thành công!");

                    // Lấy WeekID của thông báo vừa chỉnh sửa
                    var weekID = _teacherService.UpdateAnnouncement(announcementID, newContent);

                    // Đồng bộ dữ liệu thông báo lên Google Sheets
                    await _teacherService.SyncAnnouncementsDataToGoogleSheet();

                    GetWeeks(); // Tải lại danh sách tuần để hiển thị thông báo đã chỉnh sửa
                }
                else
                {
                    MessageBox.Show("Chỉnh sửa thông báo thất bại!");
                }
            }
        }
        // Xoá thông báo
        private async void DeleteAnnouncement_Click(int announcementID)
        {
            var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa thông báo này?", "Xác nhận xóa", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                var result = _teacherService.DeleteAnnouncement(announcementID);
                if (result)
                {
                    MessageBox.Show("Xóa thông báo thành công!");

                    // Lấy WeekID của thông báo vừa xóa
                    var weekID = _teacherService.DeleteAnnouncement(announcementID);

                    // Đồng bộ dữ liệu thông báo lên Google Sheets
                    await _teacherService.SyncAnnouncementsDataToGoogleSheet();

                    GetWeeks(); // Tải lại danh sách tuần để hiển thị thông báo sau khi xóa
                }
                else
                {
                    MessageBox.Show("Xóa thông báo thất bại!");
                }
            }
        }
        // Tạo điểm danh
        private void GetLocation_Click(int weekID)
        {
            this.weekIDTemp = weekID;
            GetLocation();
        }
        // Hàm lấy toạ độ (kinh độ, vĩ độ)
        public void GetLocation()
        {
            string filePath = Path.Combine(Application.StartupPath, "Resources", "Attendance", "DiemDanh.html");
            Console.WriteLine(filePath);
            webView.Source = new Uri(filePath);

            // Đảm bảo trang web đã tải xong trước khi inject JavaScript
            webView.CoreWebView2.NavigationCompleted += async (s, args) =>
            {
                if (args.IsSuccess)
                {
                    // Inject và gọi hàm JavaScript getLocation() tự động
                    await webView.CoreWebView2.ExecuteScriptAsync("getLocation();");
                }
            };
        }
    }
}
    