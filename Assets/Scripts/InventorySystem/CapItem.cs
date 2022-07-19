using Assets.Scripts.Behaviour.Entity;
using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using CuscuzBane.Behaviour.Inventory;
using CuscuzBane.Entities;
using UnityEngine;

namespace CuscuzBane.InventorySystem
{
    public class CapItem : Item
    {
        private GameObject collectible;
        private GameObject handObject;

        public int NumBullets { get; private set; } = 8;
        public int MaxBullets { get; } = 8;

        private float cooldown = 0.2f;
        private float lastHitTime;

        public CapItem(Vector3 position) : base(CapItemType.Instance)
        {
            Instantiate(position);
        }

        public CapItem(Player player) : base(CapItemType.Instance)
        {
            var emptyPos = player.Inventory.TryGetFirstEmptySlotPosition();

            if (emptyPos == null)
                Instantiate(player.Transform.position);

            if (!player.Inventory.TryAddItem(emptyPos.Value, this)) throw new System.Exception("Can't create cap item.");
        }

        private void Instantiate(Vector3 position)
        {
            collectible = Object.Instantiate(Utils.Cap, position, Quaternion.identity);
            var itemBehaviour = collectible.GetComponent<ItemBehaviour>();
            itemBehaviour.Item = this;
        }

        private void InstantiateHand()
        {
            if (handObject != null) return;

            handObject = Object.Instantiate(Utils.CapHand, ComputePosition(), Quaternion.AngleAxis(ComputeRotationAngle(), Vector3.forward));

            var cb = handObject.GetComponent<CapThrowableBehaviour>();
            cb.source = Utils.Player.GameObject;
            cb.OnDestroy = () =>
            {
                lastHitTime = 0;
                handObject = null;
            };
        }

        public override void OnGet()
        {
            Utils.Player.CapHead.SetActive(true);
            Object.Destroy(collectible);
        }

        public override void OnDrop()
        {
            Utils.Player.CapHead.SetActive(false);

            Instantiate(Utils.Player.Transform.position);

            if (handObject != null) Object.Destroy(handObject);
        }

        public override void OnSelect()
        {
            if (Utils.CooldownBar)
                Utils.CooldownBar.GetValue = () => lastHitTime / cooldown;

            base.OnSelect();
        }

        public override void OnDeselect()
        {
            if (Utils.CooldownBar)
                Utils.CooldownBar.GetValue = null;

            base.OnDeselect();
        }

        public override void SelectedUpdate()
        {
            lastHitTime += Time.deltaTime;
            if (lastHitTime < cooldown) return;

            if (handObject == null)
            {
                if (Utils.Player.UsingItemLeft)
                {
                    Utils.Player.CapHead.SetActive(false);
                    CanDrop = false;
                    InstantiateHand();
                }
                else
                {
                    Utils.Player.CapHead.SetActive(true);
                    CanDrop = true;
                }
            }
        }

        private Vector3 ComputePosition()
        {
            var playerPos = Utils.Player.BodyPos;
            playerPos -= new Vector3(0, 0, Utils.Player.BodyPos.z);
            return playerPos;
        }

        private float ComputeRotationAngle()
        {
            Vector3 playerPos = ComputePosition();

            var mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mpos -= new Vector3(0, 0, mpos.z);

            return Quaternion.FromToRotation(Vector3.right, mpos - playerPos).eulerAngles.z;
        }

        public override void Update()
        {

        }
    }
}
