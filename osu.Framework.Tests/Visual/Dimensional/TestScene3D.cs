// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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

            AddSliderStep("Rot X", 0, Math.PI * 2, Math.PI / 2 + 0.4, v => setRotation((float)v, -1, -1));
            AddSliderStep("Rot Y", 0, Math.PI * 2, 0, v => setRotation(-1, (float)v, -1));
            AddSliderStep("Rot Z", 0, Math.PI * 2, 0, v => setRotation(-1, -1, (float)v));

            void setRotation(float x, float y, float z)
            {
                var rot = scene.Camera.EulerRotation;
                if (x >= 0) rot.X = x;
                if (y >= 0) rot.Y = y;
                if (z >= 0) rot.Z = z;
                scene.Camera.EulerRotation = rot;
            }
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}
