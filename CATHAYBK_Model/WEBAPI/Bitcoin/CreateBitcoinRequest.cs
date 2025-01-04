using System.ComponentModel.DataAnnotations;

namespace CATHAYBK_Model.WEBAPI.Bitcoin
{
	public class CreateBitcoinRequest
	{
        /// <summary>
        /// 貨幣代碼，例如 USD、GBP、EUR
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        /// <summary>
        /// 貨幣符號，例如 $、£、€
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; }

        /// <summary>
        /// 格式化顯示的價格
        /// </summary>
        [Required]
        [Range(typeof(decimal), "0", "999999999999.999999", ErrorMessage = "Rate must be between 0 and 999999999999.999999")]
        public decimal Rate { get; set; }

        /// <summary>
        /// 貨幣描述
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// 浮點數格式的價格
        /// </summary>
        [Required]
        [Range(typeof(decimal), "0", "999999999999.999999", ErrorMessage = "RateFloat must be between 0 and 999999999999.999999")]
        public decimal RateFloat { get; set; }
    }
}

