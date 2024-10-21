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
using System.Net;

namespace GUI.Forms.StudentForms
{
    public partial class StudentWeeksForm : Form
    {
        private readonly StudentService _studentService;
        private readonly AttendanceService _attendanceService; 
        private MainForm.MainDashboardForm _mainDashboardForm;
        private int _userID;
        private CourseDTO _course;
        private WebView2 webView;
        private int tempWeek;

        private string receivedLat;
        private string receivedLon;
        public StudentWeeksForm(MainForm.MainDashboardForm MainDashboardForm, int userID, CourseDTO courseDTO)
        {
            InitializeComponent();
            InitializeWebView();
            this._studentService = new StudentService();
            this._attendanceService = new AttendanceService();
            this._mainDashboardForm = MainDashboardForm;
            this._userID = userID;
            this._course = courseDTO;
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

                var coordinates = _attendanceService.GetClassCoordinates(_course.ClassID);
                string class_latitude = coordinates[0];
                string class_longitude = coordinates[1];

                // Ép kiểu thành double 
                double class_lat = Convert.ToDouble(class_latitude);
                double class_lon = Convert.ToDouble(class_longitude);
                double received_lat = Convert.ToDouble(this.receivedLat);
                double received_lon = Convert.ToDouble(this.receivedLon);

                //MessageBox.Show($"Tọa độ lớp học: {class_lat}, {class_lon}\nTọa độ nhận được: {received_lat}, {received_lon}");

                // Kiểm tra tọa độ có nằm trong bán kính không
                bool isInArea = IsPointInRadius(class_lat, class_lon, received_lat, received_lon, 10.0);

                string IPAddress = GetIPAddress();
                // Nếu Ip đã tồn tại thì không cho điểm danh
                if (_attendanceService.CheckIpAddress(IPAddress, tempWeek, _course.CourseID, _course.TeacherID, _course.ClassID))
                {
                    MessageBox.Show("Điểm danh thất bại.\nBạn đã điểm danh từ thiết bị khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (isInArea)
                {
                    // Cập nhật trạng thái điểm danh
                    bool isAttendance = _attendanceService.UpdateStatusAttendance(_userID, tempWeek, _course.GroupID, 1,  receivedLat, receivedLon, IPAddress);
                    if (isAttendance)
                    {
                        Console.WriteLine("Đang cập nhật điểm danh...");
                        // Cập nhật lên Google Sheets
                        await _attendanceService.SyncAttendancesDataToGoogleSheet();

                        // Refresh lại form
                        GetWeeks();
                    }
                    MessageBox.Show("Điểm danh thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Điểm danh thất bại.\nBạn không ở trong phạm vi điểm danh.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu không phân tích được JSON
                MessageBox.Show($"Lỗi phân tích JSON: {ex.Message}");
            }
        }
        private void StudentWeeksForm_Load(object sender, EventArgs e)
        {
            GetWeeks();
            _mainDashboardForm.HideDownloadButton();
        }
        // Lấy toàn bộ tuần học của môn học
        private void GetWeeks()
        {
            var weeks = _studentService.GetWeeks(_course.CourseID, _course.GroupID);

            tlpMain.Controls.Clear();
            tlpMain.AutoScroll = true;
            tlpMain.AutoSize = false;
            tlpMain.ColumnStyles.Clear();
            tlpMain.RowStyles.Clear();

            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            Color panelColor = Color.FromArgb(242, 244, 250);

            for (int i = 0; i < weeks.Count; i++)
            {
                var week = weeks[i];
                var announcements = _studentService.GetAnnouncements(week.WeekID);

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

                if (!_studentService.CheckAttendanceLink(week.WeekID, _course.CourseID, _course.TeacherID, _course.ClassID))
                {
                    checkInButton.Enabled = true;
                }
                else
                {
                    checkInButton.Text = "Chưa mở điểm danh";
                    checkInButton.Enabled = false;
                }


                // Kiểm tra xem Sinh viên đã điểm danh chưa
                if (_studentService.CheckAttendance(_userID, week.WeekID, _course.CourseID))
                {
                    checkInButton.Text = "Đã điểm danh";
                    checkInButton.Enabled = false;
                }

                weekPanel.Controls.Add(checkInButton, 0, announcements.Count + 1);

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
        // Xử lý sự kiện khi nhấn nút Điểm danh
        private void CheckInButton_Click(int weekID)
        {
            this.tempWeek = weekID;

            string filePath = Path.Combine(Application.StartupPath, "Resources", "Attendance", "DiemDanh.html");
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
        // Hàm tính khoảng cách giữa 2 điểm
        private double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double earthRadius = 6371000; // Bán kính Trái Đất tính bằng mét
            double dLat = (lat2 - lat1) * (Math.PI / 180);
            double dLon = (lon2 - lon1) * (Math.PI / 180);

            lat1 = lat1 * (Math.PI / 180);
            lat2 = lat2 * (Math.PI / 180);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = earthRadius * c;
            //MessageBox.Show($"Khoảng cách giữa 2 điểm là: {distance} mét");
            return distance;
        }
        // Hàm kiểm tra xem tọa độ có nằm trong bán kính hay không
        private bool IsPointInRadius(double centerLat, double centerLon, double checkLat, double checkLon, double radiusInMeters)
        {
            double distance = HaversineDistance(centerLat, centerLon, checkLat, checkLon);
            return distance <= radiusInMeters;
        }
        // Hàm lấy địa chỉ IP
        private static string GetIPAddress()
        {
            string IPAddress = string.Empty;
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    IPAddress = Convert.ToString(IP);
                }
            }
            return IPAddress;
        }
    }
}
