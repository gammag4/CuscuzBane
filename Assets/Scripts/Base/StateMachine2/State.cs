using System;

namespace CuscuzBane.StateMachine2
{
    public abstract class State
    {
        public Machine Machine { get; private set; }

        public bool ShouldInitialize { get; protected set; }

        public bool ShouldStart { get; protected set; }

        public bool Running { get; protected set; }

        public State()
        {
            ShouldInitialize = true;
            ShouldStart = true;
            Running = false;
        }

        public void MachineUpdate(Machine machine)
        {
            Machine = machine;

            if (ShouldInitialize)
            {
                Init();
                ShouldInitialize = false;

                Start();
                ShouldStart = false;
            }
            else if (ShouldStart)
            {
                Start();
                ShouldStart = false;
            }

            Running = true;
            Update();
        }

        public void MachinePause()
        {
            if (!Running)
                throw new StateAlreadyPausedException(this);

            Pause();
            ShouldStart = true;
            Running = false;
        }

        public virtual void Init() { }

        public virtual void Start() { }

        public abstract void Update();

        public virtual void Pause() { }
    }
}
