namespace CuscuzBane.Base.InventorySystem
{
    public class EmptyItemType : ItemType
    {
        public static EmptyItemType Instance { get; private set; } = new EmptyItemType();

        public EmptyItemType()
        {
            Sprite = null;
            Name = "Slot Vazio";

            Instance = this;
        }
    }
}
