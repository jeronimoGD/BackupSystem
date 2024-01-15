using AutoMapper;
using Azure;
using BackupSystem.ApplicationSettings;
using BackupSystem.Common.Interfaces.Repository;
using BackupSystem.Common.Interfaces.Services;
using BackupSystem.Controllers.AplicationResponse;
using BackupSystem.Data.Entities;
using BackupSystem.DTO;
using BackupSystem.DTO.AgentDTOs;
using BackupSystem.DTO.RegisterDTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace BackupSystem.Common.Services.DbManagementServices
{
    public class AgentsService : BaseEntityService<Agent>, IAgentsService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APISettings _apiSettings;

        public AgentsService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<APISettings> apiSettings) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiSettings = apiSettings.Value;
        }

        public async Task<APIResponse> AddAgent(AgentCreateDTO createDto)
        {
            APIResponse response = new APIResponse();

            try
            {
                if (!await DoesEntityExists(a => a.AgentName == createDto.AgentName))
                {
                    Agent newAgentData = _mapper.Map<Agent>(createDto);
                    newAgentData.AgentKey = Guid.NewGuid();
                    newAgentData.BackUpConfigurations = new List<BackUpConfiguration>();
                    await _unitOfWork.Agents.Create(newAgentData);
                    response = APIResponse.Ok(_mapper.Map<AgentDTO>(newAgentData));
                }
                else
                {
                    response = APIResponse.NotFound($"Agent already exists.");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<APIResponse> IsAuthorized(Guid ConnectionKey)
        {
            APIResponse response = new APIResponse();

            try
            {
                var agent = await GetSingle(u => u.AgentKey == ConnectionKey);
                if (agent != null)
                {
                    if (agent.IsOnline == false)
                    {
                        response = APIResponse.Ok(agent);
                    }
                    else
                    {
                        response = APIResponse.BadRequest(agent, $"Agent with this key is already connected.");
                    }
                }
                else
                {
                    response = APIResponse.NotFound($"Agent with this key does not exist in the system.");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;


        }
        public async Task<APIResponse> SetOnlineStatus(Guid connectionKey, bool isOnline)
        {
            APIResponse response = new APIResponse();
            try
            {

                var agent = await GetSingle(a => a.AgentKey == connectionKey, true, a => a.BackUpConfigurations);

                if (agent != null)
                {
                    agent.IsOnline = isOnline;
                    await Update(agent);
                    response = APIResponse.Ok(agent);
                }
                else
                {
                    response = APIResponse.NotFound($"Agent with this key does not xist in the system.");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<APIResponse> SetAllOnlineStatus(bool isOnline)
        {
            APIResponse response = new APIResponse();
            try
            {

                var agents = await _unitOfWork.Agents.Get();

                if (agents != null)
                {
                    foreach (var agent in agents)
                    {
                        agent.IsOnline = isOnline;
                        await Update(agent);
                    }

                    response = APIResponse.Ok(agents);
                }
                else
                {
                    response = APIResponse.NotFound($"No agents found.");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public async Task<APIResponse> GetAgentBackUpConfiguration(Guid ConnectionKey)
        {
            APIResponse response = new APIResponse();

            try
            {
                var agent = await GetSingle(u => u.AgentKey == ConnectionKey, true, a => a.BackUpConfigurations);

                if (agent != null)
                {
                    response = APIResponse.Ok(agent.BackUpConfigurations);
                }
                else
                {
                    response = APIResponse.NotFound($"Agent with this key does not xist in the system.");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }
    }
}
