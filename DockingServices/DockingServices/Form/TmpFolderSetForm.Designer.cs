namespace DockingServices
{
    partial class TmpFolderSetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TmpFolderSetForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRevTar = new System.Windows.Forms.TextBox();
            this.txtUnTar = new System.Windows.Forms.TextBox();
            this.txtXMLBuild = new System.Windows.Forms.TextBox();
            this.txtTarBuild = new System.Windows.Forms.TextBox();
            this.btnSetFolder = new System.Windows.Forms.Button();
            this.btnRevTar = new System.Windows.Forms.Button();
            this.btnUnTar = new System.Windows.Forms.Button();
            this.btnXMLBuild = new System.Windows.Forms.Button();
            this.btnTarBuild = new System.Windows.Forms.Button();
            this.btnBeBulidXML = new System.Windows.Forms.Button();
            this.txtBeBuildXML = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBeUnTar = new System.Windows.Forms.Button();
            this.txtBeUnTar = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnMedia = new System.Windows.Forms.Button();
            this.txtMedia = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "接收TAR包文件夹：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(54, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "生成XML存放文件夹：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "存放解压TAR包文件夹：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(66, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "生成TAR包文件夹：";
            // 
            // txtRevTar
            // 
            this.txtRevTar.Location = new System.Drawing.Point(180, 29);
            this.txtRevTar.Name = "txtRevTar";
            this.txtRevTar.Size = new System.Drawing.Size(352, 21);
            this.txtRevTar.TabIndex = 4;
            // 
            // txtUnTar
            // 
            this.txtUnTar.Location = new System.Drawing.Point(179, 68);
            this.txtUnTar.Name = "txtUnTar";
            this.txtUnTar.Size = new System.Drawing.Size(353, 21);
            this.txtUnTar.TabIndex = 5;
            // 
            // txtXMLBuild
            // 
            this.txtXMLBuild.Location = new System.Drawing.Point(179, 107);
            this.txtXMLBuild.Name = "txtXMLBuild";
            this.txtXMLBuild.Size = new System.Drawing.Size(353, 21);
            this.txtXMLBuild.TabIndex = 6;
            // 
            // txtTarBuild
            // 
            this.txtTarBuild.Location = new System.Drawing.Point(179, 146);
            this.txtTarBuild.Name = "txtTarBuild";
            this.txtTarBuild.Size = new System.Drawing.Size(352, 21);
            this.txtTarBuild.TabIndex = 7;
            // 
            // btnSetFolder
            // 
            this.btnSetFolder.Location = new System.Drawing.Point(488, 313);
            this.btnSetFolder.Name = "btnSetFolder";
            this.btnSetFolder.Size = new System.Drawing.Size(89, 34);
            this.btnSetFolder.TabIndex = 8;
            this.btnSetFolder.Text = "设 置";
            this.btnSetFolder.UseVisualStyleBackColor = true;
            this.btnSetFolder.Click += new System.EventHandler(this.btnSetFolder_Click);
            // 
            // btnRevTar
            // 
            this.btnRevTar.Location = new System.Drawing.Point(538, 28);
            this.btnRevTar.Name = "btnRevTar";
            this.btnRevTar.Size = new System.Drawing.Size(39, 23);
            this.btnRevTar.TabIndex = 9;
            this.btnRevTar.Text = "选择";
            this.btnRevTar.UseVisualStyleBackColor = true;
            this.btnRevTar.Click += new System.EventHandler(this.btnRevTar_Click);
            // 
            // btnUnTar
            // 
            this.btnUnTar.Location = new System.Drawing.Point(538, 67);
            this.btnUnTar.Name = "btnUnTar";
            this.btnUnTar.Size = new System.Drawing.Size(39, 23);
            this.btnUnTar.TabIndex = 10;
            this.btnUnTar.Text = "选择";
            this.btnUnTar.UseVisualStyleBackColor = true;
            this.btnUnTar.Click += new System.EventHandler(this.btnUnTar_Click);
            // 
            // btnXMLBuild
            // 
            this.btnXMLBuild.Location = new System.Drawing.Point(538, 106);
            this.btnXMLBuild.Name = "btnXMLBuild";
            this.btnXMLBuild.Size = new System.Drawing.Size(39, 23);
            this.btnXMLBuild.TabIndex = 11;
            this.btnXMLBuild.Text = "选择";
            this.btnXMLBuild.UseVisualStyleBackColor = true;
            this.btnXMLBuild.Click += new System.EventHandler(this.btnXMLBuild_Click);
            // 
            // btnTarBuild
            // 
            this.btnTarBuild.Location = new System.Drawing.Point(538, 145);
            this.btnTarBuild.Name = "btnTarBuild";
            this.btnTarBuild.Size = new System.Drawing.Size(39, 23);
            this.btnTarBuild.TabIndex = 12;
            this.btnTarBuild.Text = "选择";
            this.btnTarBuild.UseVisualStyleBackColor = true;
            this.btnTarBuild.Click += new System.EventHandler(this.btnTarBuild_Click);
            // 
            // btnBeBulidXML
            // 
            this.btnBeBulidXML.Location = new System.Drawing.Point(538, 223);
            this.btnBeBulidXML.Name = "btnBeBulidXML";
            this.btnBeBulidXML.Size = new System.Drawing.Size(39, 23);
            this.btnBeBulidXML.TabIndex = 15;
            this.btnBeBulidXML.Text = "选择";
            this.btnBeBulidXML.UseVisualStyleBackColor = true;
            this.btnBeBulidXML.Click += new System.EventHandler(this.btnBeBulidXML_Click);
            // 
            // txtBeBuildXML
            // 
            this.txtBeBuildXML.Location = new System.Drawing.Point(179, 224);
            this.txtBeBuildXML.Name = "txtBeBuildXML";
            this.txtBeBuildXML.Size = new System.Drawing.Size(352, 21);
            this.txtBeBuildXML.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(54, 228);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "反馈生成XML文件夹：";
            // 
            // btnBeUnTar
            // 
            this.btnBeUnTar.Location = new System.Drawing.Point(538, 184);
            this.btnBeUnTar.Name = "btnBeUnTar";
            this.btnBeUnTar.Size = new System.Drawing.Size(39, 23);
            this.btnBeUnTar.TabIndex = 18;
            this.btnBeUnTar.Text = "选择";
            this.btnBeUnTar.UseVisualStyleBackColor = true;
            this.btnBeUnTar.Click += new System.EventHandler(this.btnBeUnTar_Click);
            // 
            // txtBeUnTar
            // 
            this.txtBeUnTar.Location = new System.Drawing.Point(179, 185);
            this.txtBeUnTar.Name = "txtBeUnTar";
            this.txtBeUnTar.Size = new System.Drawing.Size(352, 21);
            this.txtBeUnTar.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(48, 189);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "反馈解压缩包文件夹：";
            // 
            // btnMedia
            // 
            this.btnMedia.Location = new System.Drawing.Point(538, 262);
            this.btnMedia.Name = "btnMedia";
            this.btnMedia.Size = new System.Drawing.Size(39, 23);
            this.btnMedia.TabIndex = 21;
            this.btnMedia.Text = "选择";
            this.btnMedia.UseVisualStyleBackColor = true;
            this.btnMedia.Click += new System.EventHandler(this.btnMedia_Click);
            // 
            // txtMedia
            // 
            this.txtMedia.Location = new System.Drawing.Point(179, 263);
            this.txtMedia.Name = "txtMedia";
            this.txtMedia.Size = new System.Drawing.Size(352, 21);
            this.txtMedia.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 267);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(149, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "音频媒体文件存放文件夹：";
            // 
            // TmpFolderSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSeaGreen;
            this.ClientSize = new System.Drawing.Size(632, 379);
            this.Controls.Add(this.btnMedia);
            this.Controls.Add(this.txtMedia);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnBeUnTar);
            this.Controls.Add(this.txtBeUnTar);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnBeBulidXML);
            this.Controls.Add(this.txtBeBuildXML);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnTarBuild);
            this.Controls.Add(this.btnXMLBuild);
            this.Controls.Add(this.btnUnTar);
            this.Controls.Add(this.btnRevTar);
            this.Controls.Add(this.btnSetFolder);
            this.Controls.Add(this.txtTarBuild);
            this.Controls.Add(this.txtXMLBuild);
            this.Controls.Add(this.txtUnTar);
            this.Controls.Add(this.txtRevTar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TmpFolderSetForm";
            this.Text = "处理文件夹设置";
            this.Load += new System.EventHandler(this.TmpFolderSetForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtRevTar;
        private System.Windows.Forms.TextBox txtUnTar;
        private System.Windows.Forms.TextBox txtXMLBuild;
        private System.Windows.Forms.TextBox txtTarBuild;
        private System.Windows.Forms.Button btnSetFolder;
        private System.Windows.Forms.Button btnRevTar;
        private System.Windows.Forms.Button btnUnTar;
        private System.Windows.Forms.Button btnXMLBuild;
        private System.Windows.Forms.Button btnTarBuild;
        private System.Windows.Forms.Button btnBeBulidXML;
        private System.Windows.Forms.TextBox txtBeBuildXML;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBeUnTar;
        private System.Windows.Forms.TextBox txtBeUnTar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnMedia;
        private System.Windows.Forms.TextBox txtMedia;
        private System.Windows.Forms.Label label7;
    }
}