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
    public class MaintenanceController : ControllerBase
    {
        private readonly MaintenanceRepository _maintenanceRepository;

        public MaintenanceController(MaintenanceRepository maintenanceRepository)
        {
            _maintenanceRepository = maintenanceRepository;
        }

        [HttpGet]
        public IActionResult GetAllMaintenanceRecords()
        {
            List<MaintenanceModel> maintenanceRecords = _maintenanceRepository.GetAllMaintenanceRecords();
            return Ok(maintenanceRecords);
        }

        [HttpGet("{id}")]
        public IActionResult GetMaintenanceById(int id)
        {
            MaintenanceModel maintenance = _maintenanceRepository.GetMaintenanceById(id);
            if (maintenance == null)
                return NotFound("Maintenance record not found.");

            return Ok(maintenance);
        }

        [HttpPost]
        public IActionResult InsertMaintenance([FromBody] MaintenanceModel maintenance)
        {
            if (maintenance == null)
                return BadRequest("Maintenance object cannot be null.");

            bool isInserted = _maintenanceRepository.InsertMaintenance(maintenance);
            if (isInserted)
                return Ok(new { Message = "Maintenance record inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the maintenance record.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMaintenance(int id, [FromBody] MaintenanceModel maintenance)
        {
            if (maintenance == null)
                return BadRequest("Maintenance object cannot be null.");

            if (id != maintenance.MaintenanceID)
                return BadRequest("ID in the URL and ID in the object do not match.");

            bool isUpdated = _maintenanceRepository.UpdateMaintenance(maintenance);
            if (isUpdated)
                return Ok(new { Message = "Maintenance record updated successfully!" });

            return StatusCode(500, "An error occurred while updating the maintenance record.");
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteMaintenance(int id)
        {
            bool isDeleted = _maintenanceRepository.DeleteMaintenance(id);
            if (isDeleted)
                return Ok(new { Message = "Maintenance record deleted successfully!" });

            return StatusCode(500, "An error occurred while deleting the maintenance record.");
        }

        [HttpGet("flatnumbar")]

        [HttpGet("flat/{flatID}")]
        public IActionResult GetUserNameByFlatID(int flatID)
        {
            if (flatID != 0)
            {
                var user = _maintenanceRepository.GetUserNameByFlatID(flatID);
                if (!user.Any())
                {
                    return NotFound("No states found for the given CountryID.");
                }
                return Ok(user);
            }
            else
            {
                var flatnumber = _maintenanceRepository.GetFlatNumber();
                return Ok(flatnumber);
            }
        }
    }
}
