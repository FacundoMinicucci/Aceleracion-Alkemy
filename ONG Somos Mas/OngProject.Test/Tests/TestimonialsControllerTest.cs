using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Models;
using OngProject.Core.Services;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Test.Helper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using System;

namespace OngProject.Test.Tests
{
    [TestClass]
    public class TestimonialsControllerTest : BaseTests
    {
        private readonly ApplicationDbContext _context;
        private readonly TestimonialsController _testimonialsController;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUriService _uriService;
        private readonly ITestimonialsService _testimonialsService;
        private readonly IAWSService _awsService;

        public TestimonialsControllerTest()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.Development.json", optional: false);
            _context = MakeContext("TestimonialsTestDB");
            IConfiguration _configuration = builder.Build();
            _awsService = new AWSHandlerFilesService(_configuration);
            _unitOfWork = new UnitOfWork(_context);
            _testimonialsService = new TestimonialsService(_unitOfWork, _awsService);
            _uriService = new UriService("test/");
            _testimonialsController = new TestimonialsController(_testimonialsService, _uriService);
        }

        [TestCleanup]
        public async Task DatabaseCleanup()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Get_GetAllTestominials_ReturnOk200()
        {
            // Arranger
            await DatabaseCleanup();
            for (int i = 1; i <= 10; i++)
            {
                _context.Testimonials.Add(new TestimonialsModel()
                {
                    Name = "Testimony " + i,
                    Image = "Image " + i,
                    Content = "Content " + i
                });
            }
            await _context.SaveChangesAsync();

            // Act
            var response = _context.Testimonials.Count();
            var expectedResponse = 10;
            var result = await _testimonialsController.GetAll(1);
            var expectedResult = typeof(OkObjectResult);

            // Assert
            Assert.AreEqual(expectedResponse, response);
            Assert.AreEqual(expectedResult, result.GetType());
        }

        [TestMethod]
        public async Task Post_InsertTestimony_ReturnOk200()
        {
            // Arranger
            await DatabaseCleanup();
            var testimony = new TestimonialsRequestDTO()
            {
                Name = "Test",
                Image = "iVBORw0KGgoAAAANSUhEUgAAAoEAAABLCAYAAAAPv11DAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAtwSURBVHhe7d0/jxzHEcZhfhkJcCDAgD+AgAucMRIcMGcoiHAoOBYcC46YyIrlkDk/BWOCMRkxYbpG81BUbbH6787svrv7W+DBTVf3zN3Ny54t3YHik+++++vhVv3t5WEX2ecCAAC4JjfdBAIAACBHEwgAAHCHnvz+++8HAAAA3I9vvvn28OQg/CpfJC9evHjx4sWLF6/tXkdNYPYjQgXWBGZzAAAAmEcTiGnkoYU8tJCHFvLQQh5aSh40gZhCHlrIQwt5aCEPLeShpeRBE4gp5KGFPLSQhxby0EIeWkoeNIGYQh5ayEMLeWghDy3koaXkQROIKeShhTy0kIcW8tBCHlpKHjSBmEIeWshDC3loIQ8t5KGl5HH1TeDHjx/TetGaw5qRPLxszQiyGzOaR1aPta20rn3rua48r7a6J/d832tOef8oyryXrcnMrJ2157X31svDrNzzPWVfh9LXt6rkQROIKbN5rGZAdmNG8sju5Z73t3XtW8915Xm11T255/tec8r7RxHnR+/jyLrVTFbPU9DLo1i95yO2vOelVqvHmqqSB00gpszmsZoB2Y0ZzWOrXEa0rn3rua48r7a6J/d832tOef8oVvPaak1m9TwFp+Zxqq3uuR+35tSVPG6uCSxjz8/hdCt5xHEtGz9n87V1sXavRvOI9ywb+9rIen/cO9fzc7dm5U0u1rL7VKtlH/285+fuxer7R20+q8VzrBbHvmbjWt3GsV6bvxYr+8Ozeb8uuydWy9bV6jaO9Wzej+OxZ3VVJY+bagLjTb+GEK7NSB5enKuNa3OxXqvdq9X90TteqcW5eJyNb83Km1zrfsVaPG7N2XE2vhenvH+M1LY6bs211l2blf3hlfnavamd21ozeq3R87KxspIHTSCmzG7i0XxG5+K6e7eaR7ynXm1t/Gha497aW7PyJte7R2XstdaNzN2TU55X2TjWyrG3umZ2nR9fk5X94WX3wqvN+VptTW9dbS7WsnlVJQ+aQEw55aG6xVxcd+9W8xi5p3Ftdq7xtdpxNr41K29yvVrtnsV665zaNW7dqXmszMf6yJpsXKvX1l2DlTy8lXtWO87GtXq8RiZbq67kcbNNYAwG25jdxLV8WnPlOFsba1h/k/P12n21eu2j52ut4+zcW7KaR6tWu2exXjunHNeucetOzSPO98ZZfWRNNs7q5bi27hr08iha96U1V6tvcb6Na+uLkTVqSh43+xdDsjmcbiQPrzVfm7Pj2hz+tPomV7u/Wb03nlkT527N6Jucl835mq/7ubguG1stzt2Lmf3h71esZXPZGl/rrfH1mXVx7pqM7I/Cvt/4vWbfe2ttqz6zLn7M+Dl/rrKSx9U3gTivS+VxDRvqEtgfWshDC3loIQ8tJQ+aQEy5VB40gTn2hxby0EIeWshDS8mDJhBTzp3HtfxY/VLYH1rIQwt5aCEPLSUPmkBMIQ8t5KGFPLSQhxby0FLyoAnEFPLQQh5ayEMLeWghDy0lD5pATCEPLeShhTy0kIcW8tBS8qAJxBTy0EIeWshDC3loIQ8tJQ+aQAAAgDvzpQksBwAAALgfX34SqPoqX+Rfnv4TIr7//u+f/9BAA3loIQ8t5KGFPLRYHvK/Ds6aEVwGm1gLeWghDy3koYU8tFgeNIEYxibWQh5ayEMLeWghDy2WB00ghrGJtZCHFvLQQh5ayEOL5fFVE/jp06fD+/fvDz/88I+j+iXQBGphE2shDy3koYU8tJCHFssjbQKfP39++PDhw+Hnn/91NHdurSbw5cuXaR37aW1i+3d+i2z+mlzL91DL41ZyuDa9/TFTx+myPGxvmDgfkc92Vp5Xrfvv5+waI9eprbN6nKutt7ms7tWuZ/xclM2PnDfC8kibwIeHB4lGMGsCS/Nn4hz21drErfGlqHwde6m9yWXH2F+v6RipYzu9/TGCfLYz87wqx8ZqXu88G2fz2bEfl4/+OM77Nb4W1da0zjEj545cp8XyqDaBxbNnzw5v3rw5/Pbbf4/WnAs/CdSyxUP1nJS/ti1keXi3/v2raeVRy4KM9rPF84p8trPyvBrZN3FNb5zVR84ZrUUj165prZ25TsbyaDaBxdOnTz83gn/88b+jdedAE6hl9aFa1phY9/O+HtfW6jaOczb29Tj2a7OPft74up+v1XrnnmLloYr9tPKoZUFG+5l5XpW6ifU4HqnhayvPq9p99fW4pjfO6iPnjNai7Nom1v24VjOtuRGWR7cJtEaw/Gr411//c7R2bzSBWmYeqqb1B70c98YjxyProtVrxPGptVOsNB3Yz0oe5LSf2vPKi/O2pnec1WrXw6Ot9kestcaj150d9+pea42fy9ad8nl7LA9+EohhtYdqrHlx3o9bc3Fcjr1sTRzHOasZX4trRuZq9ewc4+unqj1U9/hc6NvqTQ7bmH1e2b7xa+L6bOz5ORxbeV5l9VrN83W/zmqx3hpn6/1cVvdaa3rnZ/OlNvJ5eyyPZhN4yQawoAnUMvtQLeK8H7fm4jjO1eqtc2pzq9eo1UeOt9B6qGZ17KuWR1HLhKz2M/O88vXacVarXQ9fW3lexbmR+21ramuzeuvz1K5TtObMKedn8yOfc4TlUW0Cy98Ofvfu3eGXX/59NH9ONIFaRjexH6/OxXGcq9Vb59TmVq8RlbmZa2X1kfVm5k0O+6vtj2Ike2xrZn/4eu04q9Wuh6+tPK/i/Mz6bG3t/FK3ud41vN580VrT+1yxVrtW7Tq19YXlkTaB/H8Ckem9yZnRuZWxaa2J47g+q9lxbRzPydTW9M739dpxpvZQ9eI89tPbHzN1nG626bA949fEYz+O9WwOf1p5XsVabU12fm9ttj7Wfc3X/XysRXFN7XrZtVrn+rmR48jySJvAt2/fHn788aej+iW0mkCcX+tNDu0Ntwfy0EIeWshDy6l5zDxfz/0svkaWx1dNoBKaQC08VNtoAu8beWghDy3kocXyoAnEMDZxrjR/l/gvT/LQQh5ayEMLeWixPGgCMYxNrIU8tJCHFvLQQh5aLA+aQAxjE2shDy3koYU8tJCHFsvjSxNYGi4l9jWVLxQAAADbOWoC1V7WBPrOFZdlf2iggTy0kIcW8tBCHlosD/lfB8cvHJfDJtZCHlrIQwt5aCEPLZYHTSCGsYm1kIcW8tBCHlrIQ4vlQROIYWxiLeShhTy0kIcW8tBiedAEYhibWAt5aCEPLeShhTy0WB5X2QSWfze4yOrZcVbL5lt11Ddx7X+W7Gsr8606yEMNeWghDy3kocXy6DaB5d8S9rI1e6EJ1MIm1kIeWshDC3loIQ8tlkezCbTG7/nz55+duxGkCdTCJtZCHlrIQwt5aCEPLZZHswl8/fr14eHh4UsTWI5LLVu7h6wJrDVysw3eyBocyzZxbSPObtCRNThGHlrIQwt5aCEPLZZHtQksP/F79erV4cWLF5+bv6Icl9q5fhq4ZRPYm2/V8WjLTdybb9XxiDy0kIcW8tBCHlosj6tuAv241+T15lt1POptYj/ubdLefKuOR+ShhTy0kIcW8tBieQz9Org0f9YMKv062I97TV5vvlXHIzaxFvLQQh5ayEMLeWixPJpNYFF+6lcav+JcPwE0I02g1VpNXm++V8ejkU1stdYm7c336nhEHlrIQwt5aCEPLZZHtwksSvN37gawmGkCs5o3Mler49HMJs5q3shcrY5H5KGFPLSQhxby0GJ5DDWBl5I1gbicbBPjcshDC3loIQ8t5KHF8qAJxDA2sRby0EIeWshDC3losTxoAjGMTayFPLSQhxby0EIeWiwPmkAMYxNrIQ8t5KGFPLSQhxbLgyYQw9jEWshDC3loIQ8t5KHF8nhSGi1l5QsFAADAdj43gb4zBAAAwH2gCQQAALhDNIEAAAB3iCYQAADgDtEEAgAA3J1vD/8HVk3d7LSxFbIAAAAASUVORK5CYII=",
                Content = "Test content"
            };

            // Act
            var result = await _testimonialsController.Insert(testimony);
            var cantTestimonials = _context.Testimonials.Count();
            var expectedCantTestimonials = 1;
            var expectedResult = typeof(OkObjectResult);

            // Assert
            Assert.AreEqual(expectedResult, result.GetType());
            Assert.AreEqual(expectedCantTestimonials, cantTestimonials);
        }

        [TestMethod]
        public async Task Post_InsertTestimony_NameIsRequired()
        {
            // Arranger
            await DatabaseCleanup();
            var testimony = new TestimonialsRequestDTO()
            {
                Content = "Test content"
            };

            var validationContext = new ValidationContext(testimony, null, null);
            var resultsList = new List<ValidationResult>();

            // Act
            var result = Validator.TryValidateObject(testimony, validationContext, resultsList, true);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Post_InsertTestimony_ContentIsRequired()
        {
            // Arranger
            await DatabaseCleanup();
            var testimony = new TestimonialsRequestDTO()
            {
                Name = "Test"
            };

            var validationContext = new ValidationContext(testimony, null, null);
            var resultsList = new List<ValidationResult>();

            // Act
            var result = Validator.TryValidateObject(testimony, validationContext, resultsList, true);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Delete_DeleteTestimony_ReturnOk200()
        {
            // Arranger
            await DatabaseCleanup();
            var testimony = new TestimonialsModel()
            {
                Name = "Test",
                Image = "Image",
                Content = "Test content"
            };
            _context.Testimonials.Add(testimony);
            await _unitOfWork.SaveChangesAsync();

            // Act
            var result = await _testimonialsController.Delete(1);
            var deleted = await _testimonialsService.GetById(1);
            var expectedResult = typeof(OkResult);

            // Assert
            Assert.AreEqual(expectedResult, result.GetType());
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public async Task Delete_DeleteTestimony_NotFound()
        {
            // Arranger
            await DatabaseCleanup();
            var testimony = new TestimonialsRequestDTO()
            {
                Name = "Test",
                Content = "Test content"
            };
            await _testimonialsController.Insert(testimony);

            // Act
            var response = await _testimonialsController.Delete(5);
            var expectedResponse = typeof(NotFoundObjectResult);

            // Assert
            Assert.IsInstanceOfType(response, expectedResponse);
        }

        [TestMethod]
        public async Task Update_UpdateTestimony_ReturnOk200()
        {
            // Arranger
            await DatabaseCleanup();
            var testimony = new TestimonialsModel()
            {
                Name = "Test",
                Image = "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",
                Content = "Test content"
            };
            _context.Testimonials.Add(testimony);
            await _context.SaveChangesAsync();

            var testimonyUpdate = new TestimonialUpdateDTO()
            {
                Name = "TestUpdate",
                Image = "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",
                Content = "Test content update"
            };
            // Act
            var response = await _testimonialsController.Update(1, testimonyUpdate);
            var testimonyModified = await _testimonialsService.GetById(1);
            var expectedResponse = typeof(OkObjectResult);

            // Assert
            Assert.AreEqual(expectedResponse, response.GetType());
            Assert.AreEqual(testimonyUpdate.Name, testimonyModified.Name);
            Assert.AreEqual(testimonyUpdate.Content, testimonyModified.Content);
        }

        [TestMethod]
        public async Task Update_UpdateTestimony_NotFound()
        {
            // Arranger
            await DatabaseCleanup();
            var testimony = new TestimonialsRequestDTO()
            {
                Name = "Test",
                Content = "Test content"
            };
            await _testimonialsController.Insert(testimony);

            var testimonyUpdate = new TestimonialUpdateDTO()
            {
                Name = "TestUpdate",
                Image = "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",
                Content = "Test content update"
            };

            // Act
            var response = await _testimonialsController.Update(5, testimonyUpdate);
            var expectedResponse = typeof(NotFoundObjectResult);

            // Assert
            Assert.IsInstanceOfType(response, expectedResponse);
        }

        [TestMethod]
        public async Task Update_UpdateTestimony_NameInvalid()
        {
            // Arranger
            await DatabaseCleanup();

            var testimonyUpdate = new TestimonialUpdateDTO()
            {
                Name = "Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test update Test ",
                Image = "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",
                Content = "Test content update"
            };

            var validationContext = new ValidationContext(testimonyUpdate, null, null);
            var resultsList = new List<ValidationResult>();

            // Act
            var result = Validator.TryValidateObject(testimonyUpdate, validationContext, resultsList, true);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Update_UpdateTestimony_ImageInvalid()
        {
            // Arranger
            await DatabaseCleanup();
            var testimony = new TestimonialsModel()
            {
                Name = "Test",
                Image = "iVBORw0KGgoAAAANSUhEUgAAAoEAAABLCAYAAAAPv11DAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAtwSURBVHhe7d0/jxzHEcZhfhkJcCDAgD+AgAucMRIcMGcoiHAoOBYcC46YyIrlkDk/BWOCMRkxYbpG81BUbbH6787svrv7W+DBTVf3zN3Ny54t3YHik+++++vhVv3t5WEX2ecCAAC4JjfdBAIAACBHEwgAAHCHnvz+++8HAAAA3I9vvvn28OQg/CpfJC9evHjx4sWLF6/tXkdNYPYjQgXWBGZzAAAAmEcTiGnkoYU8tJCHFvLQQh5aSh40gZhCHlrIQwt5aCEPLeShpeRBE4gp5KGFPLSQhxby0EIeWkoeNIGYQh5ayEMLeWghDy3koaXkQROIKeShhTy0kIcW8tBCHlpKHjSBmEIeWshDC3loIQ8t5KGl5HH1TeDHjx/TetGaw5qRPLxszQiyGzOaR1aPta20rn3rua48r7a6J/d832tOef8oyryXrcnMrJ2157X31svDrNzzPWVfh9LXt6rkQROIKbN5rGZAdmNG8sju5Z73t3XtW8915Xm11T255/tec8r7RxHnR+/jyLrVTFbPU9DLo1i95yO2vOelVqvHmqqSB00gpszmsZoB2Y0ZzWOrXEa0rn3rua48r7a6J/d832tOef8oVvPaak1m9TwFp+Zxqq3uuR+35tSVPG6uCSxjz8/hdCt5xHEtGz9n87V1sXavRvOI9ywb+9rIen/cO9fzc7dm5U0u1rL7VKtlH/285+fuxer7R20+q8VzrBbHvmbjWt3GsV6bvxYr+8Ozeb8uuydWy9bV6jaO9Wzej+OxZ3VVJY+bagLjTb+GEK7NSB5enKuNa3OxXqvdq9X90TteqcW5eJyNb83Km1zrfsVaPG7N2XE2vhenvH+M1LY6bs211l2blf3hlfnavamd21ozeq3R87KxspIHTSCmzG7i0XxG5+K6e7eaR7ynXm1t/Gha497aW7PyJte7R2XstdaNzN2TU55X2TjWyrG3umZ2nR9fk5X94WX3wqvN+VptTW9dbS7WsnlVJQ+aQEw55aG6xVxcd+9W8xi5p3Ftdq7xtdpxNr41K29yvVrtnsV665zaNW7dqXmszMf6yJpsXKvX1l2DlTy8lXtWO87GtXq8RiZbq67kcbNNYAwG25jdxLV8WnPlOFsba1h/k/P12n21eu2j52ut4+zcW7KaR6tWu2exXjunHNeucetOzSPO98ZZfWRNNs7q5bi27hr08iha96U1V6tvcb6Na+uLkTVqSh43+xdDsjmcbiQPrzVfm7Pj2hz+tPomV7u/Wb03nlkT527N6Jucl835mq/7ubguG1stzt2Lmf3h71esZXPZGl/rrfH1mXVx7pqM7I/Cvt/4vWbfe2ttqz6zLn7M+Dl/rrKSx9U3gTivS+VxDRvqEtgfWshDC3loIQ8tJQ+aQEy5VB40gTn2hxby0EIeWshDS8mDJhBTzp3HtfxY/VLYH1rIQwt5aCEPLSUPmkBMIQ8t5KGFPLSQhxby0FLyoAnEFPLQQh5ayEMLeWghDy0lD5pATCEPLeShhTy0kIcW8tBS8qAJxBTy0EIeWshDC3loIQ8tJQ+aQAAAgDvzpQksBwAAALgfX34SqPoqX+Rfnv4TIr7//u+f/9BAA3loIQ8t5KGFPLRYHvK/Ds6aEVwGm1gLeWghDy3koYU8tFgeNIEYxibWQh5ayEMLeWghDy2WB00ghrGJtZCHFvLQQh5ayEOL5fFVE/jp06fD+/fvDz/88I+j+iXQBGphE2shDy3koYU8tJCHFssjbQKfP39++PDhw+Hnn/91NHdurSbw5cuXaR37aW1i+3d+i2z+mlzL91DL41ZyuDa9/TFTx+myPGxvmDgfkc92Vp5Xrfvv5+waI9eprbN6nKutt7ms7tWuZ/xclM2PnDfC8kibwIeHB4lGMGsCS/Nn4hz21drErfGlqHwde6m9yWXH2F+v6RipYzu9/TGCfLYz87wqx8ZqXu88G2fz2bEfl4/+OM77Nb4W1da0zjEj545cp8XyqDaBxbNnzw5v3rw5/Pbbf4/WnAs/CdSyxUP1nJS/ti1keXi3/v2raeVRy4KM9rPF84p8trPyvBrZN3FNb5zVR84ZrUUj165prZ25TsbyaDaBxdOnTz83gn/88b+jdedAE6hl9aFa1phY9/O+HtfW6jaOczb29Tj2a7OPft74up+v1XrnnmLloYr9tPKoZUFG+5l5XpW6ifU4HqnhayvPq9p99fW4pjfO6iPnjNai7Nom1v24VjOtuRGWR7cJtEaw/Gr411//c7R2bzSBWmYeqqb1B70c98YjxyProtVrxPGptVOsNB3Yz0oe5LSf2vPKi/O2pnec1WrXw6Ot9kestcaj150d9+pea42fy9ad8nl7LA9+EohhtYdqrHlx3o9bc3Fcjr1sTRzHOasZX4trRuZq9ewc4+unqj1U9/hc6NvqTQ7bmH1e2b7xa+L6bOz5ORxbeV5l9VrN83W/zmqx3hpn6/1cVvdaa3rnZ/OlNvJ5eyyPZhN4yQawoAnUMvtQLeK8H7fm4jjO1eqtc2pzq9eo1UeOt9B6qGZ17KuWR1HLhKz2M/O88vXacVarXQ9fW3lexbmR+21ramuzeuvz1K5TtObMKedn8yOfc4TlUW0Cy98Ofvfu3eGXX/59NH9ONIFaRjexH6/OxXGcq9Vb59TmVq8RlbmZa2X1kfVm5k0O+6vtj2Ike2xrZn/4eu04q9Wuh6+tPK/i/Mz6bG3t/FK3ud41vN580VrT+1yxVrtW7Tq19YXlkTaB/H8Ckem9yZnRuZWxaa2J47g+q9lxbRzPydTW9M739dpxpvZQ9eI89tPbHzN1nG626bA949fEYz+O9WwOf1p5XsVabU12fm9ttj7Wfc3X/XysRXFN7XrZtVrn+rmR48jySJvAt2/fHn788aej+iW0mkCcX+tNDu0Ntwfy0EIeWshDy6l5zDxfz/0svkaWx1dNoBKaQC08VNtoAu8beWghDy3kocXyoAnEMDZxrjR/l/gvT/LQQh5ayEMLeWixPGgCMYxNrIU8tJCHFvLQQh5aLA+aQAxjE2shDy3koYU8tJCHFsvjSxNYGi4l9jWVLxQAAADbOWoC1V7WBPrOFZdlf2iggTy0kIcW8tBCHlosD/lfB8cvHJfDJtZCHlrIQwt5aCEPLZYHTSCGsYm1kIcW8tBCHlrIQ4vlQROIYWxiLeShhTy0kIcW8tBiedAEYhibWAt5aCEPLeShhTy0WB5X2QSWfze4yOrZcVbL5lt11Ddx7X+W7Gsr8606yEMNeWghDy3kocXy6DaB5d8S9rI1e6EJ1MIm1kIeWshDC3loIQ8tlkezCbTG7/nz55+duxGkCdTCJtZCHlrIQwt5aCEPLZZHswl8/fr14eHh4UsTWI5LLVu7h6wJrDVysw3eyBocyzZxbSPObtCRNThGHlrIQwt5aCEPLZZHtQksP/F79erV4cWLF5+bv6Icl9q5fhq4ZRPYm2/V8WjLTdybb9XxiDy0kIcW8tBCHlosj6tuAv241+T15lt1POptYj/ubdLefKuOR+ShhTy0kIcW8tBieQz9Org0f9YMKv062I97TV5vvlXHIzaxFvLQQh5ayEMLeWixPJpNYFF+6lcav+JcPwE0I02g1VpNXm++V8ejkU1stdYm7c336nhEHlrIQwt5aCEPLZZHtwksSvN37gawmGkCs5o3Mler49HMJs5q3shcrY5H5KGFPLSQhxby0GJ5DDWBl5I1gbicbBPjcshDC3loIQ8t5KHF8qAJxDA2sRby0EIeWshDC3losTxoAjGMTayFPLSQhxby0EIeWiwPmkAMYxNrIQ8t5KGFPLSQhxbLgyYQw9jEWshDC3loIQ8t5KHF8nhSGi1l5QsFAADAdj43gb4zBAAAwH2gCQQAALhDNIEAAAB3iCYQAADgDtEEAgAA3J1vD/8HVk3d7LSxFbIAAAAASUVORK5CYII=",
                Content = "Test content"
            };
            _context.Testimonials.Add(testimony);
            await _context.SaveChangesAsync();

            var testimonyUpdate = new TestimonialUpdateDTO()
            {
                Name = "Test update",
                Image = "Image",
                Content = "Test content update"
            };

            // Act
            var response = await _testimonialsController.Update(1, testimonyUpdate) as ObjectResult;
            var expectedResponse = 500;

            // Assert
            Assert.AreEqual(expectedResponse, response.StatusCode);
        }
    }
}
