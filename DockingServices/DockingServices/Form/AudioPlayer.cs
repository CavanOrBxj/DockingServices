using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AxWMPLib;

namespace DockingServices
{
    public partial class AudioPlayer : UserControl
    {
        private List<string> playList;
        private int loopTick;
        private AxWindowsMediaPlayer MediaPlayer;
        private string url;
        public AudioPlayer()
        {
            try
            {
                InitializeComponent();
                MediaPlayer = new AxWindowsMediaPlayer();
                playList = new List<string>();
                MediaPlayer.BeginInit();
                this.Controls.Add(MediaPlayer);
                MediaPlayer.EndInit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region 播放控制
        public void Stop()
        {
            MediaPlayer.Ctlcontrols.stop();
        }
        public void Pause()
        {
            MediaPlayer.Ctlcontrols.pause();
        }
        public void Play()
        {
            MediaPlayer.Ctlcontrols.play();
        }
        public void Next()
        {
            MediaPlayer.Ctlcontrols.next();
        }
        public void Previous()
        {
            MediaPlayer.Ctlcontrols.previous();
        }
        #endregion End

        public void AddPlayListItem(string ListItem)
        {
            playList.Add(ListItem);
        }

        public int LoopTick
        {
            get { return loopTick; }
            set { this.loopTick = value; }
        }

        public string URL
        {
            //get { return url; }
            set { this.url = value; }
        }
    }
}
