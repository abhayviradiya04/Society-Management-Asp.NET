using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocietyMangement.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SocietyMangement.Controllers.Frontend
{
    [CheckAccess]
    public class FNoticeController : Controller
    {
        private readonly HttpClient _client;
        private readonly Uri _baseAddress = new Uri("https://localhost:7057/api/NoticeBoard");

        public FNoticeController()
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
        public async Task<IActionResult> FNoticeList()
        {
            SetAuthorizationHeader();
            List<NoticeBoardModel> noticeList = new List<NoticeBoardModel>();

            try
            {
                // Fetch the data from the API
                HttpResponseMessage response = await _client.GetAsync(string.Empty);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    noticeList = JsonConvert.DeserializeObject<List<NoticeBoardModel>>(data);
                }
                else
                {
                    ModelState.AddModelError("", "Unable to fetch notice data from the API.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred: {ex.Message}");
            }

            // Pass the notice list to the view
            return View(noticeList);
        }
    }
}
