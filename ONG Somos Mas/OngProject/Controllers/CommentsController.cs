using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize(Roles = "Administrator, Standard")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;
        private readonly IUsersService _usersService;
        public CommentsController(ICommentsService commentsService, IUsersService usersService)
        {
            _commentsService = commentsService;
            _usersService = usersService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var comments = await _commentsService.GetAll();

                if (comments != null) return Ok(comments);

                return NotFound("No comments found");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(CommentsRequestDTO commentsRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());

                await _commentsService.Insert(commentsRequestDTO);

                return Ok("Comment was successfully added");
            }
            catch (NullReferenceException e)
            {
                return NotFound($"Error : {e.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(CommentsDto commentDTO, int id)
        {
            try
            {
                string userToken = Request.Headers["Authorization"];

                int userId = _usersService.GetUserId(userToken);

                var commentExist = await _commentsService.GetById(id);

                if (commentExist == null) return StatusCode(StatusCodes.Status404NotFound, "Comment does not exist.");

                var comment = await _commentsService.Update(commentDTO, userId, id);

                if (comment == null) return StatusCode(StatusCodes.Status403Forbidden, "You are not authorized to modify this comment.");

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string userToken = Request.Headers["Authorization"];

                int userId = _usersService.GetUserId(userToken);

                var commentExist = await _commentsService.GetById(id);

                if (commentExist == null) return StatusCode(StatusCodes.Status404NotFound, "Comment does not exist.");

                var comment = await _commentsService.Delete(id, userId);

                if (comment == null) return StatusCode(StatusCodes.Status403Forbidden, "You are not authorized to modify this comment.");

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }
    }
}
