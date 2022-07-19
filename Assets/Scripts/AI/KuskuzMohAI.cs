using CuscuzBane.Base;
using CuscuzBane.Behaviour;
using CuscuzBane.Entities;
using CuscuzBane.StateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace CuscuzBane
{
    public class KuskuzMohAI : Machine
    {
        private KuskuzMoh kuskuzMoh;

        private float speed = 1.2f; // velocidade com que ele anda
        private float maxIdleTime = 3; // tempo máximo que ele fica parado antes de começar a andar de novo
        private float maxFollowingTime = 12; // tempo máximo que ele fica seguindo antes de dar super ataque
        private float hitCooldown = 1; // tempo minimo pra dar dano de novo
        private float lastHitTime;

        private State CheckingLife;
        private State HittingPlayer;

        public KuskuzMohAI(KuskuzMoh kuskuzMoh)
        {
            this.kuskuzMoh = kuskuzMoh;
        }

        public override void Start()
        {
            CheckingLife = new InlineState((s) =>
            {
                if (kuskuzMoh.Health <= 0)
                    GoTo(new Dead(this));
            });

            HittingPlayer = new InlineState((s) =>
            {
                if (lastHitTime > 0)
                {
                    lastHitTime -= Time.deltaTime;
                    return;
                }

                var results = new List<Collider2D>();
                Physics2D.OverlapCollider(kuskuzMoh.Hitbox, new ContactFilter2D().NoFilter(), results);

                if (results.Contains(Utils.Player.Hitbox))
                {
                    Utils.Player.DealDamage(kuskuzMoh.Damage, kuskuzMoh);
                    lastHitTime = hitCooldown;
                }
            });

            Start(new CombinedState(new FollowingPlayer(this), HittingPlayer, CheckingLife));
        }

        class BeforeSuperAttack : State
        {
            private KuskuzMohAI ai;
            private float idleTime;
            private float transitionToSuperAttack;
            private bool attacked;

            public BeforeSuperAttack(KuskuzMohAI ai)
            {
                this.ai = ai;
            }

            public override void Start()
            {
                ai.kuskuzMoh.CurrentState = KuskuzMoh.State.BeforeSuperAttack;
                transitionToSuperAttack = Random.Range(0, ai.maxIdleTime);
            }

            public override void Pause()
            {
                idleTime = 0;
            }

            public override void Update()
            {
                if (attacked)
                {
                    Machine.GoTo(new CombinedState(new FollowingPlayer(ai), ai.HittingPlayer, ai.CheckingLife));
                    return;
                }

                idleTime += Time.deltaTime;
                if (idleTime >= transitionToSuperAttack)
                {
                    var val = Random.Range(0f, 1f);

                    if (val < 0.2f)
                    {
                        Machine.Navigate(new CombinedState(new SuperSuperAttack(ai), ai.CheckingLife));
                    }
                    else
                    {
                        Machine.Navigate(new CombinedState(new SuperAttack(ai), ai.CheckingLife));
                    }

                    attacked = true;
                }
            }
        }

        class SuperAttack : State
        {
            private KuskuzMohAI ai;

            private int numberAttacks = 2;
            private int numberTimesAttacked;

            private float maxWait = 3;
            private float wait;

            public SuperAttack(KuskuzMohAI ai)
            {
                this.ai = ai;
            }

            public override void Start()
            {
                ai.kuskuzMoh.CurrentState = KuskuzMoh.State.BeforeSuperAttack;
            }

            public override void Update()
            {
                if (numberTimesAttacked > numberAttacks)
                {
                    Machine.GoBack();
                    return;
                }

                wait += Time.deltaTime;

                if (wait < maxWait) return;

                numberTimesAttacked++;

                Machine.Navigate(new CombinedState(new SuperAttackOnce(ai), ai.CheckingLife));
            }
        }

        class SuperAttackOnce : State
        {
            private KuskuzMohAI ai;

            private float timePreparing;
            private float maxTimePreparing = 0.3f;

            private float timeAttacking;
            private float maxTimeAttacking = 2f;

            private float attackSpeed = 4f;

            private float damage = 20;

            private Vector3 initialPosition;

            private bool toLeft;

            public SuperAttackOnce(KuskuzMohAI ai)
            {
                this.ai = ai;
                ai.lastHitTime = 0;
            }

            public override void Start()
            {
                toLeft = Random.Range(0, 2) == 0;

                ai.kuskuzMoh.CurrentState = KuskuzMoh.State.SuperAttack;
                initialPosition = Utils.Player.Transform.position + new Vector3(toLeft ? 1.5f : -1.5f, 0, 0);
                ai.kuskuzMoh.Transform.position = initialPosition;
            }

            public override void Pause()
            {
                ai.kuskuzMoh.Transform.rotation = Quaternion.identity;
            }

            public override void Update()
            {
                ai.kuskuzMoh.Transform.rotation = Quaternion.AngleAxis(45 * timePreparing / maxTimePreparing, toLeft ? Vector3.forward : Vector3.back);

                if (timePreparing < maxTimePreparing)
                {
                    timePreparing += Time.deltaTime;
                    return;
                }

                CheckHit();

                timePreparing = maxTimePreparing;

                timeAttacking += Time.deltaTime;

                var z = ai.kuskuzMoh.Transform.position.z;
                ai.kuskuzMoh.Transform.position = initialPosition + new Vector3((toLeft ? -1 : 1) * timeAttacking * attackSpeed, 0, 0);
                ai.kuskuzMoh.Transform.position = new Vector3(ai.kuskuzMoh.Transform.position.x, ai.kuskuzMoh.Transform.position.y, z);
                if (timeAttacking > maxTimeAttacking)
                {
                    Machine.GoBack();
                    return;
                }
            }

            private void CheckHit()
            {
                if (ai.lastHitTime > 0)
                {
                    ai.lastHitTime -= Time.deltaTime;
                    return;
                }

                var results = new List<Collider2D>();
                Physics2D.OverlapCollider(ai.kuskuzMoh.Hitbox, new ContactFilter2D().NoFilter(), results);

                if (results.Contains(Utils.Player.Hitbox))
                {
                    Utils.Player.DealDamage(damage, ai.kuskuzMoh);
                    ai.lastHitTime = ai.hitCooldown;
                }
            }
        }

        class SuperSuperAttack : State
        {
            private KuskuzMohAI ai;

            private int numberAttacks = 10;
            private int numberTimesAttacked;

            private float maxWait = 3;
            private float wait;

            private Vector3 playerPos;

            public SuperSuperAttack(KuskuzMohAI ai)
            {
                this.ai = ai;
                ai.lastHitTime = 0;
            }

            public override void Start()
            {
                ai.kuskuzMoh.CurrentState = KuskuzMoh.State.BeforeSuperAttack;
            }

            public override void Update()
            {
                if (numberTimesAttacked > numberAttacks)
                {
                    Machine.GoBack();
                    return;
                }

                wait += Time.deltaTime;

                if (wait < maxWait) return;

                if (numberTimesAttacked == 0)
                {
                    playerPos = Utils.Player.Transform.position;
                }

                numberTimesAttacked++;

                Machine.Navigate(new CombinedState(new SuperSuperAttackOnce(ai, playerPos, numberTimesAttacked, numberAttacks), ai.CheckingLife));
            }
        }

        class SuperSuperAttackOnce : State
        {
            private KuskuzMohAI ai;

            private float timePreparing;
            private float maxTimePreparing = 0.1f;

            private float timeAttacking;
            private float maxTimeAttacking = 0.2f;

            private float attackSpeed = 2f;

            private float damage = 20;

            private Vector3 initialPosition;
            private Vector3 playerPos;

            private bool toLeft;

            private int index;
            private int maxIndex;

            public SuperSuperAttackOnce(KuskuzMohAI ai, Vector3 playerPos, int index, int maxIndex)
            {
                this.playerPos = playerPos;
                this.maxIndex = maxIndex;
                this.index = index;
                this.ai = ai;
            }

            public override void Start()
            {
                toLeft = index % 2 == 0;

                ai.kuskuzMoh.CurrentState = KuskuzMoh.State.SuperAttack;
                initialPosition = playerPos + new Vector3(toLeft ? 1.5f : -1.5f, -1.5f + index * 3.0f / maxIndex, 0);
                ai.kuskuzMoh.Transform.position = initialPosition;
            }

            public override void Pause()
            {
                ai.kuskuzMoh.Transform.rotation = Quaternion.identity;
            }

            public override void Update()
            {
                ai.kuskuzMoh.Transform.rotation = Quaternion.AngleAxis(45 * timePreparing / maxTimePreparing, toLeft ? Vector3.forward : Vector3.back);

                if (timePreparing < maxTimePreparing)
                {
                    timePreparing += Time.deltaTime;
                    return;
                }

                CheckHit();

                timePreparing = maxTimePreparing;

                timeAttacking += Time.deltaTime * attackSpeed;

                var z = ai.kuskuzMoh.Transform.position.z;
                ai.kuskuzMoh.Transform.position = initialPosition + new Vector3((toLeft ? -1 : 1) * 3.0f * timeAttacking / maxTimeAttacking, 3.0f * timeAttacking / (maxIndex * maxTimeAttacking), 0);
                ai.kuskuzMoh.Transform.position = new Vector3(ai.kuskuzMoh.Transform.position.x, ai.kuskuzMoh.Transform.position.y, z);
                if (timeAttacking > maxTimeAttacking)
                {
                    Machine.GoBack();
                    return;
                }
            }

            private void CheckHit()
            {
                if (ai.lastHitTime > 0)
                {
                    ai.lastHitTime -= Time.deltaTime;
                    return;
                }

                var results = new List<Collider2D>();
                Physics2D.OverlapCollider(ai.kuskuzMoh.Hitbox, new ContactFilter2D().NoFilter(), results);

                if (results.Contains(Utils.Player.Hitbox))
                {
                    Utils.Player.DealDamage(damage, ai.kuskuzMoh);
                    ai.lastHitTime = ai.hitCooldown / 2;
                }
            }
        }

        class FollowingPlayer : State
        {
            private KuskuzMohAI ai;
            private float followTime;
            private float transitionToBeforeSuperAttack;

            public FollowingPlayer(KuskuzMohAI ai)
            {
                this.ai = ai;
            }

            public override void Start()
            {
                ai.kuskuzMoh.CurrentState = KuskuzMoh.State.FollowingPlayer;
                transitionToBeforeSuperAttack = Random.Range(3, ai.maxFollowingTime);
            }

            public override void Update()
            {
                followTime += Time.deltaTime;
                if (followTime > transitionToBeforeSuperAttack)
                {
                    Machine.GoTo(new CombinedState(new BeforeSuperAttack(ai), ai.HittingPlayer, ai.CheckingLife));
                    return;
                }

                Vector3 direction = Utils.Player.Transform.position - ai.kuskuzMoh.Transform.position;
                ai.kuskuzMoh.Transform.Translate(direction.normalized * ai.speed * Time.deltaTime);
                ai.kuskuzMoh.Transform.Translate(new Vector3(0, 0, -0.00001f));
            }
        }

        class Dead : State
        {
            private KuskuzMohAI ai;

            private GameObject poofObj;

            private bool loaded;

            public Dead(KuskuzMohAI ai)
            {
                this.ai = ai;
            }

            public override void Start()
            {
                ai.kuskuzMoh.CurrentState = KuskuzMoh.State.Dead;
                Utils.Player.Kills += 1;
                Utils.NumKusKuzMohDefeated++;
                ai.kuskuzMoh.GameObject.GetComponent<Renderer>().enabled = false;

                Object.Instantiate(Utils.Poof, ai.kuskuzMoh.Transform.position, Quaternion.identity);
            }

            public override void Update()
            {
                if (loaded) return;

                if (poofObj == null)
                {
                    loaded = true;

                    ai.kuskuzMoh.Destroy();

                    var ls = ai.kuskuzMoh.GameObject.GetComponent<LoadScene>();
                    if (Utils.Level == 2)
                    {
                        ls.LoadEndCutscene();
                    }
                    else
                    {
                        if (Utils.KuskuzMohs.Count == 0)
                            ls.LoadNextLevel();
                    }
                }
            }
        }
    }
}
