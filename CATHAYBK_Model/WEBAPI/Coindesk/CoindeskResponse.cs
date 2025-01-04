using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CATHAYBK_Model.WEBAPI.Coindesk
{
    public class CoindeskResponse
    {
        public Time Time { get; set; }
        public Dictionary<string, CurrencyInfo> Bpi { get; set; }
    }

    public class Time
    {
        public string Updated { get; set; }
        public string UpdatedISO { get; set; }
    }

    public class CurrencyInfo
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Rate { get; set; }
        public string Description { get; set; }
        public decimal rate_float { get; set; }
    }
}