using BackUpAgent.Common.Enums;
using BackUpAgent.Common.Interfaces.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackUpAgent.Common.Services.Utils
{
    public class Utils : IUtils
    {
        public int GetAmountOfDaysFromPeriodicity(Periodicity periodicity)
        {
            int days;

            switch (periodicity)
            {
                case Periodicity.Daily:
                    days = 1;
                    break;

                case Periodicity.Weekly:
                    days = 7;
                    break;

                case Periodicity.TwoWeeks:
                    days = 15;
                    break;

                case Periodicity.Monthly:
                    days = 30;
                    break;
                default:
                    days = 1;
                    break;
            }

            return days;
        }
    }
}
