using Assets.Scripts.Behaviour;
using CuscuzBane.Entities;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class ZumbiCuzBehaviour : MonoBehaviour
    {
        public Collider2D hitbox;
        public HealthBarBehaviour healthBar;
        public EntityNameBehaviour nameText;

        public ZumbiCuz ZumbiCuz { get; set; }
        private Animator animator;
        private ZumbiCuz.State lastState;

        // Use this for initialization
        void Start()
        {
            ZumbiCuz = new ZumbiCuz(gameObject);
            ZumbiCuz.Init();
            ZumbiCuz.Hitbox = hitbox;
            if (healthBar)
                healthBar.GetValue = () => ZumbiCuz.Health / ZumbiCuz.TotalHealth;
            if (nameText)
                nameText.GetName = () => "Zumbi Cuz";

            animator = GetComponent<Animator>();
            lastState = ZumbiCuz.State.None;
        }

        // Update is called once per frame
        void Update()
        {
            ZumbiCuz.Update();
            ChangeAnimation();
        }

        private void ChangeAnimation()
        {
            if (lastState == ZumbiCuz.CurrentState) return;
            lastState = ZumbiCuz.CurrentState;

            switch (ZumbiCuz.CurrentState)
            {
                case ZumbiCuz.State.Idle:
                    animator.SetInteger("state", 1);
                    break;
                case ZumbiCuz.State.FollowingPlayer:
                case ZumbiCuz.State.Walking:
                    animator.SetInteger("state", 2);
                    break;
                case ZumbiCuz.State.Dead:
                    animator.SetInteger("state", 3);
                    break;
                case ZumbiCuz.State.None:
                    animator.SetInteger("state", 0);
                    break;
            }
        }
    }
}
