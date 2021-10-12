using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Models;
using OngProject.Core.Services;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Test.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngProject.Test.Tests
{
    [TestClass]
    public class AuthenticationTest : BaseTests
    {
        private readonly ApplicationDbContext _context;
        private readonly UsersController _usersController;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUsersService _usersService;
        private readonly IMailService _mailService;

        public AuthenticationTest()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.Development.json", optional: false);
            IConfiguration _configuration = builder.Build();
            _context = MakeContext("AuthenticationTestDB");
            var milogger = new Mock<ILogger<SendGridMailService>>();
            IAWSService _awsService = new AWSHandlerFilesService(_configuration);
            _unitOfWork = new UnitOfWork(_context);
            _mailService = new SendGridMailService(_configuration, milogger.Object);
            _usersService = new UsersService(_unitOfWork, _configuration, _mailService, _awsService);
            _usersController = new UsersController(_usersService);
        }

        [TestCleanup]
        public async Task DatabaseCleanup()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Post_RegisterUser_ObjectResult()
        {
            // Arranger
            await DatabaseCleanup();

            var userDTO = new UserRegisterDTO
            {
                FirstName = "Test name",
                LastName = "Test name",
                Password = "Password$1",
                ConfirmPassword = "Password$1",
                Email = "test@gmail.com"
            };

            // Act
            var result = await _usersController.Insert(userDTO);
            var expectedResult = typeof(ObjectResult);

            // Assert
            Assert.AreEqual(expectedResult, result.GetType());
        }

        [TestMethod]
        public async Task Post_RegisterUser_FirstNameIsInvalid()
        {
            // Arranger
            await DatabaseCleanup();

            var userDTO = new UserRegisterDTO
            {
                LastName = "Test name",
                Password = "Password$1",
                ConfirmPassword = "Password$1",
                Email = "test@gmail.com"
            };
            var validationContext = new ValidationContext(userDTO, null, null);
            var resultsList = new List<ValidationResult>();

            // Act
            var result = Validator.TryValidateObject(userDTO, validationContext, resultsList, true);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Post_RegisterUser_LastNameIsInvalid()
        {
            // Arranger
            await DatabaseCleanup();

            var userDTO = new UserRegisterDTO
            {
                FirstName = "Test name",
                Password = "Password$1",
                ConfirmPassword = "Password$1",
                Email = "test@gmail.com"
            };
            var validationContext = new ValidationContext(userDTO, null, null);
            var resultsList = new List<ValidationResult>();

            // Act
            var result = Validator.TryValidateObject(userDTO, validationContext, resultsList, true);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Post_RegisterUser_PasswordIsInvalid()
        {
            // Arranger
            await DatabaseCleanup();

            var userDTO = new UserRegisterDTO
            {
                FirstName = "Test name",
                LastName = "Test name",
                ConfirmPassword = "Password$1",
                Email = "test@gmail.com"
            };
            var validationContext = new ValidationContext(userDTO, null, null);
            var resultsList = new List<ValidationResult>();

            // Act
            var result = Validator.TryValidateObject(userDTO, validationContext, resultsList, true);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Post_RegisterUser_ConfirmPasswordIsInvalid()
        {
            // Arranger
            await DatabaseCleanup();

            var userDTO = new UserRegisterDTO
            {
                FirstName = "Test name",
                LastName = "Test name",
                Password = "Password$1",
                Email = "test@gmail.com"
            };
            var validationContext = new ValidationContext(userDTO, null, null);
            var resultsList = new List<ValidationResult>();

            // Act
            var result = Validator.TryValidateObject(userDTO, validationContext, resultsList, true);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Post_RegisterUser_EmailIsInvalid()
        {
            // Arranger
            await DatabaseCleanup();

            var userDTO = new UserRegisterDTO
            {
                FirstName = "Test name",
                LastName = "Test name",
                Password = "Password$1",
                ConfirmPassword = "Password$1"
            };
            var validationContext = new ValidationContext(userDTO, null, null);
            var resultsList = new List<ValidationResult>();

            // Act
            var result = Validator.TryValidateObject(userDTO, validationContext, resultsList, true);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Post_RegisterUser_PasswordDoesNotMatch()
        {
            // Arranger
            await DatabaseCleanup();

            var userDTO = new UserRegisterDTO
            {
                FirstName = "Test name",
                LastName = "Test name",
                Password = "Password$123",
                ConfirmPassword = "Passrd$111",
                Email = "test@gmail.com"
            };

            var validationContext = new ValidationContext(userDTO, null, null);
            var resultsList = new List<ValidationResult>();

            // Act
            var result = Validator.TryValidateObject(userDTO, validationContext, resultsList, true);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Post_RegisterUser_SendMail()
        {
            // Arranger
            var body = "El nombre de usuario test ";
            var subject = "Registration confirmation";
            var contact = "Contacto de la ONG";

            var data = new
            {
                mail_tittle = subject,
                mail_body = body,
                mail_contact = contact
            };

            var basePathTemplate = Directory.GetCurrentDirectory() + @"\Templates\TemplateMailWelcome.html";
            var content = _mailService.GetHtml(basePathTemplate, data);

            // Act
            var result = await _mailService.SendEmailAsync("testmail@gmail.com", "Test", content);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Post_LoginUser_OkObjectResult()
        {
            // Arranger
            await DatabaseCleanup();

            var rol = new RolModel
            {
                Name = "RolTest",
                Description = "Rol"
            };

            _context.Rols.Add(rol);
            await _unitOfWork.SaveChangesAsync();

            var user = new UsersModel
            {
                FirstName = "Test",
                LastName = "Name",
                Password = UsersModel.ComputeSha256Hash("Password$1"),
                ConfirmPassword = UsersModel.ComputeSha256Hash("Password$1"),
                Email = "testmail@gmail.com",
                RolId = 1
            };

            _context.Users.Add(user);
            await _unitOfWork.SaveChangesAsync();

            var loginDTO = new LoginRequestDTO
            {
                Email = "testmail@gmail.com",
                Password = "Password$1"
            };

            // Act
            var result = _usersController.Login(loginDTO);
            var expectedResult = typeof(OkObjectResult);

            // Assert
            Assert.AreEqual(expectedResult, result.Result.GetType());
        }

        [TestMethod]
        public async Task Post_LoginUser_NotFound()
        {
            // Arranger
            await DatabaseCleanup();
            
            var loginDTO = new LoginRequestDTO
            {
                Email = "test@gmail.com",
                Password = "Password$1"
            };

            // Act
            var result = _usersController.Login(loginDTO) as IActionResult;

            // Assert
            Assert.IsNull(result);
        }
    }
}
