using System;
using System.Collections.Generic;
using System.Linq;
using WaveSimulator.Extensions;
using WaveSimulator.Model;

namespace WaveSimulator.Services
{
    public class SimulatorService
    {
        public SimulatorService()
        {
        }

        public int[] GenerateSystemStateMap(SystemConfiguration systemConfiguration, int maxNormalizedTargetValue)
        {
            var allSystemStates = systemConfiguration.GetAllValidSystemStates();
            //var maxSimulatedStateVoltage = allSystemStates.Max(x => x.GetVoltage());
            var minSimulatedStateVoltage = allSystemStates.Min(x => x.GetVoltage());
            var voltagePotentialSimulated = allSystemStates.GetSimulatedVoltagePotential();
            var normalizedVoltageTargetTable = GenerateTargetTable();
            var normalizedStateTable = normalizedVoltageTargetTable
                .Select(x => GetNearestVoltage(x, allSystemStates));
            var finalConfig = normalizedStateTable.Select(x => x.GetStateRegisterState());
            return finalConfig.ToArray();

            IEnumerable<double> GenerateTargetTable()
            {
                for (int ix = 0; ix <= maxNormalizedTargetValue; ix++)
                {
                    //Take normallized state and scale to voltage
                    yield return (((double)ix / (double)maxNormalizedTargetValue) * voltagePotentialSimulated) + minSimulatedStateVoltage;
                }
            }

            SystemState GetNearestVoltage(double targetVoltage, ICollection<SystemState> systemStates)
            {
                return systemStates.Select(p => new { Value = p, Difference = Math.Abs(p.GetVoltage() - targetVoltage) })
                    .OrderBy(p => p.Difference).First().Value;
                    
            }
        }

        

    }
}
