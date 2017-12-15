using System;
using Xamarin.Forms;
using Forms9PatchDemo.Pages;

namespace Forms9PatchDemo
{
    class HomePage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();
            /*
			var modal = new Forms9Patch.ModalPopup
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
			*/
        }


        public HomePage()
        {
            // Define command for the items in the TableView.



            var navigateCommand =
                new Command<Type>(async (Type pageType) =>
                {
                    var page = (Page)Activator.CreateInstance(pageType);
                    await this.Navigation.PushAsync(page);
                    //await this.Navigation.PushModalAsync(page);  // PushModalAsync will cause popups not to work
                });

            this.Title = "Forms Gallery";
            this.Content = new TableView
            {
                Intent = TableIntent.Menu,

                Root = new TableRoot {
                    new TableSection("User Pages") {
                        new TextCell {
                            Text = "User Pages",
                            Command = navigateCommand,
                            CommandParameter = typeof(UserPagesHomePage)
                        },
                    },
                    new TableSection("XAML") {
                        new TextCell {
                            Text = "XamlContentViewDemoPage",
                            Command = navigateCommand,
                            CommandParameter = typeof(XamlContentViewDemoPage)
                        },

                        new TextCell {
                            Text = "XamlFrameDemoPage",
                            Command = navigateCommand,
                            CommandParameter = typeof(XamlFrameDemoPage)
                        },

                        new TextCell {
                            Text = "XamlSingleStateButton",
                            Command = navigateCommand,
                            CommandParameter = typeof(XamlSingleStateButtonPage)
                        },

                        new TextCell {
                            Text = "XamlStateButtonsPage",
                            Command = navigateCommand,
                            CommandParameter = typeof(XamlStateButtonsPage)
                        },

                        new TextCell {
                            Text = "XamlSegmentedControlPage ",
                            Command = navigateCommand,
                            CommandParameter = typeof(XamlSegmentedControlPage)
                        },

                        new TextCell {
                            Text = "XamlImagesPage",
                            Command = navigateCommand,
                            CommandParameter = typeof(XamlImagesPage)
                        },

                        new TextCell {
                            Text = "XamlHtmlLabelsAndButtonsPage",
                            Command = navigateCommand,
                            CommandParameter = typeof(XamlHtmlLabelsAndButtonsPage)
                        },

                        new TextCell {
                            Text = "XamlCapsInsetPage",
                            Command = navigateCommand,
                            CommandParameter = typeof(XamlCapsInsetPage)
                        },

                    },


                    new TableSection("Code") {


                        new TextCell {
                            Text = "LabelAutoFitPage",
                            Command = navigateCommand,
                            CommandParameter = typeof(LabelAutoFitPage)
                        },


                        new TextCell {
                            Text = "ImageCodePage",
                            Command = navigateCommand,
                            CommandParameter = typeof(ImageCodePage)
                        },

                        new TextCell {
                            Text = "LayoutCodePage",
                            Command = navigateCommand,
                            CommandParameter = typeof(LayoutCodePage)
                        },

                        new TextCell {
                            Text = "Button & Segment alignments",
                            Command = navigateCommand,
                            CommandParameter = typeof(ButtonAndSegmentAlignments)
                        },

                        new TextCell
                        {
                            Text = "ButtonCodePage",
                            Command = navigateCommand,
                            CommandParameter = typeof(ButtonCodePage)
                        },

                        new TextCell
                        {
                            Text = "Single Button",
                            Command = navigateCommand,
                            CommandParameter = typeof(SingleMaterialButton)
                        },

                        new TextCell
                        {
                            Text = "Single SegmentedControl",
                            Command = navigateCommand,
                            CommandParameter = typeof(SingleSegmentedController)
                        },

                        new TextCell
                        {
                            Text = "Html link example",
                            Command = navigateCommand,
                            CommandParameter = typeof(LabelLink)
                        },

                        new TextCell {
                            Text = "StateButton alignments",
                            Command = navigateCommand,
                            CommandParameter = typeof(ImageButtonAlignments)
                        },


                        new TextCell {
                            Text = "Font Size test",
                            Command = navigateCommand,
                            CommandParameter = typeof(FontSizeTest)
                        },


                        new TextCell {
                            Text = "Custom Font Effect",
                            Command = navigateCommand,
                            CommandParameter = typeof(EmbeddedResourceFontEffectPage)
                        },

                        new TextCell {
                            Text = "External EmbeddedResource Image",
                            Command = navigateCommand,
                            CommandParameter = typeof(ExternalEmbeddedResourceImage)
                        },

                        new TextCell {
                            Text = "HTML Formatted Labels",
                            Command = navigateCommand,
                            CommandParameter = typeof(HtmlLabelPage)
                        },

                        new TextCell {
                            Text = "HTML Formatted Buttons",
                            Command = navigateCommand,
                            CommandParameter = typeof(HtmlButtonsPage)
                        },

                        new TextCell {
                            Text = "StateButton",
                            Command = navigateCommand,
                            CommandParameter = typeof(OldImageButtonCodePage)
                        },

                        new TextCell {
                            Text = "Single StateButton",
                            Command = navigateCommand,
                            CommandParameter = typeof(SingleImageButtonCodePage)
                        },

                        new TextCell {
                            Text = "Modal Popup",
                            Command = navigateCommand,
                            CommandParameter = typeof(ModalPopupTestPage)
                        },

                        new TextCell {
                            Text = "Bubble Popup",
                            Command = navigateCommand,
                            CommandParameter = typeof(BubblePopupTestPage)
                        },

                        new TextCell {
                            Text = "Pan Gesture test",
                            Command = navigateCommand,
                            CommandParameter = typeof(PanGesturePage)
                        },

                    },

                    new TableSection("Single Examples")
                    {

                    }
                }
            };
        }
    }
}
