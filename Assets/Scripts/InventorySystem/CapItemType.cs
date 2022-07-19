using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using System;
using UnityEngine;

namespace CuscuzBane.InventorySystem
{
    public class CapItemType : ItemType
    {
        public static CapItemType Instance { get; private set; }

        public CapItemType(Sprite sprite)
        {
            Sprite = sprite;
            Name = "Chapeu de cangaceiro";

            Instance = this;
        }
    }
}
