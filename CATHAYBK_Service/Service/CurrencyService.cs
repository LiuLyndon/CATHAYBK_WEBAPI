using BasicEIP_Core.Repositories;
using CATHAYBK_Model.Database;
using CATHAYBK_Service.Base;
using Microsoft.Extensions.Logging;

namespace CATHAYBK_Service.Service
{
    public class CurrencyService : ServiceBase<CurrencyService>
    {
        private readonly IRepository<tblCurrency> _currencyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyService(IUnitOfWork unitOfWork, ILogger<CurrencyService> logger) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _currencyRepository = unitOfWork.Repository<tblCurrency>();
        }

        public async Task<IEnumerable<tblCurrency>> GetAllAsync()
        {
            return await _currencyRepository.GetAllAsync();
        }

        public async Task<tblCurrency> GetByIdAsync(int id)
        {
            return await _currencyRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(tblCurrency data)
        {
            await _currencyRepository.AddAsync(data);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(tblCurrency data)
        {
            await _currencyRepository.UpdateAsync(data);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(tblCurrency data)
        {
            await _currencyRepository.DeleteAsync(data);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}