using CuscuzBane.Base;
using CuscuzBane.Behaviour;
using CuscuzBane.Entities;
using CuscuzBane.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace CuscuzBane
{
    public class ZumbiCuzAI : Machine
    {
        private ZumbiCuz zumbiCuz;

        private float speed = 0.3f; // velocidade com que ele anda
        private float playerFollowRadius = 2; // distância mínima do player para ficar seguindo ele
        private float maxIdleTime = 5; // tempo máximo que ele fica parado antes de começar a andar de novo
        private float maxWalkTime = 3; // tempo máximo que ele anda antes de ficar parado de novo
        private float maxDeathTime = 0; // tempo que ele fica morto antes de o objeto ser destruído
        private float hitCooldown = 1; // tempo minimo pra dar dano de novo

        private float lastHitTime;

        private State CheckingLife;
        private State HittingPlayer;
        private State LookingForPlayer;
        private State FollowingPlayer;

        public ZumbiCuzAI(ZumbiCuz zumbiCuz)
        {
            this.zumbiCuz = zumbiCuz;
        }

        public override void Start()
        {
            CheckingLife = new InlineState((s) =>
            {
                if (zumbiCuz.Health <= 0)
                {
                    GoTo(new Dead(this));
                }
            });

            HittingPlayer = new InlineState((s) =>
            {
                if (lastHitTime > 0)
                {
                    lastHitTime -= Time.deltaTime;
                    return;
                }

                var results = new List<Collider2D>();
                Physics2D.OverlapCollider(zumbiCuz.Hitbox, new ContactFilter2D().NoFilter(), results);

                if (results.Contains(Utils.Player.Hitbox))
                {
                    Utils.Player.DealDamage(zumbiCuz.Damage, zumbiCuz);
                    lastHitTime = hitCooldown;
                }
            });

            LookingForPlayer = new InlineState((s) =>
            {
                if (Vector3.Distance(Utils.Player.Transform.position, zumbiCuz.Transform.position) <= playerFollowRadius)
                {
                    GoTo(new CombinedState(FollowingPlayer, HittingPlayer, CheckingLife));
                }
            });

            FollowingPlayer = new InlineState((s) =>
            {
                zumbiCuz.CurrentState = ZumbiCuz.State.FollowingPlayer;
            }, (s) =>
            {
                if (Vector3.Distance(Utils.Player.Transform.position, zumbiCuz.Transform.position) > playerFollowRadius)
                {
                    GoTo(new CombinedState(new Idle(this), LookingForPlayer, CheckingLife));
                }

                Vector3 direction = Utils.Player.Transform.position - zumbiCuz.Transform.position;
                zumbiCuz.Move(direction.normalized * speed * Time.deltaTime + new Vector3(0, 0, -0.00001f));
            });

            Start(new CombinedState(new State[] { new Idle(this), LookingForPlayer, CheckingLife }));
        }

        class Idle : State
        {
            private ZumbiCuzAI ai;
            private float idleTime;
            private float transitionToWalkTime;

            public Idle(ZumbiCuzAI ai)
            {
                this.ai = ai;
            }

            public override void Start()
            {
                ai.zumbiCuz.CurrentState = ZumbiCuz.State.Idle;
                transitionToWalkTime = Random.Range(0, ai.maxIdleTime);
            }

            public override void Update()
            {
                idleTime += Time.deltaTime;
                if (idleTime >= transitionToWalkTime)
                {
                    Machine.GoTo(new CombinedState(new Walking(ai), ai.LookingForPlayer, ai.CheckingLife));
                }
            }
        }

        class Walking : State
        {
            private ZumbiCuzAI ai;
            private float walkTime;
            private float transitionToIdleTime;
            private Vector3 direction;

            public Walking(ZumbiCuzAI ai)
            {
                this.ai = ai;
            }

            public override void Start()
            {
                ai.zumbiCuz.CurrentState = ZumbiCuz.State.Walking;
                transitionToIdleTime = Random.Range(0, ai.maxWalkTime);
                direction = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.back) * Vector3.right;
            }

            public override void Update()
            {
                walkTime += Time.deltaTime;
                if (walkTime >= transitionToIdleTime)
                {
                    Machine.GoTo(new CombinedState(new Idle(ai), ai.LookingForPlayer, ai.CheckingLife));
                }

                ai.zumbiCuz.Move(direction * ai.speed * Time.deltaTime);
            }
        }

        class Dead : State
        {
            private ZumbiCuzAI ai;
            private float deathTime;

            public Dead(ZumbiCuzAI ai)
            {
                this.ai = ai;
            }

            public override void Start()
            {
                ai.zumbiCuz.CurrentState = ZumbiCuz.State.Dead;
                deathTime = 0;
                Utils.Player.Kills += 1;
            }

            public override void Update()
            {
                deathTime += Time.deltaTime;
                if (deathTime > ai.maxDeathTime)
                {
                    var displacement = ai.zumbiCuz.GameObject.GetComponent<ZumbiCuzBehaviour>().hitbox.bounds.extents.y;
                    Object.Instantiate(Utils.Poof, ai.zumbiCuz.Transform.position + Vector3.up * displacement, Quaternion.identity);

                    ai.zumbiCuz.Destroy();
                }
            }
        }
    }
}
