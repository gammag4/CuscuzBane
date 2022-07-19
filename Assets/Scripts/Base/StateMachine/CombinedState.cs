using System.Collections.Generic;

namespace CuscuzBane.StateMachine
{
    /// <summary>
    /// A state that is a combination of multiple states.
    /// </summary>
    public class CombinedState : State
    {
        private List<State> states;

        /// <summary>
        /// Creates a new combined state.
        /// <para/>
        /// States will be updated in order, then if you have three states,
        /// the second may override the first with GoTo and the navigationStack may override both.
        /// </summary>
        /// <param name="states">The states to combine.</param>
        public CombinedState(params State[] states)
        {
            this.states = new List<State>(states);
        }

        /// <summary>
        /// Creates a new combined state.
        /// <para/>
        /// States will be updated in order, then if you have three states,
        /// the second may override the first with GoTo and the navigationStack may override both.
        /// </summary>
        /// <param name="states">The states to combine.</param>
        public CombinedState(List<State> states)
        {
            this.states = states;
        }

        public override void MachineUpdate(Machine machine)
        {
            foreach (var state in states) state.MachineUpdate(machine);
            base.MachineUpdate(machine);
        }

        public override void MachinePause()
        {
            foreach (var state in states) state.MachinePause();
            base.MachinePause();
        }

        public override void Update() { }
    }
}
