using CuscuzBane.Base;
using CuscuzBane.Behaviour;
using CuscuzBane.Entities;
using CuscuzBane.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace CuscuzBane
{
    public class CactusCuzAI : Machine
    {
        private CactusCuz cactusCuz;

        private int numberBullets = 7;
        private float shootInterval = 1;
        private float playerSeekRadius = 2; // distância mínima do player para ficar seguindo ele
        private float maxDeathTime = 0; // tempo que ele fica morto antes de o objeto ser destruído
        private float hitCooldown = 2; // tempo minimo pra dar dano de novo

        private float lastHitTime;

        private State HittingPlayer;
        private State CheckingLife;
        private State LookingForPlayer;

        public CactusCuzAI(CactusCuz cactusCuz)
        {
            this.cactusCuz = cactusCuz;
        }

        public override void Start()
        {
            HittingPlayer = new InlineState((s) =>
            {
                if (lastHitTime > 0)
                {
                    lastHitTime -= Time.deltaTime;
                    return;
                }

                var results = new List<Collider2D>();
                Physics2D.OverlapCollider(cactusCuz.Hitbox, new ContactFilter2D().NoFilter(), results);

                if (results.Contains(Utils.Player.Hitbox))
                {
                    Utils.Player.DealDamage(cactusCuz.Damage, cactusCuz);
                    lastHitTime = hitCooldown;
                }
            });

            CheckingLife = new InlineState((s) =>
            {
                if (cactusCuz.Health <= 0)
                {
                    GoTo(new Dead(this));
                }
            });

            LookingForPlayer = new InlineState((s) =>
            {
                if (Vector3.Distance(Utils.Player.Transform.position, cactusCuz.Transform.position) <= playerSeekRadius)
                {
                    GoTo(new CombinedState(new ShootingPlayer(this), HittingPlayer, CheckingLife));
                }
            });

            Start(new CombinedState(LookingForPlayer, CheckingLife));
        }

        class ShootingPlayer : State
        {
            private CactusCuzAI ai;
            private float shootTime;

            public ShootingPlayer(CactusCuzAI ai)
            {
                this.ai = ai;
            }

            public override void Update()
            {
                if (Vector3.Distance(Utils.Player.Transform.position, ai.cactusCuz.Transform.position) > ai.playerSeekRadius)
                {
                    Machine.GoTo(new CombinedState(ai.LookingForPlayer, ai.CheckingLife));
                }

                shootTime += Time.deltaTime;
                if (shootTime <= ai.shootInterval) return;

                shootTime = 0;

                ai.cactusCuz.ShootingPlayer = true;

                float degreeBetweenBullets = 360f / ai.numberBullets;

                // mira no meio do player
                Vector3 playerPosition = Utils.Player.CenterPos;

                Vector3 direction = playerPosition - ai.cactusCuz.ShootPosition;
                var degrees = Quaternion.FromToRotation(Vector3.right, direction).eulerAngles.z;

                for (int i = 0; i < ai.numberBullets; i++)
                {
                    var rotation = Quaternion.AngleAxis(-degrees + i * degreeBetweenBullets, Vector3.back);
                    Object.Instantiate(Utils.CuscuzBullet, ai.cactusCuz.ShootPosition, rotation);
                }
            }
        }

        class Dead : State
        {
            private CactusCuzAI ai;
            private float deathTime;

            public Dead(CactusCuzAI ai)
            {
                this.ai = ai;
            }

            public override void Start()
            {
                ai.cactusCuz.Dead = true;
                deathTime = 0;
                Utils.Player.Kills += 1;
            }

            public override void Update()
            {
                deathTime += Time.deltaTime;
                if (deathTime > ai.maxDeathTime)
                {
                    var displacement = ai.cactusCuz.GameObject.GetComponent<CactusCuzBehaviour>().hitbox.bounds.extents.y;
                    Object.Instantiate(Utils.Poof, ai.cactusCuz.Transform.position + Vector3.up * displacement, Quaternion.identity);

                    ai.cactusCuz.Destroy();
                }
            }
        }
    }
}
