﻿using System.Text;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using BackUpAgent.Models.ApiInteractions;
using static BackUpAgent.Models.ApiInteractions.APIHttpActions;
using Azure;
using System;
using BackUpAgent.Common.Interfaces.ApiRequests;
using BackUpAgent.Data.Entities;
using Microsoft.Extensions.Logging;
namespace BackUpAgent.Common.Services
{
    public class BaseApiRequestService : IBaseApiRequestService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory _httpClientFactory { get; set; }
        private readonly ILogger<StartUp> _logger;

        public BaseApiRequestService(IHttpClientFactory httpClientFactory, ILogger<StartUp> logger)
        {
            this.responseModel = new();
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<T> SendRequestAsync<T>(APIRequest apiRequest)
        {
            APIResponse response = new APIResponse();

            try
            {
                var client = _httpClientFactory.CreateClient("BackUpAgentClient");
                
                HttpRequestMessage msg = new HttpRequestMessage();
                msg.Headers.Add("Accept", "application/json");
                msg.RequestUri = new Uri(apiRequest.Url);
 
                if (apiRequest.Datos != null)
                {
                    msg.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Datos), Encoding.UTF8, "application/json");
                }

                switch (apiRequest.APITipo)
                {
                    case APITipo.POST:
                        msg.Method = HttpMethod.Post;
                        break;
                    case APITipo.PUT:
                        msg.Method = HttpMethod.Put;
                        break;
                    case APITipo.DELETE:
                        msg.Method = HttpMethod.Delete;
                        break;
                    default:
                        msg.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;

                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }

                apiResponse = await client.SendAsync(msg);

                if (apiResponse.Content != null)
                {
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    if(apiContent != string.Empty)
                    {
                        response = JsonConvert.DeserializeObject<APIResponse>(apiContent);
                    }
                    else
                    {
                        response.ErrorMessages = apiResponse.ReasonPhrase;
                    }

                    response.StatusCode = apiResponse.StatusCode;
                    response.IsSuccesful = apiResponse.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {

                response = new APIResponse
                {
                    ErrorMessages = Convert.ToString(ex.Message),
                    IsSuccesful = false
                };
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(response)); ;
        }
    }
}
