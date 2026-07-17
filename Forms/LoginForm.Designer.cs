namespace SmartInventory.Forms
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
            label1 = new Label();
            tbxName = new TextBox();
            tbxPassword = new TextBox();
            label2 = new Label();
            btnLogin = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft JhengHei UI", 12F);
            label1.Location = new Point(60, 69);
            label1.Name = "label1";
            label1.Size = new Size(41, 20);
            label1.TabIndex = 0;
            label1.Text = "帳號";
            // 
            // tbxName
            // 
            tbxName.Font = new Font("Microsoft JhengHei UI", 12F);
            tbxName.Location = new Point(107, 66);
            tbxName.Name = "tbxName";
            tbxName.Size = new Size(165, 28);
            tbxName.TabIndex = 2;
            // 
            // tbxPassword
            // 
            tbxPassword.Font = new Font("Microsoft JhengHei UI", 12F);
            tbxPassword.Location = new Point(107, 115);
            tbxPassword.Name = "tbxPassword";
            tbxPassword.PasswordChar = '*';
            tbxPassword.Size = new Size(165, 28);
            tbxPassword.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft JhengHei UI", 12F);
            label2.Location = new Point(60, 118);
            label2.Name = "label2";
            label2.Size = new Size(41, 20);
            label2.TabIndex = 3;
            label2.Text = "密碼";
            // 
            // btnLogin
            // 
            btnLogin.Font = new Font("Microsoft JhengHei UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 136);
            btnLogin.Location = new Point(107, 216);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(96, 29);
            btnLogin.TabIndex = 5;
            btnLogin.Text = "登入";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(334, 311);
            Controls.Add(btnLogin);
            Controls.Add(tbxPassword);
            Controls.Add(label2);
            Controls.Add(tbxName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "登入視窗";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox tbxName;
        private TextBox tbxPassword;
        private Label label2;
        private Button btnLogin;
    }
}