using System;

namespace CuscuzBane.StateMachine2
{
    public class StateException : InvalidOperationException
    {
        public StateException(string message) : base(message) { }
    }
}
