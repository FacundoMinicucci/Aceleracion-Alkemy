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
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;   

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;           
        }
        

        /// <summary>
        /// Action that allow obtain all user for Administrator only
        /// </summary>
        /// <returns>All User</returns>
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _usersService.GetAll();

                if (users != null)
                {
                    return Ok(users);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }

        }

        /// <summary>
        /// Action that allow obtain and User by id
        /// </summary>
        /// <param name="id">Identifier of user, must be numeric</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _usersService.GetById(id);

            if (user == null) return NotFound("User does not exists.");

            return Ok(user);
        }


        /// <summary>
        /// Action that allows the user to know their data
        /// </summary>
        /// <returns>Data from user</returns>
        [HttpGet("/auth/me")]        
        public async Task<IActionResult> GetUserInfo()
        {
            try
            {
                string userToken = Request.Headers["Authorization"];           

                int userId = _usersService.GetUserId(userToken);

                var user = await _usersService.GetById(userId);

                if (user == null) return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        /// <summary>
        /// Action for user registration
        /// </summary>
        /// <param name="user">First Name, Last Name, Email, Password, confirm password</param>
        /// <returns>The new user</returns>
        [HttpPost("/auth/register")]
        [AllowAnonymous]
        public async Task<IActionResult> Insert(UserRegisterDTO user)
        {
            try
            {
                var newUser = await _usersService.Insert(user);

                if (newUser == null) return BadRequest("Your email is already in use.");

                return Ok(newUser);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }


        /// <summary>
        /// Action that allows to enter the system
        /// </summary>
        /// <param name="login">Email and Password</param>
        /// <returns>Token, name and email</returns>
        [HttpPost("/auth/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequestDTO login)
        {
            try
            {
                var user = await _usersService.Login(login);

                if (user == null)
                {
                    return NotFound("Invalid email or password.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }

        /// <summary>
        /// Action that allow update and user
        /// </summary>
        /// <param name="id">Identifier of user, must be numeric</param>
        /// <param name="userUppodateDTO">First Name, Last Name and photo</param>
        /// <returns>Positive message for the action of update</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UsersUpdateDTO userUppodateDTO)
        {
            try
            {
                var activiti = await _usersService.Update(id, userUppodateDTO);

                if (activiti == null) return BadRequest("The user you are trying to modify was not found.");

                return Ok("The user was successfully updated.");

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }
        }


        /// <summary>
        /// Action that allow delete and user
        /// </summary>
        /// <param name="id">Identifier of user, must be numeric</param>
        /// <returns>Positive message for the action of delete</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _usersService.Delete(id);

                if (user == null)
                {
                    return NotFound("The user you are trying to delete was not found.");
                }

                return Ok("User was deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error : {ex.Message}");
            }

        }
    }
}
