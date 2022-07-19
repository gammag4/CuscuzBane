using UnityEngine;

namespace CuscuzBane.Base.InventorySystem
{
    public abstract class Item
    {
        public virtual string Name => Type.Name;

        public ItemType Type { get; }

        public virtual Sprite Sprite => Type.Sprite;

        public Inventory Inventory { get; set; }

        public Vector2Int PositionInInventory { get; set; }

        public bool InInventory => Inventory != null;

        public bool IsSelected => Inventory?.SelectedItem == this;

        public bool CanDrop { get; protected set; }

        public bool IsEmpty => ItemType.IsSameType(Type, EmptyItemType.Instance);

        public Item(ItemType type)
        {
            CanDrop = true;
            Type = type;
        }

        public virtual void OnGet() { }

        public virtual void OnDrop() { }

        public virtual void OnSelect() { }

        public virtual void OnDeselect() { }

        // Called before update if selected
        public abstract void SelectedUpdate();

        public virtual void Update() { }
    }
}
