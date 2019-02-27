namespace DockingServices
{
    partial class ServerForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Verify = new System.Windows.Forms.Button();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtMsgShow = new System.Windows.Forms.TextBox();
            this.timHold = new System.Windows.Forms.Timer(this.components);
            this.timHeart = new System.Windows.Forms.Timer(this.components);
            this.tim_MediaPlay = new System.Windows.Forms.Timer(this.components);
            this.tim_ClearMemory = new System.Windows.Forms.Timer(this.components);
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dp_RealTask = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel3_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.list_RealTask = new System.Windows.Forms.ListView();
            this.Number = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EBMID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EBMStartTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EBMEndTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dp_PendingTask = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.list_PendingTask = new System.Windows.Forms.ListView();
            this.Num = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Detail = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelContainer3 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dp_OMDRequest = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.list_OMDRequest = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dp_HanleReport = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel4_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.btn_HreartState = new System.Windows.Forms.Button();
            this.btn_InfroState = new System.Windows.Forms.Button();
            this.panelContainer2 = new DevExpress.XtraBars.Docking.DockPanel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dp_RealTask.SuspendLayout();
            this.dockPanel3_Container.SuspendLayout();
            this.dp_PendingTask.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            this.panelContainer3.SuspendLayout();
            this.dp_OMDRequest.SuspendLayout();
            this.dockPanel2_Container.SuspendLayout();
            this.dp_HanleReport.SuspendLayout();
            this.dockPanel4_Container.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_Verify);
            this.panel1.Controls.Add(this.txtServerPort);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(474, 451);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(642, 80);
            this.panel1.TabIndex = 0;
            // 
            // btn_Verify
            // 
            this.btn_Verify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Verify.Location = new System.Drawing.Point(369, 23);
            this.btn_Verify.Name = "btn_Verify";
            this.btn_Verify.Size = new System.Drawing.Size(105, 42);
            this.btn_Verify.TabIndex = 7;
            this.btn_Verify.Text = "人工审核-已开启";
            this.btn_Verify.UseVisualStyleBackColor = true;
            this.btn_Verify.Click += new System.EventHandler(this.btn_Verify_Click);
            // 
            // txtServerPort
            // 
            this.txtServerPort.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtServerPort.Location = new System.Drawing.Point(100, 31);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(100, 26);
            this.txtServerPort.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(15, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "服务端口:";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(500, 23);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(118, 42);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "服务-未开启";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(3, 66);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(188, 25);
            this.button4.TabIndex = 4;
            this.button4.Text = "终端信息上报";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(3, 35);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(188, 25);
            this.button3.TabIndex = 4;
            this.button3.Text = "平台状态信息";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(3, 97);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(188, 25);
            this.button2.TabIndex = 4;
            this.button2.Text = "终端状态上报";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(188, 25);
            this.button1.TabIndex = 4;
            this.button1.Text = "平台信息上报";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtMsgShow);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(474, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(642, 451);
            this.panel2.TabIndex = 1;
            // 
            // txtMsgShow
            // 
            this.txtMsgShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMsgShow.Location = new System.Drawing.Point(0, 0);
            this.txtMsgShow.Multiline = true;
            this.txtMsgShow.Name = "txtMsgShow";
            this.txtMsgShow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMsgShow.Size = new System.Drawing.Size(642, 451);
            this.txtMsgShow.TabIndex = 0;
            // 
            // timHold
            // 
            this.timHold.Interval = 1000;
            this.timHold.Tick += new System.EventHandler(this.timHold_Tick);
            // 
            // timHeart
            // 
            this.timHeart.Interval = 30000;
            this.timHeart.Tick += new System.EventHandler(this.timHeart_Tick);
            // 
            // tim_MediaPlay
            // 
            this.tim_MediaPlay.Interval = 1000;
            this.tim_MediaPlay.Tick += new System.EventHandler(this.tim_MediaPlay_Tick);
            // 
            // tim_ClearMemory
            // 
            this.tim_ClearMemory.Interval = 600000;
            this.tim_ClearMemory.Tick += new System.EventHandler(this.tim_ClearMemory_Tick);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.HiddenPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dp_RealTask});
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dp_PendingTask,
            this.panelContainer3});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // dp_RealTask
            // 
            this.dp_RealTask.Controls.Add(this.dockPanel3_Container);
            this.dp_RealTask.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dp_RealTask.ID = new System.Guid("fe47a71a-adaa-4080-9418-7c7e1bd69ca0");
            this.dp_RealTask.Location = new System.Drawing.Point(0, 266);
            this.dp_RealTask.Name = "dp_RealTask";
            this.dp_RealTask.OriginalSize = new System.Drawing.Size(200, 266);
            this.dp_RealTask.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dp_RealTask.SavedIndex = 1;
            this.dp_RealTask.SavedParent = this.dp_PendingTask;
            this.dp_RealTask.Size = new System.Drawing.Size(474, 265);
            this.dp_RealTask.Text = "播发消息";
            this.dp_RealTask.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
            // 
            // dockPanel3_Container
            // 
            this.dockPanel3_Container.Controls.Add(this.list_RealTask);
            this.dockPanel3_Container.Location = new System.Drawing.Point(4, 24);
            this.dockPanel3_Container.Name = "dockPanel3_Container";
            this.dockPanel3_Container.Size = new System.Drawing.Size(466, 237);
            this.dockPanel3_Container.TabIndex = 0;
            // 
            // list_RealTask
            // 
            this.list_RealTask.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Number,
            this.EBMID,
            this.EBMStartTime,
            this.EBMEndTime});
            this.list_RealTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list_RealTask.FullRowSelect = true;
            this.list_RealTask.Location = new System.Drawing.Point(0, 0);
            this.list_RealTask.Name = "list_RealTask";
            this.list_RealTask.Size = new System.Drawing.Size(466, 237);
            this.list_RealTask.TabIndex = 0;
            this.list_RealTask.UseCompatibleStateImageBehavior = false;
            this.list_RealTask.View = System.Windows.Forms.View.Details;
            // 
            // Number
            // 
            this.Number.Text = "序号";
            // 
            // EBMID
            // 
            this.EBMID.Text = "EBMID";
            this.EBMID.Width = 62;
            // 
            // EBMStartTime
            // 
            this.EBMStartTime.Text = "开始时间";
            this.EBMStartTime.Width = 123;
            // 
            // EBMEndTime
            // 
            this.EBMEndTime.Text = "结束时间";
            this.EBMEndTime.Width = 140;
            // 
            // dp_PendingTask
            // 
            this.dp_PendingTask.Controls.Add(this.dockPanel1_Container);
            this.dp_PendingTask.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dp_PendingTask.ID = new System.Guid("5cc154f6-9469-48aa-a5cc-284776cffc48");
            this.dp_PendingTask.Location = new System.Drawing.Point(0, 0);
            this.dp_PendingTask.Name = "dp_PendingTask";
            this.dp_PendingTask.OriginalSize = new System.Drawing.Size(474, 265);
            this.dp_PendingTask.Size = new System.Drawing.Size(474, 531);
            this.dp_PendingTask.Text = "待处理消息";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.list_PendingTask);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(466, 504);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // list_PendingTask
            // 
            this.list_PendingTask.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Num,
            this.Detail});
            this.list_PendingTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list_PendingTask.FullRowSelect = true;
            this.list_PendingTask.Location = new System.Drawing.Point(0, 0);
            this.list_PendingTask.Name = "list_PendingTask";
            this.list_PendingTask.Size = new System.Drawing.Size(466, 504);
            this.list_PendingTask.TabIndex = 0;
            this.list_PendingTask.UseCompatibleStateImageBehavior = false;
            this.list_PendingTask.View = System.Windows.Forms.View.Details;
            this.list_PendingTask.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.list_PendingTask_MouseDoubleClick);
            // 
            // Num
            // 
            this.Num.Text = "序号";
            // 
            // Detail
            // 
            this.Detail.Text = "应急包";
            this.Detail.Width = 400;
            // 
            // panelContainer3
            // 
            this.panelContainer3.Controls.Add(this.dp_OMDRequest);
            this.panelContainer3.Controls.Add(this.dp_HanleReport);
            this.panelContainer3.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.panelContainer3.ID = new System.Guid("594decd8-e498-46a4-ac09-4948840aa4c1");
            this.panelContainer3.Location = new System.Drawing.Point(1116, 0);
            this.panelContainer3.Name = "panelContainer3";
            this.panelContainer3.OriginalSize = new System.Drawing.Size(207, 200);
            this.panelContainer3.Size = new System.Drawing.Size(207, 531);
            this.panelContainer3.Text = "panelContainer3";
            // 
            // dp_OMDRequest
            // 
            this.dp_OMDRequest.Controls.Add(this.dockPanel2_Container);
            this.dp_OMDRequest.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dp_OMDRequest.FloatVertical = true;
            this.dp_OMDRequest.ID = new System.Guid("7b974e43-cbd2-4aff-b367-00fc3757a3c1");
            this.dp_OMDRequest.Location = new System.Drawing.Point(0, 0);
            this.dp_OMDRequest.Name = "dp_OMDRequest";
            this.dp_OMDRequest.OriginalSize = new System.Drawing.Size(474, 265);
            this.dp_OMDRequest.Size = new System.Drawing.Size(207, 266);
            this.dp_OMDRequest.Text = "上级业务数据请求";
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Controls.Add(this.list_OMDRequest);
            this.dockPanel2_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(199, 239);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // list_OMDRequest
            // 
            this.list_OMDRequest.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.list_OMDRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list_OMDRequest.FullRowSelect = true;
            this.list_OMDRequest.Location = new System.Drawing.Point(0, 0);
            this.list_OMDRequest.Name = "list_OMDRequest";
            this.list_OMDRequest.Size = new System.Drawing.Size(199, 239);
            this.list_OMDRequest.TabIndex = 0;
            this.list_OMDRequest.UseCompatibleStateImageBehavior = false;
            this.list_OMDRequest.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "请求详情";
            this.columnHeader1.Width = 194;
            // 
            // dp_HanleReport
            // 
            this.dp_HanleReport.Controls.Add(this.dockPanel4_Container);
            this.dp_HanleReport.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dp_HanleReport.ID = new System.Guid("978ef1fe-0ed2-45ea-82f8-7366205a213f");
            this.dp_HanleReport.Location = new System.Drawing.Point(0, 266);
            this.dp_HanleReport.Name = "dp_HanleReport";
            this.dp_HanleReport.OriginalSize = new System.Drawing.Size(207, 200);
            this.dp_HanleReport.Size = new System.Drawing.Size(207, 265);
            this.dp_HanleReport.Text = "手动上业务数据";
            // 
            // dockPanel4_Container
            // 
            this.dockPanel4_Container.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dockPanel4_Container.Controls.Add(this.btn_HreartState);
            this.dockPanel4_Container.Controls.Add(this.btn_InfroState);
            this.dockPanel4_Container.Controls.Add(this.button1);
            this.dockPanel4_Container.Controls.Add(this.button2);
            this.dockPanel4_Container.Controls.Add(this.button4);
            this.dockPanel4_Container.Controls.Add(this.button3);
            this.dockPanel4_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel4_Container.Name = "dockPanel4_Container";
            this.dockPanel4_Container.Size = new System.Drawing.Size(199, 238);
            this.dockPanel4_Container.TabIndex = 0;
            // 
            // btn_HreartState
            // 
            this.btn_HreartState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_HreartState.Location = new System.Drawing.Point(3, 160);
            this.btn_HreartState.Name = "btn_HreartState";
            this.btn_HreartState.Size = new System.Drawing.Size(188, 25);
            this.btn_HreartState.TabIndex = 10;
            this.btn_HreartState.Text = "心跳状态上报-未开启";
            this.btn_HreartState.UseVisualStyleBackColor = true;
            this.btn_HreartState.Click += new System.EventHandler(this.btn_HreartState_Click);
            // 
            // btn_InfroState
            // 
            this.btn_InfroState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_InfroState.Location = new System.Drawing.Point(3, 129);
            this.btn_InfroState.Name = "btn_InfroState";
            this.btn_InfroState.Size = new System.Drawing.Size(188, 25);
            this.btn_InfroState.TabIndex = 11;
            this.btn_InfroState.Text = "信息状态上报-未开启";
            this.btn_InfroState.UseVisualStyleBackColor = true;
            this.btn_InfroState.Click += new System.EventHandler(this.btn_InfroState_Click);
            // 
            // panelContainer2
            // 
            this.panelContainer2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelContainer2.ID = new System.Guid("6e297186-52d7-4ea8-ae88-665b78e3d0e4");
            this.panelContainer2.Location = new System.Drawing.Point(0, 0);
            this.panelContainer2.Name = "panelContainer2";
            this.panelContainer2.OriginalSize = new System.Drawing.Size(286, 200);
            this.panelContainer2.Size = new System.Drawing.Size(286, 531);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1323, 531);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelContainer3);
            this.Controls.Add(this.dp_PendingTask);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ServerForm";
            this.ShowIcon = false;
            this.Text = "消息服务";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dp_RealTask.ResumeLayout(false);
            this.dockPanel3_Container.ResumeLayout(false);
            this.dp_PendingTask.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            this.panelContainer3.ResumeLayout(false);
            this.dp_OMDRequest.ResumeLayout(false);
            this.dockPanel2_Container.ResumeLayout(false);
            this.dp_HanleReport.ResumeLayout(false);
            this.dockPanel4_Container.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMsgShow;
        private System.Windows.Forms.Timer timHold;
        private System.Windows.Forms.Timer timHeart;
        private System.Windows.Forms.Timer tim_MediaPlay;
        private System.Windows.Forms.Timer tim_ClearMemory;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dp_HanleReport;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel4_Container;
        private DevExpress.XtraBars.Docking.DockPanel dp_RealTask;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel3_Container;
        private DevExpress.XtraBars.Docking.DockPanel dp_OMDRequest;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        private DevExpress.XtraBars.Docking.DockPanel dp_PendingTask;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private System.Windows.Forms.Button btn_HreartState;
        private System.Windows.Forms.Button btn_InfroState;
        private System.Windows.Forms.ListView list_RealTask;
        private System.Windows.Forms.ListView list_PendingTask;
        private System.Windows.Forms.ListView list_OMDRequest;
        private System.Windows.Forms.Button btn_Verify;
        private System.Windows.Forms.ColumnHeader Num;
        private System.Windows.Forms.ColumnHeader Detail;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer3;
        private System.Windows.Forms.ColumnHeader Number;
        private System.Windows.Forms.ColumnHeader EBMID;
        private System.Windows.Forms.ColumnHeader EBMStartTime;
        private System.Windows.Forms.ColumnHeader EBMEndTime;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}