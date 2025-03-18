using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using SocietyMangement.Models;
using System.Net.Http.Headers;

namespace SocietyMangement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:7057/api/");
        }

        private void SetAuthorizationHeader()
        {
            var token = HttpContext.Session.GetString("JWTToken");

            if (!string.IsNullOrEmpty(token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _logger.LogWarning("? No JWT Token found in session.");
            }
        }

        public async Task<IActionResult> Index()
        {
            SetAuthorizationHeader(); // Ensure authentication (if required)

            var model = new HomeModel
            {
                Events = await FetchTop3<EventModel>("Event"),
                TotalEvents = await FetchCount("Event"),
                Visitors = await FetchTop3<VisitorModel>("Visitor"),
                TotalVisitors = await FetchCount("Visitor"),
                Users = await FetchTop3<UserModel>("User"),
                TotalUsers = await FetchCount("User"),
                Meetings = await FetchTop3<MeetingModel>("Meeting"),
                TotalMeetings = await FetchCount("Meeting"),
                VisitorStats = await FetchVisitorStatistics() // Add this line
            };

            return View(model);
        }

        private async Task<VisitorStatsModel> FetchVisitorStatistics()
        {
            var response = await _client.GetAsync("Visitor/Statistics");

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<VisitorStatsModel>(await response.Content.ReadAsStringAsync());
            }
            return new VisitorStatsModel(); // Return an empty model if the request fails
        }

        private async Task<List<T>> FetchTop3<T>(string endpoint)
        {
            List<T> dataList = new List<T>();
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{endpoint}/Top3");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    dataList = JsonConvert.DeserializeObject<List<T>>(data);
                    _logger.LogInformation($"? Successfully fetched top 3 {endpoint}.");
                }
                else
                {
                    _logger.LogError($"? Failed to fetch top 3 {endpoint}. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error fetching top 3 {endpoint}: {ex.Message}");
            }

            return dataList;
        }

        private async Task<int> FetchCount(string endpoint)
        {
            int count = 0;
            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{endpoint}/Count");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    count = JsonConvert.DeserializeObject<int>(data);
                    _logger.LogInformation($"? Successfully fetched {endpoint} count: {count}");
                }
                else
                {
                    _logger.LogError($"? Failed to fetch {endpoint} count. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error fetching {endpoint} count: {ex.Message}");
            }

            return count;
        }
    }
}
