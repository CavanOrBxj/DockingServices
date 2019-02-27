using System;
using System.Windows.Forms;

namespace DockingServices
{
    public partial class ServerSetForm : Form
    {
        private IniFiles inis;
        public ServerSetForm()
        {
            InitializeComponent();
        }

        private void ServerSetForm_Load(object sender, EventArgs e)
        {
            txtZJPlat.Text = SingletonInfo.GetInstance().SendTarAddress;
        }
    }
}
