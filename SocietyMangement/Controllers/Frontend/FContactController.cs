using Microsoft.AspNetCore.Mvc;
using SocietyMangement.Models;
using System.Text;
using Newtonsoft.Json;

namespace SocietyMangement.Controllers.Frontend
{
    [CheckAccess]

    public class FContactController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7057/api/Contact");
        private readonly HttpClient _client;

        public FContactController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult FContact()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContact(ContactModel contact)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(contact);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Message sent successfully!";
                    return RedirectToAction("FContact");
                }
                else
                {
                    ModelState.AddModelError("", "Error submitting form.");
                }
            }
            return View("FContact", contact);
        }
    }
}
