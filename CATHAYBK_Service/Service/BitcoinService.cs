using BasicEIP_Core.Repositories;
using CATHAYBK_Model.Database;

namespace CATHAYBK_Service.Service
{
    public class BitcoinService
    {
        private readonly IRepository<tblBitcoin> _bitcoinRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BitcoinService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _bitcoinRepository = unitOfWork.Repository<tblBitcoin>();
        }

        public async Task<IEnumerable<tblBitcoin>> GetAllAsync() => await _bitcoinRepository.GetAllAsync();

        public async Task<tblBitcoin> GetByIdAsync(int id) => await _bitcoinRepository.GetByIdAsync(id);

        public async Task AddAsync(tblBitcoin bitcoin)
        {
            await _bitcoinRepository.AddAsync(bitcoin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(tblBitcoin bitcoin)
        {
            await _bitcoinRepository.UpdateAsync(bitcoin);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(tblBitcoin bitcoin)
        {
            await _bitcoinRepository.DeleteAsync(bitcoin);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}