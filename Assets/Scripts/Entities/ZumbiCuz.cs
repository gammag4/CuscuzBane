using CuscuzBane.Base;
using CuscuzBane.Entities.Properties;
using UnityEngine;

namespace CuscuzBane.Entities
{
    public class ZumbiCuz : Entity
    {
        private ZumbiCuzAI ai;

        public float Damage => 10;

        // Fica true sempre que o minicuz está colidindo com o player.
        public bool HittingPlayer { get; set; }

        // O estado atual da AI.
        public State CurrentState { get; set; }

        public enum State
        {
            None,
            Idle,
            Walking,
            FollowingPlayer,
            Dead
        }

        private TaintRedOnDamage taintRedOnDamage;

        public ZumbiCuz(GameObject gameObject) : base(gameObject)
        {
            HasKnockback = true;
            KnockbackAmount = 1f;
            CurrentState = State.None;
            Health = 30;
            TotalHealth = 30;
        }

        public override void Init()
        {
            taintRedOnDamage = new TaintRedOnDamage(this);
            ai = new ZumbiCuzAI(this);
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

        public void Move(Vector3 direction)
        {
            TryMove(direction);
            var spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = direction.x > 0;
        }
    }
}
