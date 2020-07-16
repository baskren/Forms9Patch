using Forms9Patch;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class HtmlExport : Xamarin.Forms.ContentPage
    {

        const string sharePngButtonText = "SHARE PNG";
        const string copyPngButtonText = "COPY PNG";
        const string sharePdfButtonText = "SHARE PDF";
        const string copyPdfButtonText = "COPY PDF";
        const string printButtonText = "PRINT";

        const string htmlText = @"
<!DOCTYPE html>
<html>
<body>

<h1>Convert to PNG</h1>

<p>This html will be converted to a PNG, PDF, or print.</p>

</body>
</html>
";

        #region VisualElements
        Editor _htmlEditor = new Editor
        {
            Placeholder = "enter HTML to convert/print here",
            Text = htmlText,
            TextColor = Color.Black,
            BackgroundColor = Color.White
        };

        Forms9Patch.SegmentedControl _destinationSelector = new SegmentedControl
        {
            Segments =
            {
                new Segment(sharePngButtonText),
                new Segment(copyPngButtonText),
                new Segment(sharePdfButtonText),
                new Segment(copyPdfButtonText),
                new Segment(printButtonText),
            },
            GroupToggleBehavior = GroupToggleBehavior.None
        };


        Xamarin.Forms.Grid _grid = new Xamarin.Forms.Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = 40 },
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = 40 },
                new RowDefinition { Height = 100 }
            },
        };
        #endregion




        #region Constructor
        public HtmlExport()
        {
            BackgroundColor = Color.White;

            _grid.Children.Add(new Xamarin.Forms.Label { Text = "Convert HTML to PNG", TextColor = Color.White });
            _grid.Children.Add(_htmlEditor, 0, 1);
            _grid.Children.Add(_destinationSelector, 0, 2);
            /*
            _grid.Children.Add(new Xamarin.Forms.Label
            {
                Text = "Size: " + P42.Utils.StringExtensions.HumanReadableBytes(P42.Utils.DiskSpace.Size) + "\nUsed: " + P42.Utils.StringExtensions.HumanReadableBytes(P42.Utils.DiskSpace.Used) + "\nFree: " + P42.Utils.StringExtensions.HumanReadableBytes(P42.Utils.DiskSpace.Free),
                VerticalOptions = LayoutOptions.FillAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.Black,

            }, 0, 3);
            */
            Padding = new Thickness(10, 40);

            Content = _grid;

            _destinationSelector.SegmentTapped += OnDestinationSelector_SegmentTapped;
        }
        #endregion


        #region The good stuff!
        bool _processing;
        async void OnDestinationSelector_SegmentTapped(object sender, SegmentedControlEventArgs e)
        {
            if (_processing)
                return;
            _processing = true;

            if (e.Segment.Text.Contains("PNG") && await Forms9Patch.ToPngService.ToPngAsync(_htmlEditor.Text, "myHtmlPage") is ToFileResult pngResult)
            {
                if (pngResult.IsError)
                {
                    using (Forms9Patch.Toast.Create("PNG error", pngResult.Result)) { }
                }
                else
                {
                    var entry = new Forms9Patch.MimeItemCollection();
                    entry.AddBytesFromFile("image/png", pngResult.Result);

                    if (e.Segment.Text.Contains("SHARE"))
                        Forms9Patch.Sharing.Share(entry, _destinationSelector);
                    else
                        Forms9Patch.Clipboard.Entry = entry;
                }
            }
            else if (e.Segment.Text.Contains("PDF"))
            {
                if (Device.RuntimePlatform == Device.UWP)
                {
                    using (Forms9Patch.Toast.Create("PDF export not available in UWP", "However, you can print to PDFs!  So, try <b>Print</b> and then select <b>Microsoft Print to PDF</b> as your printer.")) { }
                }
                else if (await Forms9Patch.ToPdfService.ToPdfAsync(_htmlEditor.Text, "myHtmlPage") is ToFileResult pdfResult)
                {
                    if (pdfResult.IsError)
                    {
                        using (Forms9Patch.Toast.Create("PDF error", pdfResult.Result)) { }
                    }
                    else
                    {
                        var entry = new Forms9Patch.MimeItemCollection();
                        entry.AddBytesFromFile("application/pdf", pdfResult.Result);

                        if (e.Segment.Text.Contains("SHARE"))
                            Forms9Patch.Sharing.Share(entry, _destinationSelector);
                        else
                            Forms9Patch.Clipboard.Entry = entry;
                    }
                }
            }
            else if (e.Segment.Text.Contains("PRINT"))
                await Forms9Patch.PrintService.PrintAsync(_htmlEditor.Text, "myHtmlPage");
            _processing = false;
        }
        #endregion
    }
}