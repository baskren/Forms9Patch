// /*******************************************************************
//  *
//  * br.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
	[Xamarin.Forms.Internals.Preserve(AllMembers = true)]
	public class br : ContentPage
	{
		public br()
		{
            Padding = 20;
			Content = new StackLayout
			{
				Children = {
					new Forms9Patch.Label { HtmlText = "Hello<br><br>ContentPage", TextColor = Color.Black }
				}
			};
		}
	}
}

