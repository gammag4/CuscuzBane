using CuscuzBane.Entities;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace CuscuzBane.Base.InventorySystem
{
    public class Inventory
    {
        public Player Player { get; set; }

        public bool IsFull => TryGetFirstEmptySlotPosition() == null;

        public int Width { get; }
        public int Height { get; }

        public Vector2Int Bounds { get; }

        public Vector2Int LastPos { get; }

        public int SelectedItemIndex { get; private set; }

        public Item SelectedItem => Hotbar[SelectedItemIndex];

        private Item[] Hotbar => items[0];

        private Item[][] items;

        public Inventory(Player player, int width, int height)
        {
            Player = player;
            player.Inventory = this;
            Width = width;
            Height = height;
            Bounds = new Vector2Int(Width, Height);
            LastPos = Bounds + new Vector2Int(-1, -1);

            items = new Item[Height][];

            for (int i = 0; i < Height; i++)
            {
                items[i] = new Item[Width];

                for (int j = 0; j < Width; j++)
                    SetItem(j, i, new EmptyItem());
            }

            SelectedItem.OnSelect();
        }

        public int ComputeOffsetFromPosition(Vector2Int pos) => pos.y * Width + pos.x;

        public int ComputeLastOffsetFromPosition(Vector2Int pos) => Width * Height - ComputeOffsetFromPosition(pos) - 1;

        public Vector2Int ComputePositionFromOffset(int offset) => new Vector2Int(offset % Width, offset / Width);

        public Vector2Int ComputePositionFromLastOffset(int offset) => ComputePositionFromOffset(Width * Height - offset - 1);

        public bool IsEmpty(int width, int height) => GetItem(width, height).IsEmpty;

        public bool IsEmpty(Vector2Int position) => IsEmpty(position.x, position.y);

        public void Select(int index)
        {
            if (index == SelectedItemIndex) return;

            if (index < 0 || index > Width)
                throw new ArgumentOutOfRangeException("index", index, $"Index must be between 0 and {Width}.");

            SelectedItem.OnDeselect();
            SelectedItemIndex = index;
            SelectedItem.OnSelect();
        }

        public void RollSelection(int delta)
        {
            Select((Width + (SelectedItemIndex + delta) % Width) % Width);
        }

        public void SelectNext()
        {
            Select((SelectedItemIndex + 1) % Width);
        }

        public void SelectPrevious()
        {
            Select((Width + SelectedItemIndex - 1) % Width);
        }

        public Item GetHotbarItem(int index) => Hotbar[index];

        public Item GetItem(int width, int height) => items[height][width];

        public Item GetItem(Vector2Int position) => GetItem(position.x, position.y);

        public void SwitchItems(Vector2Int pos, Vector2Int newPos)
        {
            if (pos == newPos) return;

            var oldItem = GetItem(pos);
            var newItem = GetItem(newPos);
            SetItemUnsafe(pos.x, pos.y, newItem);
            SetItemUnsafe(newPos.x, newPos.y, oldItem);

            if (oldItem.IsSelected)
            {
                newItem.OnDeselect();
                oldItem.OnSelect();
            }
            if (newItem.IsSelected)
            {
                oldItem.OnDeselect();
                newItem.OnSelect();
            }
        }

        private void SetItem(int width, int height, Item item)
        {
            if (GetItem(width, height) == item) return;

            SetItemUnsafe(width, height, item);
            item.OnGet();
            if (item.IsSelected)
                item.OnSelect();
        }

        private void SetItemUnsafe(int width, int height, Item item)
        {
            items[height][width] = item;
            item.Inventory = this;
            item.PositionInInventory = new Vector2Int(width, height);
        }

        public bool TryAddItem(int width, int height, Item item)
        {
            if (item == null || GetItem(width, height) == item) return false;
            if (!TryRemoveItem(width, height)) return false;

            SetItem(width, height, item);
            return true;
        }

        public bool TryAddItem(Vector2Int position, Item item) => TryAddItem(position.x, position.y, item);

        public bool TryDropItem(int width, int height)
        {
            if (GetItem(width, height).IsEmpty) return true;

            if (!TryRemoveItem(width, height)) return false;

            var item = new EmptyItem();
            items[height][width] = item;
            item.Inventory = this;
            item.PositionInInventory = new Vector2Int(width, height);
            return true;
        }

        public bool TryDropItem(Vector2Int position) => TryDropItem(position.x, position.y);

        public bool TryDropSelected() => TryDropItem(SelectedItemIndex, 0);

        private bool TryRemoveItem(int width, int height)
        {
            if (GetItem(width, height).IsEmpty) return true;

            var item = items[height][width];

            if (item.IsEmpty) return true;
            if (!item.CanDrop) return false;

            item.OnDrop();
            item.Inventory = null;
            item.PositionInInventory = new Vector2Int(-1, -1);
            items[height][width] = null;

            return true;
        }

        public void IterateFirst(Action<Vector2Int> action, Vector2Int startPos)
        {
            for (int i = startPos.y; i < Height; i++)
            {
                for (int j = startPos.x; j < Width; j++)
                {
                    action(new Vector2Int(j, i));
                }
            }
        }

        public void IterateFirst(Action<Vector2Int> action) => IterateFirst(action, Vector2Int.zero);

        public void IterateLast(Action<Vector2Int> action, Vector2Int startPos)
        {
            for (int i = startPos.y; i >= 0; i--)
            {
                for (int j = startPos.x; j >= 0; j--)
                {
                    action(new Vector2Int(j, i));
                }
            }
        }

        public void IterateLast(Action<Vector2Int> action) => IterateLast(action, Vector2Int.zero);

        // Returns the position of the first item where condition will be true.
        public Vector2Int? TryGetFirstPositionCondition(Predicate<Vector2Int> predicate, Vector2Int startPos)
        {
            for (int i = startPos.y; i < Height; i++)
            {
                for (int j = startPos.x; j < Width; j++)
                {
                    if (!predicate(new Vector2Int(j, i))) continue;

                    return new Vector2Int(j, i);
                }
            }

            return null;
        }

        public Vector2Int? TryGetFirstPositionCondition(Predicate<Vector2Int> predicate) => TryGetFirstPositionCondition(predicate, Vector2Int.zero);

        // Returns the position of the navigationStack item where condition will be true.
        public Vector2Int? TryGetLastPositionCondition(Predicate<Vector2Int> predicate, Vector2Int startPos)
        {
            for (int i = startPos.y; i >= 0; i--)
            {
                for (int j = startPos.x; j >= 0; j--)
                {
                    if (!predicate(new Vector2Int(j, i))) continue;

                    return new Vector2Int(j, i);
                }
            }

            return null;
        }

        public Vector2Int? TryGetLastPositionCondition(Predicate<Vector2Int> predicate) => TryGetLastPositionCondition(predicate, new Vector2Int(Width - 1, Height - 1));

        // Returns the first item where condition will be true.
        public Item TryGetFirstItemCondition(Predicate<Vector2Int> predicate, Vector2Int startPos)
        {
            var pos = TryGetFirstPositionCondition(predicate, startPos);
            return pos == null ? null : GetItem(pos.Value);
        }

        public Item TryGetFirstItemCondition(Predicate<Vector2Int> predicate) => TryGetFirstItemCondition(predicate, Vector2Int.zero);

        // Returns the navigationStack item where condition will be true.
        public Item TryGetLastItemCondition(Predicate<Vector2Int> predicate, Vector2Int startPos)
        {
            var pos = TryGetLastPositionCondition(predicate, startPos);
            return pos == null ? null : GetItem(pos.Value);
        }

        public Item TryGetLastItemCondition(Predicate<Vector2Int> predicate) => TryGetLastItemCondition(predicate, new Vector2Int(Width - 1, Height - 1));

        // Returns the position of the first empty slot (height, width) or null if inventory is full.
        public Vector2Int? TryGetFirstEmptySlotPosition() => TryGetFirstPositionCondition((pos) => IsEmpty(pos));

        // Returns the position of the navigationStack empty slot (height, width) or null if inventory is full.
        public Vector2Int? TryGetLastEmptySlotPosition() => TryGetLastPositionCondition((pos) => IsEmpty(pos));

        // Returns the position of the first empty slot (height, width) or null if inventory is full.
        public Vector2Int? TryGetFirstEmptySlotPosition(Vector2Int startPos) => TryGetFirstPositionCondition((pos) => IsEmpty(pos), startPos);

        // Returns the position of the navigationStack empty slot (height, width) or null if inventory is full.
        public Vector2Int? TryGetLastEmptySlotPosition(Vector2Int startPos) => TryGetLastPositionCondition((pos) => IsEmpty(pos), startPos);

        // Returns the first item with the same type (height, width) or null if it can't find one.
        public Item TryGetFirstOfType(ItemType type) => TryGetFirstItemCondition((pos) => ItemType.IsSameType(GetItem(pos), type));

        // Returns the navigationStack item with the same type (height, width) or null if it can't find one.
        public Item TryGetLastOfType(ItemType type) => TryGetLastItemCondition((pos) => ItemType.IsSameType(GetItem(pos), type));

        // Returns the first item with the same type (height, width) or null if it can't find one (starts in startPos and goes right).
        public Item TryGetFirstNonEmpty(Vector2Int startPos) => TryGetFirstItemCondition((pos) => !GetItem(pos).IsEmpty, startPos);

        // Returns the navigationStack item with the same type (height, width) or null if it can't find one (starts in startPos and goes left).
        public Item TryGetLastNonEmpty(Vector2Int startPos) => TryGetLastItemCondition((pos) => !GetItem(pos).IsEmpty, startPos);

        public void Update()
        {
            SelectedItem.SelectedUpdate();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    items[i][j].Update();
                }
            }
        }

        public void Roll()
        {
            Item[] temp = items[0];
            for (int i = 0; i < Height - 1; i++)
            {
                items[i] = items[(i + 1) % Height];
            }

            items[Height - 1] = temp;
        }
    }
}
