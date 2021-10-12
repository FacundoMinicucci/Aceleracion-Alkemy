using Microsoft.EntityFrameworkCore;
using OngProject.Core.Models;

namespace OngProject.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRol(builder);
            SeedUsers(builder);
            SeedActivities(builder);

        }
        // TODO: DbSet<Models>

        public DbSet<ActivitiesModel> Activities { get; set; } = null;
        public DbSet<CategoriesModel> Categories { get; set; } = null;
        public DbSet<MembersModel> Members { get; set; } = null;
        public DbSet<NewsModel> News { get; set; } = null;
        public DbSet<OrganizationModel> Organizations { get; set; } = null;
        public DbSet<RolModel> Rols { get; set; } = null;
        public DbSet<SlidesModel> Slides { get; set; } = null;
        public DbSet<TestimonialsModel> Testimonials { get; set; } = null;
        public DbSet<UsersModel> Users { get; set; } = null;
        public DbSet<CommentsModel> Comments { get; set; } = null;
        public DbSet<ContactsModel> Contacts { get; set; } = null;

        public static void SeedUsers(ModelBuilder builder)
        {
            for (int i = 1; i < 21; i++)
            {
                if (i < 11)
                {
                    builder.Entity<UsersModel>().HasData(
                    new UsersModel
                    {
                        Id = i,
                        FirstName = "FirstName" + i,
                        LastName = "LastName" + i,
                        Email = "email." + i + "@example.com",
                        Password = UsersModel.ComputeSha256Hash("Password$" + i),
                        ConfirmPassword = UsersModel.ComputeSha256Hash("Password$" + i),
                        RolId = 1

                    }
                    );
                }
                else
                {
                    builder.Entity<UsersModel>().HasData(
                    new UsersModel
                    {
                        Id = i,
                        FirstName = "FirstName" + i,
                        LastName = "LastName" + i,
                        Email = "email." + i + "@example.com",
                        Password = UsersModel.ComputeSha256Hash("Password$" + i),
                        ConfirmPassword = UsersModel.ComputeSha256Hash("Password$" + i),
                        RolId = 2

                    }
                    );
                }
            }
        }

        public static void SeedRol(ModelBuilder builder)
        {
            builder.Entity<RolModel>().HasData(
                    new RolModel
                    {
                        Id = 1,
                        Name = "Administrator",
                        Description = ""
                    },
                    new RolModel
                    {
                        Id = 2,
                        Name = "Standard",
                        Description = ""
                    }
                );
        }

        public static void SeedActivities(ModelBuilder builder)
        {
            for (int i = 1; i < 11; i++)
            {
                builder.Entity<ActivitiesModel>().HasData(
                new ActivitiesModel
                {
                    Id = i,
                    Name = "Name" + i,
                    Content = "Content" + i,
                    Image = "Image" + i

                }
                );
            }
        }
    }
}
