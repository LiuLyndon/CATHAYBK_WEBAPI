using CATHAYBK_Model.Database;
using CATHAYBK_Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace CATHAYBK_WEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitcoinController : ControllerBase
    {
        private readonly BitcoinService _bitcoinService;

        public BitcoinController(BitcoinService bitcoinService)
        {
            _bitcoinService = bitcoinService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bitcoins = await _bitcoinService.GetAllAsync();
            return Ok(bitcoins);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bitcoin = await _bitcoinService.GetByIdAsync(id);
            if (bitcoin == null) return NotFound();
            return Ok(bitcoin);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] tblBitcoin bitcoin)
        {
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