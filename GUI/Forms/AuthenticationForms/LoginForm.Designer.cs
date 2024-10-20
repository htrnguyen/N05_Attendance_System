namespace GUI.Forms.AuthenticationForms
{
    partial class LoginForm
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
            this.pictureBoxError = new System.Windows.Forms.PictureBox();
            this.lbForgotPassword = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.panelLine2 = new System.Windows.Forms.Panel();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.panelLine1 = new System.Windows.Forms.Panel();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.lbDescribe = new System.Windows.Forms.Label();
            this.lbName = new System.Windows.Forms.Label();
            this.picbHidePassword = new System.Windows.Forms.PictureBox();
            this.picbShowPassword = new System.Windows.Forms.PictureBox();
            this.picboxPassword = new System.Windows.Forms.PictureBox();
            this.picbUsername = new System.Windows.Forms.PictureBox();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.pnError.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbHidePassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbShowPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picboxPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbUsername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnError
            // 
            this.pnError.Controls.Add(this.lbMessageError);
            this.pnError.Controls.Add(this.pictureBoxError);
            this.pnError.Location = new System.Drawing.Point(67, 384);
            this.pnError.Name = "pnError";
            this.pnError.Size = new System.Drawing.Size(448, 54);
            this.pnError.TabIndex = 43;
            this.pnError.Visible = false;
            // 
            // lbMessageError
            // 
            this.lbMessageError.AutoSize = true;
            this.lbMessageError.Font = new System.Drawing.Font("Calibri", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMessageError.ForeColor = System.Drawing.Color.Red;
            this.lbMessageError.Location = new System.Drawing.Point(43, 16);
            this.lbMessageError.Name = "lbMessageError";
            this.lbMessageError.Size = new System.Drawing.Size(0, 22);
            this.lbMessageError.TabIndex = 13;
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
            // lbForgotPassword
            // 
            this.lbForgotPassword.AutoSize = true;
            this.lbForgotPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbForgotPassword.Font = new System.Drawing.Font("Calibri", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbForgotPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(42)))), ((int)(((byte)(122)))));
            this.lbForgotPassword.Location = new System.Drawing.Point(211, 562);
            this.lbForgotPassword.Name = "lbForgotPassword";
            this.lbForgotPassword.Size = new System.Drawing.Size(155, 27);
            this.lbForgotPassword.TabIndex = 42;
            this.lbForgotPassword.Text = "Quên mật khẩu";
            this.lbForgotPassword.Click += new System.EventHandler(this.lbForgotPassword_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(253)))));
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnLogin.Location = new System.Drawing.Point(67, 483);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(448, 65);
            this.btnLogin.TabIndex = 41;
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // panelLine2
            // 
            this.panelLine2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(253)))));
            this.panelLine2.Location = new System.Drawing.Point(71, 375);
            this.panelLine2.Name = "panelLine2";
            this.panelLine2.Size = new System.Drawing.Size(448, 3);
            this.panelLine2.TabIndex = 38;
            // 
            // tbPassword
            // 
            this.tbPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(247)))), ((int)(((byte)(253)))));
            this.tbPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPassword.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPassword.Location = new System.Drawing.Point(130, 329);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(333, 40);
            this.tbPassword.TabIndex = 37;
            this.tbPassword.UseSystemPasswordChar = true;
            // 
            // panelLine1
            // 
            this.panelLine1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(253)))));
            this.panelLine1.Location = new System.Drawing.Point(71, 278);
            this.panelLine1.Name = "panelLine1";
            this.panelLine1.Size = new System.Drawing.Size(448, 3);
            this.panelLine1.TabIndex = 35;
            // 
            // tbUsername
            // 
            this.tbUsername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(247)))), ((int)(((byte)(253)))));
            this.tbUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbUsername.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUsername.Location = new System.Drawing.Point(130, 232);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(389, 40);
            this.tbUsername.TabIndex = 34;
            // 
            // lbDescribe
            // 
            this.lbDescribe.AutoSize = true;
            this.lbDescribe.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lbDescribe.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDescribe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(42)))), ((int)(((byte)(122)))));
            this.lbDescribe.Location = new System.Drawing.Point(63, 124);
            this.lbDescribe.Name = "lbDescribe";
            this.lbDescribe.Size = new System.Drawing.Size(218, 22);
            this.lbDescribe.TabIndex = 33;
            this.lbDescribe.Text = "Hệ thống quản lý điểm danh.";
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("Calibri", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(42)))), ((int)(((byte)(122)))));
            this.lbName.Location = new System.Drawing.Point(58, 75);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(200, 49);
            this.lbName.TabIndex = 32;
            this.lbName.Text = "Đăng nhập";
            // 
            // picbHidePassword
            // 
            this.picbHidePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picbHidePassword.Image = global::GUI.Properties.Resources.icons8_hide_96;
            this.picbHidePassword.Location = new System.Drawing.Point(469, 326);
            this.picbHidePassword.Name = "picbHidePassword";
            this.picbHidePassword.Size = new System.Drawing.Size(44, 43);
            this.picbHidePassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picbHidePassword.TabIndex = 44;
            this.picbHidePassword.TabStop = false;
            this.picbHidePassword.Click += new System.EventHandler(this.HidePassword);
            // 
            // picbShowPassword
            // 
            this.picbShowPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picbShowPassword.Image = global::GUI.Properties.Resources.icons8_show_password_96;
            this.picbShowPassword.Location = new System.Drawing.Point(469, 326);
            this.picbShowPassword.Name = "picbShowPassword";
            this.picbShowPassword.Size = new System.Drawing.Size(44, 43);
            this.picbShowPassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picbShowPassword.TabIndex = 40;
            this.picbShowPassword.TabStop = false;
            this.picbShowPassword.Click += new System.EventHandler(this.ShowPassword);
            // 
            // picboxPassword
            // 
            this.picboxPassword.Image = global::GUI.Properties.Resources.password__2_;
            this.picboxPassword.Location = new System.Drawing.Point(67, 329);
            this.picboxPassword.Name = "picboxPassword";
            this.picboxPassword.Size = new System.Drawing.Size(44, 43);
            this.picboxPassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picboxPassword.TabIndex = 39;
            this.picboxPassword.TabStop = false;
            // 
            // picbUsername
            // 
            this.picbUsername.Image = global::GUI.Properties.Resources.icons8_user_96;
            this.picbUsername.Location = new System.Drawing.Point(67, 223);
            this.picbUsername.Name = "picbUsername";
            this.picbUsername.Size = new System.Drawing.Size(44, 49);
            this.picbUsername.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picbUsername.TabIndex = 36;
            this.picbUsername.TabStop = false;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Image = global::GUI.Properties.Resources.TĐT_logo_removebg_preview;
            this.pictureBoxLogo.Location = new System.Drawing.Point(405, 75);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(110, 72);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 31;
            this.pictureBoxLogo.TabStop = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(247)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(576, 664);
            this.Controls.Add(this.pnError);
            this.Controls.Add(this.picbHidePassword);
            this.Controls.Add(this.picbShowPassword);
            this.Controls.Add(this.lbForgotPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.picboxPassword);
            this.Controls.Add(this.panelLine2);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.picbUsername);
            this.Controls.Add(this.panelLine1);
            this.Controls.Add(this.tbUsername);
            this.Controls.Add(this.lbDescribe);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.pictureBoxLogo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.pnError.ResumeLayout(false);
            this.pnError.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbHidePassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbShowPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picboxPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbUsername)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnError;
        private System.Windows.Forms.Label lbMessageError;
        private System.Windows.Forms.PictureBox pictureBoxError;
        private System.Windows.Forms.PictureBox picbHidePassword;
        private System.Windows.Forms.PictureBox picbShowPassword;
        private System.Windows.Forms.Label lbForgotPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.PictureBox picboxPassword;
        private System.Windows.Forms.Panel panelLine2;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.PictureBox picbUsername;
        private System.Windows.Forms.Panel panelLine1;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.Label lbDescribe;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
    }
}