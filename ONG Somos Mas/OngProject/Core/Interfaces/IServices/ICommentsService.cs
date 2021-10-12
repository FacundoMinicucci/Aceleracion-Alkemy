using OngProject.Core.DTOs;
using OngProject.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ICommentsService
    {
        Task<IEnumerable<CommentsDto>> GetAll();
        Task<CommentsModel> GetById(int id);
        Task Insert(CommentsRequestDTO entity);
        Task<string> Delete(int id, int userId);
        Task<CommentsDto> Update(CommentsDto entity, int userId, int commentId);
    }
}
