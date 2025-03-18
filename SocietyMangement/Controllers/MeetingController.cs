using Microsoft.AspNetCore.Mvc;
using SocietyMangement.Models;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace SocietyMangement.Controllers
{
    [CheckAccess]
    [AdminOnly]
    public class MeetingController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7057/api/Meeting");
        private readonly HttpClient _client;

        public MeetingController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
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
        public async Task<IActionResult> MeetingList()
        {
            SetAuthorizationHeader();
            List<MeetingModel> meetings = new List<MeetingModel>();

            try
            {
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    meetings = JsonConvert.DeserializeObject<List<MeetingModel>>(data);
                }
                else
                {
                    ModelState.AddModelError("", "Error fetching data.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Exception: {ex.Message}");
            }
            return View(meetings);
        }

        public async Task<IActionResult> MeetingAddEdit(int? MeetingID)
        {
            SetAuthorizationHeader();
            if (MeetingID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/{MeetingID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var meeting = JsonConvert.DeserializeObject<MeetingModel>(data);

                    return View(meeting);
                }
            }
            return View(new MeetingModel());
        }

        [HttpPost]
        public async Task<IActionResult> MeetingSave(MeetingModel meeting)
        {
            SetAuthorizationHeader();
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(meeting);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (meeting.MeetingID == 0)
                {
                    response = await _client.PostAsync(_client.BaseAddress, content);
                }
                else
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/{meeting.MeetingID}", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("MeetingList");
                }
            }

            return View("MeetingAddEdit", meeting);
        }

        public async Task<IActionResult> Delete(int MeetingID)
        {
            SetAuthorizationHeader();
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/{MeetingID}");
            return RedirectToAction("MeetingList");
        }
    }
}
