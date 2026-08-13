// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osu.Framework.Layout;

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
        private long updateVersion;

        private IShader shader = null!;
        private Texture texture = null!;

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaders, TextureStore textures)
        {
            blitShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
            shader = shaders.Load(VertexShaderDescriptor.TEXTURE_3, "Texture3D");

            texture = textures.Get("monokuma.jpg");
        }

        protected sealed override DrawNode CreateDrawNode() => Pipeline = CreateRenderPipeline();
        protected virtual RenderPipeline CreateRenderPipeline() => new RenderPipeline(this);

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();
            Invalidate(Invalidation.DrawNode);
        }

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {
            bool result = base.OnInvalidate(invalidation, source);

            if ((invalidation & Invalidation.DrawNode) <= 0) return result;

            updateVersion++;
            return true;
        }
    }
}
