namespace DockingServices
{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.plRegister = new System.Windows.Forms.Panel();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.gbSetting = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDb = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDbPass = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDbuser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.plRegister.SuspendLayout();
            this.gbSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // plRegister
            // 
            this.plRegister.BackColor = System.Drawing.Color.LightSeaGreen;
            this.plRegister.Controls.Add(this.txtPass);
            this.plRegister.Controls.Add(this.label4);
            this.plRegister.Controls.Add(this.txtCode);
            this.plRegister.Controls.Add(this.label2);
            this.plRegister.Controls.Add(this.label1);
            this.plRegister.Controls.Add(this.txtUser);
            this.plRegister.Controls.Add(this.btnExit);
            this.plRegister.Controls.Add(this.btnLogin);
            this.plRegister.Controls.Add(this.btnSetting);
            this.plRegister.Dock = System.Windows.Forms.DockStyle.Top;
            this.plRegister.Location = new System.Drawing.Point(0, 0);
            this.plRegister.Name = "plRegister";
            this.plRegister.Size = new System.Drawing.Size(356, 219);
            this.plRegister.TabIndex = 1;
            // 
            // txtPass
            // 
            this.txtPass.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPass.Location = new System.Drawing.Point(125, 118);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(179, 29);
            this.txtPass.TabIndex = 3;
            this.txtPass.Text = "001";
            this.txtPass.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(34, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 19);
            this.label4.TabIndex = 8;
            this.label4.Text = "用户编码";
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCode.Location = new System.Drawing.Point(125, 28);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(179, 29);
            this.txtCode.TabIndex = 1;
            this.txtCode.Text = "001";
            this.txtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(34, 123);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 19);
            this.label2.TabIndex = 6;
            this.label2.Text = "登录密码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(34, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "用户姓名";
            // 
            // txtUser
            // 
            this.txtUser.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUser.Location = new System.Drawing.Point(125, 73);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(179, 29);
            this.txtUser.TabIndex = 2;
            this.txtUser.Text = "user";
            this.txtUser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExit.Location = new System.Drawing.Point(172, 165);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(93, 32);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLogin.Location = new System.Drawing.Point(60, 165);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(93, 32);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "登录";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSetting.Location = new System.Drawing.Point(300, 165);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(46, 32);
            this.btnSetting.TabIndex = 6;
            this.btnSetting.Text = ">>";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // gbSetting
            // 
            this.gbSetting.BackColor = System.Drawing.Color.LightSeaGreen;
            this.gbSetting.Controls.Add(this.button1);
            this.gbSetting.Controls.Add(this.label7);
            this.gbSetting.Controls.Add(this.txtDb);
            this.gbSetting.Controls.Add(this.label6);
            this.gbSetting.Controls.Add(this.txtDbPass);
            this.gbSetting.Controls.Add(this.label5);
            this.gbSetting.Controls.Add(this.txtDbuser);
            this.gbSetting.Controls.Add(this.label3);
            this.gbSetting.Controls.Add(this.txtServer);
            this.gbSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSetting.Location = new System.Drawing.Point(0, 219);
            this.gbSetting.Name = "gbSetting";
            this.gbSetting.Size = new System.Drawing.Size(356, 204);
            this.gbSetting.TabIndex = 2;
            this.gbSetting.TabStop = false;
            this.gbSetting.Text = "数据库连接设置";
            this.gbSetting.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(209, 167);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 31);
            this.button1.TabIndex = 23;
            this.button1.TabStop = false;
            this.button1.Text = "确认";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(47, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 22;
            this.label7.Text = "数 据 库";
            // 
            // txtDb
            // 
            this.txtDb.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDb.Location = new System.Drawing.Point(124, 61);
            this.txtDb.Name = "txtDb";
            this.txtDb.Size = new System.Drawing.Size(179, 23);
            this.txtDb.TabIndex = 21;
            this.txtDb.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(46, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 20;
            this.label6.Text = "登录密码";
            // 
            // txtDbPass
            // 
            this.txtDbPass.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDbPass.Location = new System.Drawing.Point(124, 131);
            this.txtDbPass.Name = "txtDbPass";
            this.txtDbPass.Size = new System.Drawing.Size(179, 23);
            this.txtDbPass.TabIndex = 19;
            this.txtDbPass.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(47, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 18;
            this.label5.Text = "用 户 名";
            // 
            // txtDbuser
            // 
            this.txtDbuser.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDbuser.Location = new System.Drawing.Point(124, 96);
            this.txtDbuser.Name = "txtDbuser";
            this.txtDbuser.Size = new System.Drawing.Size(179, 23);
            this.txtDbuser.TabIndex = 17;
            this.txtDbuser.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(47, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 16;
            this.label3.Text = "服 务 器";
            // 
            // txtServer
            // 
            this.txtServer.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtServer.Location = new System.Drawing.Point(124, 26);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(179, 23);
            this.txtServer.TabIndex = 15;
            this.txtServer.TabStop = false;
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(356, 423);
            this.Controls.Add(this.gbSetting);
            this.Controls.Add(this.plRegister);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "系统登录";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.plRegister.ResumeLayout(false);
            this.plRegister.PerformLayout();
            this.gbSetting.ResumeLayout(false);
            this.gbSetting.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plRegister;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.GroupBox gbSetting;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtDb;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDbPass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDbuser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Button button1;
    }
}