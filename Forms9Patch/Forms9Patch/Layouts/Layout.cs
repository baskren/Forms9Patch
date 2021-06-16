using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;


namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch.Layout
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Preserve(AllMembers = true)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    [ContentProperty(nameof(Children))]
    public abstract class Layout<T> : BaseLayout<T>, Xamarin.Forms.IViewContainer<View> where T : Xamarin.Forms.Layout<View>, new()
    {

        /// <summary>
        /// Called when added
        /// </summary>
        /// <param name="view"></param>
        protected virtual void OnAdded(T view) { }

        /// <summary>
        /// Called when child is added
        /// </summary>
        /// <param name="child"></param>
        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
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
            if (child is T typedChild)
                OnRemoved(typedChild);
        }

        /// <summary>
        /// Called when element is removed
        /// </summary>
        /// <param name="view"></param>
        protected virtual void OnRemoved(T view) { }

    }
    /// <summary>
    /// Forms9Patch.BaseLayout
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class BaseLayout<T> : View<T>, ILayout, Xamarin.Forms.ILayout, Xamarin.Forms.ILayoutController where T : Xamarin.Forms.Layout<View>, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected BaseLayout()
        {
            _xfLayout.LayoutChanged += OnXfLayout_LayoutChanged;
        }

        /// <summary>
        /// Disposer
        /// </summary>
        bool _disposed;
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                _layoutChanged = null;
            }
            base.Dispose(disposing);
        }

        private void OnXfLayout_LayoutChanged(object sender, EventArgs e)
        => _layoutChanged?.Invoke(sender, e);

        // Frame already correctly handles IsClippedToBounds, Padding, ForceLayout, GetSizeRequest,
        /// <summary>
        /// Children of Forms9Patch.BaseLayout
        /// </summary>
        public new IList<View> Children => _xfLayout.Children;


        event EventHandler _layoutChanged;
        /// <summary>
        /// Triggered when layout has changed
        /// </summary>
        public new event EventHandler LayoutChanged
        {
            add => _layoutChanged += value;
            remove => _layoutChanged -= value;
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
        /// Internal use only
        /// </summary>
        protected VisualElement()
        {
            _xfLayout.ChildrenReordered += OnXfLayout_ChildrenReordered;
            _xfLayout.Focused += OnXfLayout_Focused;
            _xfLayout.Unfocused += OnXfLayout_Unfocused;
        }


        bool _disposed;
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _disposed = true;
                _xfLayout.ChildrenReordered -= OnXfLayout_ChildrenReordered;
                _xfLayout.Focused -= OnXfLayout_Focused;
                _xfLayout.Unfocused -= OnXfLayout_Unfocused;
            }
            base.Dispose(disposing);
        }

        private void OnXfLayout_Unfocused(object sender, FocusEventArgs e)
            => _unfocused?.Invoke(sender, e);

        private void OnXfLayout_Focused(object sender, FocusEventArgs e)
            => _focused?.Invoke(sender, e);

        private void OnXfLayout_ChildrenReordered(object sender, EventArgs e)
            => _childrenReordered?.Invoke(sender, e);

        event EventHandler _childrenReordered;
        /// <summary>
        /// Triggered when children are reordered
        /// </summary>
        public new event EventHandler ChildrenReordered
        {
            add => _childrenReordered += value;
            remove => _childrenReordered -= value;
        }

        /// <summary>
        /// Sets focus upon element
        /// </summary>
        public new void Focus() => _xfLayout.Focus();

        event EventHandler<FocusEventArgs> _focused;
        /// <summary>
        /// Triggered when element receives focus
        /// </summary>
        public new event EventHandler<FocusEventArgs> Focused
        {
            add { _focused += value; }
            remove { _focused -= value; }
        }

        /// <summary>
        /// Forces visual element to be unfocused
        /// </summary>
        public new void Unfocus()
        {
            base.Unfocus();
            _xfLayout.Unfocus();
        }

        event EventHandler<FocusEventArgs> _unfocused;

        /// <summary>
        /// Triggered when Unfocused
        /// </summary>
        public new event EventHandler<FocusEventArgs> Unfocused
        {
            add { _unfocused += value; }
            remove { _unfocused -= value; }
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
        /// Forms9Patch.Element
        /// </summary>
        protected Element()
        {
            //base.Content = _xfLayout;
            _xfLayout.ChildAdded += OnXfLayout_ChildAdded;
            _xfLayout.ChildRemoved += OnXfLayout_ChildRemoved;
            _xfLayout.DescendantAdded += OnXfLayout_DescendantAdded;
            _xfLayout.DescendantRemoved += OnXfLayout_DescendantRemoved;
        }

        bool _disposed;
        /// <summary>
        /// Disposed the layout and its children
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            // calling base.Dispose now so that PropertyChanged is deactivated.
            base.Dispose(disposing);
            if (!_disposed && disposing)
            {
                _disposed = true;

                _xfLayout.ChildAdded -= OnXfLayout_ChildAdded;
                _xfLayout.ChildRemoved -= OnXfLayout_ChildRemoved;
                _xfLayout.DescendantAdded -= OnXfLayout_DescendantAdded;
                _xfLayout.DescendantRemoved -= OnXfLayout_DescendantRemoved;

                _childAdded = null;
                _childRemoved = null;
                _descendantAdded = null;
                _descendantRemoved = null;

                if (_xfLayout is IDisposable disposableLayout)
                    disposableLayout.Dispose();
                else if (_xfLayout is Xamarin.Forms.Layout<View> layout)
                {
                    var children = layout.Children.ToArray();
                    layout.Children.Clear();
                    foreach (var child in children)
                        if (child is IDisposable disposable)
                            disposable.Dispose();
                }
            }
        }
        private void OnXfLayout_DescendantRemoved(object s, ElementEventArgs e)
            => _descendantRemoved?.Invoke(s, e);

        private void OnXfLayout_DescendantAdded(object s, ElementEventArgs e)
            => _descendantAdded?.Invoke(s, e);

        private void OnXfLayout_ChildRemoved(object s, ElementEventArgs e)
            => _childRemoved?.Invoke(s, e);

        private void OnXfLayout_ChildAdded(object s, ElementEventArgs e)
            => _childAdded?.Invoke(s, e);

        event EventHandler<ElementEventArgs> _childAdded;
        /// <summary>
        /// Triggered when child is added
        /// </summary>
        public new event EventHandler<ElementEventArgs> ChildAdded
        {
            add => _childAdded += value;
            remove => _childAdded -= value;
        }

        event EventHandler<ElementEventArgs> _childRemoved;
        /// <summary>
        /// Triggered when Child is removed
        /// </summary>
        public new event EventHandler<ElementEventArgs> ChildRemoved
        {
            add => _childRemoved += value;
            remove => _childRemoved -= value;
        }

        event EventHandler<ElementEventArgs> _descendantAdded;
        /// <summary>
        /// Triggered when Descendent is added
        /// </summary>
        public new event EventHandler<ElementEventArgs> DescendantAdded
        {
            add => _descendantAdded += value;
            remove => _descendantAdded -= value;
        }

        event EventHandler<ElementEventArgs> _descendantRemoved;
        /// <summary>
        /// Triggered when Descendant is Removed
        /// </summary>
        public new event EventHandler<ElementEventArgs> DescendantRemoved
        {
            add => _descendantRemoved += value;
            remove => _descendantRemoved -= value;
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
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                base.OnPropertyChanging(propertyName);

                if (propertyName == BindingContextProperty.PropertyName)
                    _xfLayout.BindingContext = null;
            });
        }

        /// <summary>
        /// Called when property has changed
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                base.OnPropertyChanged(propertyName);

                if (propertyName == BindingContextProperty.PropertyName)
                    _xfLayout.BindingContext = BindingContext;
            });
        }
    }
}
