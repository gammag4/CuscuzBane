
using System;

namespace CuscuzBane.StateMachine
{
    public class StatePausedException : StateException
    {
        public State State { get; }

        public StatePausedException(State state) : base($"Trying to update a machine state that has been paused. State type: ${state.GetType()}.")
        {
            State = state;
        }
    }
}
