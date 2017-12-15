using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Xamarin.Forms;
using System.Windows.Input;

namespace Forms9PatchDemo
{
    public class App : Application
    {

        ICommand _trueCommand = new Command((parameter) => System.Diagnostics.Debug.WriteLine("_simpleCommand Parameter[" + parameter + "]"), parameter => true);

        ICommand _falseCommand = new Command(parameter => System.Diagnostics.Debug.WriteLine("_commandB [" + parameter + "]"), parameter => false);


        void OnSegmentTapped(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Tapped Segment[" + e.Index + "] Text=[" + e.Segment.Text + "]");
        }

        void OnSegmentSelected(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Selected Segment[" + e.Index + "] Text=[" + e.Segment.Text + "]");
        }

        void OnSegmentLongPressing(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressing Segment[" + e.Index + "] Text=[" + e.Segment.Text + "]");
        }

        void OnSegmentLongPressed(object sender, Forms9Patch.SegmentedControlEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressed Segment[" + e.Index + "] Text=[" + e.Segment.Text + "]");
        }

        void OnMaterialButtonTapped(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Tapped Button Text=[" + ((Forms9Patch.Button)sender).Text + "]");
        }

        void OnMaterialButtonSelected(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Selected Button Text=[" + ((Forms9Patch.Button)sender).Text + "]");
        }

        void OnMaterialButtonLongPressing(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressing Button Text=[" + ((Forms9Patch.Button)sender).Text + "]");
        }

        void OnMaterialButtonLongPressed(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressed Button Text=[" + ((Forms9Patch.Button)sender).Text + "]");
        }

        void OnImageButtonTapped(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Tapped Button Text=[" + ((Forms9Patch.StateButton)sender).Text + "]");
        }

        void OnImageButtonSelected(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Selected Button Text=[" + ((Forms9Patch.StateButton)sender).Text + "]");
        }

        void OnImageButtonLongPressing(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressing Button Text=[" + ((Forms9Patch.StateButton)sender).Text + "]");
        }

        void OnImageButtonLongPressed(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("LongPressed Button Text=[" + ((Forms9Patch.StateButton)sender).Text + "]");
        }

        const bool debugProperties = true;
        //static bool debugCollections = true;

        NavigationPage navPage = new NavigationPage(new HomePage());


        void OnPagePopped(object sender, NavigationEventArgs e)
        {
            IDisposable displosable = e.Page as IDisposable;
            //System.Diagnostics.Debug.WriteLine("");
        }

        public App()
        {
            var pageStyle = new Style(typeof(Page))
            {
                Setters = {
                    new Setter { Property = Page.BackgroundColorProperty, Value = Color.White }
                },
                ApplyToDerivedTypes = true
            };
            var labelStyle = new Style(typeof(Label))
            {
                Setters = { new Setter { Property = Label.TextColorProperty, Value = Color.Black } },
                ApplyToDerivedTypes = true
            };
            var entryStyle = new Style(typeof(Entry))
            {
                Setters = { new Setter { Property = Entry.TextColorProperty, Value = Color.Black } },
                ApplyToDerivedTypes = true
            };
            var editorStyle = new Style(typeof(Editor))
            {
                Setters = { new Setter { Property = Editor.TextColorProperty, Value = Color.Black } },
                ApplyToDerivedTypes = true
            };
            var buttonStyle = new Style(typeof(Button))
            {
                Setters = { new Setter { Property = Button.TextColorProperty, Value = Color.Blue } },
                ApplyToDerivedTypes = true
            };
            var textCellStyle = new Style(typeof(TextCell))
            {
                Setters = { new Setter { Property = TextCell.TextColorProperty, Value = Color.Black } },
                ApplyToDerivedTypes = true
            };

            Resources = new ResourceDictionary();
            Resources.Add(pageStyle);
            Resources.Add(labelStyle);
            Resources.Add(entryStyle);
            Resources.Add(editorStyle);
            Resources.Add(buttonStyle);
            Resources.Add(textCellStyle);

            navPage.Popped += OnPagePopped;

            if (true)
            {
                //MainPage = new XamlSegmentedControlPage();
                //MainPage = new NinePatchButtonPage();
                //MainPage = new ModalPopupTestPage();
                //MainPage = new BubbleLayoutTestPage();
                //MainPage = new ImageBenchmarkPage();
                //MainPage = new BubblePopupTestPage();
                //MainPage = new ChatListPage();
                //MainPage = new XamlStateButtonsPage();
                //MainPage = new HtmlButtonsPage();
                //MainPage = new HtmlLabelPage();
                //MainPage = new ImageCodePage();

                //MainPage = new ZenmekPage();
                //MainPage = new HeapDemoPage();
                //MainPage = new ModalPopupOnMasterDetailPage();
                /*
                MainPage = new Forms9Patch.RootPage(new MasterDetailPage { Master = new ContentPage { Title = "Test", BackgroundColor = Color.Green }, Detail = new BubblePopupTestPage() })
                {
                    BackgroundColor = Color.Brown
                };
                */
                //MainPage = new MasterDetailPage { Master = new ContentPage { Title = "Test", BackgroundColor = Color.Green }, Detail = new BubblePopupTestPage() };

                MainPage = Forms9Patch.RootPage.Create(navPage);

            }
        }


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

