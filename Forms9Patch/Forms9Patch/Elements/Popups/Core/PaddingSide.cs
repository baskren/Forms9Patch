using System;

namespace Forms9Patch.Elements.Popups.Core
{
    /// <summary>
    /// Determines side of popup to which padding applies
    /// </summary>
    [Flags]
    public enum PaddingSide
    {
        /// <summary>
        /// Left
        /// </summary>
        Left = 1,
        /// <summary>
        /// Top
        /// </summary>
        Top = 2,
        /// <summary>
        /// Right
        /// </summary>
        Right = 4,
        /// <summary>
        /// Bottom
        /// </summary>
        Bottom = 8,
        /// <summary>
        /// Horizontal
        /// </summary>
        Horizontal = Left | Right,
        /// <summary>
        /// Vertical
        /// </summary>
        Vertical = Top | Bottom,
        /// <summary>
        /// All sides
        /// </summary>
        All = Left | Top | Right | Bottom
    }
}
