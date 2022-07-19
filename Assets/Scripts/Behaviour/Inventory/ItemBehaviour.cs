using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using System.Collections.Generic;
using UnityEngine;

namespace CuscuzBane.Behaviour.Inventory
{
    public class ItemBehaviour : MonoBehaviour
    {
        public Item Item { get; set; }

        private BoxCollider2D boxCollider;

        private float dropCooldown;
        private float maxDropCooldown = 2;

        public bool CanPick { get; private set; }

        // Use this for initialization
        void Start()
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            dropCooldown += Time.deltaTime;

            if (Utils.Player?.Inventory == null) return;

            CanPick = dropCooldown >= maxDropCooldown && !Utils.Player.Inventory.IsFull;

            if (!CanPick || Item == null) return;

            var results = new List<Collider2D>();
            Physics2D.OverlapCollider(boxCollider, new ContactFilter2D().NoFilter(), results);

            if (!results.Contains(Utils.Player.Hitbox)) return;

            var emptyPos = Utils.Player.Inventory.TryGetFirstEmptySlotPosition();
            if (emptyPos == null) return;

            Utils.Player.Inventory.TryAddItem(emptyPos.Value, Item);
        }
    }
}
