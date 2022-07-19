using CuscuzBane.Base;
using CuscuzBane.Entities.Properties;
using UnityEngine;

namespace CuscuzBane.Entities
{
    public class CactusCuz : Entity
    {
        private CactusCuzAI ai;

        public float Damage => 12;

        // Fica true sempre que o cactus cuz está colidindo com o player.
        public bool HittingPlayer { get; set; }

        // Fica true sempre quando o cactus cuz vai atirar no player.
        public bool ShootingPlayer { get; set; }

        public bool Dead { get; set; }

        public Vector3 ShootPosition => Transform.position + new Vector3(0, 0.2f, 0);

        private TaintRedOnDamage taintRedOnDamage;

        public CactusCuz(GameObject gameObject) : base(gameObject)
        {
            Health = 30;
            TotalHealth = 30;
        }

        public override void Init()
        {
            taintRedOnDamage = new TaintRedOnDamage(this);
            ai = new CactusCuzAI(this);
            ai.Start();
        }

        public override void Destroy()
        {
            ai.Stop();
            base.Destroy();
        }

        public override void Update()
        {
            ai.Update();
            taintRedOnDamage.Update();
            base.Update();
        }
    }
}
