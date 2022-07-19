using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using CuscuzBane.InventorySystem;
using UnityEngine;
using UnityEngine.UI;

namespace CuscuzBane.Behaviour
{
    public class BulletCountBehaviour : MonoBehaviour
    {
        public Text text;

        // Update is called once per frame
        void Update()
        {
            if (Utils.Player?.Inventory == null) return;

            var cg = GetComponent<CanvasGroup>();

            var item = Utils.Player.Inventory.SelectedItem;

            if (!ItemType.IsSameType(item, GunItemType.Instance))
            {
                cg.alpha = 0;
                return;
            }

            var gun = (GunItem)item;

            cg.alpha = 1;
            text.text = gun?.NumBullets.ToString();
        }
    }
}
