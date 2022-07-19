using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using CuscuzBane.Behaviour;
using CuscuzBane.Behaviour.Entity;
using CuscuzBane.Entities.Properties;
using CuscuzBane.InventorySystem;
using System.Collections.Generic;
using UnityEngine;

namespace CuscuzBane.Entities
{
    public class Player : Entity
    {
        public Inventory Inventory { get; set; }

        public Collider2D SwordHitbox { get; set; }

        public Vector3 PlayerBodyOffset { get; } = new Vector3(0, 0.05f, 0);

        public Vector3 PlayerCenterOffset { get; } = new Vector3(0, 0.08f, 0);

        public Vector3 BodyPos => Transform.position + PlayerBodyOffset;

        public Vector3 CenterPos => Transform.position + PlayerCenterOffset;

        public int Kills { get; set; }

        public Animator Animator { get; set; }

        public bool IsClickedLeft { get; set; }

        public bool IsClickedRight { get; set; }

        public bool UsingItemLeft { get; set; }

        public bool UsingItemRight { get; set; }

        public bool DroppingItem { get; set; }

        public bool InventoryOpen { get; set; }

        public bool Interacting { get; set; }

        public bool GoingRight { get; set; }

        public bool Attacking { get; set; }

        public bool Reloading { get; set; }

        public bool Dashing { get; set; }

        public bool RollingInventory { get; set; }

        public bool ResizingCamera { get; set; }

        public bool Escape { get; set; }

        public bool CloseToDie => Health / TotalHealth < 0.3f;

        public bool Dead { get; set; }

        public float Stamina { get; set; }

        public float TotalStamina { get; set; }

        public float StaminaDecreaseRate { get; set; }

        public float StaminaRecoverRate { get; set; }

        public GameObject CapHead { get; private set; }

        private TaintRedOnDamage taintRedOnDamage;

        public Player(GameObject gameObject) : base(gameObject)
        {
            Health = 100;
            TotalHealth = 100;
            Stamina = 4;
            TotalStamina = 4;
            StaminaDecreaseRate = 0.5f;
            StaminaRecoverRate = 0.3f;
        }

        public override void Init()
        {
            CapHead = GameObject.GetComponentInChildren<CapBehaviour>().gameObject;

            taintRedOnDamage = new TaintRedOnDamage(this);

            base.Init();
        }

        public override void Update()
        {
            CheckHealth();
            taintRedOnDamage.Update();
            base.Update();
            CheckInputs();
        }

        private void CheckInputs()
        {
            if (DroppingItem)
            {
                DroppingItem = false;
                Inventory?.TryDropSelected();
            }

            if (RollingInventory)
            {
                RollingInventory = false;
                SortInventory();
            }

            Inventory?.Update();

            UsingItemLeft = false;
            UsingItemRight = false;
            IsClickedLeft = false;
            IsClickedRight = false;
            DroppingItem = false;
            Reloading = false;
        }

        private void CheckHealth()
        {
            if (Health <= 0)
            {
                Dead = true;
                var displacement = GameObject.GetComponent<PlayerBehaviour>().hitbox.bounds.extents.y;
                Object.Instantiate(Utils.Poof, Transform.position + Vector3.up * displacement, Quaternion.identity);
                DisableRender();
                return;
            }
        }

        public void HealStamina(float value)
        {
            Stamina += value;
            if (Stamina > TotalStamina) Stamina = TotalStamina;
        }

        private void DisableRender()
        {
            foreach (var renderer in GameObject.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
        }

        private void SortInventory()
        {
            if (Inventory == null) return;

            List<Item> itemsFirst = new List<Item>();
            List<Item> itemsLast = new List<Item>();

            Inventory.IterateFirst((pos) =>
            {
                var item = Inventory.GetItem(pos);
                if (item == null) return;

                if (item.IsEmpty) return;

                if (ItemType.IsOneOfType(item, Utils.ItemsSortedToEnd))
                    itemsLast.Add(item);
                else
                    itemsFirst.Add(item);
            });

            for (int i = 0; i < itemsFirst.Count; i++)
            {
                Inventory.SwitchItems(itemsFirst[i].PositionInInventory, Inventory.ComputePositionFromOffset(i));
            }

            for (int i = 0; i < itemsLast.Count; i++)
            {
                Inventory.SwitchItems(itemsLast[i].PositionInInventory, Inventory.ComputePositionFromLastOffset(i));
            }
        }
    }
}
