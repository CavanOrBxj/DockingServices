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
    public partial class AddPoliceItem : Form
    {
        public string strevent_type;
        public string StartTime;
        public string EndTime;
        public string strplay_time;
        public string strplay_count;
        public string strplay_rank;
        public string strcbb_Type;

        public AddPoliceItem()
        {
            InitializeComponent();
        }

        private void ONOK_Click(object sender, EventArgs e)
        {
            strevent_type = event_type.Text;
            StartTime = dtp_StartTime.Text;
            EndTime = dtp_EndTime.Text;
            strplay_time = "1次/" + play_time.Text + "分";
            strplay_count = play_count.Text + "遍/次";
            strplay_rank = play_rank.Text;
            strcbb_Type = cbb_Type.Text;

            this.DialogResult = DialogResult.OK;
            this.Close(); 
        }
    }
}
