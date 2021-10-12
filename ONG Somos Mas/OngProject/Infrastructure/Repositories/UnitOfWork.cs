using OngProject.Core.Interfaces;
using OngProject.Core.Models;
using OngProject.Infrastructure.Data;
using System.Threading.Tasks;

namespace OngProject.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<NewsModel> _newsRepository;
        private IRepository<OrganizationModel> _organizationRepository;
        private IRepository<UsersModel> _usersRepository;
        private IRepository<CategoriesModel> _categoriesRepository;
        private IRepository<ActivitiesModel> _activitiesRepository;
        private IRepository<RolModel> _rolRepository;
        private IRepository<SlidesModel> _slidesRepository;
        private IRepository<MembersModel> _membersRepository;
        private IRepository<CommentsModel> _commentsRepository;
        private IRepository<ContactsModel> _contactsRepository;

        private IRepository<TestimonialsModel> _testimonialsRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<NewsModel> NewsRepository 
        {
            get
            {
                return _newsRepository ??= new Repository<NewsModel>(_context);
            }
        }

        public IRepository<OrganizationModel> OrganizationRepository 
        {
            get
            {
                return _organizationRepository ??= new Repository<OrganizationModel>(_context);
            }
        }


        public IRepository<UsersModel> UsersRepository
        {
            get
            {
                return _usersRepository ??= new Repository<UsersModel>(_context);
            }
        }
        public IRepository<CategoriesModel> CategoriesRepository
        {
            get
            {
                return _categoriesRepository ??= new Repository<CategoriesModel>(_context);
            }
        } 

        public IRepository<ActivitiesModel> ActivitiesRepository
        {
            get
            {
                return _activitiesRepository ??= new Repository<ActivitiesModel>(_context);

            }
        }
        public IRepository<RolModel> RolRepository
        {
            get
            {
                return _rolRepository ??= new Repository<RolModel>(_context);

            }
        }
        public IRepository<MembersModel> MembersRepository
        {
            get
            {
                return _membersRepository ??= new Repository<MembersModel>(_context);
            }
        }

        public IRepository<SlidesModel> SlidesRepository
        {
            get
            {
                return _slidesRepository ??= new Repository<SlidesModel>(_context);
            }
        }

        public IRepository<TestimonialsModel> TestimonialsRepository
        {
            get
            {
                return _testimonialsRepository ??= new Repository<TestimonialsModel>(_context);
            }
        }

        public IRepository<CommentsModel> CommentsRepository
        {
            get
            {
                return _commentsRepository ??= new Repository<CommentsModel>(_context);
            }
        }

        public IRepository<ContactsModel> ContactsRepository
        {
            get
            {
                return _contactsRepository ??= new Repository<ContactsModel>(_context);
            }
        }

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
