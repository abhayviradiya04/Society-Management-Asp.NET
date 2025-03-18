using Microsoft.AspNetCore.Mvc;
using SocietyMangement.Models;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace SocietyMangement.Controllers
{
    [CheckAccess]
    [AdminOnly]

    public class ContactController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7057/api/Contact");
        private readonly HttpClient _client;

        public ContactController()
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
        public async Task<IActionResult> ContactList()
        {
            SetAuthorizationHeader();
            List<ContactModel> contacts = new List<ContactModel>();
            try
            {
                HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    contacts = JsonConvert.DeserializeObject<List<ContactModel>>(data);
                }
                else
                {
                    ModelState.AddModelError("", "Error fetching contact data.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Exception: {ex.Message}");
            }
            return View(contacts);
        }

        public async Task<IActionResult> ContactAddEdit(int? ContactID)
        {
            SetAuthorizationHeader();
            if (ContactID.HasValue)
            {
                var response = await _client.GetAsync($"{_client.BaseAddress}/{ContactID}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var contact = JsonConvert.DeserializeObject<ContactModel>(data);
                    return View(contact);
                }
            }
            return View(new ContactModel());
        }

        [HttpPost]
        public async Task<IActionResult> ContactSave(ContactModel contact)
        {
            SetAuthorizationHeader();
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(contact);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                if (contact.ID == 0)
                {
                    response = await _client.PostAsync(_client.BaseAddress, content);
                }
                else
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/{contact.ID}", content);
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ContactList");
                }
            }
            return View("ContactDetails", contact);
        }

        public async Task<IActionResult> DeleteContact(int ContactID)
        {
            SetAuthorizationHeader();
            var response = await _client.DeleteAsync($"{_client.BaseAddress}/{ContactID}");
            return RedirectToAction("ContactList");
        }
    }
}