using OngProject.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ICategoriesService
    {
        Task Delete(int id);

        Task<IEnumerable<CategoriesOnlyNameDTO>> GetAll();

        public Task<CategoriesDTO> GetById(int id);

        public Task<CategoriesDTO> Insert(CategoriesDTO entity);

        Task Update(CategoriesDTO categoryDTO, int id);
    }
}
