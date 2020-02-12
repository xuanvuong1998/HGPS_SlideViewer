using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HGPS_SlideViewer
{
    public static class WebHelper
    {
        //private const string BASE_ADDRESS = "http://robo-ta.com/";
        private const string BASE_ADDRESS = "https://localhost:44353/";
        private const string ACCESS_TOKEN = "1H099XeDsRteM89yy91QonxH3mEd0DoE";
        public static async Task<LessonStatus> GetStatus()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_ADDRESS);
                var response = await client.GetAsync("api/StatusApi/GetLessonStatus");

                if (response.IsSuccessStatusCode)
                {
                    var statusJson = await response.Content.ReadAsStringAsync();
                    if (statusJson != null)
                    {
                        statusJson = Regex.Unescape(statusJson);
                        statusJson = statusJson.Substring(1, statusJson.Length - 2);
                        var status = JsonConvert.DeserializeObject<LessonStatus>(statusJson);
                        return status;
                    }
                }
                return null;
            }
        }

        public static async void UpdateStatus(LessonStatus status)
        {
            if (status != null)
            {
                if (status.AccessToken == null || String.IsNullOrWhiteSpace(status.AccessToken))
                    status.AccessToken = ACCESS_TOKEN;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BASE_ADDRESS);

                    using (var req = new HttpRequestMessage(HttpMethod.Post, "api/StatusApi/Update"))
                    {
                        req.Content = new StringContent(JsonConvert.SerializeObject(status), Encoding.UTF8, "application/json");
                        await client.SendAsync(req);
                    }
                }
            }
        }

        public static async Task<List<QuizData>> GetResults()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_ADDRESS);
                var response = await client.GetAsync("api/Results/GetResults");

                if (response.IsSuccessStatusCode)
                {
                    var resultsJson = await response.Content.ReadAsStringAsync();
                    if (resultsJson != null)
                    {
                        resultsJson = Regex.Unescape(resultsJson);
                        resultsJson = resultsJson.Substring(1, resultsJson.Length - 2);
                        var results = JsonConvert.DeserializeObject<List<QuizData>>(resultsJson);
                        return results;
                    }
                }
                return null;
            }
        }
    }
}
