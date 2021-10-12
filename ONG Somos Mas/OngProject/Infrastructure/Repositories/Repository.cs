using Microsoft.EntityFrameworkCore;
using OngProject.Core.Interfaces;
using OngProject.Core.Models;
using OngProject.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
;       }
        public async Task Delete(int id)
        {
            var entity = await _entities.FindAsync(id);

            entity.IsDeleted = true;

            entity.CreatedAt = DateTime.Now;

            _entities.Update(entity);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.ToListAsync().ContinueWith(x => x.Result.FindAll(e => e.IsDeleted == false));
        }

        public async Task<T> GetById(int id)
        {
            var entity = await _entities.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            return entity;
        }

        public async Task Insert(T entity)
        {
            entity.CreatedAt = DateTime.Now;

            entity.IsDeleted = false;

            await _entities.AddAsync(entity);            
        }

        public async Task Update(T entity)
        {
            entity.CreatedAt = DateTime.Now;

            _entities.Update(entity);
        }

        public async Task<UsersModel> FindByEmail(string email)
        {
            return await _context.Users.Include(x => x.Rol).FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
