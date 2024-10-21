namespace GUI.Forms.StudentForms
{
    partial class StudentAttendanceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewAttendances = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAttendances)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewAttendances
            // 
            this.dataGridViewAttendances.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAttendances.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewAttendances.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewAttendances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAttendances.GridColor = System.Drawing.Color.White;
            this.dataGridViewAttendances.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewAttendances.Name = "dataGridViewAttendances";
            this.dataGridViewAttendances.RowHeadersWidth = 62;
            this.dataGridViewAttendances.RowTemplate.Height = 28;
            this.dataGridViewAttendances.Size = new System.Drawing.Size(1181, 643);
            this.dataGridViewAttendances.TabIndex = 0;
            // 
            // StudentAttendanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1205, 667);
            this.Controls.Add(this.dataGridViewAttendances);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(42)))), ((int)(((byte)(122)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "StudentAttendanceForm";
            this.Text = "StudentAttendanceForm";
            this.Load += new System.EventHandler(this.StudentAttendanceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAttendances)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewAttendances;
    }
}