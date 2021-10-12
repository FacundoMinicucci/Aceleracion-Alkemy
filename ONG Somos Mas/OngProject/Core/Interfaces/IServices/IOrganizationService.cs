using OngProject.Core.DTOs;
using OngProject.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface IOrganizationService
    {
        Task<OrganizationDTO> GetOrganization();
        Task Update(OrganizationUpdateDTO entity);
    }
}
