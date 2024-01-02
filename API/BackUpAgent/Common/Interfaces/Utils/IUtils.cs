using BackUpAgent.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUpAgent.Common.Interfaces.Utils
{
    public interface IUtils
    {
        int GetAmountOfDaysFromPeriodicity(Periodicity periodicity);
    }
}
