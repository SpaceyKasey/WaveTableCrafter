using System;
using System.Collections.Generic;
using System.Linq;
using WaveSimulator.Model;

namespace WaveSimulator.Extensions
{
    public static class SystemConfigurationExtensions
    {
        public static double GetTotalVoltagePotentialOfSystem(this SystemConfiguration configuration)
        {
            if (configuration.SystemNegativeVoltage >= 0)
            {
                return configuration.SystemPostiveVoltage - configuration.SystemNegativeVoltage;
            }
            else
            {
                return Math.Abs(configuration.SystemNegativeVoltage) + configuration.SystemPostiveVoltage;
            }
        }

        public static ICollection<SystemState> GetAllValidSystemStates(this SystemConfiguration configuration)
        {
            //Get all combination of resistors and order by total voltage
            var allStates = configuration.Resistors.GetAllCombos()
                .Select(x => new SystemState(configuration, x.ToArray())).ToArray()
                .OrderBy(x => x.GetVoltage());

            //Remove any states without a resistor on one side
            var validStates = allStates.Where(x => x.GetPositiveResistors().Any()
                                                   && x.GetNegativeResistors().Any());

            //Remove any duplicate states, just use first
            var uniqueStates = validStates
                .GroupBy(x => x.GetVoltage())
                .OrderBy(x => x.First().GetVoltage())
                .ToArray().Select(x => x.First())
                .ToArray();

            return uniqueStates;

        }
    }
    
}
