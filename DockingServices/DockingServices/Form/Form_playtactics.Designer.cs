namespace DockingServices
{
    partial class Form_playtactics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_playtactics));
            this.btn_cancel = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader25 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader26 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_AddEvent = new System.Windows.Forms.Button();
            this.cbb_Type = new System.Windows.Forms.ComboBox();
            this.dtp_EndTime = new System.Windows.Forms.DateTimePicker();
            this.dtp_StartTime = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.play_time = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.play_count = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.event_type = new System.Windows.Forms.ComboBox();
            this.play_rank = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_del = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_cancel
            // 
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader24,
            this.columnHeader4,
            this.columnHeader25,
            this.columnHeader26,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader5});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            resources.ApplyResources(this.listView1, "listView1");
            this.listView1.Name = "listView1";
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader24
            // 
            resources.ApplyResources(this.columnHeader24, "columnHeader24");
            // 
            // columnHeader4
            // 
            resources.ApplyResources(this.columnHeader4, "columnHeader4");
            // 
            // columnHeader25
            // 
            resources.ApplyResources(this.columnHeader25, "columnHeader25");
            // 
            // columnHeader26
            // 
            resources.ApplyResources(this.columnHeader26, "columnHeader26");
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader5
            // 
            resources.ApplyResources(this.columnHeader5, "columnHeader5");
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btn_AddEvent
            // 
            resources.ApplyResources(this.btn_AddEvent, "btn_AddEvent");
            this.btn_AddEvent.Name = "btn_AddEvent";
            this.btn_AddEvent.UseVisualStyleBackColor = true;
            this.btn_AddEvent.Click += new System.EventHandler(this.btn_AddEvent_Click);
            // 
            // cbb_Type
            // 
            this.cbb_Type.FormattingEnabled = true;
            this.cbb_Type.Items.AddRange(new object[] {
            resources.GetString("cbb_Type.Items"),
            resources.GetString("cbb_Type.Items1"),
            resources.GetString("cbb_Type.Items2"),
            resources.GetString("cbb_Type.Items3")});
            resources.ApplyResources(this.cbb_Type, "cbb_Type");
            this.cbb_Type.Name = "cbb_Type";
            // 
            // dtp_EndTime
            // 
            resources.ApplyResources(this.dtp_EndTime, "dtp_EndTime");
            this.dtp_EndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_EndTime.Name = "dtp_EndTime";
            // 
            // dtp_StartTime
            // 
            resources.ApplyResources(this.dtp_StartTime, "dtp_StartTime");
            this.dtp_StartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_StartTime.Name = "dtp_StartTime";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // play_time
            // 
            this.play_time.FormattingEnabled = true;
            this.play_time.Items.AddRange(new object[] {
            resources.GetString("play_time.Items"),
            resources.GetString("play_time.Items1"),
            resources.GetString("play_time.Items2"),
            resources.GetString("play_time.Items3"),
            resources.GetString("play_time.Items4"),
            resources.GetString("play_time.Items5")});
            resources.ApplyResources(this.play_time, "play_time");
            this.play_time.Name = "play_time";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // play_count
            // 
            this.play_count.FormattingEnabled = true;
            this.play_count.Items.AddRange(new object[] {
            resources.GetString("play_count.Items"),
            resources.GetString("play_count.Items1"),
            resources.GetString("play_count.Items2"),
            resources.GetString("play_count.Items3"),
            resources.GetString("play_count.Items4"),
            resources.GetString("play_count.Items5")});
            resources.ApplyResources(this.play_count, "play_count");
            this.play_count.Name = "play_count";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // event_type
            // 
            this.event_type.FormattingEnabled = true;
            this.event_type.Items.AddRange(new object[] {
            resources.GetString("event_type.Items"),
            resources.GetString("event_type.Items1"),
            resources.GetString("event_type.Items2"),
            resources.GetString("event_type.Items3"),
            resources.GetString("event_type.Items4")});
            resources.ApplyResources(this.event_type, "event_type");
            this.event_type.Name = "event_type";
            // 
            // play_rank
            // 
            this.play_rank.FormattingEnabled = true;
            this.play_rank.Items.AddRange(new object[] {
            resources.GetString("play_rank.Items"),
            resources.GetString("play_rank.Items1")});
            resources.ApplyResources(this.play_rank, "play_rank");
            this.play_rank.Name = "play_rank";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // btn_del
            // 
            resources.ApplyResources(this.btn_del, "btn_del");
            this.btn_del.Name = "btn_del";
            this.btn_del.UseVisualStyleBackColor = true;
            this.btn_del.Click += new System.EventHandler(this.btn_del_Click);
            // 
            // Form_playtactics
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_del);
            this.Controls.Add(this.btn_AddEvent);
            this.Controls.Add(this.event_type);
            this.Controls.Add(this.play_rank);
            this.Controls.Add(this.play_count);
            this.Controls.Add(this.play_time);
            this.Controls.Add(this.cbb_Type);
            this.Controls.Add(this.dtp_EndTime);
            this.Controls.Add(this.dtp_StartTime);
            this.MaximizeBox = false;
            this.Name = "Form_playtactics";
            this.Load += new System.EventHandler(this.Form_playtactics_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader24;
        private System.Windows.Forms.ColumnHeader columnHeader25;
        private System.Windows.Forms.ColumnHeader columnHeader26;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_AddEvent;
        private System.Windows.Forms.ComboBox cbb_Type;
        private System.Windows.Forms.DateTimePicker dtp_EndTime;
        private System.Windows.Forms.DateTimePicker dtp_StartTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox play_time;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox play_count;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox event_type;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ComboBox play_rank;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button btn_del;
    }
}