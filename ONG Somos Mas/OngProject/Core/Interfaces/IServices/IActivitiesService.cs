using OngProject.Core.DTOs;
using OngProject.Core.Models;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface IActivitiesService
    {
        public Task<ActivitiesDTO> Insert(ActivitiesDTO entity);
        Task<ActivitiesModel> GetById(int id);

        Task<object> Update(int id, ActivitiesDTO entity);
    }
}
