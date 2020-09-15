using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.ComponentModel;

namespace Forms9Patch
{

    /// <summary>
    /// Forms9Patch Grid layout.
    /// </summary>
    [Preserve (AllMembers = true)]
    [DesignTimeVisible(true)]
    public class Grid : Layout<Xamarin.Forms.Grid>, IGridController, IElementConfiguration<Xamarin.Forms.Grid>, IElementConfiguration<Grid>
    {
        /// <summary>
        /// Backing store for the row spacing property.
        /// </summary>
        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create(nameof(RowSpacing), typeof(double), typeof(Grid), 6d,
                                                                                             propertyChanged: (bindable, oldValue, newValue) => ((Grid)bindable)._grid.SetValue(Xamarin.Forms.Grid.RowSpacingProperty, newValue));

        /// <summary>
        /// Backing store for the column spacing property.
        /// </summary>
        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create(nameof(ColumnSpacing), typeof(double), typeof(Grid), 6d,
                                                                                                propertyChanged: (bindable, oldValue, newValue) => ((Grid)bindable)._grid.SetValue(Xamarin.Forms.Grid.ColumnSpacingProperty, newValue));

        /// <summary>
        /// Backing store for the column definitions property.
        /// </summary>
        public static readonly BindableProperty ColumnDefinitionsProperty = BindableProperty.Create(nameof(ColumnDefinitions), typeof(ColumnDefinitionCollection), typeof(Grid), null,
            validateValue: (bindable, value) => value != null, propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((Grid)bindable)._grid.SetValue(Xamarin.Forms.Grid.ColumnDefinitionsProperty, newvalue);
            }, defaultValueCreator: bindable =>
            {

                //var colDef = new ColumnDefinitionCollection();
                //colDef.ItemSizeChanged += ((Grid)bindable).OnDefinitionChanged;
                //return colDef;
                return ((Grid)bindable)._grid.GetValue(Xamarin.Forms.Grid.ColumnDefinitionsProperty);
            });

        /// <summary>
        /// Backing store for the row definitions property.
        /// </summary>
        public static readonly BindableProperty RowDefinitionsProperty = BindableProperty.Create(nameof(RowDefinitions), typeof(RowDefinitionCollection), typeof(Grid), null,
            validateValue: (bindable, value) => value != null, propertyChanged: (bindable, oldvalue, newvalue) =>
            {
                ((Grid)bindable)._grid.SetValue(Xamarin.Forms.Grid.RowDefinitionsProperty, newvalue);
            }, defaultValueCreator: bindable =>
            {
                //var rowDef = new RowDefinitionCollection();
                //rowDef.ItemSizeChanged += ((Grid)bindable).OnDefinitionChanged;
                //return rowDef;
                return ((Grid)bindable)._grid.GetValue(Xamarin.Forms.Grid.RowDefinitionsProperty);
            });

        /// <summary>
        /// Children of Grid
        /// </summary>
        public new Xamarin.Forms.Grid.IGridList<View> Children => _grid.Children;

        /// <summary>
        /// Gets or sets the column definitions.
        /// </summary>
        /// <value>The column definitions.</value>
        public ColumnDefinitionCollection ColumnDefinitions
        {
            get => _grid.ColumnDefinitions;
            set => _grid.ColumnDefinitions = value;
        }

        /// <summary>
        /// Gets or sets the column spacing.
        /// </summary>
        /// <value>The column spacing.</value>
        public double ColumnSpacing
        {
            get => _grid.ColumnSpacing;
            set => _grid.ColumnSpacing = value;
        }

        /// <summary>
        /// Gets or sets the row definitions.
        /// </summary>
        /// <value>The row definitions.</value>
        public RowDefinitionCollection RowDefinitions
        {
            get => _grid.RowDefinitions;
            set => _grid.RowDefinitions = value;
        }

        /// <summary>
        /// Gets or sets the row spacing.
        /// </summary>
        /// <value>The row spacing.</value>
        public double RowSpacing
        {
            get => _grid.RowSpacing;
            set => _grid.RowSpacing = value;
        }

        /// <summary>
        /// Gets the column of element in Grid
        /// </summary>
        /// <returns>The column.</returns>
        /// <param name="bindable">Bindable.</param>
        public static int GetColumn(BindableObject bindable) => Xamarin.Forms.Grid.GetColumn(bindable);

        /// <summary>
        /// Gets the column span of element in Grid
        /// </summary>
        /// <returns>The column span.</returns>
        /// <param name="bindable">Bindable.</param>
        public static int GetColumnSpan(BindableObject bindable) => Xamarin.Forms.Grid.GetColumnSpan(bindable);

        /// <summary>
        /// Gets the row of element in Grid
        /// </summary>
        /// <returns>The row.</returns>
        /// <param name="bindable">Bindable.</param>
        public static int GetRow(BindableObject bindable) => Xamarin.Forms.Grid.GetRow(bindable);

        /// <summary>
        /// Gets the row span of element in Grid
        /// </summary>
        /// <returns>The row span.</returns>
        /// <param name="bindable">Bindable.</param>
        public static int GetRowSpan(BindableObject bindable) => Xamarin.Forms.Grid.GetRowSpan(bindable);

        /// <summary>
        /// Sets the column of element in Grid
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="value">Value.</param>
        public static void SetColumn(BindableObject bindable, int value) => Xamarin.Forms.Grid.SetColumn(bindable, value);

        /// <summary>
        /// Sets the column span of element in Grid
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="value">Value.</param>
        public static void SetColumnSpan(BindableObject bindable, int value) => Xamarin.Forms.Grid.SetColumnSpan(bindable, value);

        /// <summary>
        /// Sets the row of element in Grid
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="value">Value.</param>
        public static void SetRow(BindableObject bindable, int value) => Xamarin.Forms.Grid.SetRow(bindable, value);

        /// <summary>
        /// Sets the row span of element in Grid
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="value">Value.</param>
        public static void SetRowSpan(BindableObject bindable, int value) => Xamarin.Forms.Grid.SetRowSpan(bindable, value);

        /// <summary>
        /// Invalidates the measure inernal non virtual.
        /// </summary>
        /// <param name="trigger">Trigger.</param>
        public void InvalidateMeasureInernalNonVirtual(InvalidationTrigger trigger) => _grid.InvalidateMeasureInernalNonVirtual(trigger);

        /// <summary>
        /// Marker interface for returning platform-specific configuration elements.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IPlatformElementConfiguration<T, Xamarin.Forms.Grid> On<T>() where T : IConfigPlatform
        {
            return ((IElementConfiguration<Xamarin.Forms.Grid>)_grid).On<T>();
        }

        IPlatformElementConfiguration<T, Grid> IElementConfiguration<Grid>.On<T>()
        {
            return ((IElementConfiguration<Forms9Patch.Grid>)_grid).On<T>();
            //return null;
        }



        Xamarin.Forms.Grid _grid => _xfLayout as Xamarin.Forms.Grid;

    }
}

