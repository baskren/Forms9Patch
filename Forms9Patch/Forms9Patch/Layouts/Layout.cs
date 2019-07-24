using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;


namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch.Layout
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ContentProperty(nameof(Children))]
    public abstract class Layout<T> : BaseLayout<T>, Xamarin.Forms.IViewContainer<View> where T : Xamarin.Forms.Layout<View>, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected Layout()
        {
            // 190723 was getting double set_Parent in Android.
            //_xfLayout.ChildAdded += (sender, e) => OnChildAdded(e.Element);
            //_xfLayout.ChildRemoved += (sender, e) => OnChildRemoved(e.Element);
        }

        /// <summary>
        /// Called when added
        /// </summary>
        /// <param name="view"></param>
        protected virtual void OnAdded(T view)
        {
        }

        /// <summary>
        /// Called when child is added
        /// </summary>
        /// <param name="child"></param>
        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            //var typedChild = child as T;
            //if (typedChild != null)
            if (child is T typedChild)
                OnAdded(typedChild);
        }

        /// <summary>
        /// Called when child is removed
        /// </summary>
        /// <param name="child"></param>
        protected override void OnChildRemoved(Element child)
        {
            base.OnChildRemoved(child);

            //var typedChild = child as T;
            //if (typedChild != null)
            if (child is T typedChild)
                OnRemoved(typedChild);
        }

        /// <summary>
        /// Called when element is removed
        /// </summary>
        /// <param name="view"></param>
        protected virtual void OnRemoved(T view)
        {
        }

    }
    /// <summary>
    /// Forms9Patch.BaseLayout
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class BaseLayout<T> : View<T>, ILayout, Xamarin.Forms.ILayout, Xamarin.Forms.ILayoutController where T : Xamarin.Forms.Layout<View>, new()
    {
        // Frame already correctly handles IsClippedToBounds, Padding, ForceLayout, GetSizeRequest,
        /// <summary>
        /// Children of Forms9Patch.BaseLayout
        /// </summary>
        public new IList<View> Children => _xfLayout.Children;

        /// <summary>
        /// Triggered when layout has changed
        /// </summary>
        public new event EventHandler LayoutChanged
        {
            add => _xfLayout.LayoutChanged += value;
            remove => _xfLayout.LayoutChanged -= value;
        }

        /// <summary>
        /// lowers child in visual stack
        /// </summary>
        /// <param name="view"></param>
        public new void LowerChild(View view) => _xfLayout.LowerChild(view);

        /// <summary>
        /// Raises child in visual stack
        /// </summary>
        /// <param name="view"></param>
        public new void RaiseChild(View view) => _xfLayout.RaiseChild(view);
    }

    /// <summary>
    /// Forms9Patch.View
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class View<T> : VisualElement<T>, Xamarin.Forms.IViewController where T : Xamarin.Forms.Layout<View>, new()
    {
        // Handled by frame:
        // - VerticalOptions, HorizontalOptions
        // - Margin
        // - GestureRecognizers
    }

    /// <summary>
    /// Forms9Patch.VisualElement
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class VisualElement<T> : Element<T> where T : Xamarin.Forms.Layout<View>, Xamarin.Forms.IAnimatable, Xamarin.Forms.IVisualElementController, new()
    {
        // Handled by frame:
        // - Navigation
        // - InputTransparent
        // - IsEnabled
        // - X, Y, AnchorX, AnchorY, TranslationX, TranslationY, Width, Height, Rotation, RotationX, RotationY
        // - Scale
        // - IsVisible
        // - Opacity
        // - BackgroundColor
        // - Behaviors ??
        // - Triggers ??
        // - Style ??
        // - WidthRequest, HeightRequest, MinimumWidthRequest, MinimumHeightRequest
        // - IsFocused
        // - Bounds
        // - InputTransparent

        /// <summary>
        /// Triggered when children are reordered
        /// </summary>
        public new event EventHandler ChildrenReordered
        {
            add => _xfLayout.ChildrenReordered += value;
            remove => _xfLayout.ChildrenReordered -= value;
        }

        /// <summary>
        /// Sets focus upon element
        /// </summary>
        public new void Focus() => _xfLayout.Focus();

        /// <summary>
        /// Triggered when element receives focus
        /// </summary>
        public new event EventHandler<FocusEventArgs> Focused
        {
            add { _xfLayout.Focused += value; }
            remove { _xfLayout.Focused -= value; }
        }

        /// <summary>
        /// Forces visual element to be unfocused
        /// </summary>
        public new void Unfocus()
        {
            base.Unfocus();
            _xfLayout.Unfocus();
        }

        /// <summary>
        /// Triggered when Unfocused
        /// </summary>
        public new event EventHandler<FocusEventArgs> Unfocused
        {
            add { _xfLayout.Unfocused += value; }
            remove { _xfLayout.Unfocused -= value; }
        }
    }

    /// <summary>
    /// Forms9Patch.Element object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class Element<T> : BindableObject<T>, Xamarin.Forms.IElementController where T : Xamarin.Forms.Layout<View>, new()
    {
        /// <summary>
        /// Obsolete Content Property
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("USE CHILDREN INSTEAD", true)]
        public new View Content
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        /// <summary>
        /// Forms9Patch.Element
        /// </summary>
        protected Element()
        {
            //base.Content = _xfLayout;
        }

        /// <summary>
        /// Triggered when child is added
        /// </summary>
        public new event EventHandler<ElementEventArgs> ChildAdded
        {
            add => _xfLayout.ChildAdded += value;
            remove => _xfLayout.ChildAdded -= value;
        }

        /// <summary>
        /// Triggered when Child is removed
        /// </summary>
        public new event EventHandler<ElementEventArgs> ChildRemoved
        {
            add => _xfLayout.ChildRemoved += value;
            remove => _xfLayout.ChildRemoved -= value;
        }

        /// <summary>
        /// Triggered when Descendent is added
        /// </summary>
        public new event EventHandler<ElementEventArgs> DescendantAdded
        {
            add => _xfLayout.DescendantAdded += value;
            remove => _xfLayout.DescendantAdded -= value;
        }

        /// <summary>
        /// Triggered when Descendant is Removed
        /// </summary>
        public new event EventHandler<ElementEventArgs> DescendantRemoved
        {
            add => _xfLayout.DescendantRemoved += value;
            remove => _xfLayout.DescendantRemoved -= value;
        }

        /// <summary>
        /// Descendants of layout
        /// </summary>
        /// <returns></returns>
        public new IEnumerable<Element> Descendants() => _xfLayout.Descendants();
    }

    /// <summary>
    /// Forms9Patch.BindableObject
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class BindableObject<T> : Frame where T : Xamarin.Forms.Layout<View>, new()
    {
        /// <summary>
        /// Base layout
        /// </summary>
        protected Xamarin.Forms.Layout<View> _xfLayout { get; private set; } = new T();

        /// <summary>
        /// Forms9Patch BindableObject
        /// </summary>
        protected BindableObject()
        {
            base.Content = _xfLayout;
            Padding = 0;
            Margin = 0;
        }

        /// <summary>
        /// called when property is about to be changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanging(propertyName));
                return;
            }

            base.OnPropertyChanging(propertyName);

            if (propertyName == BindingContextProperty.PropertyName)
                _xfLayout.BindingContext = null;
        }

        /// <summary>
        /// Called when property has changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (!P42.Utils.Environment.IsOnMainThread)
            {
                Device.BeginInvokeOnMainThread(() => OnPropertyChanged(propertyName));
                return;
            }

            base.OnPropertyChanged(propertyName);

            if (propertyName == BindingContextProperty.PropertyName)
                _xfLayout.BindingContext = BindingContext;
        }
    }
}