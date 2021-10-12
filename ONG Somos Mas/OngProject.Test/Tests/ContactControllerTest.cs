using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces;
using OngProject.Core.Models;
using OngProject.Core.Services;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Test.Helper;

using System.Threading.Tasks;


namespace OngProject.Test.Tests
{
    [TestClass]
    public class ContactControllerTest : BaseTests
    {

        private readonly IUnitOfWork _unitOFWork;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ContactsController _contactsController;
        private readonly ContactsService _contactsService;
        private readonly IConfiguration _configuration;      
       

        public ContactControllerTest()
        {
            _applicationDbContext = MakeContext("dbContext");            
           
            var mailServiceMi = new Mock<IMailService>();           
            _unitOFWork = new UnitOfWork(_applicationDbContext);
            _contactsService = new ContactsService(_unitOFWork, mailServiceMi.Object);
            _contactsController = new ContactsController(_contactsService);
        }

        [TestCleanup]
        public async Task DatabaseCleanup()
        {
            await _applicationDbContext.Database.EnsureDeletedAsync();
            await _applicationDbContext.SaveChangesAsync();
        }


        [TestMethod]
        public async Task Post_InsertContact_ReturnOk()
        {
            // Arranger
            await DatabaseCleanup();

            var contactsDTO = new ContactsDTO
            {
                Name = "contatcName",
                Phone = "contatcPhone",
                Email = "contatcEmail",
                Message = "contatcMessage",
            };

            // Act
            
            var actual = await _contactsService.Insert(contactsDTO);            
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public async Task Post_InsertContact_ReturnBadRequest()
        {
            // Arranger
            await DatabaseCleanup();

            var contactsDTO = new ContactsDTO
            {
                Phone = "contatcPhone",               
                Message = "contatcMessage",
            };

            // Act

            var actual = await _contactsService.Insert(contactsDTO);
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public async Task Get_ShouldReturnAllContacts_ReturnStatud200OK()
        {
            // Arrange
            for (int i = 1; i <= 10; i++)
            {
                _applicationDbContext.Contacts.Add(new ContactsModel
                {
                    Name = "contatcName" + i,
                    Phone = "contatcPhone" + i,
                    Email = "contatcEmail" + i,
                    Message = "contatcMessage" + i,
                   
                });
            }

            await _applicationDbContext.SaveChangesAsync();

            // Act
            var expectedStatusCode = 200;   
          

            var actual = await _contactsController.GetAll();

            var result = actual as OkObjectResult;          


            // Assert
            Assert.AreEqual(expectedStatusCode, result.StatusCode);
            
        }

        [TestMethod]
        public async Task Get_ShouldReturnAllContacts_ReturnNotFound()
        {
            // Arrange
            for (int i = 1; i <= 10; i++)
            {
                _applicationDbContext.Contacts.Add(new ContactsModel
                {
                    Name = "contatcName" + i,
                    Phone = "contatcPhone" + i,
                    Email = "contatcEmail" + i,
                    Message = "contatcMessage" + i,

                });
            }

            await _applicationDbContext.SaveChangesAsync();

            var expected = StatusCodes.Status404NotFound;

            var response = await _contactsService.GetAll();
            var result = response as StatusCodeResult;

            //Assert

            Assert.AreEqual(expected, result.StatusCode);

        }









    }
}
