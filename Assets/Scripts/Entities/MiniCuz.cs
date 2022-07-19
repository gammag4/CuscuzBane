using CuscuzBane.Base;
using CuscuzBane.Entities.Properties;
using UnityEngine;

namespace CuscuzBane.Entities
{
    public class MiniCuz : Entity
    {
        private MiniCuzAI ai;

        public float Damage => 5;

        // Fica true sempre que o minicuz está colidindo com o player.
        public bool HittingPlayer { get; set; }

        // O estado atual da AI.
        public State CurrentState { get; set; }

        public enum State
        {
            None,
            Idle,
            Crawling,
            FollowingPlayer,
            Dead
        }

        private TaintRedOnDamage taintRedOnDamage;

        public MiniCuz(GameObject gameObject) : base(gameObject)
        {
            HasKnockback = true;
            KnockbackAmount = 1.5f;
            CurrentState = State.None;
            Health = 10;
            TotalHealth = 10;
        }

        public override void Init()
        {
            taintRedOnDamage = new TaintRedOnDamage(this);
            ai = new MiniCuzAI(this);
            ai.Start();
        }

        public override void Destroy()
        {
            ai.Stop();
            base.Destroy();
        }

        public override void Update()
        {
            ai.Update();
            taintRedOnDamage.Update();
            base.Update();
        }
    }
}
