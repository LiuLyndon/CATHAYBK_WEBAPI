using BasicEIP_Core.ApiResponse;
using BasicEIP_Core.Controllers;
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

        /// <summary>
        /// 從 Coindesk API 抓取並儲存 Bitcoin 資料
        /// </summary>
        /// <returns>操作結果</returns>
        [HttpGet("FetchAndSave")]
        [DefaultResponseType(typeof(ApiResponse<string>))]
        public async Task<IActionResult> FetchAndSaveBitcoinData()
        {
            try
            {
                var coindeskResponse = await _coindeskService.GetCurrentPriceAsync();

                foreach (var bpi in coindeskResponse.Bpi)
                {
                    var bitcoin = new tblBitcoin
                    {
                        Code = bpi.Value.Code,
                        Symbol = bpi.Value.Symbol,
                        Rate = decimal.Parse(bpi.Value.Rate.Replace(",", "")),
                        Description = bpi.Value.Description,
                        RateFloat = bpi.Value.rate_float,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _bitcoinService.AddAsync(bitcoin);
                }

                return Ok(new ApiResponse<string>("資料已成功抓取並儲存", "成功"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"伺服器錯誤: {ex.Message}"));
            }
        }

        /// <summary>
        /// 取得所有貨幣資料
        /// </summary>
        /// <returns>貨幣清單</returns>
        [HttpGet("currencies")]
        [QueriedResponseType(typeof(ApiResponse<IEnumerable<tblCurrency>>))]
        public async Task<IActionResult> GetCurrencies()
        {
            try
            {
                var currencies = await _currencyService.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<tblCurrency>>(currencies.OrderBy(c => c.Code), "成功"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"伺服器錯誤: {ex.Message}"));
            }
        }


        /// <summary>
        /// 根據 ID 取得貨幣資料
        /// </summary>
        /// <param name="id">貨幣 ID</param>
        /// <returns>貨幣資料</returns>
        [HttpGet("currencies/{id}")]
        [QueriedResponseType(typeof(ApiResponse<tblCurrency>))]
        public async Task<IActionResult> GetCurrencyById(int id)
        {
            try
            {
                var currency = await _currencyService.GetByIdAsync(id);
                if (currency == null)
                    return NotFound(new ApiResponse<string>(null, "找不到指定的資料"));

                return Ok(new ApiResponse<tblCurrency>(currency, "成功"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"伺服器錯誤: {ex.Message}"));
            }
        }

        /// <summary>
        /// 新增貨幣資料
        /// </summary>
        /// <param name="request">貨幣請求資料</param>
        /// <returns>新增的貨幣</returns>
        [HttpPost("currencies")]
        [CreatedResponseType(typeof(ApiResponse<tblCurrency>))]
        public async Task<IActionResult> CreateCurrency([FromBody] CurrencyRequert request)
        {
            try
            {
                var currency = new tblCurrency
                {
                    Code = request.Code,
                    Name = request.Name
                };

                await _currencyService.AddAsync(currency);
                return CreatedAtAction(nameof(GetCurrencyById), new { id = currency.Id }, new ApiResponse<tblCurrency>(currency, "新增成功"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"伺服器錯誤: {ex.Message}"));
            }
        }

        /// <summary>
        /// 更新貨幣資料
        /// </summary>
        /// <param name="id">貨幣 ID</param>
        /// <param name="request">更新請求資料</param>
        /// <returns>更新結果</returns>
        [HttpPut("currencies/{id}")]
        [UpdatedResponseType(typeof(ApiResponse<string>))]
        public async Task<IActionResult> UpdateCurrency(int id, [FromBody] CurrencyRequert request)
        {
            try
            {
                var currency = await _currencyService.GetByIdAsync(id);
                if (currency == null)
                    return NotFound(new ApiResponse<string>(null, "找不到指定的資料"));

                currency.Code = request.Code;
                currency.Name = request.Name;

                await _currencyService.UpdateAsync(currency);
                return Ok(new ApiResponse<string>("更新成功", "成功"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(null, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"伺服器錯誤: {ex.Message}"));
            }
        }

        /// <summary>
        /// 刪除貨幣資料
        /// </summary>
        /// <param name="id">貨幣 ID</param>
        /// <returns>刪除結果</returns>
        [HttpDelete("currencies/{id}")]
        [DeletedResponseType(typeof(ApiResponse<string>))]
        public async Task<IActionResult> DeleteCurrency(int id)
        {
            try
            {
                var currency = await _currencyService.GetByIdAsync(id);
                if (currency == null)
                    return NotFound(new ApiResponse<string>(null, "找不到指定的資料"));

                await _currencyService.DeleteAsync(currency);
                return Ok(new ApiResponse<string>("刪除成功", "成功"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(null, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"伺服器錯誤: {ex.Message}"));
            }
        }
    }
}