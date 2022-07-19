using CuscuzBane.Base;
using System.Collections.Generic;
using UnityEngine;

namespace CuscuzBane.Entities
{
    public class Cap : Entity
    {
        public float Damage => 4;

        private List<Entity> damagedEntities;

        public Cap(GameObject gameObject) : base(gameObject)
        {
            damagedEntities = new List<Entity>();
            damagedEntities.Add(Utils.Player);
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
            var results = new List<Collider2D>();
            Physics2D.OverlapCollider(GameObject.GetComponent<Collider2D>(), new ContactFilter2D().NoFilter(), results);

            foreach (var item in results)
            {
                foreach (var entity in EntityManager.Entities)
                {
                    if (item != entity.Hitbox || damagedEntities.Contains(entity)) continue;
                    entity.DealDamage(Damage, this);
                    damagedEntities.Add(entity);
                }
            }
        }
    }
}
