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
    public class MembersService : IMembersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAWSService _awsService;

        public MembersService(IUnitOfWork unitOfWork, IAWSService awsService)
        {
            _unitOfWork = unitOfWork;
            _awsService = awsService;
        }

        public async Task<IEnumerable<MembersDTO>> GetAll()
        {
            var members = await _unitOfWork.MembersRepository.GetAll();

            if (members.Any())
            {
                var membersDTO = new List<MembersDTO>();

                foreach (var member in members)
                {         
                    var memberDTO = new EntityMapper().FromMembersToMembersDto(member);
                    
                    membersDTO.Add(memberDTO);

                }
                return membersDTO;
            }
            else
            {
                return null;
            }
        }

        public async Task<MembersModel> GetById(int id)
        {
            return await _unitOfWork.MembersRepository.GetById(id);
        }

        public async Task Insert(MembersRequestDTO memberModel)
        {
            try
            {
                IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(memberModel.Image);

                var response = await _awsService.UploadImage(ImageFormFile);

                if (response.Code == 200)
                {
                    var mapper = new EntityMapper();
                    memberModel.Image = response.URL;
                    var memberObj = mapper.FromMembersRequestDTOToMembers(memberModel);
                    await _unitOfWork.MembersRepository.Insert(memberObj);
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

        public async Task Delete(int id)
        {
            try
            {
                await _unitOfWork.MembersRepository.Delete(id);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }

        public async Task Update(int id, MembersDTO memberModel)
        {
            try
            {
                IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(memberModel.Image);

                var response = await _awsService.UploadImage(ImageFormFile);

                if (response.Code == 200)
                {
                    MembersModel member = await _unitOfWork.MembersRepository.GetById(id);
                    var mapper = new EntityMapper();
                    memberModel.Image = response.URL;
                    member = mapper.FromMembersDTOToMembers(member, memberModel);
                    await _unitOfWork.MembersRepository.Update(member);
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
