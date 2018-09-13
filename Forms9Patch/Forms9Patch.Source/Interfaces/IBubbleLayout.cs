
namespace Forms9Patch
{
    interface IBubbleLayout : ILayout
    {
        float PointerLength { get; set; }

        float PointerTipRadius { get; set; }

        float PointerAxialPosition { get; set; }

        PointerDirection PointerDirection { get; set; }

        float PointerCornerRadius { get; set; }
    }
}
