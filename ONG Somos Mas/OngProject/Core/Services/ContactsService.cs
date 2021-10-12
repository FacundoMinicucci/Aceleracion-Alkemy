namespace OngProject.Core.Services
{
    using OngProject.Core.DTOs;
    using OngProject.Core.Interfaces;
    using OngProject.Core.Interfaces.IServices;
    using OngProject.Core.Mapper;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ContactsService : IContactsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public ContactsService(IUnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mailService = mailService;
        }

        public async Task<IEnumerable<ContactsDTO>> GetAll()
        {
            var contacts = await _unitOfWork.ContactsRepository.GetAll();
            if (contacts.Any())
            {
                var contactsList = new List<ContactsDTO>();

                foreach (var contact in contacts)
                {
                    contactsList.Add(new EntityMapper().FromContactsToContactsDto(contact));
                }

                return contactsList;
            }

            return null;
        }

        public async Task<bool> Insert(ContactsDTO contacts)
        {
            await _unitOfWork.ContactsRepository.Insert(new EntityMapper().FromContactDtoToContact(contacts));
            _unitOfWork.SaveChanges();

            var data = new { UserName = contacts.Name };
            var basePathTemplate = @"..\OngProject\Templates\TemplateMailWelcomeContact.html";
            var content = _mailService.GetHtml(basePathTemplate, data);
            await _mailService.SendEmailAsync(contacts.Email, "Registration confirmation", content);

            return true;
        }
    }
}
