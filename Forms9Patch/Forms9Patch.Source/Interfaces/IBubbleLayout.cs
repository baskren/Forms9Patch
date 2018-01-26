
namespace Forms9Patch
{
    interface IBubbleLayout : ILayout
    {
        float PointerLength { get; set; }

        float PointerTipRadius { get; set; }

        float PointerAxialPostition { get; set; }

        PointerDirection PointerDirection { get; set; }

        float PointerCornerRadius { get; set; }
    }
}
