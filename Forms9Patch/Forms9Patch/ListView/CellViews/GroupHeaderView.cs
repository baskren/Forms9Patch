using Xamarin.Forms;

namespace Forms9Patch
{
	/// <summary>
	/// FormsDragNDropListView Group header view.
	/// </summary>
	public class GroupHeaderView : Xamarin.Forms.ViewCell {

		/*
		public static readonly BindableProperty BackgroundColorProperty = VisualElement.BackgroundColorProperty;
		public Color BackgroundColor {
			get { return (Color)GetValue (BackgroundColorProperty); }
			set { SetValue (BackgroundColorProperty, value); }
		}*/

		/*
		/// <summary>
		/// Gets or sets the View representing the content of the Header ViewCell.
		/// </summary>
		/// <value></value>
		/// <remarks></remarks>
		public new View View {
			get { return _content.Children [1]; }
			set { _content.Children[1] = value; }
		}

		readonly ColorGradientBox _upperEdge = new ColorGradientBox {
			StartColor = Color.FromRgba(0,0,0,0),
			EndColor = Color.FromRgba(0,0,0,0.12),
			HeightRequest = 2,
			VerticalOptions = LayoutOptions.Start,
		};
		readonly ColorGradientBox _lowerEdge = new ColorGradientBox {
			StartColor = Color.FromRgba(0,0,0,0.24),
			EndColor = Color.FromRgba(0,0,0,0),
			HeightRequest = 5,
			VerticalOptions = LayoutOptions.End,
		};
		readonly StackLayout _content = new StackLayout {
			Orientation = StackOrientation.Vertical,
			Spacing = 0,
			BackgroundColor = Color.Transparent,
		};

		/// <summary>
		/// Initializes a new instance of the <see cref="FormsDragNDropListView.HeaderView"/> class.
		/// </summary>
		public GroupHeaderView() {
			var defaultView = new ContentView {
				BackgroundColor = Color.Transparent,
			};
			_content.Children.Add(_upperEdge);
			_content.Children.Add(defaultView);
			_content.Children.Add(_lowerEdge);
			base.View = _content;
		}
		*/

		/// <summary>
		/// Triggered by a changing in the binding context.
		/// </summary>
		protected override void OnBindingContextChanged ()
		{
			var group = BindingContext as Group;
			View.BindingContext = group?.Source;
			base.OnBindingContextChanged ();
		}

	}
}

