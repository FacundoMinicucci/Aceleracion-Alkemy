using OngProject.Core.DTOs;
using OngProject.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface IUsersService
    {
        public Task<object> Delete(int id);
        Task<IEnumerable<UserDTO>> GetAll();        
        public Task<UserInfoDTO> GetById(int id);
        public Task<object> Update(int id, UsersUpdateDTO entity); 
        public Task<LoginResponseDTO> Insert(UserRegisterDTO entity);
        public Task<LoginResponseDTO> Login(LoginRequestDTO login);
        public string GetToken(UsersModel user);
        public int GetUserId(string userToken);
    }
}
