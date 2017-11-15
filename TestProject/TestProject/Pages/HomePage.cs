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
                            Text = "ContentView",
                            Command = navigateCommand,
                            CommandParameter = typeof(ContentViewDemoPage)
                        },

                        new TextCell {
                            Text = "Frame",
                            Command = navigateCommand,
                            CommandParameter = typeof(FrameDemoPage)
                        },

                        new TextCell {
                            Text = "Single ImageButton",
                            Command = navigateCommand,
                            CommandParameter = typeof(SingleImageButtonPage)
                        },

                        new TextCell {
                            Text = "ImageButton",
                            Command = navigateCommand,
                            CommandParameter = typeof(ImageButtonPage)
                        },

                        new TextCell {
                            Text = "MaterialSegmentControl ",
                            Command = navigateCommand,
                            CommandParameter = typeof(MaterialSegmentedControlPage)
                        },

                        new TextCell {
                            Text = "Image",
                            Command = navigateCommand,
                            CommandParameter = typeof(MyPage)
                        },

                        new TextCell {
                            Text = "HTML Labels and Buttons",
                            Command = navigateCommand,
                            CommandParameter = typeof(HtmlLabelsAndButtons)
                        },

                        new TextCell {
                            Text = "CapsInset Test",
                            Command = navigateCommand,
                            CommandParameter = typeof(CapsInsetPage)
                        },

                    },

                    new TableSection("Code") {

                        new TextCell
                        {
                            Text = "Single MaterialButton",
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
                            Text = "Material Button & Segment alignments",
                            Command = navigateCommand,
                            CommandParameter = typeof(MaterialButtonAndSegmentAlignments)
                        },

                        new TextCell {
                            Text = "ImageButton alignments",
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
                            CommandParameter = typeof(CustomFontEffectPage)
                        },

                        new TextCell {
                            Text = "External EmbeddedResource Image",
                            Command = navigateCommand,
                            CommandParameter = typeof(ExternalEmbeddedResourceImage)
                        },

                        new TextCell {
                            Text = "Label in Layout",
                            Command = navigateCommand,
                            CommandParameter = typeof(LabelInHorizontalStackLayout)
                        },


                        new TextCell {
                            Text = "Label Fit",
                            Command = navigateCommand,
                            CommandParameter = typeof(LabelFitPage)
                        },

                        new TextCell {
                            Text = "Unimposed Height Label Fit",
                            Command = navigateCommand,
                            CommandParameter = typeof(UnconstrainedLabelFitPage)
                        },

						/*
						new TextCell {
							Text = "Scale Font to Fit Bounds",
							Command = navigateCommand,
							CommandParameter = typeof(LabelScaleToFitPage)
						},

						new TextCell {
							Text = "Html Text, Scale Font to Fit Bounds",
							Command = navigateCommand,
							CommandParameter = typeof(HtmlLabelScaleToFitPage)
						},
*/

						new TextCell {
                            Text = "Simple Font Test",
                            Command = navigateCommand,
                            CommandParameter = typeof(SimpleFontTestPage)
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
                            Text = "ImageButton",
                            Command = navigateCommand,
                            CommandParameter = typeof(ImageButtonCodePage)
                        },

                        new TextCell {
                            Text = "Single ImageButton",
                            Command = navigateCommand,
                            CommandParameter = typeof(SingleImageButtonCodePage)
                        },


                        new TextCell {
                            Text = "Images",
                            Command = navigateCommand,
                            CommandParameter = typeof(ImageCodePage)
                        },

                        new TextCell {
                            Text = "Layouts",
                            Command = navigateCommand,
                            CommandParameter = typeof(LayoutsPage)
                        },

                        new TextCell
                        {
                            Text = "Image on page",
                            Command = navigateCommand,
                            CommandParameter = typeof(ImageOnPage)
                        },

                        new TextCell {
                            Text = "Material Buttons",
                            Command = navigateCommand,
                            CommandParameter = typeof(MaterialButtonsPage)
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

                    }
                }
            };
        }
    }
}
