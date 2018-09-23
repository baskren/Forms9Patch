using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace Forms9Patch
{
    [ContentProperty("Children")]
    public abstract class Layout<T> : BaseLayout<T>, Xamarin.Forms.IViewContainer<View> where T : Xamarin.Forms.Layout<View>, new()
    {

        protected Layout()
        {
            _xfLayout.ChildAdded += (sender, e) => OnChildAdded(e.Element);
            _xfLayout.ChildRemoved += (sender, e) => OnChildRemoved(e.Element);
        }

        protected virtual void OnAdded(T view)
        {
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            var typedChild = child as T;
            if (typedChild != null)
                OnAdded(typedChild);
        }

        protected override void OnChildRemoved(Element child)
        {
            base.OnChildRemoved(child);

            var typedChild = child as T;
            if (typedChild != null)
                OnRemoved(typedChild);
        }

        protected virtual void OnRemoved(T view)
        {
        }

    }

    public abstract class BaseLayout<T> : View<T>, ILayout where T : Xamarin.Forms.Layout<View>, new()
    {
        // Frame already correctly handles IsClippedToBounds, Padding, ForceLayout, GetSizeRequest,
        public new IList<View> Children => _xfLayout.Children;

        /*
        ObservableCollection<Element> _internalChildren;
        internal ObservableCollection<Element> InternalChildren
        {
            get
            {
                _internalChildren = _internalChildren ?? (ObservableCollection<Element>)P42.Utils.ReflectionExtensions.GetPropertyValue(_xfLayout, "InternalChildren");
                return _internalChildren;
            }
        }
        */

        public new event EventHandler LayoutChanged
        {
            add => _xfLayout.LayoutChanged += value;
            remove => _xfLayout.LayoutChanged -= value;
        }

        public new void LowerChild(View view) => _xfLayout.LowerChild(view);

        public new void RaiseChild(View view) => _xfLayout.RaiseChild(view);
    }

    public abstract class View<T> : VisualElement<T> where T : Xamarin.Forms.Layout<View>, new()
    {
        // Handled by frame:
        // - VerticalOptions, HorizontalOptions
        // - Margin
        // - GestureRecognizers
    }

    public abstract class VisualElement<T> : Element<T> where T : Xamarin.Forms.Layout<View>, new()
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

        public new event EventHandler ChildrenReordered
        {
            add => _xfLayout.ChildrenReordered += value;
            remove => _xfLayout.ChildrenReordered -= value;
        }

        public new void Focus() => _xfLayout.Focus();

        public new event EventHandler<FocusEventArgs> Focused
        {
            add { _xfLayout.Focused += value; }
            remove { _xfLayout.Focused -= value; }
        }

        public new void Unfocus()
        {
            base.Unfocus();
            _xfLayout.Unfocus();
        }

        public new event EventHandler<FocusEventArgs> Unfocused
        {
            add { _xfLayout.Unfocused += value; }
            remove { _xfLayout.Unfocused -= value; }
        }


    }

    public abstract class Element<T> : BindableObject<T> where T : Xamarin.Forms.Layout<View>, new()
    {
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public new ReadOnlyCollection<Element> LogicalChildren => _xfLayout.LogicalChildren;

        //[EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("USE CHILDREN INSTEAD", true)]
        public new View Content
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }


        protected Element()
        {
            //base.Content = _xfLayout;
        }


        public new event EventHandler<ElementEventArgs> ChildAdded
        {
            add => _xfLayout.ChildAdded += value;
            remove => _xfLayout.ChildAdded -= value;
        }

        public new event EventHandler<ElementEventArgs> ChildRemoved
        {
            add => _xfLayout.ChildRemoved += value;
            remove => _xfLayout.ChildRemoved -= value;
        }

        public new event EventHandler<ElementEventArgs> DescendantAdded
        {
            add => _xfLayout.DescendantAdded += value;
            remove => _xfLayout.DescendantAdded -= value;
        }

        public new event EventHandler<ElementEventArgs> DescendantRemoved
        {
            add => _xfLayout.DescendantRemoved += value;
            remove => _xfLayout.DescendantRemoved -= value;
        }

        public new IEnumerable<Element> Descendants() => _xfLayout.Descendants();
    }

    public abstract class BindableObject<T> : Frame where T : Xamarin.Forms.Layout<View>, new()
    {

        // Bindings and PropertyChange handling will be managed by OnPropertyChanged;
        protected Xamarin.Forms.Layout<View> _xfLayout { get; private set; } = new T();

        protected BindableObject()
        {
            base.Content = _xfLayout;
            Padding = 0;
            Margin = 0;
        }

        /* this may already be coverd by Element.OnBindingContextChanged
        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == BindingContextProperty.PropertyName)
                _xfLayout.BindingContext = BindingContext;
        }
        */
    }
}