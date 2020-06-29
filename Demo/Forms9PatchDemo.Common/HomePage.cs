using System;
using Xamarin.Forms;
using Forms9PatchDemo.Pages;
using Forms9PatchDemo.Pages.Code;

namespace Forms9PatchDemo
{
    class HomePage : Xamarin.Forms.ContentPage
    {
        public HomePage()
        {
            // Define command for the items in the TableView.

            ToolbarItems.Add(new ToolbarItem("", "five.png", () => System.Diagnostics.Debug.WriteLine("Tool Bar Item clicked")));

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
                        new TextCell {
                            Text = "Detailed Gesture Test Page",
                            Command = navigateCommand,
                            CommandParameter = typeof(VariableWidthButtonPage)
                        },
                        new TextCell {
                            Text = "Detailed Gesture Test Master Detail Page A",
                            Command = navigateCommand,
                            CommandParameter = typeof(VariableWidthButtonMasterDetailPageA)
                        },
                        new TextCell {
                            Text = "Detailed Gesture Test Master Detail Page B",
                            Command = navigateCommand,
                            CommandParameter = typeof(VariableWidthButtonMasterDetailPageB)
                        },
                        new TextCell {
                            Text = "Gestures Test Page",
                            Command = navigateCommand,
                            CommandParameter = typeof(GestureTestPage)
                        },

                    },

                    new TableSection("XAML") {

                        new TextCell {
                            Text = "GridPage",
                            Command = navigateCommand,
                            CommandParameter = typeof(GridPage)
                        },


                        new TextCell {
                            Text = "XamlCDATA",
                            Command = navigateCommand,
                            CommandParameter = typeof(XamlCDATA)
                        },

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

                        new TextCell
                        {
                            Text = "WebView Export",
                            Command = navigateCommand,
                            CommandParameter = typeof(WebViewExport)
                        },

                        new TextCell
                        {
                            Text = "HTML Export",
                            Command = navigateCommand,
                            CommandParameter = typeof(HtmlExport)
                        },

                        new TextCell {
                            Text = "Flyout Popup Demo",
                            Command = navigateCommand,
                            CommandParameter = typeof(FlyoutDemo)
                        },

                        new TextCell {
                            Text = "Picker in Popup",
                            Command = navigateCommand,
                            CommandParameter = typeof(PickerInPopup)
                        },

                        new TextCell {
                            Text = "ListView Sandbox",
                            Command = navigateCommand,
                            CommandParameter = typeof(ListViewSandbox)
                        },

                        new TextCell {
                            Text = "ClipboardTest",
                            Command = navigateCommand,
                            CommandParameter = typeof(ClipboardTest)
                        },

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
                            Text = "Layout CodePage",
                            Command = navigateCommand,
                            CommandParameter = typeof(LayoutCodePage)
                        },

                        new TextCell {
                            Text = "Button & Segment Alignments",
                            Command = navigateCommand,
                            CommandParameter = typeof(ButtonAndSegmentAlignments)
                        },

                        new TextCell
                        {
                            Text = "Button CodePage",
                            Command = navigateCommand,
                            CommandParameter = typeof(ButtonCodePage)
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
                            Text = "PopupsPage",
                            Command = navigateCommand,
                            CommandParameter = typeof(PopupsPage)
                        },

                        new TextCell {
                            Text = "SVG ButtonBackgroundImage",
                            Command = navigateCommand,
                            CommandParameter = typeof(SVG_ButtonBackgroundImage)
                        },

                        new TextCell {
                            Text = "Simple Label Autofit test",
                            Command = navigateCommand,
                            CommandParameter = typeof(SimpleLabelFitting)
                        },

                        new TextCell {
                            Text = "Keyboard Height",
                            Command = navigateCommand,
                            CommandParameter = typeof(KeyboardHeight)
                        },

                        new TextCell {
                            Text = "Svg In ListView Cell",
                            Command = navigateCommand,
                            CommandParameter = typeof(SvgInCell)
                        },

                    },

                    new TableSection("Single Examples")
                    {
                        new TextCell
                        {
                            Text = "Single Button",
                            Command = navigateCommand,
                            CommandParameter = typeof(SingleButton)
                        },

                        new TextCell
                        {
                            Text = "State Button",
                            Command = navigateCommand,
                            CommandParameter = typeof(StateButton)
                        },

                        new TextCell
                        {
                            Text = "Single SegmentedControl",
                            Command = navigateCommand,
                            CommandParameter = typeof(SingleSegmentedControl)
                        },

                        new TextCell
                        {
                            Text = "Html Link",
                            Command = navigateCommand,
                            CommandParameter = typeof(HtmlLink)
                        },

                        new TextCell {
                            Text = "HardwareKeyPage",
                            Command = navigateCommand,
                            CommandParameter = typeof(Forms9PatchDemo.HardwareKeyPage)
                        },


                        new TextCell {
                            Text = "EmbeddedResource Font Effect",
                            Command = navigateCommand,
                            CommandParameter = typeof(EmbeddedResourceFontEffectPage)
                        },

                        new TextCell {
                            Text = "External EmbeddedResource Image",
                            Command = navigateCommand,
                            CommandParameter = typeof(ExternalEmbeddedResourceImage)
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
                            Text = "FontSizeTest",
                            Command = navigateCommand,
                            CommandParameter = typeof(FontSizeTest)
                        },

                        new TextCell
                        {
                            Text = "Segment Crowding",
                            Command = navigateCommand,
                            CommandParameter = typeof(SegmentedCrowdingPage)
                        },

                        new TextCell {
                            Text = "SegmentSelectedBackgroundPage",
                            Command = navigateCommand,
                            CommandParameter = typeof(SegmentSelectedBackgroundPage)
                        },



                    }
                }
            };
        }
    }
}
