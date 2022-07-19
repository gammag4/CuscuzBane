using System;

namespace CuscuzBane.StateMachine
{
    public class StateException : InvalidOperationException
    {
        public StateException(string message) : base(message) { }
    }
}
