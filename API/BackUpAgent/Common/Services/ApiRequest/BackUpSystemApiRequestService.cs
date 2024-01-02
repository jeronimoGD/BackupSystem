using BackUpAgent.Common.Interfaces.ApiRequests;
using BackUpAgent.Models.ApiInteractions;
using BackUpAgent.Models.ApplicationSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using static BackUpAgent.Models.ApiInteractions.APIHttpActions;

namespace BackUpAgent.Common.Services
{
    public class BackUpSystemApiRequestService : BaseApiRequestService, IBackUpSystemApiRequestService
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        public BackUpSystemApiRequestService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> apiSettings) : base(httpClientFactory)
        {
            _appSettings = apiSettings.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetAuthorizationToConnect<T>(Guid connectionKey, string token = null)
        {
            return await SendRequestAsync<T>(new APIRequest()
            {
                APITipo = APITipo.GET,
                Url = _appSettings.AgentManagerApiUrl + "/api/Agents/GetAuthorizationToConnect?connectionKey="+connectionKey.ToString(),
                Token = token
            });
        }

        public async Task<T> GetBackUpConfigurationByName<T>(string configurationName, string token = null)
        {
            return await SendRequestAsync<T>(new APIRequest()
            {
                APITipo = APITipo.GET,
                Url = _appSettings.AgentManagerApiUrl + "/api/BackUpConfiguration/GetBackUpConfiguration?name=" + configurationName,
                Token = token
            });
        }

        public async Task<T> GetBackUpConfigurations<T>(Guid connectionKey, string token = null)
        {
            return await SendRequestAsync<T>(new APIRequest()
            {
                APITipo = APITipo.GET,
                Url = _appSettings.AgentManagerApiUrl + "/api/Agents/GetBackUpConfigurationByAgent?connectionKey=" + connectionKey.ToString(),
                Token = token
            });
        }
    }
}
