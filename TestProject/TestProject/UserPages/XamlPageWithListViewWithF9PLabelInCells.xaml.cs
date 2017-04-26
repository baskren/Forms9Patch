// /*******************************************************************
//  *
//  * XamlPageWithListViewWithF9PLabelInCells.xaml.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public partial class XamlPageWithListViewWithF9PLabelInCells : ContentPage
	{
		public XamlPageWithListViewWithF9PLabelInCells()
		{
			InitializeComponent();
			listView.ItemSelected += (sender, e) => System.Diagnostics.Debug.WriteLine("ListView item selected");
			listView.ItemTapped += (sender, e) => System.Diagnostics.Debug.WriteLine("ListView item tapped");
			listView.ItemsSource = new List<TestClass>
			{
				new TestClass { BackgroundColor = Color.Aqua, TextColor=Color.Blue, Content="1 There are <a href=\"cheese\">Apples 1</a> for all." },
				new TestClass { BackgroundColor = Color.Fuchsia, TextColor=Color.Black, Content="2 Cheese" },
				new TestClass { BackgroundColor = Color.Blue, TextColor=Color.White, Content="3 There were <a href=\"cheese\">Apples 2</a> for all." },
				new TestClass { BackgroundColor = Color.Gray, TextColor=Color.Black, Content="4 Mango" },

				new TestClass { BackgroundColor = Color.Green, TextColor=Color.White, Content="5 There be <a href=\"cheese\">Apples 3</a> for all." },
				new TestClass { BackgroundColor = Color.Lime, TextColor=Color.Black, Content="6 Socks" },
				new TestClass { BackgroundColor = Color.Maroon, TextColor=Color.White, Content="7 There not <a href=\"cheese\">Apples 4</a> for all." },
				new TestClass { BackgroundColor = Color.Navy, TextColor=Color.White, Content="8 Pennies" },
				new TestClass { BackgroundColor = Color.Olive, TextColor=Color.White, Content="9 There ham <a href=\"cheese\">Apples 5</a> for all." },
				new TestClass { BackgroundColor = Color.Orange, TextColor=Color.Black, Content="10 Cherries" },
				new TestClass { BackgroundColor = Color.Pink, TextColor=Color.Blue, Content="11 There small <a href=\"cheese\">Apples 6</a> for all." },
				new TestClass { BackgroundColor = Color.Purple, TextColor=Color.White, Content="12 Lemons" },
				new TestClass { BackgroundColor = Color.Red, TextColor=Color.Blue, Content="13 chicken <a href=\"cheese\">Apples 7</a> for all." },
				new TestClass { BackgroundColor = Color.Silver, TextColor=Color.Black, Content="14 Zebras" },
				new TestClass { BackgroundColor = Color.Teal, TextColor=Color.Black, Content="15 Zipper <a href=\"cheese\">Apples 8</a> for all." },
				new TestClass { BackgroundColor = Color.White, TextColor=Color.Black, Content="16 Chips" },
				new TestClass { BackgroundColor = Color.Yellow, TextColor=Color.Black, Content="17 Be <a href=\"cheese\">Apples 9</a> for all." },
				new TestClass { BackgroundColor = Color.Black, TextColor=Color.White, Content="18 Salsa" },

			};
		}


	}

	public class TestClass
	{
		public Color BackgroundColor { get; set; }
		public ImageSource StatusIcon { get; set; }
		public string Content { get; set; }
		public Color TextColor { get; set; }

	}
}
