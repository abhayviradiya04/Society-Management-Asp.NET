using Microsoft.AspNetCore.Mvc;
using SocietyMangement.Models;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace SocietyMangement.Controllers
{
    [CheckAccess]
    [AdminOnly]
    public class VisitorController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7057/api/Visitor");
        private readonly HttpClient _client;

        public VisitorController()
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
        public async Task<IActionResult> VisitorList()
        {
            SetAuthorizationHeader();
            List<VisitorModel> visitors = new List<VisitorModel>();

            try
            {
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    visitors = JsonConvert.DeserializeObject<List<VisitorModel>>(data);
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
            return View(visitors);
        }

                


        public async Task<IActionResult> VisitorAddEdit(int? VisitorID)
        {
            SetAuthorizationHeader();
            if (VisitorID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/{VisitorID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var visitor = JsonConvert.DeserializeObject<VisitorModel>(data);

                    return View(visitor);
                }
            }
            return View(new VisitorModel());
        }

        [HttpPost]
        public async Task<IActionResult> VisitorSave(VisitorModel visitor)
            {
            SetAuthorizationHeader();
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(visitor);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (visitor.VisitorID == 0)
                {
                    response = await _client.PostAsync(_client.BaseAddress, content);
                }
                else
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/{visitor.VisitorID}", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("VisitorList");
                }

            }

            return View("VisitorAddEdit", visitor);
        }


        public async Task<IActionResult> Delete(int VisitorID)
        {
            SetAuthorizationHeader();
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/{VisitorID}");
            return RedirectToAction("VisitorList");
        }


        [HttpGet("GetVisitorsByEntryTime")]
        public async Task<IActionResult> GetVisitorsByEntryTime(string filter)
        {
            SetAuthorizationHeader();

            try
            {
                HttpResponseMessage response = await _client.GetAsync($"{_client.BaseAddress}/GetVisitorsByEntryTime?filter={filter}");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var visitors = JsonConvert.DeserializeObject<List<VisitorModel>>(data);
                    return Json(visitors);
                }
                else
                {
                    return BadRequest("Error fetching filtered data.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server: {ex.Message}");
            }
        }



    }
}
