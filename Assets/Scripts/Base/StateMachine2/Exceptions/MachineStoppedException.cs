using System;

namespace CuscuzBane.StateMachine2
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
