namespace CuscuzBane.Base.InventorySystem
{
    public class EmptyItem : Item
    {
        public EmptyItem() : base(EmptyItemType.Instance) { }

        public override void SelectedUpdate() { }
    }
}
