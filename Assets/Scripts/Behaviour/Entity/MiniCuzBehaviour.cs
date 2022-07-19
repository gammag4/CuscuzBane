using Assets.Scripts.Behaviour;
using CuscuzBane.Entities;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class MiniCuzBehaviour : MonoBehaviour
    {
        public Collider2D hitbox;
        public HealthBarBehaviour healthBar;
        public EntityNameBehaviour nameText;

        public MiniCuz MiniCuz { get; set; }
        private Animator animator;
        private MiniCuz.State lastState;

        // Use this for initialization
        void Start()
        {
            MiniCuz = new MiniCuz(gameObject);
            MiniCuz.Init();
            MiniCuz.Hitbox = hitbox;
            if (healthBar)
                healthBar.GetValue = () => MiniCuz.Health / MiniCuz.TotalHealth;
            if (nameText)
                nameText.GetName = () => "Mini Cuz";

            animator = GetComponent<Animator>();
            lastState = MiniCuz.State.None;
        }

        // Update is called once per frame
        void Update()
        {
            MiniCuz.Update();
            ChangeAnimation();
        }

        private void ChangeAnimation()
        {
            if (lastState == MiniCuz.CurrentState) return;
            lastState = MiniCuz.CurrentState;

            switch (MiniCuz.CurrentState)
            {
                case MiniCuz.State.Idle:
                    animator.SetInteger("state", 1);
                    break;
                case MiniCuz.State.FollowingPlayer:
                case MiniCuz.State.Crawling:
                    animator.SetInteger("state", 2);
                    break;
                case MiniCuz.State.Dead:
                    animator.SetInteger("state", 3);
                    break;
                case MiniCuz.State.None:
                    animator.SetInteger("state", 0);
                    break;
            }
        }
    }
}
