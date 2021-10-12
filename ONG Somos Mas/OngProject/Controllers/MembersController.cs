using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.DTOs;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize(Roles = "Administrator")]
    public class MembersController : ControllerBase
    {
        private IMembersService _membersService;
        private IUriService _uriService;

        public MembersController(IMembersService membersService, IUriService uriService)
        {
            _membersService = membersService;
            _uriService = uriService;
        }

        /// <summary>
        /// GET: Get all the members.
        /// </summary>
        /// <param name="page">Number of the page of type integer, if it does not exist it returns a 404."
        /// Example: 1</param>
        /// <returns>Members</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAll([FromQuery] int page)
        {
            try
            {
                var route = "/members";

                var members = await _membersService.GetAll();

                var createPagination = GenericPagination<MembersDTO>.Create(members, page, route, _uriService);

                var paginationResponse = new PaginationResponse<MembersDTO>(createPagination);

                if (members == null) return NotFound();

                return Ok(paginationResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        /// <summary>
        /// POST: Insert a Members
        /// </summary>
        /// <param name="membersRequestDTO">
        /// Name: String type field required
        /// FacebookURL: String type field
        /// InstagramURL: String
        /// LinkedinURL: String
        /// Image: String type field required
        /// Description: String
        /// Example:
        /// {
        /// "Name": "Pablo",
        /// "FacebookURL": "pablo@a",
        /// "InstagramURL": "pablo@a",
        /// "LinkedinURL": "pablo@a",
        /// "image": "image base64",
        /// "Description": "pablo"
        /// }
        /// </param>
        /// <returns>Code 200: "Members was successfully added"</returns>
        [HttpPost]
        public async Task<IActionResult> Insert(MembersRequestDTO membersRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());

                await _membersService.Insert(membersRequestDTO);

                return Ok("Member was successfully added");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        /// <summary>
        /// DELETE: Delete a Members
        /// </summary>
        /// <param name="id">Id of the members to Delete, if it can't find it, it returns a 404."
        /// Example: 1</param>
        /// <returns>Code 200</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var member = await _membersService.GetById(id);

                if (member == null) return NotFound("The entered member does not exists.");

                await _membersService.Delete(id);

                return StatusCode(StatusCodes.Status204NoContent, "The member was successfully deleted!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }            
        }

        /// <summary>
        /// PUT: Update a Members
        /// </summary>
        /// <param name="id">Id of the members to update, if it can't find it, it returns a 404."</param>
        /// <param name="memberUpdate">
        /// Name: String type field required
        /// FacebookURL: String type field
        /// InstagramURL: String
        /// LinkedinURL: String
        /// Image: String type field required
        /// Description: String
        /// Example:
        /// {
        /// "Name": "Miguel",
        /// "FacebookURL": "Miguel@a",
        /// "InstagramURL": "Miguel@a",
        /// "LinkedinURL": "Miguel@a",
        /// "image": "image base64",
        /// "Description": "Miguel"
        /// }
        /// </param>
        /// <returns>Code 200: JSON of the updated members</returns>
        [HttpPut("{id}")]        
        public async Task<IActionResult> Update(int id, MembersDTO memberUpdate)
        {
            try
            {
                var member = await _membersService.GetById(id);

                if (member == null) return NotFound("The member entered does not exist.");

                await _membersService.Update(id, memberUpdate);

                return Ok("The member was successfully updated.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }            
        }
    }
}
