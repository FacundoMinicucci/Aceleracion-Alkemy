using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize(Roles = "Administrator")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IActivitiesService _activitiesService;

        public ActivitiesController(IActivitiesService activitiesService)
        {
            _activitiesService = activitiesService;
        }

        [HttpPost]
        public async Task<IActionResult> Insert(ActivitiesDTO activitiDTO)
        {
            try
            {
                if (activitiDTO == null) return BadRequest("Activity could not be inserted");

                var activiti = await _activitiesService.Insert(activitiDTO);

                return Ok(activiti);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ActivitiesDTO activitiDTO)
        {

            try
            {
                var activiti = await _activitiesService.Update(id,activitiDTO);

                if (activiti == null) return BadRequest("The activity you are trying to modify was not found.");

                return Ok("The activity was successfully updated.");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }
    }
}
