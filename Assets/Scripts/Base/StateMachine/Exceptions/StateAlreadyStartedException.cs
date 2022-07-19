
using System;

namespace CuscuzBane.StateMachine
{
    public class StateAlreadyStartedException : StateException
    {
        public State State { get; }

        public StateAlreadyStartedException(State state) : base($"Trying to start an already Running machine state. State type: ${state.GetType()}.")
        {
            State = state;
        }
    }
}
