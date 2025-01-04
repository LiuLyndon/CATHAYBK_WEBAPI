using Microsoft.Extensions.Logging;

namespace TMSERP_Service.Base
{
    public abstract class ServiceBase<TLogger>
    {
        public readonly ILogger<TLogger> _logger;

        protected ServiceBase(ILogger<TLogger> logger)
        {
            _logger = logger;
        }
    }
}
