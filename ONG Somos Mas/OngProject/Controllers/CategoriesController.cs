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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        private IUriService _uriService;

        public CategoriesController(ICategoriesService categoriesService, IUriService uriService)
        {
            _categoriesService = categoriesService;
            _uriService = uriService;
        }

        /// <summary>
        /// Action that allows obtaining all categories
        /// </summary>
        /// <param name="page">Number of pages</param>
        /// <returns>returns all paginated categories </returns>
        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] int page)
        {
            try
            {
                var route = Request.Path.Value;
                var categories = await _categoriesService.GetAll();

                if (categories == null) return NotFound();

                if (categories.Count() < 10) page = 1;

                var createPagination = GenericPagination<CategoriesOnlyNameDTO>.Create(categories, page, route, _uriService);
                var paginationResponse = new PaginationResponse<CategoriesOnlyNameDTO>(createPagination);

                return Ok(paginationResponse);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }

        }

        /// <summary>
        /// Action that allows obtaining a category
        /// </summary>
        /// <param name="id">Identifier of category, must be numeric</param>
        /// <returns>Return de respective category</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var categories = await _categoriesService.GetById(id);

                if (categories == null) return NotFound();

                return Ok(categories);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        /// <summary>
        /// Action that allows insert a Category
        /// </summary>
        /// <param name="categoriesDTO">Name, Description, url for image</param>
        /// <returns>The category</returns>
        [HttpPost]
        public async Task<IActionResult> Insert(CategoriesDTO categoriesDTO)
        {
            try
            {
                var categories = await _categoriesService.Insert(categoriesDTO);

                if (categories == null) return BadRequest("Category could not be inserted");

                return Ok(categories);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }

        }

        /// <summary>
        /// Action that allows modifying a category  
        /// </summary>
        /// <param name="id">Identifier of the category to modify</param>
        /// <param name="categoryDTO">Name, Description, url for image</param>
        /// <returns>Positive message for the action of update </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoriesDTO categoryDTO)
        {
            try
            {
                var categoryObj = await _categoriesService.GetById(id);

                if (categoryObj == null) return NotFound("The category does not exist");

                await _categoriesService.Update(categoryDTO, id);
                return Ok("The category was successfully updated");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        /// <summary>
        /// Action that allows delete an specific category 
        /// </summary>
        /// <param name="id">Identifier of the category</param>
        /// <returns>Positive message for the action of delete</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var categoryObj = await _categoriesService.GetById(id);

                if (categoryObj == null) return NotFound("The category does not exist");

                await _categoriesService.Delete(id);
                return Ok("The category was successfully deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }
    }
}
