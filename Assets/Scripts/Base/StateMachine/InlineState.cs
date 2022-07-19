using System;

namespace CuscuzBane.StateMachine
{
    /// <summary>
    /// A small inline state.
    /// </summary>
    public class InlineState : State
    {
        private Action<State> InitFunc;
        private Action<State> StartFunc;
        private Action<State> UpdateFunc;
        private Action<State> PauseFunc;

        /// <summary>
        /// Creates a new inline state.
        /// </summary>
        /// <param name="update">The <see cref="Update"/> function</param>
        public InlineState(Action<State> update) : this(null, update) { }

        /// <summary>
        /// Creates a new inline state.
        /// </summary>
        /// <param name="start">The <see cref="Start"/> function</param>
        /// <param name="update">The <see cref="Update"/> function</param>
        public InlineState(Action<State> start, Action<State> update) : this(start, update, null) { }

        /// <summary>
        /// Creates a new inline state.
        /// </summary>
        /// <param name="start">The <see cref="Start"/> function</param>
        /// <param name="update">The <see cref="Update"/> function</param>
        /// <param name="pause">The <see cref="Pause"/> function</param>
        public InlineState(Action<State> start, Action<State> update, Action<State> pause) : this(null, start, update, pause) { }

        /// <summary>
        /// Creates a new inline state.
        /// </summary>
        /// <param name="init">The <see cref="Init"/> function</param>
        /// <param name="start">The <see cref="Start"/> function</param>
        /// <param name="update">The <see cref="Update"/> function</param>
        /// <param name="pause">The <see cref="Pause"/> function</param>
        public InlineState(Action<State> init, Action<State> start, Action<State> update, Action<State> pause)
        {
            InitFunc = init;
            StartFunc = start;
            UpdateFunc = update;
            PauseFunc = pause;
        }

        public override void Init() => InitFunc?.Invoke(this);

        public override void Start() => StartFunc?.Invoke(this);

        public override void Update() => UpdateFunc(this);

        public override void Pause() => PauseFunc?.Invoke(this);
    }
}
