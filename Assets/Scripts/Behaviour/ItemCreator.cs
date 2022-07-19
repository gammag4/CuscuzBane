using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using CuscuzBane.InventorySystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class ItemCreator : MonoBehaviour
    {
        private int numItems = 200;

        private int startBullets = 4;

        public GameObject caldoCana;
        public GameObject cafe;
        public GameObject medKit;
        public GameObject cuscuz;
        public GameObject bullet;
        public GameObject peixeira;
        public GameObject gun;
        public GameObject peixeiraHand;
        public GameObject gunHand;
        public GameObject cap;
        public GameObject capHand;

        public Sprite caldoCanaSprite;
        public Sprite cafeSprite;
        public Sprite medKitSprite;
        public Sprite cuscuzSprite;
        public Sprite bulletSprite;
        public Sprite peixeiraSprite;
        public Sprite gunSprite;
        public Sprite capSprite;

        private bool initialized;

        private Vector3 itemPosition;

        // Use this for initialization
        void Update()
        {
            if (Utils.Player == null) return;
            if (initialized) return;

            initialized = true;

            Utils.MedKit = medKit;
            Utils.Cuscuz = cuscuz;
            Utils.BulletItem = bullet;
            Utils.Peixeira = peixeira;
            Utils.Gun = gun;
            Utils.Cap = cap;

            Utils.PeixeiraHand = peixeiraHand;
            Utils.GunHand = gunHand;
            Utils.CapHand = capHand;

            new BulletItemType(bulletSprite);
            new PeixeiraItemType(peixeiraSprite);
            new GunItemType(gunSprite);
            new CapItemType(capSprite);

            CreateItems();
            CreateInventory();
            CreateItemsSortedToEnd();
        }

        private void CreateItems()
        {
            List<float> probabilities = new List<float>() { 0.15f, 0.35f, 0.1f, 0.1f, 0.3f };
            var totalProbability = probabilities.Sum();

            List<System.Func<Item>> objects = new List<System.Func<Item>>()
            {
                () => new HealingItem("Cuscuz", cuscuzSprite, itemPosition, cuscuz, 40),
                () => new HealingItem("Medkit", medKitSprite, itemPosition, medKit, 20),
                () => new IncreaseStaminaItem("Caldo de cana", caldoCanaSprite, itemPosition, caldoCana, 1f),
                () => new IncreaseStaminaItem("Cafe", cafeSprite, itemPosition, cafe, 1.4f),
                () => new BulletItem(itemPosition)
            };

            for (int i = 0; i < numItems; i++)
            {
                var x = Random.Range(Utils.BorderDistance, Utils.MapRealSize - Utils.BorderDistance);
                var y = Random.Range(Utils.BorderDistance, Utils.MapRealSize - Utils.BorderDistance);

                itemPosition = new Vector3(x, y, 0);

                var creator = Utils.ChooseObject(objects, probabilities, totalProbability);
                creator();
            }
        }

        private void CreateInventory()
        {
            if (Utils.Inventory == null || Utils.ClearInventory)
            {
                Utils.ClearInventory = false;
                Utils.Inventory = new Base.InventorySystem.Inventory(Utils.Player, 10, 4);
            }
            else
            {
                Utils.Player.Inventory = Utils.Inventory;
                return;
            }

            new PeixeiraItem(Utils.Player);
            new GunItem(Utils.Player);
            new CapItem(Utils.Player);

            for (int i = 0; i < startBullets; i++)
            {
                var b = new BulletItem(Utils.Player.Transform.position);
                var pos = Utils.Player.Inventory.TryGetLastEmptySlotPosition();
                Utils.Player.Inventory.TryAddItem(pos.Value, b);
            }
        }

        private void CreateItemsSortedToEnd()
        {
            Utils.ItemsSortedToEnd = new List<ItemType>()
            {
                BulletItemType.Instance
            };
        }
    }
}
