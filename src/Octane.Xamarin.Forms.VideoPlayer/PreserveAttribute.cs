using System;

namespace Octane.Xamarin.Forms.VideoPlayer
{
    /// <summary>
    /// Preserves a member during Xamarin linker passes.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public sealed class PreserveAttribute : Attribute
    {
        public bool AllMembers; // Keep all members
        public bool Conditional; // Keep member ONLY if type itself is kept
    }
}
