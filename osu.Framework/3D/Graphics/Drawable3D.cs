// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace osu.Framework._3D.Graphics
{
    public partial class Drawable3D : CompositeDrawable
    {
        public Matrix4 Matrix { get; set; }

        #region Position

        private Vector3 position = Vector3.Zero;

        public new Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                InvalidateMatrix();
            }
        }

        public new float X
        {
            get => Position.X;
            set => Position = Position with { X = value };
        }

        public new float Y
        {
            get => Position.Y;
            set => Position = Position with { Y = value };
        }

        public float Z
        {
            get => Position.Z;
            set => Position = Position with { Z = value };
        }

        #endregion

        #region Scale

        private Vector3 scale = Vector3.One;

        public new Vector3 Scale
        {
            get => scale;
            set
            {
                scale = value;
                InvalidateMatrix();
            }
        }

        #endregion

        #region Rotation

        private Quaternion rotation = Quaternion.Identity;
        private Vector3? euler;

        public new Quaternion Rotation
        {
            get => rotation;
            set
            {
                if (rotation == value)
                    return;

                rotation = value;
                euler = null;
                InvalidateMatrix();
            }
        }

        public Vector3 EulerRotation
        {
            get => euler ??= rotation.ToEuler();
            set
            {
                if (euler == value)
                    return;

                euler = value;
                rotation = Quaternion.FromEulerAngles(value);
                InvalidateMatrix();
            }
        }

        #endregion

        protected void InvalidateMatrix() => Invalidate(Invalidation.DrawNode | Invalidation.DrawInfo);
    }
}
