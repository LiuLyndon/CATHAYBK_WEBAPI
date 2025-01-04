using CATHAYBK_Model.Database;
using CATHAYBK_Model.WEBAPI.Coindesk;
using CATHAYBK_Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace CATHAYBK_WEBAPI.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoindeskController  : ControllerBase
    {
        private readonly CoindeskService _coindeskService;
        private readonly CurrencyService _currencyService;
        private readonly BitcoinService _bitcoinService;

        public CoindeskController(
            CoindeskService coindeskService, 
            CurrencyService currencyService, 
            BitcoinService bitcoinService)
        {
            _coindeskService = coindeskService;
            _currencyService = currencyService;
            _bitcoinService = bitcoinService;
        }

        // 1. Fetch and Save Bitcoin data from Coindesk API
        [HttpGet("FetchAndSave")]
        public async Task<IActionResult> FetchAndSave()
        {
            // 呼叫 Coindesk API
            var coindeskResponse = await _coindeskService.GetCurrentPriceAsync();

            // 解析並轉換資料
            foreach (var bpi in coindeskResponse.Bpi)
            {
                string currencyCode = bpi.Key; // 鍵：幣別代碼，例如 "USD"
                CurrencyInfo currencyInfo = bpi.Value; // 值：CurrencyInfo 物件

                var bitcoin = new tblBitcoin
                {
                    Code = currencyInfo.Code,
                    Symbol = currencyInfo.Symbol,
                    Rate = decimal.Parse(currencyInfo.Rate.Replace(",", "")),
                    Description = currencyInfo.Description,
                    RateFloat = currencyInfo.rate_float,
                    CreatedAt = DateTime.UtcNow
                };

                // 寫入資料庫
                await _bitcoinService.AddAsync(bitcoin);
            }

            return Ok("Data fetched and saved successfully.");
        }

        // 2. Get all currencies (Read)
        [HttpGet("Currencies")]
        public async Task<IActionResult> GetAllCurrencies()
        {
            var currencies = await _currencyService.GetAllAsync();
            return Ok(currencies.OrderBy(c => c.Code));
        }

        // 3. Add a new currency (Create)
        [HttpPost("Currencies")]
        public async Task<IActionResult> AddCurrency([FromBody] CurrencyRequert requert)
        {
            await _currencyService.AddAsync(requert);
            return Created("", requert);
        }

        // 4. Update an existing currency (Update)
        [HttpPut("Currencies/{id}")]
        public async Task<IActionResult> UpdateCurrency(int id, [FromBody] CurrencyRequert requert)
        {
            try
            {
                await _currencyService.UpdateAsync(id, requert);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // 5. Delete a currency (Delete)
        [HttpDelete("Currencies/{id}")]
        public async Task<IActionResult> DeleteCurrency(int id)
        {
            try
            {
                await _currencyService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}