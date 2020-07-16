using System;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class WebViewExport : ContentPage
    {
        #region VisualElements
        Grid layout = new Grid
        {
            RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = 40 },
                new RowDefinition { Height = GridLength.Star },
            }
        };

        WebView webView = new WebView();

        Forms9Patch.SegmentedControl segmentedControl = new Forms9Patch.SegmentedControl
        {
            GroupToggleBehavior = Forms9Patch.GroupToggleBehavior.None,
            Segments =
            {
                new Forms9Patch.Segment("ToPng"),
                new Forms9Patch.Segment("ToPdf Letter"),
                new Forms9Patch.Segment("ToPdf A4"),
                new Forms9Patch.Segment("Print")
            }
        };
        #endregion

        public WebViewExport()
        {
            Forms9Patch.WebViewPrintEffect.ApplyTo(webView);
            webView.Source = "https://xamarin.com";


            layout.Children.Add(segmentedControl);
            layout.Children.Add(webView, 0, 1);

            Padding = 10;

            Content = layout;


            segmentedControl.SegmentTapped += async (sender, e) =>
            {
                e.Segment.IsSelected = false;

                Forms9Patch.ToFileResult result = null;
                if (e.Segment.Text == "ToPng")
                {
                    if (await Forms9Patch.ToPngService.ToPngAsync(webView, "test") is Forms9Patch.ToFileResult fileResult)
                        result = fileResult;
                    /*
                    {
                        using (Forms9Patch.Alert.Create(e.Segment.Text + (fileResult.IsError ? " Error" : " Success"), fileResult.Result)) { }
                        var entry = new Forms9Patch.MimeItemCollection();
                        Forms9Patch.IMimeItemCollectionExtensions.AddBytesFromFile(entry, "image/png", fileResult.Result);
                        Forms9Patch.Sharing.Share(entry, segmentedControl.Segments[0]);

                    }
                    */
                }
                else if (e.Segment.Text == "ToPdf Letter")
                {
                    if (Device.RuntimePlatform == Device.UWP)
                    {
                        using (Forms9Patch.Toast.Create("ToPdf not available in UWP.", "Use Forms9Patch.PrintService.PrintAsync instead")) { }
                    }
                    else if (await Forms9Patch.ToPdfService.ToPdfAsync(webView, "test", Forms9Patch.PageSize.NaLetter) is Forms9Patch.ToFileResult fileResult)
                            result = fileResult;
                }
                else if (e.Segment.Text == "ToPdf A4")
                {
                    if (Device.RuntimePlatform == Device.UWP)
                    {
                        using (Forms9Patch.Toast.Create("ToPdf not available in UWP.", "Use Forms9Patch.PrintService.PrintAsync instead")) { }
                    }
                    else if (await Forms9Patch.ToPdfService.ToPdfAsync(webView, "test", Forms9Patch.PageSize.IsoA4) is Forms9Patch.ToFileResult fileResult)
                        result = fileResult;
                }
                else if (e.Segment.Text == "Print")
                {
                    if (Forms9Patch.PrintService.CanPrint)
                    {
                        await Forms9Patch.PrintService.PrintAsync(webView, "Forms9Patch WebView Print");
                    }
                    else
                    {
                        using (Forms9Patch.Alert.Create(e.Segment.Text, "Print not available on this device")) { }
                    }
                }

                if (result != null)
                {
                    if (result.IsError)
                        using (Forms9Patch.Alert.Create(e.Segment.Text + " Error", result.Result)) { }
                    else
                    {
                        var info = new System.IO.FileInfo(result.Result);
                        Forms9Patch.Toast.Create("Export file", result.Result + "\n size: " + info.Length);
                        
                        
                        var entry = new Forms9Patch.MimeItemCollection();
                        Forms9Patch.IMimeItemCollectionExtensions.AddBytesFromFile(entry, e.Segment.Text=="ToPng" ? "image/png" : "application/pdf", result.Result);
                        Forms9Patch.Sharing.Share(entry, e.Segment);
                        
                        /*
                        await Xamarin.Essentials.Share.RequestAsync(new Xamarin.Essentials.ShareFileRequest
                        {
                            Title = e.Segment.Text,
                            File = new Xamarin.Essentials.ShareFile(result.Result)
                        });
                        */

                    }
                }
            };
        }
    }
}