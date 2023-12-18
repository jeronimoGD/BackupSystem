using AutoMapper;
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

namespace BackupSystem.Common.Services
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
                    newAgentData.ConnectionKey = Guid.NewGuid();
                    await _unitOfWork.Agents.Create(newAgentData);
                    response = APIResponse.Ok(_mapper.Map<AgentDTO>(newAgentData));
                }
                else
                {
                    response = APIResponse.NotFound($"Agent already exists");
                }
            }
            catch (Exception e)
            {
                response = APIResponse.InternalServerError(e.ToString());
            }

            return response;
        }

        public Task<APIResponse> GetOnlineAgents()
        {
            // TODO: Implement logic to know all the online agents
            throw new NotImplementedException();
        }
    }
}
