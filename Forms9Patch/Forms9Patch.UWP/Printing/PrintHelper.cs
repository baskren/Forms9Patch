using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.Graphics.Printing.OptionDetails;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Printing;

namespace Forms9Patch.UWP
{
    public class PrintHelper
    {
        /// <summary>
        /// The percent of app's margin width, content is set at 85% (0.85) of the area's width
        /// </summary>
        protected double ApplicationContentMarginLeft = 0.075;

        /// <summary>
        /// The percent of app's margin height, content is set at 94% (0.94) of tha area's height
        /// </summary>
        protected double ApplicationContentMarginTop = 0.03;
        
        /// <summary>
        /// PrintDocument is used to prepare the pages for printing.
        /// Prepare the pages to print in the handlers for the Paginate, GetPreviewPage, and AddPages events.
        /// </summary>
        protected PrintDocument printDocument;

        /// <summary>
        /// Marker interface for document source
        /// </summary>
        protected IPrintDocumentSource printDocumentSource;

        /// <summary>
        /// A list of UIElements used to store the print preview pages.  This gives easy access
        /// to any desired preview page.
        /// </summary>
        internal List<UIElement> printPreviewPages;

        // Event callback which is called after print preview pages are generated.  Photos scenario uses this to do filtering of preview pages
        protected event EventHandler PreviewPagesCreated;


        Task<IRandomAccessStreamWithContentType> wideMarginsIconTask;
        Task<IRandomAccessStreamWithContentType> moderateMarginsIconTask;
        Task<IRandomAccessStreamWithContentType> narrowMarginsIconTask;

        /*
        /// <summary>
        ///  A reference back to the scenario page used to access XAML elements on the scenario page
        /// </summary>
        protected Page ApplicationPage
        {
            get
            {
                var rootFrame = Window.Current.Content as Windows.UI.Xaml.Controls.Frame;
                var content = rootFrame.Content as Windows.UI.Xaml.Controls.Page;
                return content;
            }
        }
        */

        /// <summary>
        ///  A hidden canvas used to hold pages we wish to print
        /// </summary>
        protected Canvas PrintCanvas
        {
            get
            {
                if (RootPage.FindName("Forms9PatchPrintCanvas") is Canvas canvas)
                    return canvas;
                RootPanel?.Children.Insert(0, new Canvas { Opacity = 0.0, Name = "Forms9PatchPrintCanvas" });
                return RootPage.FindName("Forms9PatchPrintCanvas") as Canvas;
            }
        }

       
        internal static Windows.UI.Xaml.Controls.Page RootPage
        {
            get
            {
                var rootFrame = Window.Current.Content as Windows.UI.Xaml.Controls.Frame;
                var page = rootFrame?.Content as Windows.UI.Xaml.Controls.Page;
                return page;
            }
        }

        internal static Windows.UI.Xaml.Controls.Panel RootPanel => RootPage?.Content as Panel;

        protected UIElement PrintContent;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scenarioPage">The scenario page constructing us</param>
        public PrintHelper(string jobName)
        {
            JobName = jobName;

            printPreviewPages = new List<UIElement>();
            // Start these tasks early because we know we're going to need the
            // streams in PrintTaskRequested.
            RandomAccessStreamReference wideMarginsIconReference = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Forms9Patch.UWP/Assets/wideMargins.svg"));
            wideMarginsIconTask = wideMarginsIconReference.OpenReadAsync().AsTask();

            RandomAccessStreamReference moderateMarginsIconReference = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Forms9Patch.UWP/Assets/moderateMargins.svg"));
            moderateMarginsIconTask = moderateMarginsIconReference.OpenReadAsync().AsTask();

            RandomAccessStreamReference narrowMarginsIconReference = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Forms9Patch.UWP/Assets/narrowMargins.svg"));
            narrowMarginsIconTask = narrowMarginsIconReference.OpenReadAsync().AsTask();
        }

        /// <summary>
        /// This function registers the app for printing with Windows and sets up the necessary event handlers for the print process.
        /// </summary>
        public virtual void RegisterForPrinting()
        {

            printDocument = new PrintDocument();
            printDocumentSource = printDocument.DocumentSource;
            printDocument.Paginate += CreatePrintPreviewPages;
            printDocument.GetPreviewPage += GetPrintPreviewPage;
            printDocument.AddPages += AddPrintPages;

            PrintManager printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested += PrintTaskRequested;
        }

        /// <summary>
        /// This function unregisters the app for printing with Windows.
        /// </summary>
        public virtual void UnregisterForPrinting()
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                if (printDocument != null)
                {
                    printDocument.Paginate -= CreatePrintPreviewPages;
                    printDocument.GetPreviewPage -= GetPrintPreviewPage;
                    printDocument.AddPages -= AddPrintPages;

                    // Remove the handler for printing initialization.
                    PrintManager printMan = PrintManager.GetForCurrentView();
                    printMan.PrintTaskRequested -= PrintTaskRequested;
                }

                if (PrintCanvas != null)
                {
                    PrintCanvas.Children.Clear();
                    RootPanel?.Children.Remove(PrintCanvas);
                }

                if (PrintContent != null)
                    RootPanel?.Children.Remove(PrintContent);
            });
        }

        public string JobName { get; private set; }

        public async Task ShowPrintUIAsync()
        {
            // Catch and print out any errors reported
            try
            {
                await Task.Delay(100);
                await PrintManager.ShowPrintUIAsync();
            }
            catch (Exception e)
            {
                //MainPage.Current.NotifyUser("Error printing: " + e.Message + ", hr=" + e.HResult, NotifyType.ErrorMessage);
                using (Forms9Patch.Toast.Create(null, "Error printing: " + e.Message + ", hr=" + e.HResult)) { }
            }
        }

        /// <summary>
        /// Method that will generate print content for the scenario
        /// For scenarios 1-4: it will create the first page from which content will flow
        /// Scenario 5 uses a different approach
        /// </summary>
        /// <param name="page">The page to print</param>
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task Init()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            //CreatePrintPreviewPages()
            throw new NotImplementedException();
            // Add the (newly created) page to the print canvas which is part of the visual tree and force it to go
            // through layout so that the linked containers correctly distribute the content inside them.
            //PrintCanvas.Children.Add(firstPage);
            //PrintCanvas.InvalidateMeasure();
            //PrintCanvas.UpdateLayout();
        }
        

        /// <summary>
        /// This is the event handler for PrintManager.PrintTaskRequested.
        /// </summary>
        /// <param name="sender">PrintManager</param>
        /// <param name="e">PrintTaskRequestedEventArgs </param>
        protected virtual void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            PrintTask printTask = null;
            printTask = e.Request.CreatePrintTask(JobName, sourceRequestedArgs =>
            {
                var deferral = sourceRequestedArgs.GetDeferral();

                PrintTaskOptionDetails printDetailedOptions = PrintTaskOptionDetails.GetFromPrintTaskOptions(printTask.Options);
                IList<string> displayedOptions = printTask.Options.DisplayedOptions;
                displayedOptions.Clear();
                displayedOptions.Add(Windows.Graphics.Printing.StandardPrintTaskOptions.Copies);
                displayedOptions.Add(Windows.Graphics.Printing.StandardPrintTaskOptions.Orientation);
                displayedOptions.Add(Windows.Graphics.Printing.StandardPrintTaskOptions.PrintQuality);
                displayedOptions.Add(Windows.Graphics.Printing.StandardPrintTaskOptions.MediaSize);
                displayedOptions.Add(Windows.Graphics.Printing.StandardPrintTaskOptions.Collation);
                displayedOptions.Add(Windows.Graphics.Printing.StandardPrintTaskOptions.Duplex);

                // Preset the default value of the printer option
                printTask.Options.MediaSize = PrintMediaSize.NorthAmericaLetter;

                // Create a new list option
                PrintCustomItemListOptionDetails margins = printDetailedOptions.CreateItemListOption("Margins", "Margins");
                /*
                if (Forms9Patch.OsInfoService.Version >= new Version(10, 0, 17134, 0))
                {
                    margins.AddItem("WideMargins", "Wide", "Each margin is 20% of the paper size", await wideMarginsIconTask);
                    margins.AddItem("ModerateMargins", "Moderate", "Each margin is 10% of the paper size", await moderateMarginsIconTask);
                    margins.AddItem("NarrowMargins", "Narrow", "Each margin is 5% of the paper size", await narrowMarginsIconTask);
                    // App tells the user some more information about what the feature means.
                    margins.Description = "The space between the content of your document and the edge of the paper";
                }
                else
                */
                {
                    margins.AddItem("WideMargins", "Wide");
                    margins.AddItem("ModerateMargins", "Moderate");
                    margins.AddItem("NarrowMargins", "Narrow");
                }
                // The default is ModerateMargins
                ApplicationContentMarginTop = 0.1;
                ApplicationContentMarginLeft = 0.1;
                margins.TrySetValue("ModerateMargins");


                // Add the custom option to the option list
                displayedOptions.Add("Margins");

                printDetailedOptions.OptionChanged += OnPrintDetailOptionChanged;

                // Print Task event handler is invoked when the print job is completed.
                printTask.Completed += (s, args) =>
                {
                    Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
                    {
                        // Notify the user when the print operation fails.
                        if (args.Completion == PrintTaskCompletion.Failed)
                            using (Toast.Create("Printing Failed", null)) { }
                        //else if (args.Completion == PrintTaskCompletion.Canceled)
                        //    using (Toast.Create("Printing Cancelled", null)) { }
                        else if (args.Completion == PrintTaskCompletion.Submitted)
                            using (Toast.Create("Printing ...", "Print job submitted to printer.", TimeSpan.FromSeconds(5))) { }
                        else if (args.Completion == PrintTaskCompletion.Abandoned)
                            using (Toast.Create("Printing Abandoned", null)) { }
                    });
                    UnregisterForPrinting();
                };

                sourceRequestedArgs.SetSource(printDocumentSource);
                
                deferral.Complete();
            });
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        async void OnPrintDetailOptionChanged(PrintTaskOptionDetails sender, PrintTaskOptionChangedEventArgs args)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            bool invalidatePreview = false;

            string optionId = args.OptionId as string;
            if (string.IsNullOrEmpty(optionId))
            {
                return;
            }

            if (optionId == "Margins")
            {
                PrintCustomItemListOptionDetails marginsOption = (PrintCustomItemListOptionDetails)sender.Options["Margins"];
                string marginsValue = marginsOption.Value.ToString();

                switch (marginsValue)
                {
                    case "WideMargins":
                        ApplicationContentMarginTop = 0.2;
                        ApplicationContentMarginLeft = 0.2;
                        break;
                    case "ModerateMargins":
                        ApplicationContentMarginTop = 0.1;
                        ApplicationContentMarginLeft = 0.1;
                        break;
                    case "NarrowMargins":
                        ApplicationContentMarginTop = 0.05;
                        ApplicationContentMarginLeft = 0.05;
                        break;
                }
                /*
                if (Forms9Patch.OsInfoService.Version >= new Version(10, 0, 17134, 0))
                {
                    if (marginsValue == "NarrowMargins")
                    {
                        marginsOption.WarningText = "Narrow margins may not be supported by some printers";
                    }
                    else
                    {
                        marginsOption.WarningText = "";
                    }
                }
                */
                invalidatePreview = true;
            }

            if (invalidatePreview)
            {
                //await PrintPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                //{
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {

                    printDocument.InvalidatePreview();
                });
                //});
            }
        }

        System.Threading.SemaphoreSlim _semaphoreSlim = new System.Threading.SemaphoreSlim(1, 1);
        
        /// <summary>
        /// This is the event handler for PrintDocument.Paginate. It creates print preview pages for the app.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Paginate Event  </param>
        public virtual async void CreatePrintPreviewPages(object sender, PaginateEventArgs e)
        {
            var paperSize = e.PrintTaskOptions.GetPageDescription(0).PageSize;
            System.Diagnostics.Debug.WriteLine("CreatePrintPreviewPages: {" + paperSize.Width + "," + paperSize.Height + "}");

            //lock (printPreviewPages)
            await _semaphoreSlim.WaitAsync();

            try
            {
                // Clear the cache of preview pages
                printPreviewPages.Clear();

                // Clear the print canvas of preview pages
                PrintCanvas.Children.Clear();

                // Get the PrintTaskOptions
                PrintTaskOptions printingOptions = ((PrintTaskOptions)e.PrintTaskOptions);

                // Get the page description to deterimine how big the page is
                PrintPageDescription pageDescription = printingOptions.GetPageDescription(0);

                if (await GeneratePagesAsync(pageDescription) is List<UIElement> pages)
                {

                    foreach (var page in pages)
                        PrintCanvas.Children.Add(page);
                    PrintCanvas.InvalidateMeasure();
                    PrintCanvas.UpdateLayout();

                    await Task.Delay(1000);

                    printPreviewPages.AddRange(pages);
                    await Task.Delay(1000);
                }

                if (PreviewPagesCreated != null)
                    PreviewPagesCreated.Invoke(printPreviewPages, null);

                PrintDocument printDoc = (PrintDocument)sender;

                // Report the number of preview pages created
                printDoc.SetPreviewPageCount(printPreviewPages.Count, PreviewPageCountType.Intermediate);

            }
            catch (Exception pve)
            {
                P42.Utils.BreadCrumbs.AddException(pve, GetType());

                if (pve.Message.Contains("The RPC server is unavailable", StringComparison.OrdinalIgnoreCase))
                    using (var toast = Forms9Patch.Toast.Create("The RPC server is unavailable", "Windows failed trying to setup the print preview because it could not connect to its RPC server.   There are three basic potential causes for this error message. Either the RPC service is not running, there are issues with the network, or some important registry entries that control the RPC service have been corrupted. In Windows 10, the most common cause for the error is that the RPC service is simply not running.  <a id='link' href='https://www.techjunkie.com/rpc-server-is-unavailable/'>Click here to learn more about how to fix this.</a>")) 
                    {
                        toast.ActionTagTapped += On_RpcUnavailable_Toast_ActionTagTapped;
                    }
                else
                    using (Forms9Patch.Toast.Create("Cannot print", "Windows failed trying to setup the print preview.  Below is some information from Windows about the failure.  \n\n" + pve.Message)) { }
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private void On_RpcUnavailable_Toast_ActionTagTapped(object sender, ActionTagEventArgs e)
        {
            Xamarin.Essentials.Browser.OpenAsync(e.Href, Xamarin.Essentials.BrowserLaunchMode.SystemPreferred);
        }




        /// <summary>
        /// This is the event handler for PrintDocument.GetPrintPreviewPage. It provides a specific print preview page,
        /// in the form of an UIElement, to an instance of PrintDocument. PrintDocument subsequently converts the UIElement
        /// into a page that the Windows print system can deal with.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Arguments containing the preview requested page</param>
        protected virtual async void GetPrintPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("GetPrintPreviewPage: " + e.PageNumber);

            await _semaphoreSlim.WaitAsync();

            PrintDocument printDoc = (PrintDocument)sender;
            printDoc.SetPreviewPage(e.PageNumber, printPreviewPages[e.PageNumber - 1]);
            _semaphoreSlim.Release();
        }

        /// <summary>
        /// This is the event handler for PrintDocument.AddPages. It provides all pages to be printed, in the form of
        /// UIElements, to an instance of PrintDocument. PrintDocument subsequently converts the UIElements
        /// into a pages that the Windows print system can deal with.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Add page event arguments containing a print task options reference</param>
        protected virtual void AddPrintPages(object sender, AddPagesEventArgs e)
        {
            var paperSize = e.PrintTaskOptions.GetPageDescription(0).PageSize;
            System.Diagnostics.Debug.WriteLine("AddPrintPages: {" + paperSize.Width + "," + paperSize.Height + "}");

            // Loop over all of the preview pages and add each one to  add each page to be printied
            for (int i = 0; i < printPreviewPages.Count; i++)
            {
                // We should have all pages ready at this point...
                printDocument.AddPage(printPreviewPages[i]);
            }
            
            PrintDocument printDoc = (PrintDocument)sender;
            
            // Indicate that all of the print pages have been provided
            printDoc.AddPagesComplete();
        }


#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        protected virtual async Task<IEnumerable<UIElement>> GeneratePagesAsync(PrintPageDescription pageDescription)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotImplementedException();
        }
 
    }
}
