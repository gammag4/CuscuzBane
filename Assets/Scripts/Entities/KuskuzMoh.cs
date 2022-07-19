using CuscuzBane.Base;
using CuscuzBane.Entities.Properties;
using UnityEngine;

namespace CuscuzBane.Entities
{
    public class KuskuzMoh : Entity
    {
        private KuskuzMohAI ai;

        public float Damage => 10;

        // Fica true sempre que o kuskuz moh está colidindo com o player.
        public bool HittingPlayer { get; set; }

        // O estado atual da AI.
        public State CurrentState { get; set; }

        public enum State
        {
            None,
            BeforeSuperAttack,
            SuperAttack,
            FollowingPlayer,
            Dead
        }

        private TaintRedOnDamage taintRedOnDamage;

        public KuskuzMoh(GameObject gameObject, float life) : base(gameObject)
        {
            HasKnockback = true;
            KnockbackAmount = 0.5f;
            CurrentState = State.None;
            Health = life;
            TotalHealth = life;
        }

        public override void Init()
        {
            taintRedOnDamage = new TaintRedOnDamage(this);
            ai = new KuskuzMohAI(this);
            Utils.KuskuzMohs.Add(this);
            ai.Start();
        }

        public override void Destroy()
        {
            Utils.KuskuzMohs.Remove(this);
            ai.Stop();
            base.Destroy();
        }

        public override void Update()
        {
            ai.Update();
            taintRedOnDamage.Update();
            base.Update();
        }

        public static float GetTotalLife()
        {
            var health = 0f;
            var totalHealth = 0f;

            for (int i = 0; i < Utils.KuskuzMohs.Count; i++)
            {
                health += Utils.KuskuzMohs[i].Health;
                totalHealth += Utils.KuskuzMohs[i].TotalHealth;
            }
            return health / totalHealth;
        }
    }
}
