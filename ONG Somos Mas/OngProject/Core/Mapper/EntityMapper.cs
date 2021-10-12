using OngProject.Core.DTOs;
using OngProject.Core.Models;
using System.Collections.Generic;

namespace OngProject.Core.Mapper
{
    public class EntityMapper
    {

        public UsersModel FromUserRegisterDTOToUser(UserRegisterDTO register)
        {
            var user = new UsersModel()
            {
                Email = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName,
                Password = register.Password,
                ConfirmPassword = register.ConfirmPassword
            };

            return user;
        }

        public UserCreatedDTO FromUserToUserCreatedDTO(UsersModel user)
        {
            var register = new UserCreatedDTO()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return register;
        }

        public UserDTO FromUserToUserDto(UsersModel user)
        {
            var userDTO = new UserDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Photo = user.Photo
            };

            return userDTO;
        }

        public  ActivitiesModel FromActivitiesDTOActivitiesTo(ActivitiesDTO activDTO)
        {
            var activModal = new ActivitiesModel
            {
                Name = activDTO.Name,
                Content = activDTO.Content,
                Image = activDTO.Image
            };

            return activModal;
        }

        public UserInfoDTO FromUserToUserInfoDto(UsersModel user)
        {
            var userInfoDTO = new UserInfoDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Photo = user.Photo,
                RolId = user.RolId
            };

            return userInfoDTO;
        }

        public LoginResponseDTO FromUserToLoginResponseDTO(UsersModel user, string token)
        {
            var loginResponseDTO = new LoginResponseDTO
            {
                Name = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Token = token
            };

            return loginResponseDTO;
        }

        public SlidesDto FromSlidesToSlidesDto(SlidesModel slide)
        {
            var slideDto = new SlidesDto
            {
                Image = slide.Image,
                Order = slide.Order
            };

            return slideDto;
        }

        public SlidesModel FromSlidesCreateDTOToSlides(SlidesCreateDTO slidesCreateDto)
        {
            var slides = new SlidesModel
            {
                Image = slidesCreateDto.Image,
                Text = slidesCreateDto.Text,
                Order = slidesCreateDto.Order,
                OrganizationId = 1
            };

            return slides;
        }

        public SlideOrganizationDTO FromSlidesToSlidesOrganizationDto(SlidesModel slide)
        {
            var slideDto = new SlideOrganizationDTO
            {
                Image = slide.Image,
                Text = slide.Text,
                Order = slide.Order
            };

            return slideDto;
        }

        public SlidesModel FromSlideUpdateDTOToSlide(SlidesModel slides, SlidesUpdateDTO dto)
        {
            slides.Image = dto.Image;
            slides.Text = dto.Text;
           
            return slides;

        }

        public OrganizationDTO FromOrganizationToOrganizationDTO(OrganizationModel organization, List<SlideOrganizationDTO> slides)
        {
            var organizationDTO = new OrganizationDTO
            {
                Name = organization.Name,
                Image = organization.Image,
                Phone = organization.Phone,
                Address = organization.Address,
                FacebookURL = organization.FacebookURL,
                LinkedinURL = organization.LinkedinURL,
                InstagramURL = organization.InstagramURL,
                Slides = slides
            };

            return organizationDTO;
        }

        public OrganizationModel FromOrganizationDTOToOrganization(OrganizationUpdateDTO organizationDTO, OrganizationModel organizationModel)
        {
            organizationModel.Name = organizationDTO.Name;
            organizationModel.Image = organizationDTO.Image;
            organizationModel.Phone = organizationDTO.Phone;
            organizationModel.Address = organizationDTO.Address;
            organizationModel.Email = organizationDTO.Email;
            organizationModel.WelcomeText = organizationDTO.WelcomeText;
            organizationModel.AboutUsText = organizationDTO.AboutUsText;
            organizationModel.FacebookURL = organizationDTO.FacebookURL;
            organizationModel.LinkedinURL = organizationDTO.LinkedinURL;
            organizationModel.InstagramURL = organizationDTO.InstagramURL;

            return organizationModel;
        }

        public NewsDTO FromNewsToNewsDto(NewsModel model)
        {
            var newsDTO = new NewsDTO
            {
                Name = model.Name,
                Content = model.Content,
                Image = model.Image
            };

            return newsDTO;
        }
        public NewsModel FromNewsDtoNewsTo(NewsInsertDTO model)
        {
            var newsModal = new NewsModel
            {
                Name = model.Name,
                Content = model.Content,
                Image = model.Image,
                Type = "news",
                CategoryId = model.CategoryId
            };

            return newsModal;
        }

        public NewsInsertExitDTO FromNewsToNewsInsertDto(NewsModel model)
        {
            var news = new NewsInsertExitDTO
            {
                Name = model.Name,
                Content = model.Content,
                Image = model.Image,
                Type = model.Type,
                CategoryId = model.CategoryId

            };

            return news;
        }

        public NewsModel FromNewsUpdateDTOToNews(NewsModel news, NewsUpdateDTO newsUpdateDTO)
        {
            news.Name = newsUpdateDTO.Name;
            news.Content = newsUpdateDTO.Content;
            news.Image = newsUpdateDTO.Image;
            news.Type = newsUpdateDTO.Type;                                 

            return news;
        }

        public CategoriesDTO FromCategoriesToCategoriesDTO(CategoriesModel model)
        {
            var categoriesDTO = new CategoriesDTO
            {
                Name = model.Name,
                Description = model.Description,
                Image = model.Image
            };

            return categoriesDTO;
        }

        public CategoriesOnlyNameDTO FromCategoriesToCategoriesOnlyNameDTO(CategoriesModel model)
        {
            var categoriesOnlyNameDTO = new CategoriesOnlyNameDTO
            {
                Name = model.Name
            };

            return categoriesOnlyNameDTO;
        }

        public CategoriesModel FromCategoriesDTOCategoriesTo(CategoriesDTO model)
        {
            var categoriesModel = new CategoriesModel
            {
                Name = model.Name,
                Description = model.Description,
                Image = model.Image
            };

            return categoriesModel;
        }

        public CommentsDto FromCommentsToCommentsDto(CommentsModel comment)
        {
            var commentDto = new CommentsDto
            {
                Body = comment.Body
            };

            return commentDto;
        }

        public MembersDTO FromMembersToMembersDto(MembersModel model)
        {
            var membersDTO = new MembersDTO
            {
                Name = model.Name,
                FacebookURL = model.FacebookURL,
                InstagramURL = model.InstagramURL,
                LinkedinURL = model.LinkedinURL,
                Image = model.Image,
                Description = model.Description
            };

            return membersDTO;
        }

        public MembersModel FromMembersDTOToMembers(MembersModel member, MembersDTO membersModel)
        {
            member.Name = membersModel.Name;
            member.FacebookURL = membersModel.FacebookURL;
            member.InstagramURL = membersModel.InstagramURL;
            member.LinkedinURL = membersModel.LinkedinURL;
            member.Image = membersModel.Image;
            member.Description = membersModel.Description;

            return member;
        }

        public ContactsDTO FromContactsToContactsDto(ContactsModel model)
        {
            var contactsDTO = new ContactsDTO
            {
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                Message = model.Message
            };

            return contactsDTO;
        }

        public CommentsModel FromCommentsDTOToComments(CommentsRequestDTO model)
        {
            var commentsModel = new CommentsModel
            {
                NewsId = model.NewsId,
                UserId = model.UserId,
                Body = model.Body
            };

            return commentsModel;
        }


        public ContactsModel FromContactDtoToContact(ContactsDTO contacts)
        {
            var contactsModel = new ContactsModel
            {
                Name = contacts.Name,
                Email = contacts.Email,
                Message = contacts.Message,
                Phone = contacts.Phone
            };

            return contactsModel;
        }

        public TestimonialsModel FromTestimonialsDTOToTestimonials(TestimonialsRequestDTO model)
        {
            var testimonialsModel = new TestimonialsModel
            {
                Name = model.Name,
                Image = model.Image,
                Content = model.Content
            };

            return testimonialsModel;
        }

        public TestimonialsModel FromTestimonialUpdateDTOToTestimonials(TestimonialUpdateDTO testimonialDTO, TestimonialsModel testimonial)
        {
            testimonial.Name = testimonialDTO.Name;
            testimonial.Image = testimonialDTO.Image;
            testimonial.Content = testimonialDTO.Content;

            return testimonial;
        }

        public TestimonialResponseDTO FromTestimonialsToTestimonialsDTO(TestimonialsModel testimonial)
        {
            var testimonialsDTO = new TestimonialResponseDTO
            {
                Name = testimonial.Name,
                Image = testimonial.Image,
                Content = testimonial.Content
            };

            return testimonialsDTO;
        }

        public MembersModel FromMembersRequestDTOToMembers(MembersRequestDTO model)
        {
            var membersModel = new MembersModel
            {
                Name = model.Name,
                FacebookURL = model.FacebookURL,
                InstagramURL = model.InstagramURL,
                LinkedinURL = model.LinkedinURL,
                Image = model.Image,
                Description = model.Description
            };

            return membersModel;
        }

        public CategoriesModel FromCategoriesDTOToUpdateCategories(CategoriesDTO categoryDTO, CategoriesModel categoryObj)
        {
            categoryObj.Name = categoryDTO.Name;
            categoryObj.Description = categoryDTO.Description;
            categoryObj.Image = categoryDTO.Image;

            return categoryObj;
        }
    }
}