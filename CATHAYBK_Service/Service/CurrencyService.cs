using CATHAYBK_Model.Database;
using CATHAYBK_Model.WEBAPI.Coindesk;
using CATHAYBK_Service.DatabseContext;
using Microsoft.EntityFrameworkCore;

namespace CATHAYBK_Service.Service
{
    public class CurrencyService
    {
        private readonly AppDbContext _context;

        public CurrencyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<tblCurrency>> GetAllAsync()
        {
            return await _context.Currencies.ToListAsync();
        }

        public async Task AddAsync(CurrencyRequert requert)
        {
            var currency = new tblCurrency
            {
                Code = requert.Code,
                Name = requert.Name
            };
            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, CurrencyRequert requert)
        {
            var currency = await _context.Currencies.FindAsync(id);
            if (currency == null)
            {
                throw new KeyNotFoundException("Currency not found");
            }

            currency.Code = requert.Code;
            currency.Name = requert.Name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var currency = await _context.Currencies.FindAsync(id);
            if (currency == null)
            {
                throw new KeyNotFoundException("Currency not found");
            }

            _context.Currencies.Remove(currency);
            await _context.SaveChangesAsync();
        }
    }
}