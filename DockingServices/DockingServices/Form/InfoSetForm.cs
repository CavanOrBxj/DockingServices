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
    public partial class InfoSetForm : Form
    {
       
        public InfoSetForm()
        {
            InitializeComponent();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            SingletonInfo.GetInstance().serverini.WriteValue("INFOSET", "SourceID", txtSourceID.Text);
            SingletonInfo.GetInstance().serverini.WriteValue("INFOSET", "SourceName", txtSourceName.Text);
            SingletonInfo.GetInstance().serverini.WriteValue("INFOSET", "SourceType", txtSourceType.Text);
        }

        private void InfoSetForm_Load(object sender, EventArgs e)
        {
            txtSourceID.Text = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "SourceID");
            txtSourceName.Text = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "SourceName");
            txtSourceType.Text = SingletonInfo.GetInstance().serverini.ReadValue("INFOSET", "SourceType");
        }
    }
}
