using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using CuscuzBane.Behaviour.Inventory;
using CuscuzBane.Entities;
using UnityEngine;

namespace CuscuzBane.InventorySystem
{
    public class BulletItem : Item
    {
        public GameObject collectible;

        private bool toDestroy;

        private int numBullets = 8;

        public BulletItem(Vector3 position) : base(BulletItemType.Instance)
        {
            Instantiate(position);
        }

        private void Instantiate(Vector3 position)
        {
            collectible = Object.Instantiate(Utils.BulletItem, position, Quaternion.identity);
            var itemBehaviour = collectible.GetComponent<ItemBehaviour>();
            itemBehaviour.Item = this;
        }

        public override void OnGet()
        {
            Object.Destroy(collectible);
        }

        public int TryGetBullets(int number)
        {
            if (Inventory == null) throw new System.InvalidOperationException("Trying to get bullets but is not in inventory");

            if (number < numBullets)
            {
                numBullets -= number;
                return number;
            }

            toDestroy = true;
            Inventory.TryDropItem(PositionInInventory);
            return numBullets;
        }

        public override void OnDrop()
        {
            if (toDestroy) return;
            Instantiate(Utils.Player.Transform.position);
        }

        public override void SelectedUpdate()
        {
        }

        public override void Update()
        {
        }
    }
}
