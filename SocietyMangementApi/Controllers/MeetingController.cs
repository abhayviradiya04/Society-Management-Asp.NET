using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocietyManagementApi.Data;
using SocietyManagementApi.Model;

namespace SocietyManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MeetingController : ControllerBase
    {
        private readonly MeetingRepository _meetingRepository;

        public MeetingController(MeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        [HttpGet]
        public IActionResult GetAllMeetings()
        {
            List<MeetingModel> meetings = _meetingRepository.GetAllMeetings();
            return Ok(meetings);
        }

        [HttpGet("{id}")]
        public IActionResult GetMeetingById(int id)
        {
            MeetingModel meeting = _meetingRepository.GetMeetingById(id);
            if (meeting == null)
                return NotFound("Meeting not found.");

            return Ok(meeting);
        }

        [HttpPost]
        public IActionResult InsertMeeting([FromBody] MeetingModel meeting)
        {
            if (meeting == null)
                return BadRequest("Meeting object cannot be null.");

            try
            {
                bool isInserted = _meetingRepository.InsertMeeting(meeting);
                if (isInserted)
                    return Ok(new { Message = "Meeting inserted successfully!" });

                return StatusCode(500, "Failed to insert the meeting. Please check the logs.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in API: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateMeeting([FromBody] MeetingModel meeting)
        {
            if (meeting == null)
                return BadRequest("Meeting object cannot be null.");

            bool isUpdated = _meetingRepository.UpdateMeeting(meeting);
            if (isUpdated)
                return Ok(new { Message = "Meeting updated successfully!" });

            return StatusCode(500, "An error occurred while updating the meeting.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMeeting(int id)
        {
            bool isDeleted = _meetingRepository.DeleteMeeting(id);
            if (isDeleted)
                return Ok(new { Message = "Meeting deleted successfully!" });

            return StatusCode(500, "An error occurred while deleting the meeting.");
        }

        [HttpGet("Top3")]
        public IActionResult GetTop3Meetings()
        {
            List<MeetingModel> meetings = _meetingRepository.GetTop3Meetings();
            return Ok(meetings);
        }

        [HttpGet("Count")]
        public IActionResult GetMeetingCount()
        {
            int count = _meetingRepository.GetMeetingCount();
            return Ok(count);
        }
    }
}
