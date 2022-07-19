using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using System;
using UnityEngine;

namespace CuscuzBane.InventorySystem
{
    public class PeixeiraItemType : ItemType
    {
        public static PeixeiraItemType Instance { get; private set; }

        public PeixeiraItemType(Sprite sprite)
        {
            Sprite = sprite;
            Name = "Peixeira";

            Instance = this;
        }
    }
}
