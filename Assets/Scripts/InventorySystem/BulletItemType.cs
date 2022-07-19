using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using System;
using UnityEngine;

namespace CuscuzBane.InventorySystem
{
    public class BulletItemType : ItemType
    {
        public static BulletItemType Instance { get; private set; }

        public BulletItemType(Sprite sprite)
        {
            Sprite = sprite;
            Name = "Cartucho de Bala";

            Instance = this;
        }
    }
}
