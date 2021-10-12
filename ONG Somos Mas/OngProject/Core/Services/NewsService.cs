using Microsoft.AspNetCore.Http;
using OngProject.Core.DTOs;
using OngProject.Core.Helper;
using OngProject.Core.Interfaces;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Mapper;
using OngProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAWSService _awsService;

        public NewsService(IUnitOfWork unitOfWork, IAWSService awsService)
        {
            _unitOfWork = unitOfWork;
            _awsService = awsService;
        }
        public async Task Delete(int id)
        {
            try
            {
                await _unitOfWork.NewsRepository.Delete(id);                               

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<IEnumerable<NewsDTO>> GetAll()
        {
            var news = await _unitOfWork.NewsRepository.GetAll();
            if (news.Any())
            {
                var newsDTO = new List<NewsDTO>();

                foreach (var newItem in news)
                {
                    var newDTO = new EntityMapper().FromNewsToNewsDto(newItem);

                    newsDTO.Add(newDTO);

                }
                return newsDTO;
            }
            else
            {
                return null;
            }
        }
        
        public async Task<IEnumerable<CommentsDto>> GetAllComments(int id)
        {
            var mapper = new EntityMapper();
            var commentsList = await _unitOfWork.CommentsRepository.GetAll();
            var commentsDtoList = commentsList.OrderBy(x => x.CreatedAt)
                .Where(x => x.NewsId == id)
                .Select(x => mapper.FromCommentsToCommentsDto(x))
                .ToList();
            return commentsDtoList;
        }

        public async Task<NewsDTO> GetById(int id)
        {
            var news = await _unitOfWork.NewsRepository.GetById(id);

            if (news != null)
            {

                var newsDTO = new EntityMapper().FromNewsToNewsDto(news);
                var categories = await _unitOfWork.CategoriesRepository.GetById(news.CategoryId);
                if (categories != null)
                {
                    var categoriesDTO = new EntityMapper().FromCategoriesToCategoriesDTO(categories);

                    newsDTO.Categories = categoriesDTO;
                }
                return newsDTO;
            }
            return null;
        }

        public async Task<NewsInsertExitDTO> Insert(NewsInsertDTO entity)
        {
            try
            {
                var categories = await _unitOfWork.CategoriesRepository.GetById(entity.CategoryId);

                if (categories == null)
                {
                    return null;
                }

                IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(entity.Image);

                var response = await _awsService.UploadImage(ImageFormFile);

                if (response.Code == 200)
                {
                    var mapper = new EntityMapper();
                    entity.Image = response.URL;
                    var news = mapper.FromNewsDtoNewsTo(entity);
                    await _unitOfWork.NewsRepository.Insert(news);
                    await _unitOfWork.SaveChangesAsync();

                    return mapper.FromNewsToNewsInsertDto(news);
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

        public async Task Update(int id, NewsUpdateDTO newsUpdate)
        {
            try
            {
                IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(newsUpdate.Image);

                var response = await _awsService.UploadImage(ImageFormFile);

                if (response.Code == 200)
                {
                    NewsModel news = await _unitOfWork.NewsRepository.GetById(id);
                    var mapper = new EntityMapper();
                    newsUpdate.Image = response.URL;
                    news = mapper.FromNewsUpdateDTOToNews(news, newsUpdate);
                    await _unitOfWork.NewsRepository.Update(news);
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
