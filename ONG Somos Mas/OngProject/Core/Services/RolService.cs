using OngProject.Core.Interfaces;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class RolService : IRolService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RolService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Delete(int id)
        {
            await _unitOfWork.RolRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<RolModel>> GetAll()
        {
            return await _unitOfWork.RolRepository.GetAll();
        }

        public async Task<RolModel> GetById(int id)
        {
            return await _unitOfWork.RolRepository.GetById(id);
        }

        public async Task Insert(RolModel entity)
        {
            await _unitOfWork.RolRepository.Insert(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(RolModel entity)
        {
            await _unitOfWork.RolRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
