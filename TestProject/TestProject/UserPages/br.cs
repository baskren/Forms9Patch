// /*******************************************************************
//  *
//  * br.cs copyright 2017 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public class br : ContentPage
	{
		public br()
		{
			Content = new StackLayout
			{
				Children = {
					new Forms9Patch.Label { HtmlText = "Hello<br>ContentPage" }
				}
			};
		}
	}
}

