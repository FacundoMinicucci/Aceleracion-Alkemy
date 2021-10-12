using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces;
using OngProject.Core.Services;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Test.Helper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Test.Tests
{
    [TestClass]
    public class UsersTest : BaseTest
    {
        private readonly IUnitOfWork _unitOFWork;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UsersController _usersController;
        private readonly UsersService _usersService;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;
        private readonly IAWSService _aWSService;
        private readonly ILogger<SendGridMailService> logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersTest()
        {
            _applicationDbContext = MakeContext("dbContext");
            var config = new Dictionary<string, string>
                {
                     {"SendGridAPIKey", "SecretKey"},
                     {"Secret_Key", "AquiMiSecretKey2012"},

            };
           _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();
            var milogger = new Mock<ILogger<SendGridMailService>>();
            var mailServiceMi = new Mock<IMailService>();
            _mailService = new SendGridMailService(_configuration, milogger.Object);
            _unitOFWork = new UnitOfWork(_applicationDbContext);
            
            _usersService = new UsersService(_unitOFWork, _configuration, mailServiceMi.Object, _aWSService);
            _usersController = new UsersController(_usersService);
            
        }

      

        [TestMethod]
        public async Task Get_ShouldReturnAllUser_ReturnNotFound()
        {
            // Arranger
            

            // Act
            var expected = StatusCodes.Status404NotFound;

            var response = await _usersController.GetAll();
            var result = response as StatusCodeResult;

            //Assert

            Assert.AreEqual(expected, result.StatusCode);

        }

        [TestMethod]
        public async Task Get_ShouldReturnAllUser_ReturnStatud200OK()
        {
            // Arranger

            _applicationDbContext.Rols.AddRange(GetRol());
            _applicationDbContext.Users.AddRange(GetUsers());

            _applicationDbContext.SaveChanges();
            // Act
            var expected = StatusCodes.Status200OK;

            var response = await _usersController.GetAll();
            var result = response as ObjectResult;

            var listUserDto = result.Value as List<UserDTO>;

            //Assert

            Assert.IsInstanceOfType(listUserDto, typeof(List<UserDTO>));
            Assert.AreEqual(expected, result.StatusCode);

        }

        [TestMethod]
        public async Task Get_ShouldReturnValidUser_ReturnStatud200OK()
        {
            // Arranger           
          
            // Act
            var expected = StatusCodes.Status200OK;

            var response = await _usersController.GetUserById(1);
            var result = response as ObjectResult;

            var userInfoDto = result.Value as UserInfoDTO;

            //Assert

            Assert.IsInstanceOfType(userInfoDto, typeof(UserInfoDTO));
            Assert.AreEqual(expected, result.StatusCode);

        }
              

      

    }
}
