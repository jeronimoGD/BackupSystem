using AutoMapper;
using BackUpAgent.Common.Interfaces.ApiRequests;
using BackUpAgent.Common.Services.ApiRequest.DTO;
using BackUpAgent.Data.Entities;
using BackUpAgent.Models.ApiInteractions;
using BackUpAgent.Models.ApplicationSettings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static BackUpAgent.Models.ApiInteractions.APIHttpActions;

namespace BackUpAgent.Common.Services
{
    public class BackUpSystemApiRequestService : BaseApiRequestService, IBackUpSystemApiRequestService
    {
        private readonly AppSettings _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<StartUp> _logger;
        private string _sessionToken;


        public BackUpSystemApiRequestService(IHttpClientFactory httpClientFactory, IOptions<AppSettings> apiSettings, IMapper mapper, ILogger<StartUp> logger) : base(httpClientFactory, logger)
        {
            _appSettings = apiSettings.Value;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public void SetSessionToken(string SetSessionToken)
        {
            _sessionToken = SetSessionToken;
        }

        public async Task<T> GetAuthorizationToConnect<T>(Guid connectionKey)
        {
            return await SendRequestAsync<T>(new APIRequest()
            {
                APITipo = APITipo.GET,
                Url = _appSettings.AgentManagerApiUrl + "/api/Agents/GetAuthorizationToConnect?connectionKey="+connectionKey.ToString(),
                Token = _sessionToken
            });
        }

        public async Task<T> GetBackUpConfigurationByName<T>(string configurationName)
        {
            return await SendRequestAsync<T>(new APIRequest()
            {
                APITipo = APITipo.GET,
                Url = _appSettings.AgentManagerApiUrl + "/api/BackUpConfiguration/GetBackUpConfiguration?name=" + configurationName,
                Token = _sessionToken
            });
        }

        public async Task<T> GetBackUpConfigurations<T>(Guid connectionKey)
        {
            return await SendRequestAsync<T>(new APIRequest()
            {
                APITipo = APITipo.GET,
                Url = _appSettings.AgentManagerApiUrl + "/api/Agents/GetBackUpConfigurationByAgent?connectionKey=" + connectionKey.ToString(),
                Token = _sessionToken
            });   
        }

        public async Task<T> RegisterBackUpHistoryRecord<T>(BackUpHistory backUpHistory)
        {

            BackUpHistoryCreateDTO backUpHistoryDTO = _mapper.Map<BackUpHistoryCreateDTO>(backUpHistory);
            backUpHistoryDTO.AgentId = Guid.Parse(_appSettings.AgentConnectionKey);
            return await SendRequestAsync<T>(new APIRequest()
            {
                APITipo = APITipo.POST,
                Datos = backUpHistoryDTO,
                Url = _appSettings.AgentManagerApiUrl + "/api/BackUpHistory/AddBackUpHistory",
                Token = _sessionToken
            });
        }

        public async Task<T> APILoging<T>()
        {
            return await SendRequestAsync<T>(new APIRequest()
            {
                APITipo = APITipo.POST,
                Datos = _appSettings.LoggingCredentials,
                Url = _appSettings.AgentManagerApiUrl + "/api/Users/LoginUser"
            });
        }
    }
}
