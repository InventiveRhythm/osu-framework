// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;

namespace osu.Framework._3D.Rendering
{
    public partial class Scene
    {
        public class RenderPipeline : DrawNode
        {
            public RenderPipeline(IDrawable source)
                : base(source)
            {
            }
        }
    }
}
