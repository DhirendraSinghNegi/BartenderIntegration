using BartenderIntegration.Infrastructure.Models;
using Microsoft.Extensions.Options;

namespace BartenderIntegration.API.Helper
{
    public class BartenderHelper : IBartenderHelper
    {
        private readonly ILogger<BartenderHelper> _logger;
        private readonly AppSettings _appSettings;

        public BartenderHelper(ILogger<BartenderHelper> logger, IOptions<AppSettings> options)
        {
            _logger = logger;
            _appSettings = options.Value;
        }

        public async Task SendRequestAsync(List<string> data)
        {
            try
            {
                var client = new HttpClient();
                var url = new Uri(_appSettings.BartenderURL);
                foreach (var item in data)
                {
                    var response = await client.PostAsJsonAsync(url, data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while sending the request.");
                throw;
            }
        }
    }

    public interface IBartenderHelper
    {
        Task SendRequestAsync(List<string> data);
    }
}
