using CuscuzBane.Base;
using CuscuzBane.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Behaviour.Entity
{
    public class CapThrowableBehaviour : MonoBehaviour
    {
        public GameObject capObj;

        public GameObject source { get; set; }

        private Vector3 direction;
        private Vector3 target;

        private float distance = 1f;
        private float accel = 1f;
        private float dampSpeed = 10f;

        private Vector3 velocity;

        private bool followSource;

        private Cap cap;

        public Action OnDestroy { get; set; }

        // Use this for initialization
        void Start()
        {
            cap = new Cap(gameObject);
            cap.Init();

            direction = transform.rotation * Vector3.right;
            target = transform.position + direction * distance;

            if (direction.x < 0)
            {
                FlipAll(true);
            }
        }

        // Update is called once per frame
        void Update()
        {
            cap.Update();

            transform.rotation = Quaternion.FromToRotation(Vector3.right, velocity);

            if (!followSource)
            {
                var newVelocity = Utils.GetVelocityTowards(transform.position, target, velocity, accel, 0);
                var newPos = transform.position + newVelocity;

                if ((target - newPos).magnitude < (target - transform.position).magnitude)
                {
                    velocity = newVelocity;
                    transform.position = newPos;
                    return;
                }
                else
                {
                    followSource = true;
                }
            }

            var results = new List<Collider2D>();
            Physics2D.OverlapCollider(GetComponent<Collider2D>(), new ContactFilter2D().NoFilter(), results);

            if (results.Contains(Utils.Player.Hitbox))
            {
                cap.Destroy();
                OnDestroy?.Invoke();
                return;
            }

            velocity = Utils.GetVelocityTowards(transform.position, Utils.Player.CenterPos, velocity, accel, dampSpeed);
            transform.position += velocity;
        }

        private void FlipAll(bool flip)
        {
            var renderers = GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].flipY = flip;
            }
        }
    }
}
