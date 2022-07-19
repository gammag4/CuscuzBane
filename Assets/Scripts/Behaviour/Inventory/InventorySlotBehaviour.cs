using Assets.Scripts.Behaviour;
using CuscuzBane.Base;
using CuscuzBane.Base.InventorySystem;
using CuscuzBane.InventorySystem;
using UnityEngine;
using UnityEngine.UI;

namespace CuscuzBane.Behaviour
{
    public class InventorySlotBehaviour : MonoBehaviour
    {
        public GameObject showSelectedItem;

        public Image slotItem;

        public bool Started { get; set; }

        public GameObject SelectedItemOutline { get; set; }

        public InventoryBehaviour ParentCanvas { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Vector2Int Position { get; set; }

        private Item lastItem;

        private bool selectChecked;

        private bool itemChanged;

        public Item Item => Utils.Player.Inventory.GetItem(Position);

        // Use this for initialization
        void Start()
        {
            slotItem.preserveAspect = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (!Started) return;

            //var rt = GetComponent<RectTransform>();
            //if (rt != null)
            //    rt.position = Input.mousePosition + new Vector3(-10, 10, 0);

            if (Item.IsSelected)
            {
                var rect = SelectedItemOutline.GetComponent<RectTransform>();
                var mainRect = GetComponent<RectTransform>();

                rect.localPosition = mainRect.localPosition;
            }

            itemChanged = lastItem != Item;
            lastItem = Item;

            ChangeOpacity();
            CheckSelected();
        }

        private void ChangeOpacity()
        {
            if (!itemChanged) return;

            if (Item.IsEmpty)
            {
                slotItem.color = new Color(0, 0, 0, 0);
            }
            else
            {
                slotItem.sprite = Item.Sprite;
                slotItem.color = new Color(1, 1, 1, 1);
            }
        }

        private void CheckSelected()
        {
            if (Item.IsSelected)
            {
                if (selectChecked && !itemChanged) return;

                selectChecked = true;

                var obj = Instantiate(showSelectedItem, Vector3.zero, Quaternion.identity);
                obj.GetComponentInChildren<Text>().text = Item.Name;
                var tr = obj.GetComponentInChildren<TransitionInOut>();
                tr.transitionSpeed = 2;
                tr.transitionTime0 = 1;
                tr.transitionTimeMiddle = 0;
                tr.maxTransitionTimeMiddle = 3f;

                if (Utils.CurrentShowSelectedItem != null) Destroy(Utils.CurrentShowSelectedItem);
                Utils.CurrentShowSelectedItem = obj;
            }
            else
            {
                selectChecked = false;
            }
        }
    }
}
