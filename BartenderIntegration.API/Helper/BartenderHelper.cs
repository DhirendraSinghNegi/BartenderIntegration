using BartenderIntegration.API.Model;
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

        public async Task SendRequestAsync(CustomerModel data)
        {
            try
            {
                if (data == null)
                {
                    _logger.LogInformation("CustomerIds are not exists in the request.");
                    return;
                }
                var client = new HttpClient();
                var url = new Uri(_appSettings.BartenderURL); 

                var response = await client.PostAsJsonAsync(url, data);

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
        Task SendRequestAsync(CustomerModel data);
    }
}
