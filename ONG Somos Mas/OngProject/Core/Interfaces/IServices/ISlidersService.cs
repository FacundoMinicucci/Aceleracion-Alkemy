using OngProject.Core.DTOs;
using OngProject.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ISlidersService
    {
        Task<IEnumerable<SlidesDto>> GetAll();
        Task<SlidesModel> GetById(int id);
        Task<SlidesModel> Insert(SlidesCreateDTO entity);
        Task<string> Delete(int id);
        Task<SlidesModel> Update(int id, SlidesUpdateDTO slides);
    }
}
