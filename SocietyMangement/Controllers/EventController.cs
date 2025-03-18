using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocietyMangement.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SocietyMangement.Controllers
{
    [CheckAccess]
    [AdminOnly]
   
    public class EventController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7057/api/Event");
        private readonly HttpClient _client;

        public EventController()
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

        #region EventList
        [HttpGet]
        public async Task<IActionResult> EventList()
        {
            SetAuthorizationHeader();
            List<EventModel> events = new List<EventModel>();

            try
            {
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    events = JsonConvert.DeserializeObject<List<EventModel>>(data);
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
            return View(events);
        }
        #endregion

        #region EventAddEdit
        public async Task<IActionResult> EventAddEdit(int? EventID)
        {
            SetAuthorizationHeader();
            if (EventID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/{EventID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var eventModel = JsonConvert.DeserializeObject<EventModel>(data);

                    return View(eventModel);
                }
                
            }
            return View(new EventModel());
        }
        #endregion

        #region EventSave
        [HttpPost]
        public async Task<IActionResult> EventSave(EventModel eventModel, IFormFile? EventImageFile)
        {
            SetAuthorizationHeader();
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (EventImageFile != null && EventImageFile.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}_{EventImageFile.FileName}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await EventImageFile.CopyToAsync(stream);
                    }

                    eventModel.EventImage = $"/images/{fileName}";
                }

                var json = JsonConvert.SerializeObject(eventModel);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (eventModel.EventID == 0)
                {
                    response = await _client.PostAsync(_client.BaseAddress, content);
                }
                else
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/{eventModel.EventID}", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("EventList");
                }
            }

            return View("EventAddEdit", eventModel);
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int EventID)
        {
            SetAuthorizationHeader();
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/{EventID}");
            return RedirectToAction("EventList");
        }
        #endregion
    }
}