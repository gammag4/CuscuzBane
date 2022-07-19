using System;
using System.Collections.Generic;
using UnityEngine;

namespace CuscuzBane.Base.InventorySystem
{
    public abstract class ItemType
    {
        public string Name { get; set; }

        public Sprite Sprite { get; set; }

        public static bool IsSameType(ItemType type1, ItemType type2)
        {
            if (type1 == null || type2 == null) return false;
            return type1.GetType() == type2.GetType();
        }

        public static bool IsSameType(Item item, ItemType type) => IsSameType(item?.Type, type);

        public static bool IsSameType(Item item1, Item item2) => IsSameType(item1?.Type, item2?.Type);

        public static bool IsOneOfType(ItemType type, List<ItemType> typeList)
        {
            if (type == null || typeList == null) return false;

            for (int i = 0; i < typeList.Count; i++)
            {
                if (type.GetType() == typeList[i].GetType()) return true;
            }

            return false;
        }

        public static bool IsOneOfType(Item item, List<ItemType> typeList) => IsOneOfType(item.Type, typeList);
    }
}
