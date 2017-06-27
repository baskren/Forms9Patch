// /*******************************************************************
//  *
//  * ModalPopupOnMasterDetailPage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    public class ModalPopupOnMasterDetailPage : MasterDetailPage
    {
        public ModalPopupOnMasterDetailPage()
        {
            Master = new ContentPage
            {
                Content = new Label { Text = "Master", BackgroundColor = Color.Blue },
                BackgroundColor = Color.FromRgb(0, 0, 180),
                Title = "Master Page",
            };
            Detail = new ContentPage
            {
                Content = new Label { Text = "Detail", BackgroundColor = Color.White },
                BackgroundColor = Color.Gray,
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

