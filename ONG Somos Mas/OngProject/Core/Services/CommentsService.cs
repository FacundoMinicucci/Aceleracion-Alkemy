using OngProject.Core.DTOs;
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
    public class CommentsService : ICommentsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Delete(int id, int userId)
        {
            try
            {
                var user = await _unitOfWork.UsersRepository.GetById(userId);
                var comment = await _unitOfWork.CommentsRepository.GetById(id);

                if (comment.UserId == user.Id || user.RolId == 1)
                {
                    await _unitOfWork.CommentsRepository.Delete(id);
                    await _unitOfWork.SaveChangesAsync();

                    return "The comment has been deleted.";
                }

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<CommentsDto>> GetAll()
        {
            var mapper = new EntityMapper();
            var commentsList = await _unitOfWork.CommentsRepository.GetAll();
            var commentsDtoList = commentsList.OrderBy(x => x.CreatedAt).Select(x => mapper.FromCommentsToCommentsDto(x)).ToList();
            return commentsDtoList;
        }

        public async Task<CommentsModel> GetById(int id)
        {
            return await _unitOfWork.CommentsRepository.GetById(id);
        }

        public async Task Insert(CommentsRequestDTO entity)
        {
            var newsObj = await _unitOfWork.NewsRepository.GetById(entity.NewsId);
            var userObj = await _unitOfWork.UsersRepository.GetById(entity.UserId);

            if (newsObj == null || userObj == null) throw new NullReferenceException("News or user does not exist");

            var mapper = new EntityMapper();
            var commentObj = mapper.FromCommentsDTOToComments(entity);
            await _unitOfWork.CommentsRepository.Insert(commentObj);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<CommentsDto> Update(CommentsDto commentDTO, int userId, int commentId)
        {
            try
            {
                var comment = await _unitOfWork.CommentsRepository.GetById(commentId);
                var user = await _unitOfWork.UsersRepository.GetById(userId);

                if (comment.UserId == user.Id || user.RolId == 1)
                {
                    var mapper = new EntityMapper();
                    comment.Body = commentDTO.Body;
                    await _unitOfWork.CommentsRepository.Update(comment);
                    await _unitOfWork.SaveChangesAsync();
                    var commentUpdateDTO = mapper.FromCommentsToCommentsDto(comment);
                    return commentUpdateDTO;
                }

                return null;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
