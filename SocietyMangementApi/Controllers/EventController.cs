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
    public class EventController : ControllerBase
    {
        private readonly EventRepository _eventRepository;

        public EventController(EventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public IActionResult GetAllEvents()
        {
            List<EventModel> events = _eventRepository.GetAllEvents();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public IActionResult GetEventById(int id)
        {
            EventModel eventModel = _eventRepository.GetEventById(id);
            if (eventModel == null)
                return NotFound("Event not found.");

            return Ok(eventModel);
        }

        [HttpPost]
        public IActionResult InsertEvent([FromBody] EventModel eventModel)
        {
            if (eventModel == null)
                return BadRequest("Event object cannot be null.");

            bool isInserted = _eventRepository.InsertEvent(eventModel);
            if (isInserted)
                return Ok(new { Message = "Event inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the event.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEvent([FromBody] EventModel eventModel)
        {
            if (eventModel == null)
                return BadRequest("Event object cannot be null.");

            bool isUpdated = _eventRepository.UpdateEvent(eventModel);
            if (isUpdated)
                return Ok(new { Message = "Event updated successfully!" });

            return StatusCode(500, "An error occurred while updating the event.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            bool isDeleted = _eventRepository.DeleteEvent(id);
            if (isDeleted)
                return Ok(new { Message = "Event deleted successfully!" });

            return StatusCode(500, "An error occurred while deleting the event.");
        }

        [HttpGet("Top3")]
        public IActionResult GetTop3Events()
        {
            List<EventModel> events = _eventRepository.GetTop3Events();
            return Ok(events);
        }

        [HttpGet("Count")]
        public IActionResult GetEventCount()
        {
            int count = _eventRepository.GetEventCount();
            return Ok(count);
        }


    }
}
