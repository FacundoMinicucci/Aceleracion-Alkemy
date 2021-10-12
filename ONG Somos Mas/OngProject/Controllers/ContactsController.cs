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
    public class ContactsController : ControllerBase
    {
        private readonly IContactsService _contactsService;

        public ContactsController(IContactsService contactsService)
        {
            _contactsService = contactsService;
        }

        [HttpPost()]
        [AllowAnonymous]
        public async Task<IActionResult> Insert(ContactsDTO contact)
        {
            try
            {
                var newContact = await _contactsService.Insert(contact);

                if (newContact)  return Ok();

                return BadRequest();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }

        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var contacts = await _contactsService.GetAll();

                if (contacts == null) return NotFound();
                                
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }

        }
    }
}
