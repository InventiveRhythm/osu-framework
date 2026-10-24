// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Framework.Graphics.Transforms
{
    internal class DuringTransform<T> : Transform<bool, T>
        where T : class, ITransformable
    {
        private static ulong id;

        private readonly Action<T> action;

        public override string TargetMember { get; } = $"During_{System.Threading.Interlocked.Increment(ref id)}";

        public DuringTransform(Action<T> action)
        {
            this.action = action;

            StartValue = false;
            EndValue = true;
        }

        protected override void Apply(T d, double time) => action(d);

        protected override void ReadIntoStartValue(T d) { }
    }
}
