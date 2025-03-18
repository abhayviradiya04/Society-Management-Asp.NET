using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocietyManagementApi.Data;
using SocietyManagementApi.Model;
using SocietyMangementApi.Data;
using SocietyMangementApi.Model;

namespace SocietyMangementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactRepository _contactRepository;

        public ContactController(ContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        [HttpGet]
        public IActionResult GetAllContacts()
        {
            List<ContactModel> contacts = _contactRepository.GetAllContacts();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public IActionResult GetContactById(int id)
        {
            ContactModel contact = _contactRepository.GetContactById(id);
            if (contact == null)
                return NotFound("Contact not found.");

            return Ok(contact);
        }

        [HttpPost]
        public IActionResult InsertContact([FromBody] ContactModel contact)
        {
            if (contact == null)
                return BadRequest("Contact object cannot be null.");

            bool isInserted = _contactRepository.InsertContact(contact);
            if (isInserted)
                return Ok(new { Message = "Contact inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the contact.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateContact([FromBody] ContactModel contact)
        {
            if (contact == null)
                return BadRequest("Contact object cannot be null.");

            bool isUpdated = _contactRepository.UpdateContact(contact);
            if (isUpdated)
                return Ok(new { Message = "Contact updated successfully!" });

            return StatusCode(500, "An error occurred while updating the contact.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            bool isDeleted = _contactRepository.DeleteContact(id);
            if (isDeleted)
                return Ok(new { Message = "Contact deleted successfully!" });

            return StatusCode(500, "An error occurred while deleting the contact.");
        }
    }
}
