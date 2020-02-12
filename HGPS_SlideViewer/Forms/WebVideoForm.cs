using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using CSCore.CoreAudioAPI;

namespace HGPS_SlideViewer
{
    public partial class WebVideoForm : Form
    {
        private string _url;
        private int _width, _height, _audioStateCount = 0;
        private bool _audioPlaying = false, _videoStarted = false;
        private System.Timers.Timer audioStateTimer;
        private const int VIDEO_ENDED_ASSUMPTION = 5; //5 seconds after audio (true -> false) application will close

        public string VideoID
        {
            get
            {
                var yMatch = new Regex(@"http(?:s?)://(?:www\.)?youtu(?:be\.com/watch\?v=|\.be/)([\w\-]+)(&(amp;)?[\w\?=]*)?").Match(_url);
                return yMatch.Success ? yMatch.Groups[1].Value : String.Empty;
            }
        }
        public bool AudioPlaying
        {
            get { return _audioPlaying; }
            set
            {
                AudioStateUpdate(_audioPlaying, value);
                _audioPlaying = value;
            }
        }
        public WebVideoForm(string url)
        {
            InitializeComponent();
            _url = url;
        }
        private void WebVideoForm_Shown(object sender, EventArgs e)
        {
            var screen = Screen.PrimaryScreen.Bounds;
            _width = screen.Width;
            _height = screen.Height;

            this.Width = _width;
            this.Height = _height;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.webBrowser.DocumentText = String.Format("<html><head>" +
                    "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=Edge\"/>" +
                    "</head><body>" +
                    "<iframe width=\"100%\" height=" + (_height - 20).ToString() + " src=\"https://www.youtube.com/embed/{0}?autoplay=1\"" +
                    "frameborder = \"0\" allow = \"autoplay; encrypted-media\" allowfullscreen></iframe>" +
                    "</body></html>", VideoID);

            audioStateTimer = new System.Timers.Timer();
            audioStateTimer.Interval = 1000;
            audioStateTimer.Elapsed += AudioStateTimer_Elapsed;
            audioStateTimer.Enabled = true;
        }
        private void AudioStateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            AudioPlaying = IsAudioPlaying(GetDefaultRenderDevice());
        }
        private void AudioStateUpdate(bool previousState, bool currentState)
        {
            if (previousState != currentState)
            {
                _audioStateCount = 0;

                if (currentState == true & previousState == false)
                {
                    _videoStarted = true;
                }
            }
            else
            {
                _audioStateCount++;
                if (_videoStarted == currentState == false && _audioStateCount == VIDEO_ENDED_ASSUMPTION)
                {
                    Stop();
                }
            }
        }
        private void Stop()
        {
            audioStateTimer.Enabled = false;
            var status = LessonStatusHelper.LessonStatus;
            status.MediaPath = null;
            status.MediaCompleted = "true";
            WebHelper.UpdateStatus(status);

            if (InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    this.Close();
                }));
            }
            else
            {
                this.Close();
            }
        }
        private MMDevice GetDefaultRenderDevice()
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                return enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            }
        }
        private bool IsAudioPlaying(MMDevice device)
        {
            using (var meter = AudioMeterInformation.FromDevice(device))
            {
                return meter.PeakValue > 0.001;
            }
        }
    }
}
