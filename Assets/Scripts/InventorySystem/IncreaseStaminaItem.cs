using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using CuscuzBane.Behaviour.Inventory;
using UnityEngine;

namespace CuscuzBane.InventorySystem
{
    public class IncreaseStaminaItem : Item
    {
        private GameObject original;

        private GameObject collectible;

        private float healAmount;

        private bool toDestroy;

        private Sprite sprite;
        public override Sprite Sprite => sprite;

        private string name;
        public override string Name => name;

        public IncreaseStaminaItem(string name, Sprite sprite, Vector3 position, GameObject original, float healAmount) : base(GenericItemType.Instance)
        {
            this.name = name;
            this.original = original;
            this.healAmount = healAmount;
            this.sprite = sprite;
            Instantiate(position);
        }

        private void Instantiate(Vector3 position)
        {
            collectible = Object.Instantiate(original, position, Quaternion.identity);
            var itemBehaviour = collectible.GetComponent<ItemBehaviour>();
            itemBehaviour.Item = this;
        }

        public override void OnGet()
        {
            Object.Destroy(collectible);
        }

        public override void OnDrop()
        {
            if (toDestroy) return;
            Instantiate(Utils.Player.Transform.position);
        }

        public override void SelectedUpdate()
        {
            if (Utils.Player.UsingItemRight)
            {
                Utils.Player.UsingItemRight = false;
                Utils.Player.HealStamina(healAmount);
                toDestroy = true;
                Inventory.TryDropItem(PositionInInventory);
            }
        }

        public override void Update()
        {
        }
    }
}
