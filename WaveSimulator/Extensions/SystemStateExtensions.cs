using System;
using System.Collections.Generic;
using System.Linq;
using WaveSimulator.Model;

namespace WaveSimulator.Extensions
{
    public static class SystemStateExtensions
    {
        public static IEnumerable<Resistor> GetPositiveResistors(this SystemState state) =>
            state.Resistors.Where(x => x.ResistorValue > 0);

        public static IEnumerable<Resistor> GetNegativeResistors(this SystemState state) =>
            state.Resistors.Where(x => x.ResistorValue < 0);

        public static double GetPositiveResistance(this SystemState state) =>
            1 / state.GetPositiveResistors().Sum(x => 1 / x.ResistorValue);

        public static double GetNegativeResistance(this SystemState state) =>
            1 / Math.Abs(state.GetNegativeResistors().Sum(x => 1 / x.ResistorValue));

        public static double GetTotalResistanceOfSystem(this SystemState state) =>
            state.GetPositiveResistance() + state.GetNegativeResistance();

        public static double GetMaxSimulatedVoltage(this ICollection<SystemState> states) =>
            states.Max(x => x.GetVoltage());

        public static double GetMinimulatedVoltage(this ICollection<SystemState> states) =>
            states.Min(x => x.GetVoltage());

        public static double GetSimulatedVoltagePotential(this ICollection<SystemState> states)
        {
            var minVoltage = states.GetMinimulatedVoltage();
            var maxVoltage = states.GetMaxSimulatedVoltage();

            if (minVoltage >= 0)
            {
                return maxVoltage - minVoltage;
            }
            else
            {
                return Math.Abs(minVoltage) + maxVoltage;
            }
        }


        public static double GetVoltage(this SystemState state)
        {
                var ratioOfSystem = state.GetNegativeResistance() / state.GetTotalResistanceOfSystem();
                var voltageOfSystem = state.SystemConfiguration.GetTotalVoltagePotentialOfSystem() * ratioOfSystem;
                var offsetApplied = voltageOfSystem - Math.Abs(state.SystemConfiguration.SystemNegativeVoltage);
                return offsetApplied;
        }

        public static int GetStateRegisterState(this SystemState state)
        {
            var value = 0;
            var allAddresses = state.Resistors.Select(x => x.WaveTableAddress);
            foreach (var address in allAddresses)
            {
                value |= address;
            }
            return value;
        }
    }
}
