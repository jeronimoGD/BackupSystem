using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUpAgent.Common.Interfaces
{
    public interface IStartUp
    {
        Task StartAgentAsync();
    }
}
