using Microsoft.AspNetCore.Mvc;
using SocietyMangement.Models;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace SocietyMangement.Controllers
{
    [CheckAccess]
    [AdminOnly]
    public class FlatController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7057/api/Flat");
        private readonly HttpClient _client;

        public FlatController()
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
        public async Task<IActionResult> FlatList()
        {
            SetAuthorizationHeader();
            List<FlatModel> flats = new List<FlatModel>();

            try
            {
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    flats = JsonConvert.DeserializeObject<List<FlatModel>>(data);
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
            return View(flats);
        }

        public async Task<IActionResult> FlatAddEdit(int? FlatID)
        {
            SetAuthorizationHeader();
            if (FlatID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/{FlatID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var flat = JsonConvert.DeserializeObject<FlatModel>(data);

                    return View(flat);
                }
            }
            return View(new FlatModel());
        }

        [HttpPost]
        public async Task<IActionResult> FlatSave(FlatModel flat)
        {
            SetAuthorizationHeader();
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(flat);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (flat.FlatID == 0)
                {
                    response = await _client.PostAsync(_client.BaseAddress, content);
                }
                else
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/{flat.FlatID}", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("FlatList");
                }
            }

            return View("FlatAddEdit", flat);
        }

        public async Task<IActionResult> Delete(int FlatID)
        {
            SetAuthorizationHeader();
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/{FlatID}");
            return RedirectToAction("FlatList");
        }
    }
}
