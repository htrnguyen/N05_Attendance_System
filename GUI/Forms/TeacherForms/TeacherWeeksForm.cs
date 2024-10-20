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
                    MessageBox.Show("Cập nhật tọa độ thành công!");

                    Console.WriteLine(weekIDTemp + " " + _course.CourseID + " " + _userID);
                    if (_attendanceService.UpdateAttendanceLinkStatus(weekIDTemp, _course.CourseID, _userID))
                    {
                        MessageBox.Show("Cập nhật trạng thái điểm danh thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật trạng thái điểm danh thất bại!");
                    }

                    await _attendanceService.UpdateClassesToGoogleSheet();
                    await _attendanceService.UpdateWeeksToGoogleSheet();

                    GetWeeks();

                }
                else
                {
                    MessageBox.Show("Cập nhật tọa độ thất bại!");
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
        }
        // Lấy toàn bộ tuần học của môn học
        private void GetWeeks()
        {
            this._weeks = _teacherService.GetWeeks(_course.CourseID);

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
                    ColumnCount = 1,
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
                }

                var checkInButton = new Button
                {
                    Text = "Điểm danh",
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    AutoSize = true,
                    Margin = new Padding(5),
                    Padding = new Padding(5),
                    MinimumSize = new Size(80, 25)
                };
                checkInButton.Click += (s, e) => CheckInButton_Click(week.WeekID);

                if (_teacherService.CheckAttendanceLink(week.WeekID, _course.CourseID, _course.TeacherID))
                {
                    checkInButton.Enabled = true;
                }
                else
                {
                    checkInButton.Text = "Đã điểm danh";
                    checkInButton.Enabled = false;
                }

                weekPanel.Controls.Add(checkInButton, 0, announcements.Count + 1);

                tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tlpMain.Controls.Add(weekPanel, 0, i);
            }
        }
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
        private void CheckInButton_Click(int weekID)
        {
            this.weekIDTemp = weekID;
            GetLocation();
        }
        private void GetLocation_Click(int weekID)
        {
            this.weekIDTemp = weekID;
            GetLocation();
        }
        // Hàm lấy toạ độ (kinh độ, vĩ độ)
        public void GetLocation()
        {
            string projectRoot = Directory.GetParent(Application.StartupPath).Parent.FullName;
            string filePath = Path.Combine(projectRoot, "Attendance", "DiemDanh.html");
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
