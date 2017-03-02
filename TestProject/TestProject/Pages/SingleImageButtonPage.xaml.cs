using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public partial class SingleImageButtonPage : ContentPage
	{
		public SingleImageButtonPage ()
		{
			InitializeComponent ();
		}

		protected override void OnDisappearing()
		{
			BindingContext = null;
			Content = null;
			base.OnDisappearing();
			GC.Collect();
		}
	}
}

