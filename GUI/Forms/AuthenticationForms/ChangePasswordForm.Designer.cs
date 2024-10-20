namespace GUI.Forms.AuthenticationForms
{
    partial class ChangePasswordForm
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
            this.pnError = new System.Windows.Forms.Panel();
            this.lbMessageError = new System.Windows.Forms.Label();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.panelLine2 = new System.Windows.Forms.Panel();
            this.tbConfirmPassword = new System.Windows.Forms.TextBox();
            this.panelLine1 = new System.Windows.Forms.Panel();
            this.tbNewPassword = new System.Windows.Forms.TextBox();
            this.lbDescribe = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.pictureBoxError = new System.Windows.Forms.PictureBox();
            this.picbUsername = new System.Windows.Forms.PictureBox();
            this.picboxPassword = new System.Windows.Forms.PictureBox();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.pnError.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbUsername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picboxPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnError
            // 
            this.pnError.Controls.Add(this.lbMessageError);
            this.pnError.Controls.Add(this.pictureBoxError);
            this.pnError.Location = new System.Drawing.Point(67, 389);
            this.pnError.Name = "pnError";
            this.pnError.Size = new System.Drawing.Size(448, 54);
            this.pnError.TabIndex = 67;
            this.pnError.Visible = false;
            // 
            // lbMessageError
            // 
            this.lbMessageError.AutoSize = true;
            this.lbMessageError.Font = new System.Drawing.Font("Calibri", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMessageError.ForeColor = System.Drawing.Color.Red;
            this.lbMessageError.Location = new System.Drawing.Point(50, 16);
            this.lbMessageError.Name = "lbMessageError";
            this.lbMessageError.Size = new System.Drawing.Size(0, 22);
            this.lbMessageError.TabIndex = 13;
            // 
            // btnChangePassword
            // 
            this.btnChangePassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(253)))));
            this.btnChangePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChangePassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangePassword.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangePassword.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnChangePassword.Location = new System.Drawing.Point(67, 484);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(448, 65);
            this.btnChangePassword.TabIndex = 66;
            this.btnChangePassword.Text = "Đổi mật khẩu";
            this.btnChangePassword.UseVisualStyleBackColor = false;
            this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
            // 
            // panelLine2
            // 
            this.panelLine2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(253)))));
            this.panelLine2.Location = new System.Drawing.Point(71, 376);
            this.panelLine2.Name = "panelLine2";
            this.panelLine2.Size = new System.Drawing.Size(448, 3);
            this.panelLine2.TabIndex = 64;
            // 
            // tbConfirmPassword
            // 
            this.tbConfirmPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(247)))), ((int)(((byte)(253)))));
            this.tbConfirmPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbConfirmPassword.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbConfirmPassword.Location = new System.Drawing.Point(130, 330);
            this.tbConfirmPassword.Name = "tbConfirmPassword";
            this.tbConfirmPassword.Size = new System.Drawing.Size(385, 40);
            this.tbConfirmPassword.TabIndex = 63;
            // 
            // panelLine1
            // 
            this.panelLine1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(253)))));
            this.panelLine1.Location = new System.Drawing.Point(71, 279);
            this.panelLine1.Name = "panelLine1";
            this.panelLine1.Size = new System.Drawing.Size(448, 3);
            this.panelLine1.TabIndex = 61;
            // 
            // tbNewPassword
            // 
            this.tbNewPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(247)))), ((int)(((byte)(253)))));
            this.tbNewPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbNewPassword.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNewPassword.Location = new System.Drawing.Point(130, 233);
            this.tbNewPassword.Name = "tbNewPassword";
            this.tbNewPassword.Size = new System.Drawing.Size(389, 40);
            this.tbNewPassword.TabIndex = 60;
            // 
            // lbDescribe
            // 
            this.lbDescribe.AutoSize = true;
            this.lbDescribe.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lbDescribe.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDescribe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(42)))), ((int)(((byte)(122)))));
            this.lbDescribe.Location = new System.Drawing.Point(63, 125);
            this.lbDescribe.Name = "lbDescribe";
            this.lbDescribe.Size = new System.Drawing.Size(145, 22);
            this.lbDescribe.TabIndex = 59;
            this.lbDescribe.Text = "Tạo mật khẩu mới ";
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("Calibri", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(42)))), ((int)(((byte)(122)))));
            this.lbName.Location = new System.Drawing.Point(58, 76);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(246, 49);
            this.lbName.TabIndex = 58;
            this.lbName.Text = "Đổi mật khẩu";
            // 
            // pictureBoxError
            // 
            this.pictureBoxError.Image = global::GUI.Properties.Resources.icons8_error_48;
            this.pictureBoxError.Location = new System.Drawing.Point(4, 11);
            this.pictureBoxError.Name = "pictureBoxError";
            this.pictureBoxError.Size = new System.Drawing.Size(33, 32);
            this.pictureBoxError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxError.TabIndex = 12;
            this.pictureBoxError.TabStop = false;
            // 
            // picbUsername
            // 
            this.picbUsername.Image = global::GUI.Properties.Resources.icons8_key_128;
            this.picbUsername.Location = new System.Drawing.Point(68, 233);
            this.picbUsername.Name = "picbUsername";
            this.picbUsername.Size = new System.Drawing.Size(41, 40);
            this.picbUsername.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picbUsername.TabIndex = 62;
            this.picbUsername.TabStop = false;
            // 
            // picboxPassword
            // 
            this.picboxPassword.Image = global::GUI.Properties.Resources.icons8_key_128;
            this.picboxPassword.Location = new System.Drawing.Point(67, 330);
            this.picboxPassword.Name = "picboxPassword";
            this.picboxPassword.Size = new System.Drawing.Size(44, 43);
            this.picboxPassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picboxPassword.TabIndex = 65;
            this.picboxPassword.TabStop = false;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Image = global::GUI.Properties.Resources.TĐT_logo_removebg_preview;
            this.pictureBoxLogo.Location = new System.Drawing.Point(405, 76);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(110, 72);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 57;
            this.pictureBoxLogo.TabStop = false;
            // 
            // ChangePasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(247)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(576, 664);
            this.Controls.Add(this.pnError);
            this.Controls.Add(this.btnChangePassword);
            this.Controls.Add(this.panelLine2);
            this.Controls.Add(this.tbConfirmPassword);
            this.Controls.Add(this.panelLine1);
            this.Controls.Add(this.tbNewPassword);
            this.Controls.Add(this.lbDescribe);
            this.Controls.Add(this.picbUsername);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.picboxPassword);
            this.Controls.Add(this.pictureBoxLogo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "ChangePasswordForm";
            this.Text = "ChangePasswordForm";
            this.Load += new System.EventHandler(this.ChangePasswordForm_Load);
            this.pnError.ResumeLayout(false);
            this.pnError.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbUsername)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picboxPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnError;
        private System.Windows.Forms.Label lbMessageError;
        private System.Windows.Forms.PictureBox pictureBoxError;
        private System.Windows.Forms.Button btnChangePassword;
        private System.Windows.Forms.Panel panelLine2;
        private System.Windows.Forms.TextBox tbConfirmPassword;
        private System.Windows.Forms.Panel panelLine1;
        private System.Windows.Forms.TextBox tbNewPassword;
        private System.Windows.Forms.Label lbDescribe;
        private System.Windows.Forms.PictureBox picbUsername;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.PictureBox picboxPassword;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
    }
}