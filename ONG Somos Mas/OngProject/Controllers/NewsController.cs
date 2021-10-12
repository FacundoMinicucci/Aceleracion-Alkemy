using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.DTOs;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    [Authorize(Roles = "Administrator")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private IUriService _uriService;

        public NewsController(INewsService newsService, IUriService uriService)
        {
            _newsService = newsService;
            _uriService = uriService;
        }

        // GET: All News
        /// <summary>
        /// Retrieves 10 news per page
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /News/GetAll?page=1
        ///
        /// </remarks>
        /// <param name="page">The page number to display the corresponding news</param>
        /// <returns>200: A list of 10 News and relevant paging information</returns>
        /// <response code="404">No news has been found</response>
        [HttpGet("GetAll")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginationResponse<NewsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromQuery] int page)
        {
            try
            {
                var route = "/News/GetAll";

                var news = await _newsService.GetAll();
                if (news == null) return NotFound();

                if (news.Count() < 10) page = 1;

                var createPagination = GenericPagination<NewsDTO>.Create(news, page, route, _uriService);
                var paginationResponse = new PaginationResponse<NewsDTO>(createPagination);

                return Ok(paginationResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
           

        }

        // GET: New by ID
        /// <summary>
        /// Retrieve the New by its ID.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /News/1
        ///
        /// </remarks>
        /// <param name="id">The id number to display the corresponding new</param>
        /// <returns>200: A new with your informationn</returns>
        /// <response code="404">No news has been found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(NewsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var news = await _newsService.GetById(id);
                if (news != null) return Ok(news);

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        // GET: News all its comments
        /// <summary>
        /// Retrieves all comments of a specific News
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Get /News/1/comments
        ///
        /// </remarks>
        /// <param name="id">The id number of the New to retrieve all your comments</param>
        /// <returns>200: A list of comments</returns>
        /// <response code="404">No news has been found</response>
        [HttpGet("{id}/comments")]
        [ProducesResponseType(typeof(IEnumerable<CommentsDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllComments(int id)
        {
            try
            {
                var news = await _newsService.GetById(id);

                if (news != null)
                {
                    var comments = await _newsService.GetAllComments(id);
                    return Ok(comments);
                }

                return NotFound("News has not been found");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        // POST: Insert New
        /// <summary>
        /// Insert a New
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /News
        ///
        /// </remarks>
        /// <param name="newsDTO">
        /// Required: Name, Image and Content. Optional: CategoryID
        /// Example:
        /// {
        /// "name": "nameNew",
        /// "image": "base64Image",
        /// "content": "contentNew"
        /// }
        /// </param>
        /// <returns>200: A new with its content inserted</returns>
        /// <response code="400">Category id does not exist</response>
        [HttpPost]
        [ProducesResponseType(typeof(NewsInsertExitDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Insert(NewsInsertDTO newsDTO)
        {
            try
            {
                var news = await _newsService.Insert(newsDTO);

                if (news == null) return BadRequest("Category id does not exist");

                return Ok(news);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        // PUT: Update New
        /// <summary>
        /// Update a New that exist
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Put /News/1/
        ///
        /// </remarks>
        /// <param name="id">The id number of the New to update</param>
        /// <param name="newsUpdate">
        /// Required: Name, Image and Content. Optional: CategoryID
        /// Example:
        /// {
        /// "name": "nameNew",
        /// "image": "base64Image",
        /// "content": "contentNew"
        /// }
        /// </param>
        /// <returns>200: A new with its content updated</returns>
        /// <response code="404">The entered news does not exist</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(NewsUpdateDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, NewsUpdateDTO newsUpdate)
        {
            try
            {
                var news = await _newsService.GetById(id);
                
                if (news == null) return NotFound("The entered news does not exist");

                await _newsService.Update(id, newsUpdate);

                return Ok(newsUpdate);
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");                
            }            
        }

        // DELETE: Delete New
        /// <summary>
        /// Delete a New that exist
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /News/1/
        ///
        /// </remarks>
        /// <param name="id">The id number of the New to delete</param>
        /// <returns>204: The news was successfully deleted</returns>
        /// <response code="404">The entered news does not exist.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var news = await _newsService.GetById(id);

                if (news == null) return NotFound("The entered news does not exist.");

                await _newsService.Delete(id);

                return StatusCode(StatusCodes.Status204NoContent, "The news was successfully deleted!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
            
        }
    }
}
