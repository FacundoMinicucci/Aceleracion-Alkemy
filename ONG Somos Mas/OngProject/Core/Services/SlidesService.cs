using Microsoft.AspNetCore.Http;
using OngProject.Core.DTOs;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Mapper;
using OngProject.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class SlidesService : ISlidersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAWSService _awsService;

        public SlidesService(IUnitOfWork unitOfWork, IAWSService awsService)
        {
            _unitOfWork = unitOfWork;
            _awsService = awsService;
        }

        public async Task<string> Delete(int id)
        {
            try
            {
                //var slideModel = await _unitOfWork.SlidesRepository.GetById(id);
                var slideList = await _unitOfWork.SlidesRepository.GetAll();

                if (!slideList.Any(x=> x.Id == id)) return "Not Found";
                                
               
                var resultAWSService = await _awsService.DeleteImage(
                    slideList.Where(x => x.Id == id).FirstOrDefault().Image);

                if (resultAWSService.Code == 204)
                {
                    slideList = slideList.OrderBy(x => x.Order).ToList();

                    UpdateOrderSliders(id, slideList);
                    await _unitOfWork.SlidesRepository.Delete(id);
                    await _unitOfWork.SaveChangesAsync();
                    return "Slide deleted";
                }
                
                return resultAWSService.Errors;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }
       

        public async Task<IEnumerable<SlidesDto>> GetAll()
        {
            var mapper = new EntityMapper();
            var slidesList = await _unitOfWork.SlidesRepository.GetAll();
            var slidesDtoList = slidesList.Select(x => mapper.FromSlidesToSlidesDto(x)).ToList();
            return slidesDtoList;
        }

        public async Task<SlidesModel> GetById(int id)
        {
            return await _unitOfWork.SlidesRepository.GetById(id);
        }

        public async Task<SlidesModel> Insert(SlidesCreateDTO slideCreateDto)
        {
            try
            {
                var slidesList = await _unitOfWork.SlidesRepository.GetAll();

                if (slideCreateDto.Order == 0)
                {
                    if (slidesList.Any())
                    {
                        var slideLast = slidesList.Last();
                        slideCreateDto.Order = slideLast.Order;
                    }

                    slideCreateDto.Order += 1;
                }

                var slideExist = slidesList.FirstOrDefault(x => x.Order == slideCreateDto.Order);

                if (slideExist == null)
                {
                   
                    IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(slideCreateDto.Image);
                    
                    var response = await _awsService.UploadImage(ImageFormFile);

                    if (response.Code == 200)
                    {
                        var mapper = new EntityMapper();
                        slideCreateDto.Image = response.URL;
                        var slide = mapper.FromSlidesCreateDTOToSlides(slideCreateDto);
                        await _unitOfWork.SlidesRepository.Insert(slide);
                        await _unitOfWork.SaveChangesAsync();
                        return slide;
                    }
                    else
                    {
                        throw new Exception(response.Errors);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
       
        public async Task<SlidesModel> Update(int id, SlidesUpdateDTO dto)
        {
            try
            {                

                SlidesModel slideToUpdate = await _unitOfWork.SlidesRepository.GetById(id);

                if (slideToUpdate != null)
                {

                    IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(dto.Image);

                    var response = await _awsService.UploadImage(ImageFormFile);
                    if (response.Code == 200)
                    {
                        await _awsService.DeleteImage(slideToUpdate.Image);

                        var mapper = new EntityMapper();
                        dto.Image = response.URL;

                        var slide = mapper.FromSlideUpdateDTOToSlide(slideToUpdate, dto);

                        await _unitOfWork.SlidesRepository.Update(slide);
                        await _unitOfWork.SaveChangesAsync();
                        
                        return slide;
                    }
                    else
                    {
                        throw new Exception(response.Errors);
                    }

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private async void UpdateOrderSliders(int id, IEnumerable<SlidesModel> slideList)
        {
            var bandera = false;
            var orderToReplace = 0;           

            foreach (var item in slideList)
            {
                if (item.Id == id)
                {
                    orderToReplace = item.Order;
                    bandera = true;
                }
                if (bandera)
                {
                    if (item.Id != id)
                    {
                        item.Order = orderToReplace;
                        await _unitOfWork.SlidesRepository.Update(item);
                        orderToReplace += 1;
                    }
                }
            }
        }
    }
}
