using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocietyMangement.Models;
using System.Net.Http.Headers;

namespace SocietyMangement.Controllers.Frontend
{
    [CheckAccess]
    public class FEventController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri _baseAddress = new Uri("https://localhost:7057/api/Event");

        public FEventController()
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
        public async Task<IActionResult> FEventList()
        {
            SetAuthorizationHeader();
            List<EventModel> events = new List<EventModel>();

            try
            {
                // Fetch the data from the API
                HttpResponseMessage response = await _client.GetAsync(string.Empty);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    events = JsonConvert.DeserializeObject<List<EventModel>>(data);
                }
                else
                {
                    ModelState.AddModelError("", "Unable to fetch event data from the API.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }

            // Pass the events list to the view
            return View(events);
        }
    }
}
