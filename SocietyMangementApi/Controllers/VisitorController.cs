using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocietyMangementApi.Data;
using SocietyMangementApi.Model;

namespace SocietyMangementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorController : ControllerBase
    {
        private readonly VisitorRepository _visitorRepository;

        public VisitorController(VisitorRepository visitorRepository)
        {
            _visitorRepository = visitorRepository;
        }

        [HttpGet]
        public IActionResult GetAllVisitors()
        {
            List<VisitorModel> visitors = _visitorRepository.GetAllVisitors();
            return Ok(visitors);
        }
        [HttpGet("{id}")]
        public IActionResult GetVisitorById(int id)
        {
            VisitorModel visitor = _visitorRepository.GetVisitorById(id);
            if (visitor == null)
                return NotFound("Visitor not found.");

            return Ok(visitor);
        }

        [HttpPost]
        public IActionResult InsertVisitor([FromBody] VisitorModel visitor)
        {
            if (visitor == null)
                return BadRequest("Visitor object cannot be null.");

            bool isInserted = _visitorRepository.InsertVisitor(visitor);
            if (isInserted)
                return Ok(new { Message = "Visitor inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the visitor.");
        }

        [HttpPut("{id}")]   
        public IActionResult UpdateVisitor([FromBody] VisitorModel visitor)
        {
            if (visitor == null)
                return BadRequest("Visitor object cannot be null.");
        
            bool isUpdated = _visitorRepository.UpdateVisitor(visitor);
            if (isUpdated)
                return Ok(new { Message = "Visitor updated successfully!" });

            return StatusCode(500, "An error occurred while updating the visitor.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVisitor(int id)
        {
            bool isDeleted = _visitorRepository.DeleteVisitor(id);
            if (isDeleted)
                return Ok(new { Message = "Visitor deleted successfully!" });

            return StatusCode(500, "An error occurred while deleting the visitor.");
        }




        [HttpGet("GetFlatsByFlatType")]
        public IActionResult GetFlatsByFlatType([FromQuery] string flatTypeName)
        {
            if (string.IsNullOrWhiteSpace(flatTypeName))
                return BadRequest("Flat type is required.");

            var flats = _visitorRepository.GetFlatsByFlatType(flatTypeName);
            if (flats == null || !flats.Any())
                return NotFound("No flats found for the specified flat type.");

            return Ok(flats);
        }


        [HttpGet("GetVisitorsByEntryTime")]
        public IActionResult GetVisitorsByEntryTime([FromQuery] string filter)
        {
            DateTime? startDate = null; // Nullable to support "all" case

            switch (filter.ToLower())
            {
                case "today":
                    startDate = DateTime.Today;
                    break;
                case "lastweek":
                    startDate = DateTime.Today.AddDays(-7);
                    break;
                case "lasttwoweeks":
                    startDate = DateTime.Today.AddDays(-14);
                    break;
                case "all":
                    startDate = null; // No date filter applied
                    break;
                default:
                    return BadRequest("Invalid filter option.");
            }

            var visitors = _visitorRepository.GetVisitorsByEntryTime(startDate);
            if (visitors == null || visitors.Count == 0)
                return NotFound("No visitors found for the selected timeframe.");

            return Ok(visitors);
        }

        [HttpGet("Top3")]
        public IActionResult GetTop3Visitors()
        {
            List<VisitorModel> visitors = _visitorRepository.GetTop3Visitors();
            return Ok(visitors);
        }

        [HttpGet("Count")]
        public IActionResult GetVisitorCount()
        {
            int count = _visitorRepository.GetVisitorCount();
            return Ok(count);
        }


        [HttpGet("Statistics")]
        public IActionResult GetVisitorStatistics()
        {
            VisitorStatsModel stats = _visitorRepository.GetVisitorStatistics();
            return Ok(stats);
        }

    }
}

