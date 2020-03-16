using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace HGPS_SlideViewer
{
    public partial class Form1 : Form
    {
        int width, height;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            //Max screen
            var screen = Screen.PrimaryScreen.Bounds;
            width = screen.Width;
            height = screen.Height;

            picSlide.Location = new Point(0, 0);
            picSlide.Width = width;
            picSlide.Height = height;

            this.Width = width;
            this.Height = height;
            //this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            var status = await WebHelper.GetStatus();

            if (status.LessonState != "ended"
                && status.LessonState != null)
            {
                StartLesson(status);
            }
            else
            {
                NoLesson();
            }
            SyncHelper.StatusChanged += SyncHelper_StatusChanged;
        }

        private void SyncHelper_StatusChanged(object sender, StatusEventArgs e)
        {
            var status = e.Status;
            LessonStatusHelper.LessonStatus = status;
            
            if (status.LessonState == null || status.LessonState == "ended")
            {
                NoLesson();
            }
            else //if (status.LessonState == "started")
            {
                StartLesson(status);

                if (status.MediaPath != null)
                {
                    Action action = new Action(() =>
                    {
                        if (Uri.IsWellFormedUriString(status.MediaPath, UriKind.Absolute))
                        {
                            var mediaForm = new WebVideoForm(status.MediaPath);
                            mediaForm.ShowDialog();
                        }
                        else
                        {
                            var localPath = FileHelper.DropboxPath + $@"\Media\{status.MediaPath}";
                            if (File.Exists(localPath))
                            {
                                var mediaForm = new MediaPlayerForm(localPath);
                                mediaForm.ShowDialog();
                            }
                            else
                            {
                                status.MediaPath = null;
                                status.MediaCompleted = "true";
                                WebHelper.UpdateStatus(status);
                            }
                        }
                    });
                    BeginInvoke(action);
                }
            }
        }

        private void StartLesson(LessonStatus status)
        {
            var path = FileHelper.BasePath +
                        $@"{status.LessonName}\Images\{status.LessonSlide.ToString()}.png";
            if (File.Exists(path))
            {
                Action action = new Action(() =>
                {
                    lblStatus.Visible = false;
                    picSlide.Image = Image.FromFile(path);
                });
                BeginInvoke(action);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ResultsDisplayHelper.DisplayGroupCompetitionResult();
            }
        }

        private void NoLesson()
        {
            var defaultImage =  Application.StartupPath + @"\images\default.png";
            Action action = new Action(() =>
            {
                if (File.Exists(defaultImage))
                {
                    picSlide.Image = Image.FromFile(defaultImage);
                }
                else
                {
                    picSlide.Image = null;
                    lblStatus.Visible = true;
                    lblStatus.Text = "Please start the lesson";
                    lblStatus.Location = new Point((int)(width / 2) - 170, (int)height / 2);
                }
            });
            BeginInvoke(action);
        }
    }
}
