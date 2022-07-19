using CuscuzBane.Base;
using CuscuzBane.Behaviour;
using CuscuzBane.Entities;
using CuscuzBane.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace CuscuzBane
{
    public class MiniCuzAI : Machine
    {
        private MiniCuz miniCuz;

        private float speed = 0.5f; // velocidade com que ele anda
        private float playerFollowRadius = 2; // distância mínima do player para ficar seguindo ele
        private float maxIdleTime = 5; // tempo máximo que ele fica parado antes de começar a andar de novo
        private float maxWalkTime = 3; // tempo máximo que ele anda antes de ficar parado de novo
        private float maxDeathTime = 0; // tempo que ele fica morto antes de o objeto ser destruído
        private float hitCooldown = 2; // tempo minimo pra dar dano de novo

        private float lastHitTime;

        private State HittingPlayer;
        private State CheckingLife;
        private State LookingForPlayer;
        private State FollowingPlayer;

        public MiniCuzAI(MiniCuz miniCuz)
        {
            this.miniCuz = miniCuz;
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
                Physics2D.OverlapCollider(miniCuz.Hitbox, new ContactFilter2D().NoFilter(), results);

                if (results.Contains(Utils.Player.Hitbox))
                {
                    Utils.Player.DealDamage(miniCuz.Damage, miniCuz);
                    lastHitTime = hitCooldown;
                }
            });

            CheckingLife = new InlineState((s) =>
            {
                if (miniCuz.Health <= 0)
                {
                    GoTo(new Dead(this));
                }
            });

            LookingForPlayer = new InlineState((s) =>
            {
                if (Vector3.Distance(Utils.Player.Transform.position, miniCuz.Transform.position) <= playerFollowRadius)
                {
                    GoTo(new CombinedState(FollowingPlayer, HittingPlayer, CheckingLife));
                }
            });

            FollowingPlayer = new InlineState((s) =>
            {
                miniCuz.CurrentState = MiniCuz.State.FollowingPlayer;
            }, (s) =>
            {
                if (Vector3.Distance(Utils.Player.Transform.position, miniCuz.Transform.position) > playerFollowRadius)
                {
                    GoTo(new CombinedState(new Idle(this), LookingForPlayer, CheckingLife));
                }

                Vector3 direction = Utils.Player.Transform.position - miniCuz.Transform.position;
                miniCuz.TryMove(direction.normalized * speed * Time.deltaTime);
                miniCuz.Transform.Translate(new Vector3(0, 0, -0.00001f));
            });

            Start(new CombinedState(new Idle(this), LookingForPlayer, CheckingLife));
        }

        class Idle : State
        {
            private MiniCuzAI ai;
            private float idleTime;
            private float transitionToWalkTime;

            public Idle(MiniCuzAI ai)
            {
                this.ai = ai;
            }

            public override void Start()
            {
                ai.miniCuz.CurrentState = MiniCuz.State.Idle;
                transitionToWalkTime = Random.Range(0, ai.maxIdleTime);
            }

            public override void Update()
            {
                idleTime += Time.deltaTime;
                if (idleTime >= transitionToWalkTime)
                {
                    Machine.GoTo(new CombinedState(new Crawling(ai), ai.LookingForPlayer, ai.CheckingLife));
                }
            }
        }

        class Crawling : State
        {
            private MiniCuzAI ai;
            private float walkTime;
            private float transitionToIdleTime;
            private Vector3 direction;

            public Crawling(MiniCuzAI ai)
            {
                this.ai = ai;
            }

            public override void Start()
            {
                ai.miniCuz.CurrentState = MiniCuz.State.Crawling;
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

                ai.miniCuz.Transform.Translate(direction * ai.speed * Time.deltaTime);
            }
        }

        class Dead : State
        {
            private MiniCuzAI ai;
            private float deathTime;

            public Dead(MiniCuzAI ai)
            {
                this.ai = ai;
            }

            public override void Start()
            {
                ai.miniCuz.CurrentState = MiniCuz.State.Dead;
                deathTime = 0;
                Utils.Player.Kills += 1;
            }

            public override void Update()
            {
                deathTime += Time.deltaTime;
                if (deathTime > ai.maxDeathTime)
                {
                    var displacement = ai.miniCuz.GameObject.GetComponent<MiniCuzBehaviour>().hitbox.bounds.extents.y;
                    Object.Instantiate(Utils.Poof, ai.miniCuz.Transform.position + Vector3.up * displacement, Quaternion.identity);

                    ai.miniCuz.Destroy();
                }
            }
        }
    }
}
