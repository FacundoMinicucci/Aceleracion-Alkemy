namespace OngProject.Core.Interfaces.IServices
{
    using OngProject.Core.DTOs;
    using OngProject.Core.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMembersService
    {
        Task<IEnumerable<MembersDTO>> GetAll();
        Task<MembersModel> GetById(int id);
        Task Insert(MembersRequestDTO memberModel);
        Task Delete(int id);
        Task Update(int id, MembersDTO memberModel);
    }
}
