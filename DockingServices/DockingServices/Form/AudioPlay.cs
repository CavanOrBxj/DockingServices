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
    public partial class AudioPlay : Form
    {

        public string PlayUrl = "";
        public AudioPlay()
        {
            InitializeComponent();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AudioPlay_Load(object sender, EventArgs e)
        {
            if (PlayUrl != "")
            {
                WMPLib.IWMPMedia me = this.axWindowsMediaPlayer1.newMedia(PlayUrl); //声明一个播放器列表对象
                this.axWindowsMediaPlayer1.currentPlaylist.appendItem(me);    //把地址添加进播放器列表
                this.axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }
    }
}
