﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HGPS_SlideViewer
{
    class ResultsDisplayHelper
    {
        private const string URL1 = "http://robo-ta.com/Ranking/GroupCompetition";
        private const string URL2 = "http://robo-ta.com/Ranking/GroupChallenge";
        private const string URL3 = "http://robo-ta.com/Ranking/Individual";
        private const string URL4 = "http://robo-ta.com/Ranking/Improvement";
        
        
        public static void DisplayGroupCompetitionResult()
        {
            ProcessStartInfo pInfo = new ProcessStartInfo(URL1);

            Process.Start(pInfo);
        }

        public static void DisplayGroupChallengeResult()
        {
            ProcessStartInfo pInfo = new ProcessStartInfo(URL2);

            Process.Start(pInfo);
        }

        public static void DisplayBestStudents()
        {
            ProcessStartInfo pInfo = new ProcessStartInfo(URL3);

            Process.Start(pInfo);
        }

        public static void DisplayBestImprovements()
        {
            ProcessStartInfo pInfo = new ProcessStartInfo(URL4);

            Process.Start(pInfo);
        }

        /// <summary>
        /// Kill any chrome instance
        /// </summary>
        public static void HideResult()
        {
            var chromeInstances = Process.GetProcessesByName("chrome");

            foreach (var chrome in chromeInstances)
            {
                if (chrome.HasExited == false)
                {
                    chrome.Kill();
                }
            }
        }
    }
}
