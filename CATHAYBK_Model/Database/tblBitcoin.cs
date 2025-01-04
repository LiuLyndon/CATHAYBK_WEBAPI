using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CATHAYBK_Model.Database
{
    /// <summary>
    /// 用來記錄比特幣的價格詳細數據（不同貨幣）
    /// </summary>
    public class tblBitcoin
    {
        /// <summary>
        /// 唯一識別碼
        /// </summary>
        [Key]
        public int Id { get; set; }

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

        /// <summary>
        /// 資料建立時間
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // 表示此屬性由資料庫生成
        public DateTime CreatedAt { get; set; }
    }
}

