namespace CuscuzBane.Base.InventorySystem
{
    public class GenericItemType : ItemType
    {
        public static GenericItemType Instance { get; private set; } = new GenericItemType();

        public GenericItemType()
        {
            Sprite = null;
            Name = "Item Generico";

            Instance = this;
        }
    }
}
