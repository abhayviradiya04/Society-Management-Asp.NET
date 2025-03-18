using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocietyManagementApi.Data;
using SocietyManagementApi.Model;
using System.Collections.Generic;

namespace SocietyManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoticeBoardController : ControllerBase
    {
        private readonly NoticeBoardRepository _noticeBoardRepository;

        public NoticeBoardController(NoticeBoardRepository noticeBoardRepository)
        {
            _noticeBoardRepository = noticeBoardRepository;
        }

        [HttpGet]
        public IActionResult GetAllNotices()
        {
            List<NoticeBoardModel> notices = _noticeBoardRepository.GetAllNotices();
            return Ok(notices);
        }

        [HttpGet("{id}")]
        public IActionResult GetNoticeById(int id)
        {
            NoticeBoardModel notice = _noticeBoardRepository.GetNoticeById(id);
            if (notice == null)
                return NotFound("Notice not found.");

            return Ok(notice);
        }

        [HttpPost]
        public IActionResult InsertNotice([FromBody] NoticeBoardModel notice)
        {
            if (notice == null)
                return BadRequest("Notice object cannot be null.");

            bool isInserted = _noticeBoardRepository.InsertNotice(notice);
            if (isInserted)
                return Ok(new { Message = "Notice inserted successfully!" });

            return StatusCode(500, "An error occurred while inserting the notice.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNotice([FromBody] NoticeBoardModel notice)
        {
            if (notice == null)
                return BadRequest("Notice object cannot be null.");

            bool isUpdated = _noticeBoardRepository.UpdateNotice(notice);
            if (isUpdated)
                return Ok(new { Message = "Notice updated successfully!" });

            return StatusCode(500, "An error occurred while updating the notice.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNotice(int id)
        {
            bool isDeleted = _noticeBoardRepository.DeleteNotice(id);
            if (isDeleted)
                return Ok(new { Message = "Notice deleted successfully!" });

            return StatusCode(500, "An error occurred while deleting the notice.");
        }
    }
}
