using System;

namespace Forms9Patch.Elements.Popups.Core
{
    [Flags]
    public enum PaddingSide
    {
        Left = 1,
        Top = 2,
        Right = 4,
        Bottom = 8,
        All = Left | Top | Right | Bottom
    }
}
