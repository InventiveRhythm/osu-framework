// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Platform;

namespace osu.Framework.Graphics.Cursor
{
    public partial class CursorTypeContainer : CursorEffectContainer<CursorTypeContainer, IHasCursorType>
    {
        [Resolved]
        private GameHost host { get; set; } = null!;

        public CursorType LastCursorType { get; private set; } = CursorType.Arrow;

        protected override void Update()
        {
            base.Update();

            var valid = FindTargets();
            var current = valid.FirstOrDefault(x => x.Cursor != CursorType.Ignore)?.Cursor ?? CursorType.Arrow;

            if (current == LastCursorType)
                return;

            host.Window.ChangeCursor(current);
            LastCursorType = current;
        }
    }
}
