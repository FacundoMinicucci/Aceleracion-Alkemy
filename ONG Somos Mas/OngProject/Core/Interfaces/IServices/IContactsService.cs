namespace OngProject.Core.Interfaces.IServices
{
    using OngProject.Core.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IContactsService
    {
        Task<IEnumerable<ContactsDTO>> GetAll();
        Task<bool> Insert(ContactsDTO contacts);
    }
}
