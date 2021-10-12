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
    public class ActivitiesService : IActivitiesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAWSService _awsService;

        public ActivitiesService(IUnitOfWork unitOfWork, IAWSService awsService)
        {
            _unitOfWork = unitOfWork;
            _awsService = awsService;
        }

        public async Task Delete(int id)
        {
            await _unitOfWork.ActivitiesRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActivitiesModel>> GetAll()
        {
            return await _unitOfWork.ActivitiesRepository.GetAll();
        }

        public async Task<ActivitiesModel> GetById(int id)
        {
            return await _unitOfWork.ActivitiesRepository.GetById(id);
        }

        public async Task<ActivitiesDTO> Insert(ActivitiesDTO entity)
        {
            try
            {
                IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(entity.Image);

                var response = await _awsService.UploadImage(ImageFormFile);

                if (response.Code == 200)
                {
                    var mapper = new EntityMapper();
                    entity.Image = response.URL;
                    var newAtiv = mapper.FromActivitiesDTOActivitiesTo(entity);
                    await _unitOfWork.ActivitiesRepository.Insert(newAtiv);
                    await _unitOfWork.SaveChangesAsync();

                    return entity;
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

        public async Task<object> Update(int id, ActivitiesDTO entity)
        {
            try
            {
                IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(entity.Image);

                var response = await _awsService.UploadImage(ImageFormFile);

                if (response.Code == 200)
                {
                    var act = await _unitOfWork.ActivitiesRepository.GetById(id);

                    if (act == null)
                    {
                        return null;
                    }

                    act.Name = entity.Name;
                    act.Content = entity.Content;
                    act.Image = response.URL;


                    await _unitOfWork.ActivitiesRepository.Update(act);
                    await _unitOfWork.SaveChangesAsync();

                    return true;
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
