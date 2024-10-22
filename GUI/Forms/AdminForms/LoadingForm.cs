using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms.AdminForms
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ControlBox = false; // Loại bỏ nút đóng
            this.Text = string.Empty; // Loại bỏ tiêu đề
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Loại bỏ khả năng thay đổi kích thước
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {

        }
    }
}
