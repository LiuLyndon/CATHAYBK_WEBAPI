using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CATHAYBK_Model.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class tblCurrency
    {
        public int Id { get; set; }

        /// <summary>
        /// 貨幣代碼，例如 USD、GBP、EUR
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Code { get; set; } // 幣別代碼

        /// <summary>
        /// 貨幣代碼，例如 USD、GBP、EUR
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Name { get; set; } // 幣別中文名稱

        /// <summary>
        /// 資料建立時間
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // 表示此屬性由資料庫生成
        [JsonIgnore] // 排除序列化
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 資料更新時間
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)] // 表示此屬性由資料庫生成
        [JsonIgnore] // 排除序列化
        public DateTime UpdatedAt { get; set; }
    }
}