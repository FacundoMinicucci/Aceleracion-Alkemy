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
    public class SlidesController : ControllerBase
    {
        private readonly ISlidersService _slidersService;
        public SlidesController(ISlidersService slidersService)
        {
            _slidersService = slidersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var slides = await _slidersService.GetAll();
            return Ok(slides);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var slide = await _slidersService.GetById(id);

                if(slide != null) return Ok(slide);

                return NotFound("Slide not found");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Insert(SlidesCreateDTO slideDTO)
        {
            try
            {
                var slide = await _slidersService.Insert(slideDTO);

                if (slide != null) return Ok(slide);

                return BadRequest("The order of the slide is already taken");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SlidesUpdateDTO slidesDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(slidesDTO.Image)) return BadRequest("Image cant be null or empty");
                if (id == 0) return BadRequest("id cant be cero (0)");

                var slide = await _slidersService.Update(id, slidesDTO);

                if (slide != null) return Ok("Slide be Update");

                return NotFound();
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
                if (id <= 0) return BadRequest("id must be great that cero (0)");

                var slideDeletedMessage = await _slidersService.Delete(id);

                return Ok(slideDeletedMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }
    }
}
