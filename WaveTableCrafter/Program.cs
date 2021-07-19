using System;
using System.Collections.Generic;
using System.Linq;
using WaveSimulator.Extensions;
using WaveSimulator.Model;

namespace WaveCrafter
{
    class MainClass
    {
       
        public static void Main(string[] args)
        {
            var systemConfig = new SystemConfiguration();

           
            var validStates = systemConfig.GetAllValidSystemStates();
             foreach (var state in validStates)
            {
                Console.WriteLine($"State Voltage = {state.GetVoltage()} System Positive Resistance = {state.GetPositiveResistance()} Negative Resistance = {state.GetNegativeResistance()}, Address = 0b{Convert.ToString(state.GetStateRegisterState(), 2).PadLeft(8,'0')} 0x{Convert.ToString(state.GetStateRegisterState(), 16)}, Resistors = {string.Join(',',state.Resistors.Select(x => x.ResistorValue.ToString()))}");
            }
            Console.WriteLine($"Total Unique States := {validStates.Count()}");

            var sinTable = GetSigTable(32, 0, 255).ToArray();
            var simulatorService = new WaveSimulator.Services.SimulatorService();
            var map = simulatorService.GenerateSystemStateMap(systemConfig, 255);
            var mapped = sinTable.Select(x => map[(int)Math.Round(x)]).ToArray();

            Console.WriteLine($"{{{string.Join(", ", map.Select(x => $"0x{Convert.ToString(x, 16)}"))}}}");


            //var sinTableMapped = sinTable.Select(x => validStates.Aggregate((y, z) => Math.Abs(y.GetVoltage() - x) < Math.Abs(z.GetVoltage() - x) ? y : z));
            Console.WriteLine();
             Console.WriteLine("Sin Table:");
             foreach(var mappedState in mapped)
             {
                var state = validStates.First(x => x.GetStateRegisterState() == mappedState);
                
                Console.WriteLine($"State Voltage = {state.GetVoltage()} System Positive Resistance = {state.GetPositiveResistance()} Negative Resistance = {state.GetNegativeResistance()}, Address = 0b{Convert.ToString(state.GetStateRegisterState(), 2).PadLeft(8, '0')} 0x{Convert.ToString(state.GetStateRegisterState(), 16)}, Resistors = {string.Join(',', state.Resistors.Select(x => x.ResistorValue.ToString()))}");
             }
            // Console.WriteLine($"{{{string.Join(", ",sinTableMapped.Select(x => $"0x{Convert.ToString(x.GetStateRegisterState(),16)}"))}}}"); 
            Console.WriteLine($"{{{string.Join(", ", sinTable.Select(x => $"0x{Convert.ToString((int)Math.Round( x), 16)}"))}}}");


        }


        private static IEnumerable<double> GetSigTable(int steps, double minValue, double maxValue)
        {
            for (var ix = 0; ix < steps; ix++)
            {
                double stepValue = ((2.0 * Math.PI) / (double)steps) * ix;
                yield return Math.Min(Math.Max(minValue, (Math.Sin(stepValue) + 1) * (maxValue / 2)), maxValue);

            }
        }
        private static IEnumerable<double> GetSquareTable(int steps, double minValue, double maxValue)
        {
            for (var ix = 0; ix < steps; ix++)
            {
                if (ix < (steps / 2))
                {
                    yield return minValue;
                }
                else
                {
                    yield return maxValue;
                }
                
            }
        }

        private static IEnumerable<double> GetSawToothTable(int steps, double minValue, double maxValue)
          {

            var diff = difference();
              for (var ix = 0; ix < steps; ix++)
              {

                yield return (((double)ix * ((double)1/steps)) * maxValue) + minValue;
              }

              double difference()
            {
                if (minValue >= 0)
                {
                    return maxValue - minValue;
                }
                else
                {
                    return Math.Abs(minValue) + maxValue;
                }
            }
          }
    }
}