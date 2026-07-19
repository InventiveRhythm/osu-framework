// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Framework.Graphics
{
    /// <summary>
    /// Determines how the source color is adjusted based on its alpha channel before the blend equations.
    /// </summary>
    public enum BlendColourMode
    {
        /// <summary>
        /// Inherits from parent.
        /// </summary>
        Inherit = 0,

        /// <summary>
        /// No adjustment is applied. The source color is used as‑is.
        /// </summary>
        None = 1,

        /// <summary>
        /// Treats the source color as if it were composited over a solid white background,
        /// so fully transparent areas become white.
        /// </summary>
        NeutralWhite = 2,

        /// <summary>
        /// Treats the source color as if it were composited over a solid black background,
        /// so fully transparent areas become black.
        /// </summary>
        NeutralBlack = 3,

        /// <summary>
        /// Identical to <see cref="NeutralBlack"/>,
        /// Only used as marker for premultiplied textures for now.
        /// </summary>
        Premultiply = 4
    }
}
