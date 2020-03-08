using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGPS_SlideViewer
{
    public static class SyncHelper
    {
        private static HubConnection _hubConnection;
        private static IHubProxy _syncHub, _myHub;
        private const string CLIENT_NAME = "Robo-TA Slides";
        private const string _baseAddress = "http://robo-ta.com/";
        //private const string _baseAddress = "https://localhost:44353/";

        public static event EventHandler<StatusEventArgs> StatusChanged;

        static SyncHelper()
        {
            _hubConnection = new HubConnection(_baseAddress);
            _syncHub = _hubConnection.CreateHubProxy("SyncHub");
            _myHub = _hubConnection.CreateHubProxy("MyHub");

            _syncHub.On<string>("statusChanged", (status) => OnStatusChanged(status));

            _myHub.On("OpenResultUrl", (rankingsType)
                => OnDisplayResult(rankingsType));
           
            _hubConnection.Start().Wait();
            _syncHub.Invoke("Notify", CLIENT_NAME, _hubConnection.ConnectionId);
        }

        private static void OnDisplayResult(string rankingsType)
        {
            if (rankingsType.Contains("hide"))
            {
                //System.Windows.Forms.MessageBox.Show("HIDE RESULT");
                ResultsDisplayHelper.HideResult();
                return;
            }

            if (rankingsType.Contains("group-competition"))
            {
                ResultsDisplayHelper.DisplayGroupCompetitionResult();
            }else if (rankingsType.Contains("group-challenge"))
            {
                ResultsDisplayHelper.DisplayGroupChallengeResult();
            }else if (rankingsType.Contains("individual"))
            {
                if (rankingsType.Contains("improvement"))
                {
                    ResultsDisplayHelper.DisplayBestImprovements();
                }else if (rankingsType.Contains("top-students"))
                {
                    ResultsDisplayHelper.DisplayBestStudents();
                }
            }
        }

        private static void OnStatusChanged(string status)
        {
            var _status = JsonConvert.DeserializeObject<LessonStatus>(status);
            var e = new StatusEventArgs(_status);
            StatusChanged?.Invoke(null, e);
        }
    }

    public class StatusEventArgs
    {
        public LessonStatus Status { get; set; }
        public StatusEventArgs(LessonStatus status)
        {
            Status = status;
        }
    }
}
