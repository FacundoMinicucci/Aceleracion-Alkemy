using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OngProject.Core.DTOs;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Mapper;
using OngProject.Core.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IAWSService _awsService;

        public UsersService(IUnitOfWork unitOfWork, IConfiguration configuration, IMailService mailService, IAWSService awsService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mailService = mailService;
            _awsService = awsService;
        }

        public async Task<object> Delete(int id)
        {
            var user = await _unitOfWork.UsersRepository.GetById(id);
            if(user != null)
            {
                user.IsDeleted = true;
                user.CreatedAt = DateTime.Now;

                await _unitOfWork.SaveChangesAsync();
                
                return true;
            }

            return null;

        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var users = await _unitOfWork.UsersRepository.GetAll();

            if (users.Any())
            {
                var usersDTO = new List<UserDTO>();

                foreach (var user in users)
                {
                    var rol = await _unitOfWork.RolRepository.GetById(user.RolId);
                    var rolName = rol != null ? rol.Name : "NOT FOUND";

                    var userDTO = new EntityMapper().FromUserToUserDto(user);
                    userDTO.RolName = rolName;

                    usersDTO.Add(userDTO);

                }
                return usersDTO;
            }
            else
            {
                return null;
            }

        }

        public async Task<UserInfoDTO> GetById(int id)
        {
            var user = await _unitOfWork.UsersRepository.GetById(id);

            var mapper = new EntityMapper();                      

            return mapper.FromUserToUserInfoDto(user);
        }

        public async Task<object> Update(int id, UsersUpdateDTO entity)
        {
            var user = await _unitOfWork.UsersRepository.GetById(id);

            if (user == null)
            {
                return null;
            }

            IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(entity.Photo);

            var response = await _awsService.UploadImage(ImageFormFile);

            if (response.Code == 200)
            {
                user.Photo = response.URL;
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;

                await _unitOfWork.UsersRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new Exception(response.Errors);
            }
        }

        public async Task<LoginResponseDTO> Insert(UserRegisterDTO register)
        {
            var userExists = await _unitOfWork.UsersRepository.FindByEmail(register.Email);

            if(userExists != null)
            {
                return null;
            }
            
            register.Password = UsersModel.ComputeSha256Hash(register.Password);

            register.ConfirmPassword = UsersModel.ComputeSha256Hash(register.ConfirmPassword);

            var mapper = new EntityMapper();
                        
            var user = mapper.FromUserRegisterDTOToUser(register);          
                                    
            user.Rol = await _unitOfWork.RolRepository.GetById(2); // 2 for Standard User Role ID

            await _unitOfWork.UsersRepository.Insert(user);

            await _unitOfWork.SaveChangesAsync();

            var body = "El nombre de usuario es: " + user.FirstName;
            var subject = "Registration confirmation";
            var contact = "Contacto de la ONG";

            var data = new { mail_tittle = subject,
                            mail_body = body,
                            mail_contact = contact}; 
            var basePathTemplate = @"..\OngProject\Templates\TemplateMailWelcome.html"; 
            var content = _mailService.GetHtml(basePathTemplate, data);
            await _mailService.SendEmailAsync(user.Email, subject, content);

            var token = GetToken(user);

            return mapper.FromUserToLoginResponseDTO(user, token);
            
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO login)
        {
            var user = await _unitOfWork.UsersRepository.FindByEmail(login.Email);

            if (user != null && (user.Password == UsersModel.ComputeSha256Hash(login.Password)))
            {
                var token = GetToken(user);

                var mapper = new EntityMapper();                

                return mapper.FromUserToLoginResponseDTO(user, token);
            }

            return null;
        }

        public string GetToken(UsersModel user)
        {
            // Header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Secret_Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);            

            // Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Rol?.Name),
            };

            // Payload & token creation
            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int GetUserId(string userToken)
        {
            userToken = userToken.Replace("Bearer ", string.Empty); // Delete Bearer + blank space from JWT Token -> Bearer {token} to {token}

            var handler = new JwtSecurityTokenHandler();

            var token = handler.ReadJwtToken(userToken);

            var claims = token.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

            var userId = int.Parse(claims.Value);

            return userId;
        }
    }
}
