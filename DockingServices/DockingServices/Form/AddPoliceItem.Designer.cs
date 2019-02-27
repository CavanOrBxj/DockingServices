namespace DockingServices
{
    partial class AddPoliceItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddPoliceItem));
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.event_type = new System.Windows.Forms.ComboBox();
            this.play_rank = new System.Windows.Forms.ComboBox();
            this.play_count = new System.Windows.Forms.ComboBox();
            this.play_time = new System.Windows.Forms.ComboBox();
            this.cbb_Type = new System.Windows.Forms.ComboBox();
            this.dtp_EndTime = new System.Windows.Forms.DateTimePicker();
            this.dtp_StartTime = new System.Windows.Forms.DateTimePicker();
            this.ONOK = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(27, 208);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 25;
            this.label7.Text = "播报优先级：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(27, 174);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 24;
            this.label5.Text = "每次播报遍数：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(28, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 26;
            this.label4.Text = "播报时间间隔：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(28, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 28;
            this.label3.Text = "事件等级：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(28, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 27;
            this.label2.Text = "结束时间:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(29, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 22;
            this.label6.Text = "事件类型:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(28, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "开始时间:";
            // 
            // event_type
            // 
            this.event_type.FormattingEnabled = true;
            this.event_type.Items.AddRange(new object[] {
            "气象类",
            "民航类",
            "地震类",
            "水利类",
            "海洋类"});
            this.event_type.Location = new System.Drawing.Point(115, 16);
            this.event_type.Name = "event_type";
            this.event_type.Size = new System.Drawing.Size(117, 20);
            this.event_type.TabIndex = 18;
            // 
            // play_rank
            // 
            this.play_rank.FormattingEnabled = true;
            this.play_rank.Items.AddRange(new object[] {
            "自定义优先",
            "下发优先"});
            this.play_rank.Location = new System.Drawing.Point(115, 204);
            this.play_rank.Name = "play_rank";
            this.play_rank.Size = new System.Drawing.Size(117, 20);
            this.play_rank.TabIndex = 17;
            // 
            // play_count
            // 
            this.play_count.FormattingEnabled = true;
            this.play_count.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "5",
            "8",
            "10"});
            this.play_count.Location = new System.Drawing.Point(115, 171);
            this.play_count.Name = "play_count";
            this.play_count.Size = new System.Drawing.Size(117, 20);
            this.play_count.TabIndex = 19;
            // 
            // play_time
            // 
            this.play_time.FormattingEnabled = true;
            this.play_time.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "5",
            "8",
            "10"});
            this.play_time.Location = new System.Drawing.Point(115, 140);
            this.play_time.Name = "play_time";
            this.play_time.Size = new System.Drawing.Size(117, 20);
            this.play_time.TabIndex = 21;
            // 
            // cbb_Type
            // 
            this.cbb_Type.FormattingEnabled = true;
            this.cbb_Type.Items.AddRange(new object[] {
            "红色预警",
            "橙色预警",
            "黄色预警",
            "蓝色预警"});
            this.cbb_Type.Location = new System.Drawing.Point(115, 108);
            this.cbb_Type.Name = "cbb_Type";
            this.cbb_Type.Size = new System.Drawing.Size(117, 20);
            this.cbb_Type.TabIndex = 20;
            // 
            // dtp_EndTime
            // 
            this.dtp_EndTime.CustomFormat = "HH:mm:ss";
            this.dtp_EndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_EndTime.Location = new System.Drawing.Point(115, 76);
            this.dtp_EndTime.Name = "dtp_EndTime";
            this.dtp_EndTime.Size = new System.Drawing.Size(117, 21);
            this.dtp_EndTime.TabIndex = 16;
            // 
            // dtp_StartTime
            // 
            this.dtp_StartTime.CustomFormat = "HH:mm:ss";
            this.dtp_StartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_StartTime.Location = new System.Drawing.Point(115, 46);
            this.dtp_StartTime.Name = "dtp_StartTime";
            this.dtp_StartTime.Size = new System.Drawing.Size(117, 21);
            this.dtp_StartTime.TabIndex = 15;
            // 
            // ONOK
            // 
            this.ONOK.Location = new System.Drawing.Point(226, 245);
            this.ONOK.Name = "ONOK";
            this.ONOK.Size = new System.Drawing.Size(75, 23);
            this.ONOK.TabIndex = 29;
            this.ONOK.Text = "确定";
            this.ONOK.UseVisualStyleBackColor = true;
            this.ONOK.Click += new System.EventHandler(this.ONOK_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(239, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 30;
            this.label8.Text = "分钟";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(238, 175);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 30;
            this.label9.Text = "遍";
            // 
            // AddPoliceItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 280);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ONOK);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.event_type);
            this.Controls.Add(this.play_rank);
            this.Controls.Add(this.play_count);
            this.Controls.Add(this.play_time);
            this.Controls.Add(this.cbb_Type);
            this.Controls.Add(this.dtp_EndTime);
            this.Controls.Add(this.dtp_StartTime);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AddPoliceItem";
            this.Text = "自定义策略";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox event_type;
        private System.Windows.Forms.ComboBox play_rank;
        private System.Windows.Forms.ComboBox play_count;
        private System.Windows.Forms.ComboBox play_time;
        private System.Windows.Forms.ComboBox cbb_Type;
        private System.Windows.Forms.DateTimePicker dtp_EndTime;
        private System.Windows.Forms.DateTimePicker dtp_StartTime;
        private System.Windows.Forms.Button ONOK;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}