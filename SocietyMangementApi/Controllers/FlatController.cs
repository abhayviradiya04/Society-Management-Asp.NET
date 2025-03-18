using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocietyManagementApi.Data;
using SocietyManagementApi.Model;

namespace SocietyMangementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FlatController : ControllerBase
    {
        private readonly FlatRepository _flatRepository;

        public FlatController(FlatRepository flatRepository)
        {
            _flatRepository = flatRepository;
        }

        [HttpGet]
        public IActionResult GetAllFlats()
        {
            List<FlatModel> flats = _flatRepository.GetAllFlats();
            return Ok(flats);
        }

        [HttpGet("{id}")]
        public IActionResult GetFlatById(int id)
        {
            FlatModel flat = _flatRepository.GetFlatById(id);
            if (flat == null)
                return NotFound("Flat not found.");

            return Ok(flat);
        }

        [HttpPost]
        public IActionResult InsertFlat([FromBody] FlatModel flat)
        {
            if (flat == null)
                return BadRequest("Flat object cannot be null.");

            bool isInserted = _flatRepository.InsertFlat(flat);
            if (isInserted)
                return Ok(new { Message = "Flat inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the flat.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFlat([FromBody] FlatModel flat)
        {
            if (flat == null)
                return BadRequest("Flat object cannot be null.");

            bool isUpdated = _flatRepository.UpdateFlat(flat);
            if (isUpdated)
                return Ok(new { Message = "Flat updated successfully!" });

            return StatusCode(500, "An error occurred while updating the flat.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFlat(int id)
        {
            bool isDeleted = _flatRepository.DeleteFlat(id);
            if (isDeleted)
                return Ok(new { Message = "Flat deleted successfully!" });

            return StatusCode(500, "An error occurred while deleting the flat.");
        }
    }
}
