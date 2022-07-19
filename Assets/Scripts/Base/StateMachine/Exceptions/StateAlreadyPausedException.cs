
using System;

namespace CuscuzBane.StateMachine
{
    public class StateAlreadyPausedException : StateException
    {
        public State State { get; }

        public StateAlreadyPausedException(State state) : base($"Trying to pause an already paused machine state. State type: ${state.GetType()}.")
        {
            State = state;
        }
    }
}
