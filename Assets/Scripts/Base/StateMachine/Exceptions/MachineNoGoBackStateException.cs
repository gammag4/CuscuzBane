using System;

namespace CuscuzBane.StateMachine
{
    public class MachineNoGoBackStateException : MachineException
    {
        public Machine Machine { get; }

        public MachineNoGoBackStateException(Machine machine) : base($"Trying to go back to a previous state, but there are no states to go back to.")
        {
            Machine = machine;
        }
    }
}
