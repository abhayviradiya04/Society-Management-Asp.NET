using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocietyMangement.Models;
using System.Net.Http.Headers;

namespace SocietyMangement.Controllers.Frontend
{
    [CheckAccess]
    public class FMeetingController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri _baseAddress = new Uri("https://localhost:7057/api/Meeting");

        public FMeetingController()
        {
            _client = new HttpClient { BaseAddress = _baseAddress };
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
                Console.WriteLine("⚠️ No JWT Token found in session.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> FMeetingList()
        {

            SetAuthorizationHeader();
            List<MeetingModel> meetings = new List<MeetingModel>();

            try
            {
                // Fetch the data from the API
                HttpResponseMessage response = await _client.GetAsync(string.Empty);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    meetings = JsonConvert.DeserializeObject<List<MeetingModel>>(data);
                }
                else
                {
                    ModelState.AddModelError("", "Unable to fetch meeting data from the API.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }

            // Pass the meetings list to the view
            return View(meetings);
        }
    }
}
