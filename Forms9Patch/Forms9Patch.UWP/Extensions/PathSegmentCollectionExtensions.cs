using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Forms9Patch.UWP
{
    static class PathFigureExtensions
    {
        static public void AddLineToPoint(this PathFigure pathFigure, double x, double y)
        {
            pathFigure.Segments.Add(new LineSegment{ Point = new Windows.Foundation.Point(x, y) });
        }

        static public void AddArcToPoint(this PathFigure pathFigure, double x, double y, double r, SweepDirection sweepDirection = SweepDirection.Clockwise, double rotationAngle = 90)
        {
            var arcSegment = new ArcSegment
            {
                Point = new Windows.Foundation.Point(x, y),
                RotationAngle = rotationAngle,
                Size = new Windows.Foundation.Size(r, r),
                IsLargeArc = rotationAngle > 180,
                SweepDirection = sweepDirection
            };
            pathFigure.Segments.Add(arcSegment);
        }

        static public void SetStartPoint(this PathFigure pathFigure, double x, double y)
        {
            pathFigure.StartPoint = new Windows.Foundation.Point(x, y);
        }
    }
}
