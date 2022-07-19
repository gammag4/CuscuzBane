using System;

namespace CuscuzBane.StateMachine
{
    public class MachineException : InvalidOperationException
    {
        public MachineException(string message) : base(message) { }
    }
}
