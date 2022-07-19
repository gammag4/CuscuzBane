using CuscuzBane.Base;
using UnityEngine;

namespace CuscuzBane.Entities.Properties
{
    public class TaintRedOnDamage
    {
        private Entity entity;

        private float lastHealth;

        private float redTime;
        private float maxRedTime = 0.3f;

        private bool changeTainted;

        private bool tainted;

        public TaintRedOnDamage(Entity entity)
        {
            this.entity = entity;
            lastHealth = entity.Health;
        }

        public void Update()
        {
            redTime += Time.deltaTime;
            CheckChange();
            ChangeColor();
        }

        private void CheckChange()
        {
            if (entity.Health < lastHealth)
            {
                redTime = 0;
                lastHealth = entity.Health;
                if (!tainted) changeTainted = true;
            }
            else if (redTime > maxRedTime && tainted)
            {
                changeTainted = true;
            }
        }

        private void ChangeColor()
        {
            if (!changeTainted) return;

            tainted = !tainted;

            var sr = entity.GameObject.GetComponent<SpriteRenderer>();
            sr.color = tainted ? new Color(1, 0, 0) : new Color(1, 1, 1);

            changeTainted = false;
        }
    }
}
