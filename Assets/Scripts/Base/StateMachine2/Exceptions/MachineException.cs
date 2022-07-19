using System;

namespace CuscuzBane.StateMachine2
{
    public class MachineException : InvalidOperationException
    {
        public MachineException(string message) : base(message) { }
    }
}
