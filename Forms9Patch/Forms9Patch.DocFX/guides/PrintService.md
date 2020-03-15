# Printing

Forms9Patch makes it easy to print some HTML or the contents of a Xamarin.Forms.WebView.

## Verifying that Printing is available

Before printing, you should verify that printing is available on your device.  To do so, call:

```c-sharp
if (Forms9Patch.PrintService.CanPrint)
{
    // do the printing here
}
```

## Print the contents of a Xamarin.Forms.WebView



```c-sharp
    using Forms9Patch;

    ...

    var myWebView = new Xamarin.Forms.WebView();
    WebViewPrintEffect.ApplyTo(myWebView);
    myWebView.Source = new HtmlWebViewSource { Html = "some HTML text here" };

    ...

    myWebView.Print("my_print_job_name");
```

Note that your WebView does not have to be attached to a Layout.  This allows you to Print without having to display the WebView in your app's UI.



## Printing an HTML string



```c-sharp

    using Forms9Patch;

    ...

    var myHtmlString =  @"
                            <!DOCTYPE html>
                            <html>
                            <body>

                            <h1>Convert to PNG</h1>

                            <p>This html will be converted to a PNG, PDF, or print.</p>

                            </body>
                            </html>
                            ";

    ...

    myHtmlString.Print("my_print_job_name");    
```

PLEASE NOTE: iOS sometimes places the page breaks in weird places.  I have a [StackOverflow Bounty](https://stackoverflow.com/questions/59273274/uiprintinteractioncontroller-breaking-single-page-into-multiple-pages-when-it-sh) on why this happens and how to fix it.

## Using EmbeddedResource as a source for a Xamarin.Forms.WebView

This is sort of an experimental feature but I've found it useful.  As such the documentation is sparse.  It allow you to put HTML content in a folder in your app's EmbeddedResources folder and then use it as a source for a WebView.   A much nicer solution than using platform specific approach provided by Xamarin.  It also supports putting all of the HTML content into a zip file.  Please take a look at the source code to see how it works.