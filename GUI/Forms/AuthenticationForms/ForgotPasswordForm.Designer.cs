namespace GUI.Forms.AuthenticationForms
{
    partial class ForgotPasswordForm
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
            this.lbNameReceiveCode = new System.Windows.Forms.Label();
            this.pnReceiveCode = new System.Windows.Forms.Panel();
            this.picbReceiveCode = new System.Windows.Forms.PictureBox();
            this.BackToLogin = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.panelLine2 = new System.Windows.Forms.Panel();
            this.tbCode = new System.Windows.Forms.TextBox();
            this.panelLine1 = new System.Windows.Forms.Panel();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.lbName = new System.Windows.Forms.Label();
            this.lbDescribe = new System.Windows.Forms.Label();
            this.picbUsername = new System.Windows.Forms.PictureBox();
            this.picboxPassword = new System.Windows.Forms.PictureBox();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.pnError.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxError)).BeginInit();
            this.pnReceiveCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbReceiveCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picbUsername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picboxPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pnError
            // 
            this.pnError.Controls.Add(this.lbMessageError);
            this.pnError.Controls.Add(this.pictureBoxError);
            this.pnError.Location = new System.Drawing.Point(67, 388);
            this.pnError.Name = "pnError";
            this.pnError.Size = new System.Drawing.Size(448, 54);
            this.pnError.TabIndex = 56;
            this.pnError.Visible = false;
            // 
            // lbMessageError
            // 
            this.lbMessageError.AutoSize = true;
            this.lbMessageError.Font = new System.Drawing.Font("Calibri", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMessageError.ForeColor = System.Drawing.Color.Red;
            this.lbMessageError.Location = new System.Drawing.Point(42, 16);
            this.lbMessageError.Name = "lbMessageError";
            this.lbMessageError.Size = new System.Drawing.Size(0, 22);
            this.lbMessageError.TabIndex = 13;
            // 
            // pictureBoxError
            // 
            this.pictureBoxError.Image = global::GUI.Properties.Resources.icons8_error_48;
            this.pictureBoxError.Location = new System.Drawing.Point(3, 11);
            this.pictureBoxError.Name = "pictureBoxError";
            this.pictureBoxError.Size = new System.Drawing.Size(33, 32);
            this.pictureBoxError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxError.TabIndex = 12;
            this.pictureBoxError.TabStop = false;
            // 
            // lbNameReceiveCode
            // 
            this.lbNameReceiveCode.AutoSize = true;
            this.lbNameReceiveCode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbNameReceiveCode.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNameReceiveCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(253)))));
            this.lbNameReceiveCode.Location = new System.Drawing.Point(9, 10);
            this.lbNameReceiveCode.Name = "lbNameReceiveCode";
            this.lbNameReceiveCode.Size = new System.Drawing.Size(103, 29);
            this.lbNameReceiveCode.TabIndex = 17;
            this.lbNameReceiveCode.Text = "Nhận mã";
            this.lbNameReceiveCode.Click += new System.EventHandler(this.lbNameReceiveCode_Click);
            // 
            // pnReceiveCode
            // 
            this.pnReceiveCode.AutoSize = true;
            this.pnReceiveCode.Controls.Add(this.picbReceiveCode);
            this.pnReceiveCode.Controls.Add(this.lbNameReceiveCode);
            this.pnReceiveCode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnReceiveCode.Location = new System.Drawing.Point(363, 322);
            this.pnReceiveCode.Name = "pnReceiveCode";
            this.pnReceiveCode.Size = new System.Drawing.Size(156, 50);
            this.pnReceiveCode.TabIndex = 57;
            // 
            // picbReceiveCode
            // 
            this.picbReceiveCode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picbReceiveCode.Image = global::GUI.Properties.Resources.icons8_receive_mail_68;
            this.picbReceiveCode.Location = new System.Drawing.Point(109, 3);
            this.picbReceiveCode.Name = "picbReceiveCode";
            this.picbReceiveCode.Size = new System.Drawing.Size(44, 43);
            this.picbReceiveCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picbReceiveCode.TabIndex = 16;
            this.picbReceiveCode.TabStop = false;
            this.picbReceiveCode.Click += new System.EventHandler(this.picbReceiveCode_Click);
            // 
            // BackToLogin
            // 
            this.BackToLogin.AutoSize = true;
            this.BackToLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BackToLogin.Font = new System.Drawing.Font("Calibri", 11F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackToLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(42)))), ((int)(((byte)(122)))));
            this.BackToLogin.Location = new System.Drawing.Point(193, 562);
            this.BackToLogin.Name = "BackToLogin";
            this.BackToLogin.Size = new System.Drawing.Size(190, 27);
            this.BackToLogin.TabIndex = 55;
            this.BackToLogin.Text = "Quay lại Đăng nhập";
            this.BackToLogin.Click += new System.EventHandler(this.BackToLogin_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(253)))));
            this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("Calibri", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnConfirm.Location = new System.Drawing.Point(67, 483);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(448, 65);
            this.btnConfirm.TabIndex = 54;
            this.btnConfirm.Text = "Xác nhận";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // panelLine2
            // 
            this.panelLine2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(253)))));
            this.panelLine2.Location = new System.Drawing.Point(71, 375);
            this.panelLine2.Name = "panelLine2";
            this.panelLine2.Size = new System.Drawing.Size(448, 3);
            this.panelLine2.TabIndex = 52;
            // 
            // tbCode
            // 
            this.tbCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(247)))), ((int)(((byte)(253)))));
            this.tbCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbCode.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCode.Location = new System.Drawing.Point(130, 329);
            this.tbCode.Name = "tbCode";
            this.tbCode.Size = new System.Drawing.Size(227, 40);
            this.tbCode.TabIndex = 51;
            // 
            // panelLine1
            // 
            this.panelLine1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(89)))), ((int)(((byte)(253)))));
            this.panelLine1.Location = new System.Drawing.Point(71, 278);
            this.panelLine1.Name = "panelLine1";
            this.panelLine1.Size = new System.Drawing.Size(448, 3);
            this.panelLine1.TabIndex = 49;
            // 
            // tbEmail
            // 
            this.tbEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(247)))), ((int)(((byte)(253)))));
            this.tbEmail.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbEmail.Font = new System.Drawing.Font("Calibri", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbEmail.Location = new System.Drawing.Point(130, 232);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(389, 40);
            this.tbEmail.TabIndex = 48;
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Font = new System.Drawing.Font("Calibri", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(42)))), ((int)(((byte)(122)))));
            this.lbName.Location = new System.Drawing.Point(58, 75);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(277, 49);
            this.lbName.TabIndex = 46;
            this.lbName.Text = "Quên mật khẩu";
            // 
            // lbDescribe
            // 
            this.lbDescribe.AutoSize = true;
            this.lbDescribe.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lbDescribe.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDescribe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(42)))), ((int)(((byte)(122)))));
            this.lbDescribe.Location = new System.Drawing.Point(63, 124);
            this.lbDescribe.Name = "lbDescribe";
            this.lbDescribe.Size = new System.Drawing.Size(234, 22);
            this.lbDescribe.TabIndex = 47;
            this.lbDescribe.Text = "Nhập email để lấy lại mật khẩu";
            // 
            // picbUsername
            // 
            this.picbUsername.Image = global::GUI.Properties.Resources.email;
            this.picbUsername.Location = new System.Drawing.Point(68, 232);
            this.picbUsername.Name = "picbUsername";
            this.picbUsername.Size = new System.Drawing.Size(41, 40);
            this.picbUsername.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picbUsername.TabIndex = 50;
            this.picbUsername.TabStop = false;
            // 
            // picboxPassword
            // 
            this.picboxPassword.Image = global::GUI.Properties.Resources.icons8_email_send_64;
            this.picboxPassword.Location = new System.Drawing.Point(67, 329);
            this.picboxPassword.Name = "picboxPassword";
            this.picboxPassword.Size = new System.Drawing.Size(44, 43);
            this.picboxPassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picboxPassword.TabIndex = 53;
            this.picboxPassword.TabStop = false;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Image = global::GUI.Properties.Resources.TĐT_logo_removebg_preview;
            this.pictureBoxLogo.Location = new System.Drawing.Point(405, 75);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(110, 72);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLogo.TabIndex = 45;
            this.pictureBoxLogo.TabStop = false;
            // 
            // ForgotPasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(247)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(576, 664);
            this.Controls.Add(this.pnError);
            this.Controls.Add(this.pnReceiveCode);
            this.Controls.Add(this.BackToLogin);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.panelLine2);
            this.Controls.Add(this.tbCode);
            this.Controls.Add(this.panelLine1);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.picbUsername);
            this.Controls.Add(this.lbDescribe);
            this.Controls.Add(this.picboxPassword);
            this.Controls.Add(this.pictureBoxLogo);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.Name = "ForgotPasswordForm";
            this.Text = "ForgotPasswordForm";
            this.Load += new System.EventHandler(this.ForgotPasswordForm_Load);
            this.pnError.ResumeLayout(false);
            this.pnError.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxError)).EndInit();
            this.pnReceiveCode.ResumeLayout(false);
            this.pnReceiveCode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picbReceiveCode)).EndInit();
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
        private System.Windows.Forms.PictureBox picbReceiveCode;
        private System.Windows.Forms.Label lbNameReceiveCode;
        private System.Windows.Forms.Panel pnReceiveCode;
        private System.Windows.Forms.Label BackToLogin;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Panel panelLine2;
        private System.Windows.Forms.TextBox tbCode;
        private System.Windows.Forms.Panel panelLine1;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.PictureBox picbUsername;
        private System.Windows.Forms.Label lbDescribe;
        private System.Windows.Forms.PictureBox picboxPassword;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
    }
}