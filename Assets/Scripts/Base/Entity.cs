using CuscuzBane.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

namespace CuscuzBane.Base
{
    public abstract class Entity
    {
        public GameObject GameObject { get; private set; }
        public Transform Transform => GameObject.transform;

        public Collider2D Hitbox { get; set; }

        public float Health { get; protected set; }

        public float TotalHealth { get; protected set; }

        public bool HasKnockback { get; protected set; }

        public bool Knockbacking { get; protected set; }

        public float KnockbackAmount { get; protected set; }

        Collider2D collider;

        private float knockbackOffset = 1.5f;
        private float maxKnockbackTime = 1f;
        private float knockbackTime;
        private Vector3 knockbackDirection;

        public Entity(GameObject gameObject)
        {
            KnockbackAmount = 1f;

            GameObject = gameObject;
            EntityManager.Register(this);

            collider = gameObject.GetComponent<Collider2D>();
        }

        public bool TryMove(Vector3 delta) => TryMove(delta, collider);

        public bool TryMove(Vector3 delta, Collider2D collider)
        {
            return TryMoveOnce(delta, collider);
        }

        public bool TryMoveOnce(Vector2 delta, Collider2D collider)
        {
            // nao move se nao tiver movimento
            if (delta == Vector2.zero) return false;

            if (collider) delta = ComputeDeltaRemoveColliders(delta);

            Transform.position += (Vector3)delta;
            return !Mathf.Approximately(delta.magnitude, 0);
        }

        private Vector2 ComputeDeltaRemoveColliders(Vector2 delta)
        {
            List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
            var count = collider.Cast(Vector2.zero, new ContactFilter2D().NoFilter(), castCollisions, 0);

            for (int i = 0; i < count; i++)
            {
                if (castCollisions[i].transform.tag == "Wall")
                {
                    return delta;
                }
            }

            var allow = false;

            castCollisions = new List<RaycastHit2D>();
            count = collider.Cast(delta, new ContactFilter2D().NoFilter(), castCollisions, delta.magnitude + 0.01f);

            for (int i = 0; i < count; i++)
            {
                if (castCollisions[i].transform.tag == "AllowWalk")
                {
                    allow = true;
                    break;
                }
            }

            if (allow) return delta;

            for (int i = 0; i < count; i++)
            {
                if (castCollisions[i].transform.tag == "Wall")
                {
                    delta -= Vector2.Dot(castCollisions[i].normal, delta) * castCollisions[i].normal;
                }
            }

            return delta;
        }

        public virtual void DealDamage(float amount, Entity source)
        {
            Health -= amount;
            if (Health < 0) Health = 0;

            DealKnockback(source);
        }

        public void DealKnockback(Entity source)
        {
            if (!HasKnockback) return;

            if (Knockbacking)
            {
                knockbackTime = 0;
            }

            var direction = Transform.position - source.Transform.position;
            direction.Normalize();

            knockbackDirection = direction * KnockbackAmount * knockbackOffset;
            Knockbacking = true;

            // TODO
            //var rb = GameObject.GetComponent<Rigidbody2D>();

            //if (!HasKnockback) return;
            //if (rb == null) return;

            //var direction = Transform.position - source.Transform.position;
            //direction.Normalize();

            //rb.AddForce(direction * KnockbackAmount, ForceMode2D.Impulse);
        }

        public virtual void Heal(float amount)
        {
            Health += amount;
            if (Health > TotalHealth) Health = TotalHealth;
        }

        public virtual void Init() { }

        public virtual void Destroy()
        {
            EntityManager.Remove(this);
            Object.Destroy(GameObject);
        }

        public virtual void Update()
        {
            if (Knockbacking)
            {
                knockbackTime += Time.deltaTime;
                if (knockbackTime > maxKnockbackTime)
                {
                    knockbackTime = 0;
                    Knockbacking = false;
                    return;
                }

                var kn = maxKnockbackTime - knockbackTime;

                var amount = Mathf.Pow(kn, 3);

                var dir = knockbackDirection * Time.deltaTime * amount;

                TryMove(dir);
            }
        }
    }
}
