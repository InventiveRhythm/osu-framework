// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Framework.Graphics.Rendering
{
    public enum FrontFace
    {
        Clockwise,
        CounterClockwise,
    }

    public enum FaceCullingMode
    {
        Back,
        Front,
        FrontAndBack, // idk why we would ever need this, but opengl supports it, so adding it there just in case
    }

    public readonly struct CullingInfo : IEquatable<CullingInfo>
    {
        public static CullingInfo Default = new CullingInfo(false);

        public readonly bool Enabled;

        public readonly FaceCullingMode Mode;

        public readonly FrontFace FrontFace;

        public CullingInfo(bool cullFace, FaceCullingMode mode = FaceCullingMode.Back, FrontFace frontFace = FrontFace.CounterClockwise)
        {
            Enabled = cullFace;
            Mode = mode;
            FrontFace = frontFace;
        }

        public bool Equals(CullingInfo other) => Enabled == other.Enabled && Mode == other.Mode && FrontFace == other.FrontFace;

        public override bool Equals(object? obj) => obj is CullingInfo other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Enabled, Mode, FrontFace);
    }
}
