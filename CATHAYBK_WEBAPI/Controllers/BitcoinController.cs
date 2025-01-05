using BasicEIP_Core.ApiResponse;
using BasicEIP_Core.Controllers;
using BasicEIP_Core.NLog;
using CATHAYBK_Model.Database;
using CATHAYBK_Model.WEBAPI.Bitcoin;
using CATHAYBK_Service.Service;
using Microsoft.AspNetCore.Mvc;
using CATHAYBK_Main.Controllers;

namespace CATHAYBK_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitcoinController : BaseController<BitcoinController>
    {
        private readonly BitcoinService _bitcoinService;

        public BitcoinController(
            BitcoinService bitcoinService,
            IAppLogger<BitcoinController> logger) : base (logger)
        {
            _bitcoinService = bitcoinService;
        }

        /// <summary>
        /// 取得所有 Bitcoin 資料
        /// </summary>
        /// <returns>Bitcoin 清單</returns>
        [HttpGet]
        [QueriedResponseType(typeof(ApiResponse<IEnumerable<tblBitcoin>>))]
        public async Task<IActionResult> GetBitcoins()
        {
            try
            {
                var bitcoins = await _bitcoinService.GetAllAsync();
                return Ok(new ApiResponse<IEnumerable<tblBitcoin>>(bitcoins.OrderBy(c => c.Code), "成功"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"伺服器錯誤: {ex.Message}"));
            }
        }

        /// <summary>
        /// 根據 ID 取得 Bitcoin 資料
        /// </summary>
        /// <param name="id">Bitcoin ID</param>
        /// <returns>Bitcoin 資料</returns>
        [HttpGet("{id}")]
        [QueriedResponseType(typeof(ApiResponse<tblBitcoin>))]
        public async Task<IActionResult> GetBitcoinById(int id)
        {
            try
            {
                var bitcoin = await _bitcoinService.GetByIdAsync(id);
                if (bitcoin == null)
                    return NotFound(new ApiResponse<string>(null, "找不到指定的資料"));

                return Ok(new ApiResponse<tblBitcoin>(bitcoin, "成功"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"伺服器錯誤: {ex.Message}"));
            }
        }

        /// <summary>
        /// 新增 Bitcoin 資料
        /// </summary>
        /// <param name="request">新增的請求資料</param>
        /// <returns>新增的 Bitcoin</returns>
        [HttpPost]
        [CreatedResponseType(typeof(ApiResponse<tblBitcoin>))]
        public async Task<IActionResult> CreateBitcoin([FromBody] CreateBitcoinRequest request)
        {
            try
            {
                var bitcoin = new tblBitcoin
                {
                    Code = request.Code,
                    Symbol = request.Symbol,
                    Rate = request.Rate,
                    Description = request.Description,
                    RateFloat = request.RateFloat
                };

                await _bitcoinService.AddAsync(bitcoin);
                return CreatedAtAction(nameof(GetBitcoinById), new { id = bitcoin.Id }, new ApiResponse<tblBitcoin>(bitcoin, "新增成功"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"伺服器錯誤: {ex.Message}"));
            }
        }

        /// <summary>
        /// 更新 Bitcoin 資料
        /// </summary>
        /// <param name="id">Bitcoin ID</param>
        /// <param name="bitcoin">更新的資料</param>
        /// <returns>更新結果</returns>
        [HttpPut("{id}")]
        [UpdatedResponseType(typeof(ApiResponse<string>))]
        public async Task<IActionResult> UpdateBitcoin(int id, [FromBody] tblBitcoin bitcoin)
        {
            try
            {
                if (id != bitcoin.Id)
                    return BadRequest(new ApiResponse<string>(null, "ID 不匹配"));

                await _bitcoinService.UpdateAsync(bitcoin);
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
        /// 刪除 Bitcoin 資料
        /// </summary>
        /// <param name="id">Bitcoin ID</param>
        /// <returns>刪除結果</returns>
        [HttpDelete("{id}")]
        [DeletedResponseType(typeof(ApiResponse<string>))]
        public async Task<IActionResult> DeleteBitcoin(int id)
        {
            try
            {
                var bitcoin = await _bitcoinService.GetByIdAsync(id);
                if (bitcoin == null)
                    return NotFound(new ApiResponse<string>(null, "找不到指定的資料"));

                await _bitcoinService.DeleteAsync(bitcoin);
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