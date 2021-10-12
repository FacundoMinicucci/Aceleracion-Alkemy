using OngProject.Core.Models;
using System;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<NewsModel> NewsRepository { get; }
        IRepository<OrganizationModel> OrganizationRepository { get; }
        IRepository<UsersModel> UsersRepository { get; }
        IRepository<CategoriesModel> CategoriesRepository { get; }
        IRepository<ActivitiesModel> ActivitiesRepository { get; }
        IRepository<RolModel> RolRepository { get; }
        IRepository<SlidesModel> SlidesRepository { get; }
        IRepository<MembersModel> MembersRepository { get; }
        IRepository<TestimonialsModel> TestimonialsRepository { get; }
        IRepository<CommentsModel> CommentsRepository { get; }
        IRepository<ContactsModel> ContactsRepository { get; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
