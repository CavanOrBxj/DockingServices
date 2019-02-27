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
    public partial class EBMInfo : Form
    {
        public EBD ebd;
        public string AudioUrl = "";
        public EBMInfo()
        {
            InitializeComponent();
        }

        private void EBMInfo_Load(object sender, EventArgs e)
        {
            if (ebd != null)
            {
                lab_EBDID.Text = ebd.SRC.EBRID;
                lab_Version.Text = ebd.EBDVersion;
                lab_EBDDID.Text = ebd.EBDID;
                lab_EBDType.Text = "应急事件";
                lab_CodeA.Text = ebd.EBM.MsgBasicInfo.SenderCode;
                lab_NameA.Text = ebd.EBM.MsgBasicInfo.SenderName;
                lab_EBMID.Text = ebd.EBM.EBMID;
                lab_SentTime.Text = ebd.EBM.MsgBasicInfo.SendTime;
                lab_EBMStartTime.Text = ebd.EBM.MsgBasicInfo.StartTime;
                lab_EBMEndTime.Text = ebd.EBM.MsgBasicInfo.EndTime;
                lab_LanguageType.Text = "中文";
                lab_EBMTitle.Text = ebd.EBM.MsgContent.MsgTitle;
                if (ebd.EBM.MsgContent.Auxiliary != null)
                {
                    lab_EBMType.Text = "音频文件播发";
                }
                else
                {
                    lab_EBMType.Text = "文本转语音播发";
                }
                txt_EBMDesc.Text = ebd.EBM.MsgContent.MsgDesc;
                lab_EBMCode.Text = ebd.EBM.MsgContent.AreaCode;
                if (ebd.EBM.MsgContent.Auxiliary != null)
                {
                    lab_EBMUrl.Text = ebd.EBM.MsgContent.Auxiliary.AuxiliaryDesc;
                }
                else
                {
                    lab_EBMUrl.Text = "";
                }
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_PlayAudio_Click(object sender, EventArgs e)
        {
            AudioPlay play = new AudioPlay();
            play.PlayUrl = AudioUrl;
            play.Show();
        }
    }
}
