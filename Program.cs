using System;
using System.Collections.Generic;
using System.Linq;

namespace WaveCrafter
{
    class MainClass
    {
        public const double MaxVoltage = 5;
        public const double MinVoltage = 0;

        public const int StepsInWavetable = 32;


        internal static Dictionary<byte, byte> mappingDictionary = new Dictionary<byte, byte>()
        {
            {0,0x00},
            {1,0x01},
            {2,0x02},
            {3,0x02},
            {4,0x03},
            {5,0x04},
            {6,0x05},
            {7,0x07},
            {8,0x08},
            {9,0x09},
            {10,0x10},
            {11,0x11},
            {12,0x12},
            {13,0x13},
            {14,0x14},
            {15,0x15},
            {16,0x16}
        };

        internal static List<Resistor> resistors = new List<Resistor>()
        {
            new Resistor(0b00000001, 3.3),
            new Resistor(0b00000010, 2.2),
            new Resistor(0b00000100, 1.5),
            new Resistor(0b00001000, 1),
            new Resistor(0b10000000, -3.3),
            new Resistor(0b01000000, -2.2),
            new Resistor(0b00100000, -1.5),
            new Resistor(0b00010000, -1)

        };


        public static double VoltagePotentialOfSystem
        {
            get
            {
                if (MinVoltage >= 0)
                {
                    return MaxVoltage - MinVoltage;
                }
                else
                {
                    return Math.Abs(MinVoltage) + MaxVoltage;
                }
            }
        }

        public static void Main(string[] args)
        {
            var allResistors = GetAllCombos(resistors.ToArray());
            var allStates = allResistors.Select(x => new State(x.ToArray())).ToArray().OrderBy(x => x.Voltage);
            var validStates = allStates.Where(x => x.PositiveResistors.Any() && x.NegativeResistors.Any());
            var uniqueStates = allStates.GroupBy(x => x.Voltage).OrderBy(x => x.First().Voltage).ToArray().Select(x => x.First()).ToArray();
            foreach (var state in uniqueStates)
            {
                Console.WriteLine($"State Voltage = {state.Voltage} System Positive Resistance = {state.PositiveResistance} Negative Resistance = {state.NegativeResistance}, Address = 0b{Convert.ToString(state.Address, 2).PadLeft(8,'0')} 0x{Convert.ToString(state.Address, 16)}, Resistors = {string.Join(',',state.Resistors.Select(x => x.ResistorValue.ToString()))}");
            }
            Console.WriteLine($"Total Unique States := {uniqueStates.Count()}");

            var sinTable = GetSigTable(StepsInWavetable, MinVoltage, MaxVoltage);
            var sinTableMapped = sinTable.Select(x => uniqueStates.Aggregate((y, z) => Math.Abs(y.Voltage - x) < Math.Abs(z.Voltage - x) ? y : z));
            Console.WriteLine();
            Console.WriteLine("Sin Table:");
            foreach(var state in sinTableMapped)
            {
                Console.WriteLine($"State Voltage = {state.Voltage} System Positive Resistance = {state.PositiveResistance} Negative Resistance = {state.NegativeResistance}, Address = 0b{Convert.ToString(state.Address, 2).PadLeft(8, '0')} 0x{Convert.ToString(state.Address, 16)}, Resistors = {string.Join(',', state.Resistors.Select(x => x.ResistorValue.ToString()))}");
            }
            Console.WriteLine($"{{{string.Join(", ",sinTableMapped.Select(x => $"0x{Convert.ToString(x.Address,16)}"))}}}");
        }

        public static List<List<T>> GetAllCombos<T>(T[] list)
        {
            List<List<T>> result = new List<List<T>>();
            // head
            result.Add(new List<T>());
            result.Last().Add(list[0]);
            if (list.Length == 1)
                return result;
            // tail
            List<List<T>> tailCombos = GetAllCombos(list.Skip(1).ToArray());
            tailCombos.ForEach(combo =>
            {
                result.Add(new List<T>(combo));
                combo.Add(list[0]);
                result.Add(new List<T>(combo));
            });
            return result;
        }

        private static IEnumerable<double> GetSigTable(int steps, double minValue, double maxValue)
        {
            for (var ix = 0; ix < steps; ix++)
            {
                double stepValue = ((2.0 * Math.PI) / (double)steps) * ix;
                yield return Math.Min(Math.Max(minValue, (Math.Sin(stepValue) + 1) * (maxValue / 2)), maxValue);

            }
        }


        public class Resistor
        {
            public int WaveTableAddress { get; }
            public double ResistorValue { get; }


            public Resistor()
            {

            }
            public Resistor(int address, double value)
            {
                WaveTableAddress = address;
                ResistorValue = value;
            }
        }

        public class State
        {
            public IEnumerable<Resistor> PositiveResistors => Resistors.Where(x => x.ResistorValue > 0);
            public IEnumerable<Resistor> NegativeResistors => Resistors.Where(x => x.ResistorValue < 0);
            public double PositiveResistance => PositiveResistors.Sum(x => 1 / x.ResistorValue);
            public double NegativeResistance => Math.Abs(NegativeResistors.Sum(x => 1 / x.ResistorValue));
            public double TotalResistanceOfSystem => PositiveResistance + NegativeResistance;

            public double Voltage
            {
                get
                {
                    var ratioOfSystem = NegativeResistance / TotalResistanceOfSystem;
                    var voltageOfSystem = VoltagePotentialOfSystem * ratioOfSystem;
                    var offsetApplied = voltageOfSystem - Math.Abs(MinVoltage);
                    return offsetApplied;
 
                }
            }
            public Resistor[] Resistors { get; }
            public int Address
            {
                get
                {
                    var value = 0;
                    var allAddresses = Resistors.Select(x => x.WaveTableAddress);
                    foreach (var address in allAddresses)
                    {
                        value |= address;
                    }
                    return value;
                }
            }


            public State()
            {

            }
            public State(Resistor[] resistors)
            {
                Resistors = resistors;
            }
        }
    }
}