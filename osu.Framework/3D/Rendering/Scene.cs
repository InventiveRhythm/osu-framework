// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework._3D.Rendering
{
    public partial class Scene : CompositeDrawable
    {
        public RenderPipeline? Pipeline { get; private set; }

        public Camera Camera
        {
            get => camera ??= new Camera();
            set => camera = value;
        }

        private Camera? camera;
        private IShader blitShader = null!;

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders)
        {
            blitShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
        }

        protected sealed override DrawNode CreateDrawNode() => Pipeline = CreateRenderPipeline();
        protected virtual RenderPipeline CreateRenderPipeline() => new(this);
    }
}
