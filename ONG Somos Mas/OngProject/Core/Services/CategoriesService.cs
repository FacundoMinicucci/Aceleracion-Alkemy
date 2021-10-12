using Microsoft.AspNetCore.Http;
using OngProject.Core.DTOs;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAWSService _awsService;

        public CategoriesService(IUnitOfWork unitOfWork, IAWSService awsService)
        {
            _unitOfWork = unitOfWork;
            _awsService = awsService;
        }

        public async Task Delete(int id)
        {
            await _unitOfWork.CategoriesRepository.Delete(id);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoriesOnlyNameDTO>> GetAll()
        {
            var categories = await _unitOfWork.CategoriesRepository.GetAll();
            if (categories.Any())
            {
                var categoriesListDTO = new List<CategoriesOnlyNameDTO>();
                foreach (var category in categories)
                {
                    categoriesListDTO.Add(
                        new EntityMapper().FromCategoriesToCategoriesOnlyNameDTO(category)
                        );
                }

                return categoriesListDTO;
            }
            else
            {
                return null;
            }
        }

        public async Task<CategoriesDTO> GetById(int id)
        {
            var categorie = await _unitOfWork.CategoriesRepository.GetById(id);

            if (categorie == null)
            {
                return null;
            }

            var categorieDTO = new CategoriesDTO();

            categorieDTO.Name = categorie.Name;
            categorieDTO.Description = categorie.Description;
            categorieDTO.Image = categorie.Image;

            return categorieDTO;
        }

        public async Task<CategoriesDTO> Insert(CategoriesDTO entity)
        {
            try
            {
                AWSManagerResponse response = new AWSManagerResponse();

                if (entity.Image != null)
                {
                    IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(entity.Image);

                    response = await _awsService.UploadImage(ImageFormFile);
                }

                if (entity.Image == null || response.Code == 200)
                {
                    var mapper = new EntityMapper();
                    entity.Image = response.URL;
                    var newCat = mapper.FromCategoriesDTOCategoriesTo(entity);
                    await _unitOfWork.CategoriesRepository.Insert(newCat);
                    await _unitOfWork.SaveChangesAsync();

                    return mapper.FromCategoriesToCategoriesDTO(newCat);
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

        public async Task Update(CategoriesDTO categoryDTO, int id)
        {
            try
            {
                IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(categoryDTO.Image);

                var response = await _awsService.UploadImage(ImageFormFile);

                if (response.Code == 200)
                {
                    var categoryObj = await _unitOfWork.CategoriesRepository.GetById(id);

                    var mapper = new EntityMapper();
                    categoryDTO.Image = response.URL;
                    var categoryModified = mapper.FromCategoriesDTOToUpdateCategories(categoryDTO, categoryObj);
                    await _unitOfWork.CategoriesRepository.Update(categoryModified);
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
