﻿using Microsoft.AspNet.SignalR.Client;
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
        private static IHubProxy _hub;
        private const string CLIENT_NAME = "Robo-TA Slides";
        //private const string _baseAddress = "http://robo-ta.com/";
        private const string _baseAddress = "https://localhost:44353/";


        public static event EventHandler<StatusEventArgs> StatusChanged;

        static SyncHelper()
        {
            _hubConnection = new HubConnection(_baseAddress);
            _hub = _hubConnection.CreateHubProxy("SyncHub");
            _hub.On<string>("statusChanged", (status) => OnStatusChanged(status));
            _hubConnection.Start().Wait();
            _hub.Invoke("Notify", CLIENT_NAME, _hubConnection.ConnectionId);
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