using Microsoft.AspNetCore.Mvc;
using SocietyMangement.Models;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace SocietyMangement.Controllers
{
    [CheckAccess]
    [AdminOnly]
    public class NoticeBoardController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7057/api/NoticeBoard");
        private readonly HttpClient _client;

        public NoticeBoardController()
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
        public async Task<IActionResult> NoticeList()
        {
            SetAuthorizationHeader();
            List<NoticeBoardModel> notices = new List<NoticeBoardModel>();

            try
            {
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    notices = JsonConvert.DeserializeObject<List<NoticeBoardModel>>(data);
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
            return View(notices);
        }

        public async Task<IActionResult> NoticeAddEdit(int? NoticeID)
        {
            SetAuthorizationHeader();
            if (NoticeID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/{NoticeID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var notice = JsonConvert.DeserializeObject<NoticeBoardModel>(data);

                    return View(notice);
                }
            }
            return View(new NoticeBoardModel());
        }

        [HttpPost]
        public async Task<IActionResult> NoticeSave(NoticeBoardModel notice)
        {
            SetAuthorizationHeader();
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(notice);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (notice.NoticeID == 0)
                {
                    response = await _client.PostAsync(_client.BaseAddress, content);
                }
                else
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/{notice.NoticeID}", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("NoticeList");
                }
            }

            return View("NoticeAddEdit", notice);
        }

        public async Task<IActionResult> Delete(int NoticeID)
        {
            SetAuthorizationHeader();
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/{NoticeID}");
            return RedirectToAction("NoticeList");
        }
    }
}
