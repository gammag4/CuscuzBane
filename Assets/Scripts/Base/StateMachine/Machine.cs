using System.Collections.Generic;

namespace CuscuzBane.StateMachine
{
    /// <summary>
    /// State machine class (not thread safe).
    /// </summary>
    public abstract class Machine
    {
        private State state;
        private State next;
        private Stack<State> last;
        private NavigationType navigationType;

        /// <summary>
        /// Returns whether this machine is started or not.
        /// </summary>
        public bool Running { get; private set; }

        private enum NavigationType
        {
            None,
            GoTo,
            GoBack,
            Navigate,
        }

        /// <summary>
        /// Creates a new state machine.
        /// </summary>
        public Machine()
        {
            last = new Stack<State>();
            navigationType = NavigationType.None;
            Running = false;
        }

        /// <summary>
        /// Starts the state machine.
        /// <para/>
        /// If the machine is already started, <see cref="MachineAlreadyStartedException"/> will be thrown.
        /// <para/>
        /// Check <see cref="Start(State)"/> for more details.
        /// </summary>
        /// <exception cref="MachineAlreadyStartedException"></exception>
        public abstract void Start();

        /// <summary>
        /// Starts the state machine, specifying its initial state.
        /// <para/>
        /// If the machine is already started, <see cref="MachineAlreadyStartedException"/> will be thrown.
        /// <para/>
        /// Override <see cref="Start"/> and call this method with the initial state in it.
        /// </summary>
        /// <param name="initialState">The starting point of this state machine.</param>
        /// <exception cref="MachineAlreadyStartedException"></exception>
        protected void Start(State initialState)
        {
            if (Running) throw new MachineAlreadyStartedException(this);

            GoTo(initialState);
            Running = true;
        }

        /// <summary>
        /// Stops the state machine, clearing the navigation stack and pausing and removing the current state.
        /// <para/>
        /// To use this machine again after calling this function, you should call <see cref="Start"/> first.
        /// <para/>
        /// If the machine is already stopped, <see cref="MachineAlreadyStoppedException"/> will be thrown.
        /// </summary>
        /// <exception cref="MachineAlreadyStoppedException"></exception>
        public void Stop()
        {
            if (!Running) throw new MachineAlreadyStoppedException(this);

            last.Clear();

            state.MachinePause();
            state = null;
            next = null;
            Running = false;
        }

        /// <summary>
        /// Updates this state machine, changing the current state if needed and updating the current state.
        /// <para/>
        /// If the machine is stopped, <see cref="MachineStoppedException"/> will be thrown.
        /// <para/>
        /// When <see cref="GoBack"/> is called and there are no states to go back to, <see cref="MachineNoGoBackStateException"/> will be thrown.
        /// </summary>
        /// <exception cref="MachineStoppedException"></exception>
        /// <exception cref="MachineNoGoBackStateException"></exception>
        public void Update()
        {
            if (!Running) throw new MachineStoppedException(this);

            if (navigationType != NavigationType.None)
            {
                state?.MachinePause();
                CheckNavigationType();
                state = next;
                next = null;
            }

            state.MachineUpdate(this);
        }

        // Checks if there are any navigations needed to be done.
        private void CheckNavigationType()
        {
            switch (navigationType)
            {
                case NavigationType.GoTo:
                    last.Clear();
                    break;
                case NavigationType.Navigate:
                    last.Push(state);
                    break;
                case NavigationType.GoBack:
                    if (last.Count == 0)
                        throw new MachineNoGoBackStateException(this);

                    next = last.Pop();
                    break;
            }

            navigationType = NavigationType.None;
        }

        /// <summary>
        /// Goes to a new state, clearing the navigation stack.
        /// <para/>
        /// When this function is used, it is not possible to go back to the previous states.
        /// <para/>
        /// If you want to be able to go to previous states from the next states, use <see cref="Navigate(State)"/> instead.
        /// </summary>
        /// <param name="newState">The new state to go to.</param>
        public void GoTo(State newState)
        {
            navigationType = NavigationType.GoTo;
            next = newState;
        }

        /// <summary>
        /// Navigates to a new state, appending the current state to the navigation stack.
        /// <para/>
        /// When this function is used, you can go back to previous states later by calling <see cref="GoBack"/>.
        /// </summary>
        /// <param name="newState">The new state to go to.</param>
        public void Navigate(State newState)
        {
            navigationType = NavigationType.Navigate;
            next = newState;
        }


        /// <summary>
        /// Goes back to the navigationStack state in the navigation stack.
        /// <para/>
        /// If there are no states to go back to, a <see cref="MachineNoGoBackStateException"/> exception is thrown when calling <see cref="Update"/> after in the state machine.
        /// <para/>
        /// To be able to use this function, go to new states using <see cref="Navigate(State)"/>, as using <see cref="GoTo(State)"/> will not allow you to go back.
        /// </summary>
        public void GoBack()
        {
            navigationType = NavigationType.GoBack;
        }
    }
}
