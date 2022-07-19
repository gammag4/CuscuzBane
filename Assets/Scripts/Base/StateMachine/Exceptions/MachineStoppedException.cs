using System;

namespace CuscuzBane.StateMachine
{
    public class MachineStoppedException : MachineException
    {
        public Machine Machine { get; }

        public MachineStoppedException(Machine machine) : base($"Trying to run a machine that is stopped.")
        {
            Machine = machine;
        }
    }
}
