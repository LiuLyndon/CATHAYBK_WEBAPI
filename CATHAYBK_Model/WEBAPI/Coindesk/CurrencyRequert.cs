using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CATHAYBK_Model.WEBAPI.Coindesk
{
    public class CurrencyRequert
    {
        /// <summary>
        /// 幣別代碼
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 幣別中文名稱
        /// </summary>
        public string Name { get; set; } 
    }
}