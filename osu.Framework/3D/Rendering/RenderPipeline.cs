// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osuTK;

namespace osu.Framework._3D.Rendering
{
    public partial class Scene
    {
        public class RenderPipeline : DrawNode, ICompositeDrawNode
        {
            protected new Scene Source => (Scene)base.Source;

            private Quad quad;
            private Vector2 size;
            private IShader blit = null!;
            private Matrix4 projection;

            private IFrameBuffer? buffer;

            public RenderPipeline(Scene source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                quad = Source.ScreenSpaceDrawQuad;
                size = quad.Size;
                blit = Source.blitShader;
                projection = Source.Camera.GetProjectionMatrix(size.X, size.Y);
            }

            protected sealed override void Draw(IRenderer renderer)
            {
                buffer ??= renderer.CreateFrameBuffer([RenderBufferFormat.D32S8]);
                buffer.Size = size;

                DrawInternal(renderer);
                base.Draw(renderer);

                blit.Bind();
                buffer.Texture.Bind();
                renderer.DrawQuad(buffer.Texture, quad, DrawColourInfo.Colour);
                blit.Unbind();
            }

            internal void DrawInternal(IRenderer renderer)
            {
                Debug.Assert(buffer != null);

                renderer.PushScissorState(false);
                renderer.PushMaskingInfo(new MaskingInfo
                {
                    ScreenSpaceAABB = new RectangleI(0, 0, (int)buffer.Size.X, (int)buffer.Size.Y),
                    MaskingRect = new RectangleF(0, 0, size.X, size.Y),
                    ToMaskingSpace = Matrix3.Identity,
                    BlendRange = 1,
                    AlphaExponent = 1
                }, true);
                renderer.PushViewport(new RectangleI(0, 0, (int)buffer.Size.X, (int)buffer.Size.Y));
                renderer.PushDepthInfo(new DepthInfo(function: BufferTestFunction.LessThan));

                buffer.Bind();
                renderer.Clear(new ClearInfo(depth: 1));
                renderer.PushProjectionMatrix(projection);

                renderer.DrawQuad(renderer.WhitePixel, quad, Colour4.White);

                buffer.Unbind();
                renderer.PopProjectionMatrix();
                renderer.PopDepthInfo();
                renderer.PopViewport();
                renderer.PopMaskingInfo();
                renderer.PopScissorState();
            }

            protected override void Dispose(bool isDisposing)
            {
                buffer?.Dispose();

                base.Dispose(isDisposing);
            }

            List<DrawNode>? ICompositeDrawNode.Children { get; set; }
            bool ICompositeDrawNode.AddChildDrawNodes => false;
        }
    }
}
