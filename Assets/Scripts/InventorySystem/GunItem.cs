using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using CuscuzBane.Behaviour.Inventory;
using CuscuzBane.Entities;
using UnityEngine;

namespace CuscuzBane.InventorySystem
{
    public class GunItem : Item
    {
        private GameObject collectible;
        private GameObject handObject;

        public int NumBullets { get; private set; } = 8;
        public int MaxBullets { get; } = 8;

        private float cooldown = 0.2f;
        private float lastHitTime;

        private Vector3 gunOffset = new Vector3(0.32f, 0.03f, 0) * 0.7f;

        public GunItem(Vector3 position) : base(GunItemType.Instance)
        {
            Instantiate(position);
        }

        public GunItem(Player player) : base(GunItemType.Instance)
        {
            var emptyPos = player.Inventory.TryGetFirstEmptySlotPosition();

            if (emptyPos == null)
                Instantiate(player.Transform.position);

            if (!player.Inventory.TryAddItem(emptyPos.Value, this)) throw new System.Exception("Can't create gun item.");
        }

        private void Instantiate(Vector3 position)
        {
            collectible = Object.Instantiate(Utils.Gun, position, Quaternion.identity);
            var itemBehaviour = collectible.GetComponent<ItemBehaviour>();
            itemBehaviour.Item = this;
        }

        private void InstantiateHand()
        {
            if (handObject != null) return;

            handObject = Object.Instantiate(Utils.GunHand, Vector3.zero, Quaternion.identity);
            UpdateHandObjectPosition();
        }

        public override void OnGet()
        {
            Object.Destroy(collectible);
        }

        public override void OnDrop()
        {
            if (handObject != null) Object.Destroy(handObject);

            Instantiate(Utils.Player.Transform.position);
        }

        public override void OnSelect()
        {
            if (Utils.CooldownBar)
                Utils.CooldownBar.GetValue = () => lastHitTime / cooldown;

            InstantiateHand();

            base.OnSelect();
        }

        public override void OnDeselect()
        {
            if (Utils.CooldownBar)
                Utils.CooldownBar.GetValue = null;

            if (handObject != null) Object.Destroy(handObject);
            handObject = null;

            base.OnDeselect();
        }

        public override void SelectedUpdate()
        {
            UpdateHandObjectPosition();

            lastHitTime += Time.deltaTime;

            if (lastHitTime < cooldown) return;

            if (Utils.Player.Reloading && NumBullets != MaxBullets)
            {
                var item = Inventory.TryGetFirstOfType(BulletItemType.Instance);
                while (item != null)
                {
                    var bullet = (BulletItem)item;

                    var numBullets = MaxBullets - NumBullets;
                    var newBullets = bullet.TryGetBullets(numBullets);
                    NumBullets += newBullets;
                    if (NumBullets == MaxBullets) break;

                    item = Inventory.TryGetFirstOfType(BulletItemType.Instance);
                }
            }

            if (Utils.Player.UsingItemLeft && NumBullets > 0)
            {
                //Utils.Player.Animator.SetTrigger("shoot");
                lastHitTime = 0;
                NumBullets--;

                var playerPos = ComputePosition();
                var angle = ComputeRotationAngle();
                var bpos = playerPos + Quaternion.AngleAxis(angle, Vector3.forward) * gunOffset;
                var brot = Quaternion.AngleAxis(angle, Vector3.forward);

                Object.Instantiate(Utils.Bullet, bpos, brot);
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

        private void UpdateHandObjectPosition()
        {
            var angle = ComputeRotationAngle();
            handObject.transform.position = ComputePosition() + new Vector3(0, 0, Utils.Player.Transform.position.z - 0.00001f);
            handObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
