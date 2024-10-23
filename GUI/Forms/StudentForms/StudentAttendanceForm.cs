using BLL.Services;
using DTO;
using GUI.Forms.MainForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms.StudentForms
{
    public partial class StudentAttendanceForm : Form
    {
        private MainDashboardForm _mainDashboardForm;
        private StudentService _studentService;
        private int _userID;
        private int _termID;
        public StudentAttendanceForm(MainDashboardForm mainDashboardForm, int userID, int termID)
        {
            InitializeComponent();
            this._mainDashboardForm = mainDashboardForm;
            this._studentService = new StudentService();
            this._userID = userID;
            this._termID = termID;
        }

        private void StudentAttendanceForm_Load(object sender, EventArgs e)
        {
            ShowAttendanceList();
            _mainDashboardForm.UpdateBreadcrumb("/ Điểm danh");
        }
        // Hiển thị danh sách điểm danh lên DataGridView
        private void ShowAttendanceList()
        {
            var listAttendances = _studentService.GetListAttendances(_userID, _termID);

            if (listAttendances != null && listAttendances.Count > 0)
            {
                // Gán danh sách vào DataGridView
                dataGridViewAttendances.DataSource = listAttendances;

                // Điều chỉnh nội dung header
                dataGridViewAttendances.Columns["CourseName"].HeaderText = "Môn học";
                dataGridViewAttendances.Columns["DateAbsence"].HeaderText = "Ngày Vắng Mặt";
                dataGridViewAttendances.Columns["AbsenceCount"].HeaderText = "Số Buổi Vắng";

                // Chỉnh sửa giao diện
                dataGridViewAttendances.Font = new Font("Arial", 12);
                dataGridViewAttendances.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
                dataGridViewAttendances.DefaultCellStyle.Font = new Font("Arial", 12);
                dataGridViewAttendances.RowTemplate.Height = 40;

                // Màu nền trắng
                dataGridViewAttendances.BackgroundColor = Color.White;
                dataGridViewAttendances.DefaultCellStyle.BackColor = Color.White;
                dataGridViewAttendances.DefaultCellStyle.ForeColor = Color.Black;

                // Bật ngắt dòng cho cột DateAbsence
                dataGridViewAttendances.Columns["DateAbsence"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                // Cho phép kéo thả để thay đổi kích thước cột
                dataGridViewAttendances.AllowUserToResizeColumns = true;

                // Loại bỏ row headers (cột đầu tiên)
                dataGridViewAttendances.RowHeadersVisible = false;

                // Đặt chế độ chỉ đọc (không thể chỉnh sửa dữ liệu)
                dataGridViewAttendances.ReadOnly = true;

                // Tô màu tiêu đề (header)
                dataGridViewAttendances.EnableHeadersVisualStyles = false; // Đảm bảo màu tiêu đề được áp dụng
                dataGridViewAttendances.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue; // Thay đổi màu header
                dataGridViewAttendances.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                // Tô màu xen kẽ cho các hàng, bắt đầu từ dòng dữ liệu
                dataGridViewAttendances.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

                // Thêm viền cho các ô
                dataGridViewAttendances.CellBorderStyle = DataGridViewCellBorderStyle.Single; // Đặt kiểu viền cho từng ô
                dataGridViewAttendances.GridColor = Color.Black; // Đặt màu viền giữa các ô là màu đen

                // Đặt chế độ tự động điều chỉnh chiều cao hàng dựa trên nội dung
                dataGridViewAttendances.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
            else
            {
                MessageBox.Show("Không có dữ liệu điểm danh cho sinh viên này.");
            }
        }

    }
}
    