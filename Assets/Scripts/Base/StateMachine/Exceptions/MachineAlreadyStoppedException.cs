using System;

namespace CuscuzBane.StateMachine
{
    public class MachineAlreadyStoppedException : MachineException
    {
        public Machine Machine { get; }

        public MachineAlreadyStoppedException(Machine machine) : base($"Trying to stop a machine that is already stopped.")
        {
            Machine = machine;
        }
    }
}
