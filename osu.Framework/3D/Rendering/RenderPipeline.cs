// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Diagnostics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Rendering.Vertices;
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
                projection = Source.Camera.GetProjectionMatrix(Source.DrawWidth, Source.DrawHeight);

                /*var proj = Matrix4.CreatePerspectiveFieldOfView(cam.Fov, size.X / size.Y, cam.NearPlaneDist, cam.FarPlaceDist);
                var flipZ = Matrix4.CreateScale(1, 1, -1);
                projection = proj * flipZ * view;

                if (projection.M11 == 0 && projection.M22 == 0 && projection.M33 == 0 && projection.M44 == 0)
                    projection = Matrix4.CreatePerspectiveFieldOfView(Source.Camera.Fov, size.X / size.Y, Source.Camera.NearPlaneDist, Source.Camera.FarPlaceDist);*/
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

            private IVertexBatch<TexturedVertex3D>? batch;

            internal void DrawInternal(IRenderer renderer)
            {
                Debug.Assert(buffer != null);

                renderer.PushDepthInfo(new DepthInfo(
                    depthTest: true,
                    writeDepth: false,
                    function: BufferTestFunction.Always
                ));

                renderer.PushViewport(new RectangleI(0, 0, (int)buffer.Size.X, (int)buffer.Size.Y));
                buffer.Bind();

                renderer.Clear(new ClearInfo(Colour4.Gray, depth: 1f));

                Source.shader.Bind();
                renderer.WhitePixel.Bind();

                batch ??= renderer.CreateLinearBatch<TexturedVertex3D>(3 * 64, 1, PrimitiveTopology.Triangles);

                renderer.PushProjectionMatrix(
                    Matrix4.Identity
                    * Matrix4.CreateFromQuaternion(Source.Camera.Rotation)
                );

                var s = Source.ScreenSpaceDrawQuad.Size;

                renderer.PushMaskingInfo(new MaskingInfo
                {
                    ScreenSpaceAABB = new RectangleI(0, 0, (int)s.X, (int)s.Y),
                    MaskingRect = new RectangleF(0, 0, s.X, s.Y),
                    ToMaskingSpace = Matrix3.Identity,
                    BlendRange = 1,
                    AlphaExponent = 1,
                    CornerExponent = 2f,
                }, true);

                batch.Add(new TexturedVertex3D { Position = new Vector3(0f, 0.5f, 0), TexturePosition = new Vector2(0, 0) });
                batch.Add(new TexturedVertex3D { Position = new Vector3(0.5f, -0.5f, 0), TexturePosition = new Vector2(0, 0) });
                batch.Add(new TexturedVertex3D { Position = new Vector3(-0.5f, -0.5f, 0), TexturePosition = new Vector2(0, 0) });

                batch.Add(new TexturedVertex3D { Position = new Vector3(-0.5f, -0.5f, -.1f), TexturePosition = new Vector2(0, 0) });
                batch.Add(new TexturedVertex3D { Position = new Vector3(0f, 0.5f, -.1f), TexturePosition = new Vector2(0, 0) });
                batch.Add(new TexturedVertex3D { Position = new Vector3(0.5f, -0.5f, -.1f), TexturePosition = new Vector2(0, 0) });

                batch.Draw();

                renderer.PopMaskingInfo();
                renderer.PopProjectionMatrix();

                Source.shader.Unbind();
                buffer.Unbind();

                renderer.PopViewport();
                renderer.PopDepthInfo();
            }

            protected override void Dispose(bool isDisposing)
            {
                buffer?.Dispose();
                buffer = null;

                base.Dispose(isDisposing);
            }

            List<DrawNode>? ICompositeDrawNode.Children { get; set; }
            bool ICompositeDrawNode.AddChildDrawNodes => false;
        }
    }
}
