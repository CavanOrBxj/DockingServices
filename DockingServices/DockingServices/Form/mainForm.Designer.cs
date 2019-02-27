namespace DockingServices
{
    partial class mainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.SystemToolStripMnu = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuServerAddrSet = new System.Windows.Forms.ToolStripMenuItem();
            this.ServerIPSet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFolderSet = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSysInfoSet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuauditpolicy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuServoSet = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuServerStart = new System.Windows.Forms.ToolStripMenuItem();
            this.nIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.iMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuShow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.iMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SystemToolStripMnu,
            this.mnuServoSet});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(991, 25);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // SystemToolStripMnu
            // 
            this.SystemToolStripMnu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuServerAddrSet,
            this.ServerIPSet,
            this.toolStripMenuItem1,
            this.mnuFolderSet,
            this.mnuSysInfoSet,
            this.toolStripMenuItem2,
            this.mnuauditpolicy,
            this.toolStripSeparator1,
            this.mnuExit});
            this.SystemToolStripMnu.Name = "SystemToolStripMnu";
            this.SystemToolStripMnu.Size = new System.Drawing.Size(68, 21);
            this.SystemToolStripMnu.Text = "系统设置";
            // 
            // mnuServerAddrSet
            // 
            this.mnuServerAddrSet.Name = "mnuServerAddrSet";
            this.mnuServerAddrSet.Size = new System.Drawing.Size(159, 22);
            this.mnuServerAddrSet.Text = "服务地址设置";
            this.mnuServerAddrSet.Click += new System.EventHandler(this.mnuServerAddrSet_Click);
            // 
            // ServerIPSet
            // 
            this.ServerIPSet.Name = "ServerIPSet";
            this.ServerIPSet.Size = new System.Drawing.Size(159, 22);
            this.ServerIPSet.Text = "服务IP地址设置";
            this.ServerIPSet.Click += new System.EventHandler(this.ServerIPSet_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(156, 6);
            // 
            // mnuFolderSet
            // 
            this.mnuFolderSet.Name = "mnuFolderSet";
            this.mnuFolderSet.Size = new System.Drawing.Size(159, 22);
            this.mnuFolderSet.Text = "文件夹设置";
            this.mnuFolderSet.Click += new System.EventHandler(this.mnuFolderSet_Click);
            // 
            // mnuSysInfoSet
            // 
            this.mnuSysInfoSet.Name = "mnuSysInfoSet";
            this.mnuSysInfoSet.Size = new System.Drawing.Size(159, 22);
            this.mnuSysInfoSet.Text = "系统信息设置";
            this.mnuSysInfoSet.Click += new System.EventHandler(this.mnuSysInfoSet_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(156, 6);
            // 
            // mnuauditpolicy
            // 
            this.mnuauditpolicy.Name = "mnuauditpolicy";
            this.mnuauditpolicy.Size = new System.Drawing.Size(159, 22);
            this.mnuauditpolicy.Text = "审核时间策略";
            this.mnuauditpolicy.Click += new System.EventHandler(this.mnuauditpolicy_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(156, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(159, 22);
            this.mnuExit.Text = "退出";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuServoSet
            // 
            this.mnuServoSet.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuServerStart});
            this.mnuServoSet.Name = "mnuServoSet";
            this.mnuServoSet.Size = new System.Drawing.Size(68, 21);
            this.mnuServoSet.Text = "伺服设置";
            // 
            // mnuServerStart
            // 
            this.mnuServerStart.Name = "mnuServerStart";
            this.mnuServerStart.Size = new System.Drawing.Size(124, 22);
            this.mnuServerStart.Text = "启动伺服";
            this.mnuServerStart.Click += new System.EventHandler(this.mnuServerStart_Click);
            // 
            // nIcon
            // 
            this.nIcon.ContextMenuStrip = this.iMenu;
            this.nIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("nIcon.Icon")));
            this.nIcon.Text = "应急广播消息服务";
            this.nIcon.Visible = true;
            // 
            // iMenu
            // 
            this.iMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShow,
            this.toolStripMenuItem3,
            this.mnuQuit});
            this.iMenu.Name = "iMenu";
            this.iMenu.Size = new System.Drawing.Size(101, 54);
            // 
            // mnuShow
            // 
            this.mnuShow.Name = "mnuShow";
            this.mnuShow.Size = new System.Drawing.Size(100, 22);
            this.mnuShow.Text = "显示";
            this.mnuShow.Click += new System.EventHandler(this.mnuShow_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(97, 6);
            // 
            // mnuQuit
            // 
            this.mnuQuit.Name = "mnuQuit";
            this.mnuQuit.Size = new System.Drawing.Size(100, 22);
            this.mnuQuit.Text = "退出";
            this.mnuQuit.Click += new System.EventHandler(this.mnuQuit_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(991, 513);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "应急广播消息服务V1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.iMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem SystemToolStripMnu;
        private System.Windows.Forms.ToolStripMenuItem mnuServerAddrSet;
        private System.Windows.Forms.ToolStripMenuItem mnuServoSet;
        private System.Windows.Forms.ToolStripMenuItem mnuServerStart;
        private System.Windows.Forms.ToolStripMenuItem ServerIPSet;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuFolderSet;
        private System.Windows.Forms.ToolStripMenuItem mnuSysInfoSet;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.NotifyIcon nIcon;
        private System.Windows.Forms.ContextMenuStrip iMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuShow;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mnuQuit;
        private System.Windows.Forms.ToolStripMenuItem mnuauditpolicy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

