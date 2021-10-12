using System.Collections.Generic;
using System.Threading.Tasks;
using OngProject.Core.DTOs;
using OngProject.Core.Models;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ITestimonialsService
    {
        Task Delete(int id);
        Task<IEnumerable<TestimonialResponseDTO>> GetAll();
        Task<TestimonialsModel> GetById(int id);
        Task Insert(TestimonialsRequestDTO testimonialsRequestDTO);
        Task Update(TestimonialUpdateDTO entity, int id);
    }
}
