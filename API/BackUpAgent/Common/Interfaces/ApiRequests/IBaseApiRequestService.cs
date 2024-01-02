using BackUpAgent.Models.ApiInteractions;

namespace BackUpAgent.Common.Interfaces.ApiRequests
{
    public interface IBaseApiRequestService
    {
        Task<T> SendRequestAsync<T>(APIRequest apiRequest);
    }
}
