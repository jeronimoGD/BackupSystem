using BackUpAgent.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUpAgent.Common.Interfaces.BackUpManaging
{
    public interface IBackUpManager
    {
        Task<BackUpHistory> DoBackUp(BackUpConfiguration conf);
    }
}
