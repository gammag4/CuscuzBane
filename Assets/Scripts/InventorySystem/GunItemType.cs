using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using System;
using UnityEngine;

namespace CuscuzBane.InventorySystem
{
    public class GunItemType : ItemType
    {
        public static GunItemType Instance { get; private set; }

        public GunItemType(Sprite sprite)
        {
            Sprite = sprite;
            Name = "Metralhadora que parece espingarda";

            Instance = this;
        }
    }
}
