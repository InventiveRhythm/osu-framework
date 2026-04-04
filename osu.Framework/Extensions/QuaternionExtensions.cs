// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osuTK;

namespace osu.Framework.Extensions
{
    public static class QuaternionExtensions
    {
        public static Vector3 ToEuler(this Quaternion q)
        {
            float xSquare = q.X * q.X;
            float ySquare = q.Y * q.Y;
            float zSquare = q.Z * q.Z;
            float wSquare = q.W * q.W;

            return new Vector3(
                MathF.Atan2(-2 * (q.Y * q.Z - q.W * q.X), wSquare - xSquare - ySquare + zSquare),
                MathF.Asin(2 * (q.X * q.Z + q.W * q.Y)),
                MathF.Atan2(-2 * (q.X * q.Y - q.W * q.Z), wSquare + xSquare - ySquare - zSquare)
            );
        }
    }
}
