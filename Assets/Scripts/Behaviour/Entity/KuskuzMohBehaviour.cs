using Assets.Scripts.Behaviour;
using CuscuzBane.Base;
using CuscuzBane.Entities;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class KuskuzMohBehaviour : MonoBehaviour
    {
        public Collider2D hitbox;
        public HealthBarBehaviour healthBar;
        public EntityNameBehaviour nameText;

        public KuskuzMoh KuskuzMoh { get; set; }
        private Animator animator;
        private KuskuzMoh.State lastState;

        // Use this for initialization
        void Start()
        {
            float life = 100;
            if (Utils.Level == 2) life = 200;

            life = (Utils.Level / 5f + 1f) * 250;

            KuskuzMoh = new KuskuzMoh(gameObject, life);
            KuskuzMoh.Init();
            KuskuzMoh.Hitbox = hitbox;
            if (healthBar)
                healthBar.GetValue = () => KuskuzMoh.Health / KuskuzMoh.TotalHealth;
            if (nameText)
                nameText.GetName = () => "Kuskuz'Moh";

            animator = GetComponent<Animator>();
            lastState = KuskuzMoh.State.None;
        }

        // Update is called once per frame
        void Update()
        {
            KuskuzMoh.Update();
            ChangeAnimation();
        }

        private void ChangeAnimation()
        {
            if (lastState == KuskuzMoh.CurrentState) return;
            lastState = KuskuzMoh.CurrentState;

            switch (KuskuzMoh.CurrentState)
            {
                case KuskuzMoh.State.BeforeSuperAttack:
                    animator.SetInteger("state", 1);
                    break;
                case KuskuzMoh.State.FollowingPlayer:
                case KuskuzMoh.State.SuperAttack:
                    animator.SetInteger("state", 2);
                    break;
                case KuskuzMoh.State.Dead:
                    animator.SetInteger("state", 3);
                    break;
                case KuskuzMoh.State.None:
                    animator.SetInteger("state", 0);
                    break;
            }
        }
    }
}
