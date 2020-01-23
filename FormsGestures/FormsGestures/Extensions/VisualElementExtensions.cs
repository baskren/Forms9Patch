using System;
using System.Collections.Generic;
using Xamarin.Forms;
using P42.Utils;
using System.Collections;

namespace FormsGestures
{
    /// <summary>
    /// Xamarin.Forms.VisualElement extension methods
    /// </summary>
    public static class VisualElementExtensions
    {
        //public static readonly BindableProperty InterceptGesturesProperty = BindableProperty.Create("InterceptGestures",typeof(bool),typeof(Listener),false);


        static ICoordTransform _service;
        static ICoordTransform Service
        {
            get
            {
                if (_service == null)
                    _service = DependencyService.Get<ICoordTransform>();
                return _service;
            }
        }


        // THIS MAY NOT WORK WITH UWP .NET NATIVE COMPILER CHAIN
        /// <summary>
        /// Is this element a descendent of an ancestor element?
        /// </summary>
        /// <param name="child"></param>
        /// <param name="ancestor"></param>
        /// <returns></returns>
        public static bool IsDescendentOf(this Element child, Element ancestor)
        {
            //if (child == ancestor)
            //	return true;
            if (child.Parent == ancestor)
                return true;
            return child.Parent != null && child.Parent.IsDescendentOf(ancestor);
        }

        /// <summary>
        /// Is this element an ancestor or a descendent element?
        /// </summary>
        /// <param name="ancestor"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static bool IsAncestorOf(this Element ancestor, Element child)
            => child.IsDescendentOf(ancestor);
        

        /// <summary>
        /// Translates the bounds of an element to the coordinates of app's window
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Rectangle BoundsInWindowCoord(this VisualElement element)
            => Service.BoundsInWindowCoord(element);

        /// <summary>
        /// Translates the bounds of an element to the coordinates of another, reference element
        /// </summary>
        /// <param name="fromElement"></param>
        /// <param name="toElement"></param>
        /// <returns></returns>
        public static Rectangle BoundsInElementCoord(this VisualElement fromElement, VisualElement toElement)
            => Service.CoordTransform(fromElement, fromElement.Bounds, toElement);
        

        /// <summary>
        /// Translates the location of an element to the app's window's coordinates
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Point LocationInWindowCoord(this VisualElement element)
            => Service.PointInWindowCoord(element, Point.Zero);

        /// <summary>
        /// Returns a point (in a view) to the DIP Screen coordinates
        /// </summary>
        /// <param name="element"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point PointInWindowCoord(this VisualElement element, Point p)
            => Service.PointInWindowCoord(element, p);

        /// <summary>
        /// Translates the location of an element to the coordinates of another, reference element
        /// </summary>
        /// <param name="fromElement"></param>
        /// <param name="toElement"></param>
        /// <returns></returns>
        public static Point LocationInElementCoord(this VisualElement fromElement, VisualElement toElement)
            => Service.CoordTransform(fromElement, Point.Zero, toElement);

        /// <summary>
        /// Translates a point from its local view coordinates to that of another view
        /// </summary>
        /// <param name="fromElement"></param>
        /// <param name="p"></param>
        /// <param name="toElement"></param>
        /// <returns></returns>
        public static Point PointInElementCoord(this VisualElement fromElement, Point p, VisualElement toElement)
            => Service.CoordTransform(fromElement, p, toElement);


        /// <summary>
        /// determines if point in this element is withing the bounds of another, test element
        /// </summary>
        /// <param name="hitElement"></param>
        /// <param name="hitPoint"></param>
        /// <param name="targetElement"></param>
        /// <returns></returns>
        public static bool HitTest(this VisualElement hitElement, Point hitPoint, VisualElement targetElement)
        {
            var testPoint = CoordTransform(hitElement, hitPoint, targetElement);
            return targetElement.Bounds.Contains(testPoint);
        }

        /// <summary>
        /// translates a point from the coordinates of this element to that of another
        /// </summary>
        /// <param name="fromElement"></param>
        /// <param name="p"></param>
        /// <param name="toElement"></param>
        /// <returns></returns>
        public static Point CoordTransform(VisualElement fromElement, Point p, VisualElement toElement)
        {
            return Service.CoordTransform(fromElement, p, toElement);
        }
        /// <summary>
        /// translates a rectangle from the coordinates of this element to that of another
        /// </summary>
        /// <param name="fromElement"></param>
        /// <param name="r"></param>
        /// <param name="toElement"></param>
        /// <returns></returns>
        public static Rectangle CoordTransform(VisualElement fromElement, Rectangle r, VisualElement toElement)
        {
            return Service.CoordTransform(fromElement, r, toElement);
        }

        /// <summary>
        /// Gets or creates a FormsGestures.Listener for a Xamarin.Forms.VisualElement
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Listener GestureListener(this VisualElement element)
        {
            return Listener.For(element);
        }

        /// <summary>
        /// Enumerates all of the children of a parent element of a given type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentElement"></param>
        /// <param name="propertyName"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static List<T> FindChildrenWithPropertyAndOfType<T>(this VisualElement parentElement, string propertyName, List<T> result = null) where T : VisualElement
        {
            result = result ?? new List<T>();
            if (parentElement.GetPropertyValue("Children") is IEnumerable children)
            {
                foreach (var child in children)
                    if (child is VisualElement visualElement)
                        FindChildrenWithPropertyAndOfType<T>(visualElement, propertyName, result);
            }
            else
            {
                if (parentElement.GetPropertyValue("Content") is VisualElement content)
                    FindChildrenWithPropertyAndOfType<T>(content, propertyName, result);
            }
            if (parentElement is T && propertyName == null || parentElement.HasProperty(propertyName))
                result.Add(parentElement as T);
            return result;
        }

        /// <summary>
        /// Enumarates all the chilren of a VisualElement with a given property name
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static List<VisualElement> FindChildrenWithProperty(this VisualElement parentElement, string propertyName)
        {
            return FindChildrenWithPropertyAndOfType<VisualElement>(parentElement, propertyName);
        }

        /// <summary>
        /// Enumarates all the chilren of a VisualElement
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentElement"></param>
        /// <returns></returns>
        public static List<T> FindVisualElementsOfType<T>(this VisualElement parentElement) where T : VisualElement
        {
            return FindChildrenWithPropertyAndOfType<T>(parentElement, null);
        }

        /// <summary>
        /// Enumarates all VisualElements with a given type (T) and property name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static List<T> FindVisualElementsWithPropertyAndOfType<T>(string propertyName) where T : VisualElement
        {
            return FindChildrenWithPropertyAndOfType<T>(Xamarin.Forms.Application.Current?.MainPage, propertyName);
        }

        /// <summary>
        /// Enumarates all VisualElements with a given property name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static List<VisualElement> FindVisualElementsWithProperty(string propertyName)
        {
            return FindChildrenWithPropertyAndOfType<VisualElement>(Xamarin.Forms.Application.Current?.MainPage, propertyName);
        }


        /// <summary>
        /// Enumarates all VisualElements with a given type (T) 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> FindVisualElementsOfType<T>() where T : VisualElement
        {
            return FindChildrenWithPropertyAndOfType<T>(Xamarin.Forms.Application.Current?.MainPage, null);
        }

        /// <summary>
        /// Finds the currently focused VisualElement.
        /// </summary>
        /// <returns>The focused.</returns>
        public static VisualElement FindFocused()
        {
            return Xamarin.Forms.Application.Current?.MainPage?.FindChildWithFocus();
        }

        /// <summary>
        /// Finds the child VisualElement that is currently focused.
        /// </summary>
        /// <returns>The child with focus.</returns>
        /// <param name="element">Element.</param>
        public static VisualElement FindChildWithFocus(this Element element)
        {
            if (element is VisualElement visualElement)
            {
                if (visualElement.IsFocused)
                    return visualElement;
                if (visualElement is Xamarin.Forms.Layout layout)
                {
                    foreach (var child in layout.Children)
                    {
                        var focused = child.FindChildWithFocus();
                        if (focused != null)
                            return focused;
                    }
                }
            }
            if (element is Page page && page.IsFocused)
                return page;

            if (element is IPageController pageController)
            {
                foreach (var child in pageController.InternalChildren)
                {
                    var focused = child.FindChildWithFocus();
                    if (focused != null)
                        return focused;
                }
            }
            return null;
        }

        /// <summary>
        /// returns first ancestor element of given type
        /// </summary>
        /// <param name="element"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Element FindAncestorOfType(this Element element, Type type)
        {
            var parent = element.Parent;
            while (parent != null)
            {
                if (parent.GetType() == type)
                    return parent;
                parent = parent.Parent;
            }
            return null;
        }

        /// <summary>
        /// Is the element in the current visible view tree?
        /// </summary>
        /// <param name="visualElement"></param>
        /// <returns></returns>
        public static bool IsInVisibleViewTree(this VisualElement visualElement)
        {
            Element parent = visualElement;
            while (parent is Element element)
            {
                if (element is Page)
                    return true;
                if (element is VisualElement ve && !ve.IsVisible)
                    return false;
                parent = element.Parent;
            }
            return false;
        }
    }
}

