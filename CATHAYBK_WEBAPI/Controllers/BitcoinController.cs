using BasicEIP_Core.ApiResponse;
using BasicEIP_Core.Controllers;
using BasicEIP_Core.NLog;
using CATHAYBK_Model.Database;
using CATHAYBK_Model.WEBAPI.Bitcoin;
using CATHAYBK_Service.Service;
using Microsoft.AspNetCore.Mvc;
using TMSERP_Main.Controllers;

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

        [HttpGet]
        [QueriedResponseType(typeof(ApiResponse<IEnumerable<tblBitcoin>>))]
        public async Task<IActionResult> GetAll()
        {
            var bitcoins = await _bitcoinService.GetAllAsync();
            return Ok(bitcoins);
        }

        [HttpGet("{id}")]
        [QueriedResponseType(typeof(ApiResponse<tblBitcoin>))]
        public async Task<IActionResult> GetById(int id)
        {
            var bitcoin = await _bitcoinService.GetByIdAsync(id);
            if (bitcoin == null) return NotFound();
            return Ok(bitcoin);
        }

        [HttpPost]
        [CreatedResponseType(typeof(ApiResponse<int>))]
        public async Task<IActionResult> Create([FromBody] CreateBitcoinRequest request)
        {
            // 將 request 映射到資料庫實體
            tblBitcoin bitcoin = new tblBitcoin
            {
                Code = request.Code,
                Symbol = request.Symbol,
                Rate = request.Rate,
                Description = request.Description,
                RateFloat = request.RateFloat
            };

            await _bitcoinService.AddAsync(bitcoin);
            return CreatedAtAction(nameof(GetById), new { id = bitcoin.Id }, bitcoin);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, [FromBody] tblBitcoin bitcoin)
        {
            if (id != bitcoin.Id)
            {
                return BadRequest();
            }
                
            await _bitcoinService.UpdateAsync(bitcoin);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bitcoin = await _bitcoinService.GetByIdAsync(id);
            if (bitcoin == null) return NotFound();
            await _bitcoinService.DeleteAsync(bitcoin);
            return NoContent();
        }
    }
}