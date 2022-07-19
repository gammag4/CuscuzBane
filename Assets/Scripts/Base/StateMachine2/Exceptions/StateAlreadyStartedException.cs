
using System;

namespace CuscuzBane.StateMachine2
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
