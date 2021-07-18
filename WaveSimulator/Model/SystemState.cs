using System;
namespace WaveSimulator.Model
{
    public class SystemState
    {
        
        public Resistor[] Resistors { get; set; }
        public SystemConfiguration Configuration { get; set; }

        public SystemState() { }

        public SystemState(SystemConfiguration Configuration, Resistor[] resistors)
        {
            Resistors = resistors;
        }
    
    }
}
