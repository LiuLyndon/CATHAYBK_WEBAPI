using BasicEIP_Core.ApiResponse;
using BasicEIP_Core.Controllers;
using BasicEIP_Core.NLog;
using CATHAYBK_Service.Service;
using Microsoft.AspNetCore.Mvc;
using CATHAYBK_Main.Controllers;
using BasicEIP_Core.Security;

namespace CATHAYBK_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitcoinAESController : BaseController<BitcoinController>
    {
        private readonly BitcoinService _bitcoinService;
        private readonly AESService _aesService;

        public BitcoinAESController(
        BitcoinService bitcoinService,
        AESService aesService,
        IAppLogger<BitcoinController> logger) : base(logger)
        {
            _bitcoinService = bitcoinService;
            _aesService = aesService;
        }

        /// <summary>
        /// 取得所有 Bitcoin 資料
        /// </summary>
        /// <returns>Bitcoin 清單</returns>
        [HttpGet]
        [QueriedResponseType(typeof(ApiResponse<IEnumerable<object>>))]
        public async Task<IActionResult> GetBitcoins()
        {
            try
            {
                var bitcoins = await _bitcoinService.GetAllAsync();
                
                // 將敏感資料加密
                var encryptedBitcoins = bitcoins.Select(b => new
                {
                    Code = b.Code, // 將 Code 加密
                    Symbol = b.Symbol,
                    Rate = _aesService.Encrypt($"{b.Rate}"), // 加密 Code
                    Description = b.Description,
                    RateFloat = _aesService.Encrypt($"{b.RateFloat}") // 加密 Code
                }); 

                return Ok(new ApiResponse<IEnumerable<object>>(encryptedBitcoins.OrderBy(c => c.Code), "成功"));
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
        [QueriedResponseType(typeof(ApiResponse<object>))]
        public async Task<IActionResult> GetBitcoinById(string id) // 使用 string 接收加密的 ID
        {
            try
            {
                // 解密前端傳入的加密 ID
                var decryptedId = int.Parse(_aesService.Decrypt(id));

                var bitcoin = await _bitcoinService.GetByIdAsync(decryptedId);
                if (bitcoin == null)
                    return NotFound(new ApiResponse<string>(null, "找不到指定的資料"));

                // 將資料加密後返回
                var encryptedResult = new
                {
                    Code = bitcoin.Code,
                    Symbol = bitcoin.Symbol,
                    Rate = _aesService.Encrypt($"{bitcoin.Rate}"), // 加密 Code
                    Description = bitcoin.Description,
                    RateFloat = _aesService.Encrypt($"{bitcoin.RateFloat}") // 加密 Code
                };

                return Ok(new ApiResponse<object>(encryptedResult, "成功"));
            }
            catch (FormatException)
            {
                return BadRequest(new ApiResponse<string>(null, "ID 格式錯誤"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(null, $"伺服器錯誤: {ex.Message}"));
            }
        }
    }
}