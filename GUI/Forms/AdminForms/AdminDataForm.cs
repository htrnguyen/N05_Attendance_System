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

namespace GUI.Forms.AdminForms
{
    public partial class AdminDataForm : Form
    {
        private AdminService _adminService;
        public AdminDataForm()
        {
            InitializeComponent();
            this._adminService = new AdminService();
        } 

        private void AdminDataForm_Load(object sender, EventArgs e)
        {
            LoadTableNames();
        }

        public void LoadTableNames()
        {
            var tableNames = _adminService.GetTableNames();
            // Thêm vào combobox trừ "sqlite_sequence"
            foreach (var tableName in tableNames)
            {
                if (tableName != "sqlite_sequence")
                {
                    cbTableName.Items.Add(tableName);
                }
            }
            cbTableName.SelectedIndex = 0;      
        }
        // Hiển thị dữ liệu từ bảng
        private void cbTableName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tableName = cbTableName.SelectedItem.ToString();
            Console.WriteLine(tableName);

            var tableData = _adminService.GetTableData(tableName);
            dataGridView.DataSource = tableData;

            // Chỉnh sửa giao diện DataGridView
            dataGridView.Font = new Font("Arial", 12);
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            dataGridView.DefaultCellStyle.Font = new Font("Arial", 12);
            dataGridView.RowTemplate.Height = 40;

            // Màu nền và màu chữ
            dataGridView.BackgroundColor = Color.White;
            dataGridView.DefaultCellStyle.BackColor = Color.White;
            dataGridView.DefaultCellStyle.ForeColor = Color.Black;

            // Màu tiêu đề
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue; // Thay đổi màu tiêu đề
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

            // Tô màu xen kẽ cho các hàng
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // Thêm viền cho các ô
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView.GridColor = Color.Black;

            // Đặt chế độ tự động điều chỉnh chiều cao hàng dựa trên nội dung
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Loại bỏ row headers (cột đầu tiên)
            dataGridView.RowHeadersVisible = false;

            // Đặt chế độ chỉ đọc (không thể chỉnh sửa dữ liệu)
            dataGridView.ReadOnly = true;

            // Cho phép kéo thả để thay đổi kích thước cột
            dataGridView.AllowUserToResizeColumns = true;

            // Điều chỉnh chiều rộng của tất cả các cột dựa trên nội dung
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // Tự động điều chỉnh chiều rộng
            }

            // Sau khi đặt AutoSizeMode, có thể điều chỉnh lại chiều rộng tổng thể nếu cần
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Để lấp đầy không gian
        }

    }
}
