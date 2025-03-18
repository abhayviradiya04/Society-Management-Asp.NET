using Microsoft.AspNetCore.Mvc;
using SocietyMangement.Models;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace SocietyMangement.Controllers
{
    [CheckAccess]
    [AdminOnly]
    public class MaintenanceController : Controller
    {
        private readonly Uri _baseAddress = new Uri("https://localhost:7057/api/Maintenance");
        private readonly HttpClient _client;

        public MaintenanceController()
        {
            _client = new HttpClient
            {
                BaseAddress = _baseAddress
            };
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
        public async Task<IActionResult> MaintenanceList()
        {
            SetAuthorizationHeader();
            List<MaintenanceModel> maintenanceRecords = new List<MaintenanceModel>();

            try
            {
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    maintenanceRecords = JsonConvert.DeserializeObject<List<MaintenanceModel>>(data);
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

            return View(maintenanceRecords);
        }
        public async Task<IActionResult> MaintenanceAddEdit(int? MaintenanceID)
        {
            SetAuthorizationHeader();
            // Fetch Flat Numbers (List for dropdown)
            await GetFlatNumber();
            

            if (MaintenanceID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/{MaintenanceID}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var maintenance = JsonConvert.DeserializeObject<MaintenanceModel>(data);

                    // Fetch the related user details (username by FlatID)
                    ViewBag.UserName = await GetUserNameByFlatIDs(maintenance.FlatID);

                    return View(maintenance);
                }
                else
                {
                    ModelState.AddModelError("", "Error fetching maintenance data.");
                }
            }

            return View(new MaintenanceModel());
        }



        [HttpPost]
        public async Task<IActionResult> MaintenanceSave(MaintenanceModel maintenance)
        {
            SetAuthorizationHeader();

            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(maintenance);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (maintenance.MaintenanceID == 0)
                {
                    response = await _client.PostAsync(_client.BaseAddress, content);
                }
                else
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/{maintenance.MaintenanceID}", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("MaintenanceList");
                }
            }

            return View("MaintenanceAddEdit", maintenance);
        }

        public async Task<IActionResult> Delete(int MaintenanceID)
        {
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/{MaintenanceID}");
            return RedirectToAction("MaintenanceList");
        }

        private async Task GetFlatNumber()
        {
            SetAuthorizationHeader();
            try
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/flatnumbar");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var flatnumber = JsonConvert.DeserializeObject<List<GetFlatnumber>>(data);
                    ViewBag.FlatNumber = flatnumber ?? new List<GetFlatnumber>(); // Ensure it's not null
                }
                else
                {
                    ViewBag.FlatNumber = new List<GetFlatnumber>(); // Assign an empty list if API fails
                }
            }
            catch
            {
                ViewBag.FlatNumber = new List<GetFlatnumber>(); // Handle exception safely
            }
        }



        [HttpPost]
        public async Task<JsonResult> GetUserNameByFlatID(int FlatID)
        {
            SetAuthorizationHeader();
            var user = await GetUserNameByFlatIDs(FlatID);
            return Json(user);
        }

        private async Task<List<GetUserNameByFlatID>> GetUserNameByFlatIDs(int FlatID)
        {
            SetAuthorizationHeader();
            var response = await _client.GetAsync($"{_client.BaseAddress}/flat/{FlatID}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<GetUserNameByFlatID>>(data);
            }
            return new List<GetUserNameByFlatID>();
        }
    }
}
