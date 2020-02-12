using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HGPS_SlideViewer
{
    public partial class MediaPlayerForm : Form
    {
        private int _width, _height;
        private string _videoPath = "";
        public MediaPlayerForm(string videoPath)
        {
            InitializeComponent();
            _videoPath = videoPath;
        }

        private void MediaPlayerForm_Shown(object sender, EventArgs e)
        {
            var screen = Screen.PrimaryScreen.Bounds;
            _width = screen.Width;
            _height = screen.Height;

            this.Width = _width;
            this.Height = _height;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            axWMP.Location = new Point(0, 0);
            axWMP.Size = new Size(_width, _height);
            axWMP.URL = _videoPath;
            axWMP.Ctlcontrols.play();
            axWMP.PlayStateChange += AxWMP_PlayStateChange;
        }

        private void AxWMP_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if ((WMPLib.WMPPlayState)e.newState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWMP.fullScreen = true;
            }
            else if ((WMPLib.WMPPlayState)e.newState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                var status = LessonStatusHelper.LessonStatus;

                status.MediaPath = null;
                status.MediaCompleted = "true";
                WebHelper.UpdateStatus(status);

                axWMP.Dispose();
                this.Close();
            }
        }
    }
}
