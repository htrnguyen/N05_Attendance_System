namespace GUI.Forms.AdminForms
{
    partial class LoadingForm
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
            this.labelLoading = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelLoading
            // 
            this.labelLoading.AutoSize = true;
            this.labelLoading.Font = new System.Drawing.Font("Segoe UI", 28F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLoading.Location = new System.Drawing.Point(77, 73);
            this.labelLoading.Name = "labelLoading";
            this.labelLoading.Size = new System.Drawing.Size(647, 74);
            this.labelLoading.TabIndex = 0;
            this.labelLoading.Text = "Đang tải, vui lòng đợi...";
            // 
            // LoadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 220);
            this.Controls.Add(this.labelLoading);
            this.Name = "LoadingForm";
            this.Text = "LoadingForm";
            this.Load += new System.EventHandler(this.LoadingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelLoading;
    }
}