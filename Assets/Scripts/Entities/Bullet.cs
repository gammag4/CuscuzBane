using CuscuzBane.Base;
using System.Collections.Generic;
using UnityEngine;

namespace CuscuzBane.Entities
{
    public class Bullet : Entity
    {
        private float speed = 8;
        private float maxTimeAlive = 4;
        private float timeAlive;

        public float Damage => 3;

        public Bullet(GameObject gameObject) : base(gameObject)
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
            CheckCollision();
            base.Update();
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

            bool destroy = false;

            foreach (var item in results)
            {
                foreach (var entity in EntityManager.Entities)
                {
                    if (item != entity.Hitbox || entity == Utils.Player) continue;
                    entity.DealDamage(Damage, Utils.Player);
                    destroy = true;
                }
            }

            if (destroy) Destroy();
        }
    }
}
