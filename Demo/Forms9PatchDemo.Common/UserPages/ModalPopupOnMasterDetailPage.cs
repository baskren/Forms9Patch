// /*******************************************************************
//  *
//  * ModalPopupOnMasterDetailPage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

using Xamarin.Forms;
using P42.Utils;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class ModalPopupOnMasterDetailPage : MasterDetailPage
    {
        public ModalPopupOnMasterDetailPage()
        {
            var masterButton = new Forms9Patch.Button
            {
                Text = "Show Popup",
                TextColor = Color.White,
                Padding = 5,
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                OutlineRadius = 4,
                HasShadow = true,
            };
            masterButton.Clicked += (s, e) =>
            {
                Forms9Patch.TargetedToast.Create(masterButton, "POP-UP", "From Master Page");
            };
            Master = new ContentPage
            {
                Content = masterButton,
                BackgroundColor = Color.DarkGray,
                Title = "Master Page",
            };

            var detailButton = new Forms9Patch.Button
            {
                Text = "Return",
                TextColor = Color.White,
                Padding = 5,
                BackgroundColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                OutlineRadius = 4,
                HasShadow = true,
            };
            detailButton.Clicked += (s, e) =>
            {
                if (Navigation.ModalStack.Contains(this) || Navigation.ModalStack.Contains(Parent))
                    Navigation.PopModalAsync();
                else
                    Navigation.PopAsync();
            };
            Detail = new ContentPage
            {
                Content = detailButton,
                BackgroundColor = Color.LightGray,
                Title = "Detail Page",
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var modal = new Forms9Patch.ModalPopup()
            {
                Content = new Forms9Patch.StackLayout
                {
                    Children = {
                                        new Xamarin.Forms.Label {
                                            Text = "Hello Modal popup!",
                                            TextColor = Color.Black}
                                    },
                    BackgroundColor = Color.FromRgb(100, 100, 100),
                    Padding = 20,
                },
                OutlineRadius = 4,
                OutlineWidth = 1,
                OutlineColor = Color.Black,
                BackgroundColor = Color.Aqua,
                HasShadow = true,
                HeightRequest = 200,
                WidthRequest = 200,
            };
            modal.BindingContext = this;
            modal.IsVisible = true;
        }
    }
}

