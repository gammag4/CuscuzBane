using System.Collections.Generic;

namespace CuscuzBane.StateMachine2
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

        /// <summary>
        /// Adds a new state to this combined state. The state will be initialized and started if needed in the next update.
        /// </summary>
        /// <param name="state">The state to add.</param>
        public void Add(State state)
        {
            states.Add(state);
        }

        /// <summary>
        /// Pauses and removes a state from this combined state.
        /// </summary>
        /// <param name="state">The state to remove.</param>
        public void Remove(State state)
        {
            state.MachinePause();
            states.Remove(state);
        }

        public override void Init()
        {
            foreach (var state in states) state.Init();
        }

        public override void Start()
        {
            foreach (var state in states) state.Start();
        }

        public override void Update()
        {
            foreach (var state in states) state.Update();
            ShouldInitialize = true;
            ShouldStart = true;
        }

        public override void Pause()
        {
            foreach (var state in states) state.Pause();
        }
    }
}
