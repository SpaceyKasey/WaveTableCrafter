using System;
namespace WaveSimulator.Model
{
    public class SystemState
    {
        
        public Resistor[] Resistors { get; set; }
        public SystemConfiguration SystemConfiguration { get; set; }

        public SystemState() { }

        public SystemState(SystemConfiguration Configuration, Resistor[] resistors)
        {
            Resistors = resistors;
            SystemConfiguration = Configuration;
        }
    
    }
}
