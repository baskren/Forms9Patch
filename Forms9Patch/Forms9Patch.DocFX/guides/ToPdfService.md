# ToPdf Service

Often I need to do one of the following:

 - Convert some HTML to a PDF
 - Take the content of a Xamarin.Forms.WebView and turn it into a PNG
 
I've had to do this enough times that I finally broke down and added it to Forms9Patch.

## Convert HTML to a PDF

Usage is as follows:

```c-sharp
    if (await Forms9Patch.ToPdfService.ToPdfAsync(_htmlEditor.Text, "myHtmlPage") is ToFileResult result)
    {
        if (result.IsError)
        {
            using (Forms9Patch.Toast.Create("PDF error", result.Result)) { }
        }
        else
        {
            var entry = new Forms9Patch.MimeItemCollection();
            entry.AddBytesFromFile("application/pdf", result.Result);

            if (e.Segment.Text == shareButtonText)
                Forms9Patch.Sharing.Share(entry, _destinationSelector);
            else if (e.Segment.Text == copyButtonText)
                Forms9Patch.Clipboard.Entry = entry;
        }
    }
```        

In the above example, we are putting the PNG onto the clipboard.  Xamarin.Essentials.Sharing could also be used.


## Xamarin.Forms.WebView to a PNG

Below, we take the contents of a Xamarin.Forms.WebView and Share it as a PNG:

```c-sharp

...

var myWebView = new Xamarin.Forms.WebView();
WebViewPrintEffect.ApplyTo(myWebView);
myWebView.Source = new HtmlWebViewSource { Html = "some HTML text here" };

...

if (await myWebView.ToPdfAsync("output.png") is ToFileResult result)
{
    if (result.IsError)
            using (Forms9Patch.Toast.Create("PNG error", result.Result)) { }
        else
        {
            await Xamarin.Essentials.Share.RequestAsync(new Xamarin.Essentials.ShareFileRequest
            {
                Title = "myWebView sharing title",
                File = new Xamarin.Forms.ShareFile(result.Result, "application/pdf")
            });
        }
}
```


## Using EmbeddedResource as a source for a Xamarin.Forms.WebView

This is sort of an experimental feature but I've found it useful.  As such the documentation is sparse.  It allow you to put HTML content in a folder in your app's EmbeddedResources folder and then use it as a source for a WebView.  It also supports putting all of the HTML content into a zip file.  Please take a look at the source code to see how it works.