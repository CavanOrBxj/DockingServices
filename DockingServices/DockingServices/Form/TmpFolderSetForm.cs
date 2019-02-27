using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DockingServices
{
    public partial class TmpFolderSetForm : Form
    {
        public TmpFolderSetForm()
        {
            InitializeComponent();
        }

        private void btnRevTar_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtRevTar.Text = dialog.SelectedPath;
            }
        }

        private void TmpFolderSetForm_Load(object sender, EventArgs e)
        {
            txtRevTar.Text = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "RevTarFolder");
            txtTarBuild.Text = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "SndTarFolder");
            txtUnTar.Text = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "UnTarFolder");
            txtXMLBuild.Text = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "XmlBuildFolder");

            txtMedia.Text = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "AudioFileFolder");
            txtBeUnTar.Text = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "BeUnTarFolder");
            txtBeBuildXML.Text = SingletonInfo.GetInstance().serverini.ReadValue("FolderSet", "BeXmlFileMakeFolder");
        }

        private void btnUnTar_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtUnTar.Text = dialog.SelectedPath;
            }
        }

        private void btnXMLBuild_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
               txtXMLBuild.Text = dialog.SelectedPath;
            }
        }

        private void btnTarBuild_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtTarBuild.Text = dialog.SelectedPath;
            }
        }

        private void btnSetFolder_Click(object sender, EventArgs e)
        {
            if (txtRevTar.Text == "")
            {
                MessageBox.Show("请选择接收Tar包文件夹路径！");
                return;
            }
            if (txtTarBuild.Text == "")
            {
                MessageBox.Show("请选择Tar包生成文件夹路径！");
                return;
            }
            if (txtUnTar.Text == "")
            {
                MessageBox.Show("请选择解压Tar包文件夹路径！");
                return;
            }
            if (txtXMLBuild.Text == "")
            {
                MessageBox.Show("请选择生成XML存放文件夹路径！");
                return;
            }
            if (txtBeUnTar.Text == "")
            {
                MessageBox.Show("请选择同步反馈解压Tar包文件夹路径！");
                return;
            }
            if (txtBeBuildXML.Text == "")
            {
                MessageBox.Show("请选择同步反馈生成XML文件夹路径！");
                return;
            }
            if (txtMedia.Text == "")
            {
                MessageBox.Show("请选择音频存放文件夹路径！");
                return;
            }

            SingletonInfo.GetInstance().serverini.WriteValue("FolderSet", "RevTarFolder", txtRevTar.Text);
            SingletonInfo.GetInstance().serverini.WriteValue("FolderSet", "UnTarFolder", txtUnTar.Text);
            SingletonInfo.GetInstance().serverini.WriteValue("FolderSet", "SndTarFolder", txtTarBuild.Text);
            SingletonInfo.GetInstance().serverini.WriteValue("FolderSet", "XmlBuildFolder", txtXMLBuild.Text);

            SingletonInfo.GetInstance().serverini.WriteValue("FolderSet", "BeUnTarFolder", txtBeUnTar.Text);
            SingletonInfo.GetInstance().serverini.WriteValue("FolderSet", "BeXmlFileMakeFolder", txtBeBuildXML.Text);
            SingletonInfo.GetInstance().serverini.WriteValue("FolderSet", "AudioFileFolder", txtMedia.Text);

        }

        private void btnBeUnTar_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtBeUnTar.Text = dialog.SelectedPath;
            }
        }

        private void btnBeBulidXML_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtBeBuildXML.Text = dialog.SelectedPath;
            }
        }

        private void btnMedia_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtMedia.Text = dialog.SelectedPath;
            }
        }
    }
}
