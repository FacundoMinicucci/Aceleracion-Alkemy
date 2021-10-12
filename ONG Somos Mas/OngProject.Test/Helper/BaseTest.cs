using Microsoft.EntityFrameworkCore;
using OngProject.Core.DTOs;
using OngProject.Core.Models;
using OngProject.Infrastructure.Data;
using System.Collections.Generic;

namespace OngProject.Test.Helper
{
    public class BaseTest
    {
        protected ApplicationDbContext MakeContext(string nombreDB)
        {
            var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(nombreDB).Options;
            var dbcontext = new ApplicationDbContext(opciones);          

            return dbcontext;
        }

        public IEnumerable<RolModel> GetRol()
        {
            var listRol = new List<RolModel>();
            listRol.Add(
            new RolModel
            {
                Id = 1,
                Name = "Administrator",
                Description = ""
            });
            listRol.Add(
                    new RolModel
                    {
                        Id = 2,
                        Name = "Standard",
                        Description = ""
                    });
            return listRol;
        }

        public IEnumerable<UsersModel> GetUsers()
        {
            var listUser = new List<UsersModel>();

            for (int i = 1; i < 21; i++)
            {
                if (i < 11)
                {
                    listUser.Add(
                    new UsersModel
                    {
                        Id = i,
                        FirstName = "FirstName" + i,
                        LastName = "LastName" + i,
                        Email = "email." + i + "@example.com",
                        Password = UsersModel.ComputeSha256Hash("Password$" + i),
                        ConfirmPassword = UsersModel.ComputeSha256Hash("Password$" + i),
                        RolId = 1

                    });


                }
                else
                {
                    listUser.Add(
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


            return listUser;
        }

       
    }
}
