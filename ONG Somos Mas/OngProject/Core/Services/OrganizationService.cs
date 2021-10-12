using OngProject.Core.Interfaces;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Models;
using System.Linq;
using System.Threading.Tasks;
using OngProject.Core.Mapper;
using OngProject.Core.DTOs;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using OngProject.Core.Helper;

namespace OngProject.Core.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAWSService _awsService;

        public OrganizationService(IUnitOfWork unitOfWork, IAWSService awsService)
        {
            _unitOfWork = unitOfWork;
            _awsService = awsService;
        }

        public async Task<OrganizationDTO> GetOrganization()
        {
            var mapper = new EntityMapper();
            var organization = await _unitOfWork.OrganizationRepository.GetAll().ContinueWith(x => x.Result.FirstOrDefault());

            List<SlideOrganizationDTO> slidesList = new List<SlideOrganizationDTO>();
            var slides = await _unitOfWork.SlidesRepository.GetAll();

            foreach (var x in slides)
            {
                var slide = mapper.FromSlidesToSlidesOrganizationDto(x);
                slidesList.Add(slide);
            }

            var organizationDTO = organization != null ? mapper.FromOrganizationToOrganizationDTO(organization, slidesList) : null;
            return organizationDTO;
        }

        public async Task Update(OrganizationUpdateDTO organizationDTO)
        {
            try
            {
                IFormFile ImageFormFile = ConvertBinaryToFormFile.BinaryToFormFile(organizationDTO.Image);

                var response = await _awsService.UploadImage(ImageFormFile);

                if (response.Code == 200)
                {
                    OrganizationModel organization = await _unitOfWork.OrganizationRepository.GetById(1);

                    var mapper = new EntityMapper();
                    organizationDTO.Image = response.URL;
                    var organizationUpdate = mapper.FromOrganizationDTOToOrganization(organizationDTO, organization);
                    await _unitOfWork.OrganizationRepository.Update(organizationUpdate);
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