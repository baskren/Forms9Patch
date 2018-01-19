using System;
using Xamarin.Forms;

namespace FormsGestures
{
    /// <summary>
    /// Interface for coordinate tranform complient
    /// </summary>
	public interface ICoordTransform
	{
        /// <summary>
        /// point transform
        /// </summary>
        /// <param name="fromElement"></param>
        /// <param name="p"></param>
        /// <param name="toElement"></param>
        /// <returns></returns>
		Point CoordTransform (VisualElement fromElement, Point p, VisualElement toElement);

        /// <summary>
        /// rectangle transform
        /// </summary>
        /// <param name="fromElement"></param>
        /// <param name="r"></param>
        /// <param name="toElement"></param>
        /// <returns></returns>
		Rectangle CoordTransform (VisualElement fromElement, Rectangle r, VisualElement toElement);
	}
}

