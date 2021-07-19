using System;
using System.Collections.Generic;

namespace WaveSimulator.Model
{
    public class SystemConfiguration
    {

        public double SystemPostiveVoltage { get; set; } = 5;
        public double SystemNegativeVoltage { get; set; } = 0;

        public ICollection<Resistor> Resistors { get; set; } = new List<Resistor>()
        {
            new Resistor(0b00001000, 3.3),
            new Resistor(0b00000100, 2.2),
            new Resistor(0b00000010, 1.5),
            new Resistor(0b00000001, 1),
            new Resistor(0b00010000, -3.3),
            new Resistor(0b00100000, -2.2),
            new Resistor(0b01000000, -1.5),
            new Resistor(0b10000000, -1)

        };

       /* public ICollection<Resistor> Resistors { get; set; } = new List<Resistor>()
        {
            new Resistor(0b00001000, -3.3),
            new Resistor(0b00000100, -2.2),
            new Resistor(0b00000010, -1.5),
            new Resistor(0b00000001, -1),
            new Resistor(0b00010000, 3.3),
            new Resistor(0b00100000, 2.2),
            new Resistor(0b01000000, 1.5),
            new Resistor(0b10000000, 1)

        };*/
    }
}
