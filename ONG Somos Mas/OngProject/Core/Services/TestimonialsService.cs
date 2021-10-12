using Microsoft.AspNetCore.Http;
using OngProject.Core.DTOs;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Mapper;
using OngProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class TestimonialsService : ITestimonialsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAWSService _awsService;

        public TestimonialsService(IUnitOfWork unitOfWork, IAWSService awsService)
        {
            _unitOfWork = unitOfWork;
            _awsService = awsService;
        }

        public async Task Delete(int id)
        {
            try
            {
                var testimonial = await _unitOfWork.TestimonialsRepository.GetById(id);

                testimonial.IsDeleted = true;
                testimonial.CreatedAt = DateTime.Now;

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<TestimonialResponseDTO>> GetAll()
        {
            var testimonials = await _unitOfWork.TestimonialsRepository.GetAll();
        
            IList<TestimonialResponseDTO> testimonialsListDTO = new List<TestimonialResponseDTO>();

            var mapper = new EntityMapper();

            foreach (TestimonialsModel testimonial in testimonials)
            {
                var objDTO = mapper.FromTestimonialsToTestimonialsDTO(testimonial);
                testimonialsListDTO.Add(objDTO);
            }

            return testimonialsListDTO;
        }

        public async Task<TestimonialsModel> GetById(int id)
        {
            return await _unitOfWork.TestimonialsRepository.GetById(id);
        }

        public async Task Insert(TestimonialsRequestDTO entity)
        {
            try
            {
                IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(entity.Image);

                var response = await _awsService.UploadImage(ImageFormFile);

                if (response.Code == 200)
                {
                    var mapper = new EntityMapper();
                    entity.Image = response.URL;
                    var testimonalObj = mapper.FromTestimonialsDTOToTestimonials(entity);
                    await _unitOfWork.TestimonialsRepository.Insert(testimonalObj);
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    throw new Exception(response.Errors);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task Update(TestimonialUpdateDTO testimonialDTO, int id)
        {
            try
            {
                IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(testimonialDTO.Image);

                var response = await _awsService.UploadImage(ImageFormFile);

                if (response.Code == 200)
                {
                    var testimonial = await _unitOfWork.TestimonialsRepository.GetById(id);

                    var mapper = new EntityMapper();
                    testimonialDTO.Image = response.URL;
                    var testimonialUpdate = mapper.FromTestimonialUpdateDTOToTestimonials(testimonialDTO, testimonial);
                    await _unitOfWork.TestimonialsRepository.Update(testimonialUpdate);
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    throw new Exception(response.Errors);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}