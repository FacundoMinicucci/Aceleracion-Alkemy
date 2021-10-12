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
    [Authorize(Roles = "Standard, Administrator")]
    public class TestimonialsController : ControllerBase
    {
        private readonly ITestimonialsService _testimonialsService;
        private readonly IUriService _IUriService;

        public TestimonialsController(ITestimonialsService testimonialsService, IUriService uriService)
        {
            _testimonialsService = testimonialsService;
            _IUriService = uriService;
        }

        /// <summary>
        /// GET: Get all the testimonials.
        /// </summary>
        /// <param name="page">Number of the page of type integer, if it does not exist it returns a 404 error and the following message "The page number you are looking for does not exist."
        /// Example: 1</param>
        /// <returns>Testimonials</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page)
        {
            try
            {
                var route = "/testimonials";
                var objList = await _testimonialsService.GetAll();
                var createPagination = GenericPagination<TestimonialResponseDTO>.Create(objList, page, route, _IUriService);
                var paginationResponse = new PaginationResponse<TestimonialResponseDTO>(createPagination);

                return Ok(paginationResponse);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return NotFound("The page number you are looking for does not exist.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        /// <summary>
        /// POST: Insert a Testimony
        /// </summary>
        /// <param name="testimonialsRequestDTO">
        /// Name: String type field required
        /// Image: String type field
        /// Content: String type field required
        /// Example:
        /// {
        /// "name": "Jorge",
        /// "image": "image base64",
        /// "content": "Testimonio de Jorge"
        /// }
        /// </param>
        /// <returns>Code 200: "Testimony was successfully added"</returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Insert(TestimonialsRequestDTO testimonialsRequestDTO)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList());

                await _testimonialsService.Insert(testimonialsRequestDTO);

                return Ok("Testimony was successfully added");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        /// <summary>
        /// PUT: Update a Testimony
        /// </summary>
        /// <param name="id">Id of the testimony to update, if it can't find it, it returns a 404 error and the following message "The testimonial you are trying to modify was not found."</param>
        /// <param name="testimonalDTO">
        /// Name: String type field
        /// Image: String type field
        /// Content: String type field
        /// Example: 1
        /// {
        /// "name": "Daniela",
        /// "image": "Image base64",
        /// "content": "Testimonio de Daniela"
        /// }
        /// </param>
        /// <returns>Code 200: JSON of the updated testimonial</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(int id, TestimonialUpdateDTO testimonalDTO)
        {
            try
            {
                var testimonialExist = await _testimonialsService.GetById(id);

                if (testimonialExist == null) return NotFound("The testimonial you are trying to modify was not found.");

                await _testimonialsService.Update(testimonalDTO, id);
                return Ok(testimonalDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        /// <summary>
        /// DELETE: Delete a Testimony
        /// </summary>
        /// <param name="id">Id of the testimony to Delete, if it can't find it, it returns a 404 error and the following message "The testimonial you want to delete was not found."
        /// Example: 1</param>
        /// <returns>Code 200</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var testimonialExist = await _testimonialsService.GetById(id);

                if (testimonialExist == null) return NotFound("The testimonial you want to delete was not found.");

                await _testimonialsService.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }
    }
}