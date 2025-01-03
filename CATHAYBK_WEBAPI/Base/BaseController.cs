using BasicEIP_Core.NLog;
using Microsoft.AspNetCore.Mvc;

namespace TMSERP_Main.Controllers
{
    /// <summary>
    /// 公用Controller類別
    /// </summary>
    /// <typeparam name="T">Controller Type</typeparam>
    public class BaseController<T> : Controller
    {
        /// <summary>
        /// Logger
        /// </summary>
        public readonly IAppLogger<T> _logger;

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="logger"></param>
        public BaseController(IAppLogger<T> logger)
        {
            _logger = logger;
        }
    }
}
