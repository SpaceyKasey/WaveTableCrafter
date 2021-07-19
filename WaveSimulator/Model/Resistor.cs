using System;
namespace WaveSimulator.Model
{
    public class Resistor
    {
        public int WaveTableAddress { get; set; }
        public double ResistorValue { get; set; }

        public Resistor()
        {

        }
        
        public Resistor(int address, double value)
        {
            WaveTableAddress = address;
            ResistorValue = value;
        }
    }
}
