// /*******************************************************************
//  *
//  * Burkhart.xaml.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;
using System.Collections.Generic;
using Forms9Patch;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
	public partial class Burkhart : ContentPage
	{
		public Burkhart()
		{
			InitializeComponent();
			SignInButton.DefaultState = new ImageButtonState() { Text = "Sign In", BackgroundColor = Color.FromHex("84849B"), FontColor = Color.Black };
			SignInButton.DisabledState = new ImageButtonState() { FontColor = Color.Gray };

		}
	}
}
