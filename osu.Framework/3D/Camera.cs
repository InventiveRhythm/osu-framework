// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework._3D.Graphics;
using osuTK;

namespace osu.Framework._3D
{
    public partial class Camera : Drawable3D
    {
        public float Fov { get; set; } = MathF.PI / 2f;
        public float NearPlaneDist { get; set; } = 0.01f;
        public float FarPlaceDist { get; set; } = 1000f;

        public Camera()
        {
            Matrix = Matrix4.Identity;
        }

        public Matrix4 GetProjectionMatrix(float width, float height)
        {
            var view = Matrix == default ? Matrix4.Identity : Matrix.Inverted();
            var flipZ = Matrix4.CreateScale(1, 1, -1);

            var projection = Matrix4.CreatePerspectiveFieldOfView(Fov, width / height, NearPlaneDist, FarPlaceDist);
            return projection * flipZ * view;
        }
    }
}
