// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework._3D.Graphics;
using osu.Framework._3D.Rendering;
using osu.Framework.Graphics;
using osuTK;

namespace osu.Framework.Tests.Visual.Dimensional
{
    public partial class TestScene3D : FrameworkTestScene
    {
        private readonly Scene scene;

        public TestScene3D()
        {
            Child = scene = new Scene
            {
                RelativeSizeAxes = Axes.Both,
                InternalChild = new Drawable3D { Size = new Vector2(200) }
            };
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
