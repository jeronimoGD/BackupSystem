using AutoMapper;
using BackUpAgent.Common.Interfaces.ApiRequests;
using BackUpAgent.Common.Services.ApiRequest.DTO;
using BackUpAgent.Data.Entities;
using BackUpAgent.Models.ApiInteractions;
using BackUpAgent.Models.ApplicationSettings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using static BackUpAgent.Models.ApiInteractions.APIHttpActions;

namespace BackUpAgent.Common.Services
{
    public class BackUpSystemApiRequestService : BaseApiRequestService, IBackUpSystemApiRequestService
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        public BackUpSystemApiRequestService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> apiSettings, IMapper mapper) : base(httpClientFactory)
        {
            _appSettings = apiSettings.Value;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
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

        public async Task<T> RegisterBackUpHistoryRecord<T>(BackUpHistory backUpHistory, string token = null)
        {

            BackUpHistoryCreateDTO backUpHistoryDTO = _mapper.Map<BackUpHistoryCreateDTO>(backUpHistory);
            backUpHistoryDTO.AgentId = Guid.Parse(_appSettings.AgentConnectionKey);
            return await SendRequestAsync<T>(new APIRequest()
            {
                APITipo = APITipo.POST,
                Datos = backUpHistoryDTO,
                Url = _appSettings.AgentManagerApiUrl + "/api/BackUpHistory/AddBackUpHistory",
                Token = token
            });
        }
    }
}
