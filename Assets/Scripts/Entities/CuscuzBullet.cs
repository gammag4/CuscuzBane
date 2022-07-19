using CuscuzBane.Base;
using System.Collections.Generic;
using UnityEngine;

namespace CuscuzBane.Entities
{
    public class CuscuzBullet : Entity
    {
        private float speed = 4;
        private float maxTimeAlive = 4;
        private float timeAlive;

        public float Damage => 6;

        public CuscuzBullet(GameObject gameObject) : base(gameObject)
        {
        }

        public override void Init()
        {
        }

        public override void Destroy()
        {
            base.Destroy();
        }

        public override void Update()
        {
            base.Update();
            CheckCollision();
        }

        private void CheckCollision()
        {
            timeAlive += Time.deltaTime;
            if (timeAlive > maxTimeAlive)
            {
                Destroy();
                return;
            }

            Transform.Translate(Vector3.right * speed * Time.deltaTime);

            var results = new List<Collider2D>();
            Physics2D.OverlapCollider(Hitbox, new ContactFilter2D().NoFilter(), results);

            if (results.Contains(Utils.Player.Hitbox))
            {
                Utils.Player.DealDamage(Damage, this);
                Destroy();
            }
        }
    }
}
