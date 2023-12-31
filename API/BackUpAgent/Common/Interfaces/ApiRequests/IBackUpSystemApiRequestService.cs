﻿using BackUpAgent.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUpAgent.Common.Interfaces.ApiRequests
{
    public interface IBackUpSystemApiRequestService
    {
        Task<T> GetAuthorizationToConnect<T>(Guid connectionKey, string token = null);
        Task<T> GetBackUpConfigurations<T>(Guid connectionKey, string token = null);
        Task<T> GetBackUpConfigurationByName<T>(string configurationName, string token = null);
        Task<T> RegisterBackUpHistoryRecord<T>(BackUpHistory backUpHistory, string token = null);
    }
}
