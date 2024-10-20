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

namespace GUI.Forms.MainForm
{
    public partial class MainDashboardForm : Form
    {
        private TermService _termService;
        private UserDTO _user;

        public MainDashboardForm(UserDTO user)
        {
            InitializeComponent();
            this._user = user;
            this._termService = new TermService();
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
            var termID = _termService.GetTermIDByTermName(termName);

            if (_user.Role == "Sinh viên")
            {
                Console.WriteLine(termID);  
                ShowForm(new StudentCoursesForm(this, _user.UserID, termID));
            }
            else if (_user.Role == "Giáo viên")
            {
                Console.WriteLine(_user.UserID);
                ShowForm(new TeacherCoursesForm(this, _user.UserID, termID));
            }
        }
        private void lbDashboard_Click(object sender, EventArgs e)
        {
            cbbTerms_SelectedIndexChanged(sender, e);
        }
    }
}
