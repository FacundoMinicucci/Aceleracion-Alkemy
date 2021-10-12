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
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetOrganization()
        {
            try
            {
                var organization = await _organizationService.GetOrganization();

                if (organization == null) return NotFound();

                return Ok(organization);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        [HttpPut("public")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(OrganizationUpdateDTO organization)
        {
            try
            {
                await _organizationService.Update(organization);
                return Ok("Organization has been modified successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }
    }
}
