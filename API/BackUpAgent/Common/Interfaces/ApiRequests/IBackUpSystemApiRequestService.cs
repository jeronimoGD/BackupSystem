using BackUpAgent.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUpAgent.Common.Interfaces.ApiRequests
{
    public interface IBackUpSystemApiRequestService
    {
        void SetSessionToken(string SetSessionToken);
        Task<T> GetAuthorizationToConnect<T>(Guid connectionKey);
        Task<T> GetBackUpConfigurations<T>(Guid connectionKey);
        Task<T> GetBackUpConfigurationByName<T>(string configurationName);
        Task<T> RegisterBackUpHistoryRecord<T>(BackUpHistory backUpHistory);
        Task<T> APILoging<T>();
    }
}
