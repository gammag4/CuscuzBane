using Assets.Scripts.Behaviour;
using CuscuzBane.Entities;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class CactusCuzBehaviour : MonoBehaviour
    {
        public Collider2D hitbox;
        public HealthBarBehaviour healthBar;
        public EntityNameBehaviour nameText;

        public CactusCuz CactusCuz { get; set; }
        private Animator animator;

        private bool dead;

        // Use this for initialization
        void Start()
        {
            CactusCuz = new CactusCuz(gameObject);
            CactusCuz.Init();
            CactusCuz.Hitbox = hitbox;
            if (healthBar)
                healthBar.GetValue = () => CactusCuz.Health / CactusCuz.TotalHealth;
            if (nameText)
                nameText.GetName = () => "Cactus Cuz";

            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            CactusCuz.Update();
            ChangeAnimation();
        }

        private void ChangeAnimation()
        {
            if (dead) return;

            if (CactusCuz.Dead)
            {
                dead = true;
                animator.SetTrigger("dead");
            }

            if (!CactusCuz.ShootingPlayer) return;

            animator.SetTrigger("shoot");
            CactusCuz.ShootingPlayer = false;
        }
    }
}
