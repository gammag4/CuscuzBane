using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using CuscuzBane.Behaviour.Inventory;
using CuscuzBane.Entities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CuscuzBane.InventorySystem
{
    public class PeixeiraItem : Item
    {
        private GameObject collectible;

        private float damage = 7;

        private float cooldown = 0.4f;
        private float lastHitTime;

        public PeixeiraItem(Vector3 position) : base(PeixeiraItemType.Instance)
        {
            Instantiate(position);
        }

        public PeixeiraItem(Player player) : base(PeixeiraItemType.Instance)
        {
            var emptyPos = player.Inventory.TryGetFirstEmptySlotPosition();

            if (emptyPos == null)
                Instantiate(player.Transform.position);

            if (!player.Inventory.TryAddItem(emptyPos.Value, this)) throw new System.Exception("Can't create gun item.");
        }

        private void Instantiate(Vector3 position)
        {
            collectible = Object.Instantiate(Utils.Peixeira, position, Quaternion.identity);
            var itemBehaviour = collectible.GetComponent<ItemBehaviour>();
            itemBehaviour.Item = this;
        }

        public override void OnGet()
        {
            Object.Destroy(collectible);
        }

        public override void OnDrop()
        {
            Instantiate(Utils.Player.Transform.position);
        }

        public override void OnSelect()
        {
            if (Utils.CooldownBar)
                Utils.CooldownBar.GetValue = () => lastHitTime / cooldown;

            base.OnSelect();
        }

        public override void OnDeselect()
        {
            if (Utils.CooldownBar)
                Utils.CooldownBar.GetValue = null;

            base.OnDeselect();
        }

        public override void SelectedUpdate()
        {
            lastHitTime += Time.deltaTime;

            if (lastHitTime < cooldown) return;

            if (Utils.Player.UsingItemLeft)
            {
                Utils.Player.IsClickedLeft = false;
                Utils.Player.Animator.SetTrigger("swordAttack");
                lastHitTime = 0;

                var results = new List<Collider2D>();
                Physics2D.OverlapCollider(Utils.Player.SwordHitbox, new ContactFilter2D().NoFilter(), results);

                foreach (var item in results)
                {
                    foreach (var entity in EntityManager.Entities)
                    {
                        if (item != entity.Hitbox || entity == Utils.Player) continue;
                        entity.DealDamage(damage, Utils.Player);
                    }
                }
            }
        }

        public override void Update()
        {
        }
    }
}
