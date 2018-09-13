using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static Xamarin.Forms.Grid;

namespace Forms9Patch
{
    /// <summary>
    /// Forms9Patch Grid layout.
    /// </summary>
    public class Grid : Layout<Xamarin.Forms.Grid>, IGridController
    {
        public static readonly BindableProperty RowProperty = Xamarin.Forms.Grid.RowProperty;

        public static readonly BindableProperty RowSpanProperty = Xamarin.Forms.Grid.RowSpanProperty;

        public static readonly BindableProperty ColumnProperty = Xamarin.Forms.Grid.ColumnProperty;

        public static readonly BindableProperty ColumnSpanProperty = Xamarin.Forms.Grid.ColumnSpanProperty;

        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create("RowSpacing", typeof(double), typeof(Grid), 6d,
                                                                                             propertyChanged: (bindable, oldValue, newValue) => ((Grid)bindable)._grid.SetValue(Xamarin.Forms.Grid.RowSpacingProperty, newValue));

        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create("ColumnSpacing", typeof(double), typeof(Grid), 6d,
                                                                                                propertyChanged: (bindable, oldValue, newValue) => ((Grid)bindable)._grid.SetValue(Xamarin.Forms.Grid.ColumnSpacingProperty, newValue));

        public static readonly BindableProperty ColumnDefinitionsProperty = BindableProperty.Create("ColumnDefinitions", typeof(ColumnDefinitionCollection), typeof(Grid), null,
            validateValue: (bindable, value) => value != null, propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((Grid)bindable)._grid.SetValue(Xamarin.Forms.Grid.ColumnDefinitionsProperty, newvalue);
            });

        public static readonly BindableProperty RowDefinitionsProperty = BindableProperty.Create("RowDefinitions", typeof(RowDefinitionCollection), typeof(Grid), null,
            validateValue: (bindable, value) => value != null, propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((Grid)bindable)._grid.SetValue(Xamarin.Forms.Grid.RowDefinitionsProperty, newvalue);
            });

        public new IGridList<View> Children => _grid.Children;

        public ColumnDefinitionCollection ColumnDefinitions
        {
            get => _grid.ColumnDefinitions;
            set => _grid.ColumnDefinitions = value;
        }

        public double ColumnSpacing
        {
            get => _grid.ColumnSpacing;
            set => _grid.ColumnSpacing = value;
        }

        public RowDefinitionCollection RowDefinitions
        {
            get => _grid.RowDefinitions;
            set => _grid.RowDefinitions = value;
        }

        public double RowSpacing
        {
            get => _grid.RowSpacing;
            set => _grid.RowSpacing = value;
        }

        public static int GetColumn(BindableObject bindable) => Xamarin.Forms.Grid.GetColumn(bindable);

        public static int GetColumnSpan(BindableObject bindable) => Xamarin.Forms.Grid.GetColumnSpan(bindable);

        public static int GetRow(BindableObject bindable) => Xamarin.Forms.Grid.GetRow(bindable);

        public static int GetRowSpan(BindableObject bindable) => Xamarin.Forms.Grid.GetRowSpan(bindable);

        public static void SetColumn(BindableObject bindable, int value) => Xamarin.Forms.Grid.SetColumn(bindable, value);

        public static void SetColumnSpan(BindableObject bindable, int value) => Xamarin.Forms.Grid.SetColumnSpan(bindable, value);

        public static void SetRow(BindableObject bindable, int value) => Xamarin.Forms.Grid.SetRow(bindable, value);

        public static void SetRowSpan(BindableObject bindable, int value) => Xamarin.Forms.Grid.SetRowSpan(bindable, value);

        public void InvalidateMeasureInernalNonVirtual(InvalidationTrigger trigger) => _grid.InvalidateMeasureInernalNonVirtual(trigger);

        readonly Xamarin.Forms.Grid _grid;

        public Grid()
        {
            _grid = _xfLayout as Xamarin.Forms.Grid;
        }

    }
}

