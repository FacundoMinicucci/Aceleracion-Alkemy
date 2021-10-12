using OngProject.Core.DTOs;
using OngProject.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface INewsService
    {
        Task<NewsDTO> GetById(int id);
        Task Delete(int id);
        Task<IEnumerable<NewsDTO>> GetAll();
        Task<IEnumerable<CommentsDto>> GetAllComments(int id);
        public Task<NewsInsertExitDTO> Insert(NewsInsertDTO entity);
        Task Update(int id, NewsUpdateDTO entity);
    }
}
