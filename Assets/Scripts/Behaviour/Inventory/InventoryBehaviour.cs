using Assets.Scripts.Behaviour;
using CuscuzBane.Base;
using CuscuzBane.Entities;
using UnityEngine;

namespace CuscuzBane.Behaviour
{
    public class InventoryBehaviour : MonoBehaviour
    {
        public GameObject itemSlot;

        public GameObject inventory;
        public GameObject hotbar;

        public GameObject selectedItemOutline;

        private GameObject[][] slots;

        private bool initialized;

        public int SlotSize => 60;
        public int HotbarGap => 15;

        public HealthBarBehaviour healthBar;
        public HealthBarBehaviour staminaBar;
        public HealthBarBehaviour cooldownBar;
        public HealthBarBehaviour kuskuzMohHealthBar;

        private bool lastInventoryState;

        // Use this for initialization
        void Start()
        {
            if (cooldownBar)
            {
                cooldownBar.HideWhenNull = true;
                cooldownBar.HideWhenFull = true;
                Utils.CooldownBar = cooldownBar;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Utils.Player?.Inventory == null) return;

            CheckInventoryOpen();
            UpdateBackgroundPositions();

            if (initialized) return;
            Init();
            initialized = true;
        }

        void CheckInventoryOpen()
        {
            if (lastInventoryState == Utils.Player.InventoryOpen) return;

            lastInventoryState = Utils.Player.InventoryOpen;

            if (Utils.Player.InventoryOpen)
            {
                selectedItemOutline.transform.SetParent(inventory.transform, true);
                for (int i = 0; i < Utils.Player.Inventory.Width; i++)
                {
                    slots[0][i].transform.SetParent(inventory.transform, true);
                }
            }
            else
            {
                selectedItemOutline.transform.SetParent(hotbar.transform, true);
                for (int i = 0; i < Utils.Player.Inventory.Width; i++)
                {
                    slots[0][i].transform.SetParent(hotbar.transform, true);
                }
            }
            inventory.SetActive(Utils.Player.InventoryOpen);
        }

        private void Init()
        {
            CreateItems();
            SetLifeStamina();

            lastInventoryState = !Utils.Player.InventoryOpen;
        }

        private void CreateItems()
        {
            slots = new GameObject[Utils.Player.Inventory.Height][];

            for (int i = 0; i < Utils.Player.Inventory.Height; i++)
            {
                slots[i] = new GameObject[Utils.Player.Inventory.Width];

                for (int j = 0; j < Utils.Player.Inventory.Width; j++)
                {
                    var origin = new Vector3(j * SlotSize + HotbarGap, -i * SlotSize - HotbarGap, 0);
                    if (i > 0) origin += new Vector3(0, -HotbarGap, 0);

                    var trans = i > 0 ? inventory.transform : hotbar.transform;

                    slots[i][j] = Instantiate(itemSlot, trans);

                    var rect = slots[i][j].GetComponent<RectTransform>();
                    rect.localPosition = origin;

                    var invb = slots[i][j].GetComponent<InventorySlotBehaviour>();
                    invb.Position = new Vector2Int(j, i);
                    invb.SelectedItemOutline = selectedItemOutline;
                    invb.ParentCanvas = this;
                    invb.Started = true;
                }
            }
        }

        private void UpdateBackgroundPositions()
        {
            var mainRect = GetComponent<RectTransform>();
            var pos = new Vector3(mainRect.rect.x + 20, mainRect.rect.y + mainRect.rect.height - 20, 0);

            var rect = inventory.GetComponent<RectTransform>();
            rect.localPosition = pos;

            rect = hotbar.GetComponent<RectTransform>();
            rect.localPosition = pos;
        }

        private void SetLifeStamina()
        {
            if (healthBar)
                healthBar.GetValue = () => Utils.Player.Health / Utils.Player.TotalHealth;
            if (staminaBar)
                staminaBar.GetValue = () => Utils.Player.Stamina / Utils.Player.TotalStamina;
            if (kuskuzMohHealthBar)
            {
                kuskuzMohHealthBar.GetValue = KuskuzMoh.GetTotalLife;
                kuskuzMohHealthBar.IsActive = () => Utils.KuskuzMohs.Count != 0;
            }
        }
    }
}
