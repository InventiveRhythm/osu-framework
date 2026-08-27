using System;
using System.Collections.Generic;

namespace osu.Framework.Graphics.Containers
{
    /// <summary>
    /// This is vertical only
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class CullableFlowContainer<T> : Container<T> where T : Drawable
    {
        public ScrollContainer<Drawable>? ScrollContainer { get; set; }

        public float Spacing { get; set; } = 8;
        public float CullPadding { get; set; } = 250;

        public float ItemSize { get; set; } = 40;

        private readonly List<T> items = new();
        private readonly HashSet<T> filteredItems = new();
        public readonly HashSet<T> VisibleItems = new();

        public IEnumerable<T> Items => items;

        public CullableFlowContainer()
        {
            // TODO: Maybe make one that supports having a Direction prop in the future

            // do NOT use AutoSizeAxes = Axes.Y
            // we manually compute the height
            RelativeSizeAxes = Axes.X;
        }

        public void Sort(Comparison<T> comparison) => items.Sort(comparison);

        public void SetFiltered(T drawable, bool filtered)
        {
            if (filtered)
            {
                filteredItems.Add(drawable);

                if (VisibleItems.Contains(drawable))
                {
                    base.Remove(drawable, false);
                    VisibleItems.Remove(drawable);
                }
            }
            else
            {
                filteredItems.Remove(drawable);
            }
        }

        public override void Add(T drawable) => items.Add(drawable);

        public new void AddRange(IEnumerable<T> drawables) => items.AddRange(drawables);

        public override bool Remove(T drawable, bool disposeImmediately)
        {
            items.Remove(drawable);
            filteredItems.Remove(drawable);

            if (VisibleItems.Contains(drawable))
            {
                VisibleItems.Remove(drawable);
                return base.Remove(drawable, disposeImmediately);
            }

            if (disposeImmediately) drawable.Dispose();
            return true;
        }

        public override void Clear(bool disposeChildren)
        {
            if (disposeChildren)
            {
                foreach (var item in items)
                    item.Dispose();
            }

            items.Clear();
            filteredItems.Clear();
            VisibleItems.Clear();
            base.Clear(false);
        }

        private ScrollContainer<Drawable>? getScrollContainer()
        {
            if (ScrollContainer != null) return ScrollContainer;

            CompositeDrawable parent = Parent;

            while (parent != null)
            {
                if (parent is ScrollContainer<Drawable> scroll)
                    return ScrollContainer = scroll;

                parent = parent.Parent;
            }

            return null;
        }

        protected override void Update()
        {
            base.Update();

            var scroll = getScrollContainer();
            if (scroll == null) return;

            float current = (float)scroll.Current;
            float scrollHeight = scroll.DrawHeight;

            float pos = 0f;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (filteredItems.Contains(item))
                    continue;

                // ItemSize here is only a fallback
                float size = item.DrawHeight > 0 ? item.DrawHeight : (item.Height > 0 ? item.Height : ItemSize);

                item.Y = pos;
                pos += size + Spacing;

                bool inBounds = item.Y + size >= current - CullPadding &&
                                item.Y <= current + scrollHeight + CullPadding;

                if (inBounds)
                {
                    if (!VisibleItems.Contains(item))
                    {
                        base.Add(item);
                        VisibleItems.Add(item);
                    }
                }
                else
                {
                    if (VisibleItems.Contains(item))
                    {
                        base.Remove(item, false);
                        VisibleItems.Remove(item);
                    }
                }
            }

            if (pos > 0) pos -= Spacing;

            Height = pos;
        }
    }
}
