using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using WApplication = Windows.UI.Xaml.Application;
using Xamarin.Forms;
using Windows.UI.Text;
using SharpDX;
using SharpDX.DirectWrite;
using System.Diagnostics;
using Windows.Storage;
using System.IO;

namespace Forms9Patch.UWP
{
    static class FontExtensions
    {
        //const double lineHeightToFontSizeRatio = 1.4;
        //const double roundToNearest = 1;

        public static SharpDX.DirectWrite.FontWeight ToDxFontWeight(this Windows.UI.Text.FontWeight fontWeight)
            => DxFontWeight(fontWeight.Weight);
        

        static SharpDX.DirectWrite.FontWeight DxFontWeight(int fontWeight)
        {
            var weights = Enum.GetValues(typeof(SharpDX.DirectWrite.FontWeight)).Cast<SharpDX.DirectWrite.FontWeight>().ToList();
            var lowIndex = -1;
            var lowError = int.MaxValue;
            for (int i = 0; i < weights.Count; i++)
            {
                var error = Math.Abs((int)weights[i] - fontWeight);
                if (error < lowError)
                {
                    lowError = error;
                    lowIndex = i;
                }
            }
            return lowIndex > -1 ? weights[lowIndex] : SharpDX.DirectWrite.FontWeight.Normal;
        }


        public static SharpDX.DirectWrite.FontStyle ToDxFontStyle(this Windows.UI.Text.FontStyle fontStyle)
        {
            var result = SharpDX.DirectWrite.FontStyle.Normal;
            if ((fontStyle & Windows.UI.Text.FontStyle.Oblique) > 0)
                result = SharpDX.DirectWrite.FontStyle.Oblique;
            if ((fontStyle & Windows.UI.Text.FontStyle.Italic) > 0)
                result |= SharpDX.DirectWrite.FontStyle.Italic;
            return result;
        }

        public static SharpDX.DirectWrite.FontStyle ToDxFontStyle(string fontStyle)
        {
            fontStyle = fontStyle.ToLower();
            var result = SharpDX.DirectWrite.FontStyle.Normal;
            if (fontStyle.Contains("bold") || fontStyle.Contains("oblique"))
                result = SharpDX.DirectWrite.FontStyle.Oblique;
            if (fontStyle.Contains("italic"))
                result |= SharpDX.DirectWrite.FontStyle.Italic;
            return result;
        }

        public static SharpDX.DirectWrite.FontStyle ToDxFontStyle(this Xamarin.Forms.FontAttributes fontAttributes)
        {
            var result = SharpDX.DirectWrite.FontStyle.Normal;
            if ((fontAttributes & Xamarin.Forms.FontAttributes.Bold) > 0)
                result = SharpDX.DirectWrite.FontStyle.Oblique;
            if ((fontAttributes & Xamarin.Forms.FontAttributes.Italic) > 0)
                result |= SharpDX.DirectWrite.FontStyle.Italic;
            return result;
        }


        public static SharpDX.DirectWrite.FontStretch ToDxFontStretch(this Windows.UI.Text.FontStretch fontStretch)
        {
            foreach (var stretch in Enum.GetValues(typeof(SharpDX.DirectWrite.FontStretch)).Cast<SharpDX.DirectWrite.FontStretch>())
            {
                if (((uint)stretch) == ((uint)fontStretch))
                    return stretch;
            }
            return SharpDX.DirectWrite.FontStretch.Undefined;
        }

        public static SharpDX.DirectWrite.FontMetrics GetFontMetrics(this TextBlock textBlock)
        {
            if (textBlock.GetDxFont() is SharpDX.DirectWrite.Font font)
                return font.Metrics;
            return new FontMetrics();
        }

        public static SharpDX.DirectWrite.Font GetDxFont(this Xamarin.Forms.Label label)
            => GetDxFont(label.FontFamily, SharpDX.DirectWrite.FontWeight.Normal, SharpDX.DirectWrite.FontStretch.Normal, label.FontAttributes.ToDxFontStyle());
        

        public static SharpDX.DirectWrite.Font GetDxFont(this TextBlock textBlock)
            => GetDxFont(textBlock.FontFamily.Source, textBlock.FontWeight.ToDxFontWeight(), textBlock.FontStretch.ToDxFontStretch(), textBlock.FontStyle.ToDxFontStyle());
        

        readonly static Dictionary<string, SharpDX.DirectWrite.Font> _loadedFonts = new Dictionary<string, SharpDX.DirectWrite.Font>();

        public static SharpDX.DirectWrite.Font GetDxFont(string fontFamily, SharpDX.DirectWrite.FontWeight weight , SharpDX.DirectWrite.FontStretch stretch, SharpDX.DirectWrite.FontStyle style)
        {
            //Windows.UI.Xaml.Media.FontFamily fontFamily = textBlock.FontFamily;

            if (fontFamily == null)
                fontFamily = LabeLRenderer._defaultTextBlock.FontFamily.Source;

            var fontKey = fontFamily + "-" + weight + "-" + stretch + "-" + style;

            if (_loadedFonts.TryGetValue(fontKey, out SharpDX.DirectWrite.Font font))
                return font;

            using (var factory = new Factory())
            {
                if (fontFamily.StartsWith("ms-appdata:///"))
                {
                    var fontFamilyFilePath = fontFamily.Substring(14);
                    string dir = null;
                    if (fontFamilyFilePath.StartsWith("local/"))
                    {
                        dir = ApplicationData.Current.LocalFolder.Path;
                        fontFamilyFilePath = fontFamilyFilePath.Substring(6);
                    }
                    else if (fontFamilyFilePath.StartsWith("localcache/"))
                    {
                        dir = ApplicationData.Current.LocalCacheFolder.Path;
                        fontFamilyFilePath = fontFamilyFilePath.Substring(11);
                    }
                    else if (fontFamilyFilePath.StartsWith("roaming/"))
                    {
                        dir = ApplicationData.Current.RoamingFolder.Path;
                        fontFamilyFilePath = fontFamilyFilePath.Substring(8);
                    }
                    else if (fontFamilyFilePath.StartsWith("temp/"))
                    {
                        dir = ApplicationData.Current.TemporaryFolder.Path;
                        fontFamilyFilePath = fontFamilyFilePath.Substring(5);
                    }
                    else
                    {
                        System.Console.WriteLine("Unknown StorageFolder for " + fontFamily);
                        return null;
                    }
                    var path = System.IO.Path.Combine(dir, fontFamilyFilePath.Split('#')[0]);

                    var fontFile = new SharpDX.DirectWrite.FontFile(factory, path );

                    var loader = fontFile.Loader;

                    var key = fontFile.GetReferenceKey();

                    using (var fontCollectionLoader = new FontCollectionLoader(fontFile))
                    {
                        factory.RegisterFontCollectionLoader(fontCollectionLoader);

                        using (var fontCollection = new FontCollection(factory, fontCollectionLoader, key))
                        {

                            var family = fontCollection.GetFontFamily(0);


                            var familyNames = family.FamilyNames;

                            font = family.GetFirstMatchingFont(weight, stretch, style);

                            _loadedFonts[fontKey] = font;
                        }
                    }
                    return font;
    
                }
                using (var fontCollection = factory.GetSystemFontCollection(false))
                {
                    var familyCount = fontCollection.FontFamilyCount;
                    for (int i = 0; i < familyCount; i++)
                    {
                        try
                        {
                            using (var dxFontFamily = fontCollection.GetFontFamily(i))
                            {
                                var familyNames = dxFontFamily.FamilyNames;

                                if (!familyNames.FindLocaleName(System.Globalization.CultureInfo.CurrentCulture.Name, out int index))
                                    familyNames.FindLocaleName("en-us", out index);

                                var name = familyNames.GetString(index);

                                var display = name;
                                using (var dxFont = dxFontFamily.GetFont(index))
                                {
                                    if (dxFont.IsSymbolFont)
                                        display = "Segoe UI";
                                }

                                //fontList.Add(new InstalledFont { Name = name, DisplayFont = display });
                                if ((fontFamily == name || fontFamily == display) && dxFontFamily.GetFirstMatchingFont(weight, stretch, style) is SharpDX.DirectWrite.Font systemFont)
                                {
                                    _loadedFonts[fontKey] = systemFont;
                                    return systemFont;
                                }
                            }
                        }
#pragma warning disable CC0004 // Catch block cannot be empty
                        catch { }       // Corrupted font files throw an exception - ignore them
#pragma warning restore CC0004 // Catch block cannot be empty

                    }
                }
            }
            return null;

        }

        internal static void DebugMetricsForLabel(this TextBlock textBlock)
        {
            Debug.WriteLine("TextBlock: " + textBlock.Text);
            textBlock.GetFontMetrics().DebugMetricsForFontSize(textBlock.FontSize);
            Debug.WriteLine("\t LineHeight: " + textBlock.LineHeight);
            Debug.WriteLine("");
        }

        internal static void DebugMetricsForFontSize(this FontMetrics metric, double fontSize)
        {
            Debug.WriteLine("Metrics for FontSize: " + fontSize);
            Debug.WriteLine("\t Ascent: " + metric.AscentForFontSize(fontSize));
            Debug.WriteLine("\t Descent: " + metric.DescentForFontSize(fontSize));
            Debug.WriteLine("\t CapHeight: " + metric.CapHeightForFontSize(fontSize));
            Debug.WriteLine("\t XHeight: " + metric.XHeightForFontSize(fontSize));
            Debug.WriteLine("\t LineGap: " + metric.LineGapForFontSize(fontSize));
            Debug.WriteLine("\t LineHeight: " + metric.LineHeightForFontSize(fontSize));



        }

        internal static double Ascent(this TextBlock textBlock) => textBlock.GetFontMetrics().AscentForFontSize(textBlock.FontSize);
        internal static double AscentForFontSize(this FontMetrics metric, double fontSize) => fontSize * Math.Abs(metric.Ascent) / metric.DesignUnitsPerEm;

        internal static double Descent(this TextBlock textBlock) => textBlock.GetFontMetrics().DescentForFontSize(textBlock.FontSize);
        internal static double DescentForFontSize(this FontMetrics metric, double fontSize) => fontSize * Math.Abs(metric.Descent) / metric.DesignUnitsPerEm;

        internal static double CapHeight(this TextBlock textBlock) => textBlock.GetFontMetrics().CapHeightForFontSize(textBlock.FontSize);
        internal static double CapHeightForFontSize(this FontMetrics metric, double fontSize) => fontSize * Math.Abs(metric.CapHeight) / metric.DesignUnitsPerEm;

        internal static double XHeight(this TextBlock textBlock) => textBlock.GetFontMetrics().XHeightForFontSize(textBlock.FontSize);
        internal static double XHeightForFontSize(this FontMetrics metric, double fontSize) => fontSize * Math.Abs(metric.XHeight) / metric.DesignUnitsPerEm;

        internal static double LineGap(this TextBlock textBlock) => textBlock.GetFontMetrics().LineGapForFontSize(textBlock.FontSize);
        internal static double LineGapForFontSize(this FontMetrics metric, double fontSize) => fontSize * Math.Abs(metric.LineGap) / metric.DesignUnitsPerEm;

        internal static double LineHeight(this TextBlock textBlock) => textBlock.GetFontMetrics().LineHeightForFontSize(textBlock.FontSize);
        internal static double LineHeightForFontSize(this FontMetrics metric, double fontSize)
        {
            //return lineHeightToFontSizeRatio * fontSize;
            var result = metric.CapHeightForFontSize(fontSize) + metric.DescentForFontSize(fontSize);
            if (Double.IsNaN(result))
                System.Diagnostics.Debug.WriteLine("");
            return result;
        }

        internal static double HeightForLinesAtFontSize(this FontMetrics metric, int lines, double fontSize)
            => fontSize * DesignUnitsHeightForLines(metric, lines) / metric.DesignUnitsPerEm;
        

        internal static double FontSizeFromLineHeight(this FontMetrics metric, double lineHeight)
            => FontSizeFromLinesInHeight(metric, 1, lineHeight);
        

        static int DesignUnitsHeightForLines(this FontMetrics metric, int lines)
        {
            var designUnitsHeight = lines * (Math.Max(metric.Ascent, metric.CapHeight) + metric.Descent) + (lines - 1) * metric.LineGap;
            if (metric.CapHeight == metric.Ascent && metric.Descent + metric.LineGap == 0 && lines > 0)
                designUnitsHeight = lines * (Math.Max(2210, 1434) + 514);
            return designUnitsHeight;
        }

        internal static double FontSizeFromLinesInHeight(this FontMetrics metric, int lines, double height)
            => height * metric.DesignUnitsPerEm / DesignUnitsHeightForLines(metric,lines);
        

        internal static double ClipFontSize(double size, Forms9Patch.Label label)
            => ClipFontSize(size, label.MinFontSize, label.FontSize);
        

        internal static double ClipFontSize(double size, double min, double max)
        {
            if (max == -1)
                max = DefaultFontSize();
            if (min == -1)
                min = 4;
            if (size >= max)
                return max;
            return ClipFontSize(size, min);
        }

        internal static double DefaultFontSize()
            => (double)Windows.UI.Xaml.Application.Current.Resources["ControlContentThemeFontSize"];
        

        internal static double ClipFontSize(double size, double min)
        {
            if (size < 0)
                return DefaultFontSize() * Math.Abs(size);
            if (size < min)
                return min;
            return size;
        }

        internal static double DecipheredFontSize(this Forms9Patch.Label label)
        {
            if (label == null)
                return DefaultFontSize();
            return ClipFontSize(label.FontSize, DecipheredMinFontSize(label));
        }

        internal static double DecipheredMinFontSize(this Forms9Patch.Label label)
        {
            if (label == null)
                return 4;
            if (label.MinFontSize <= 0)
                return 4;
            return label.MinFontSize;
        }


        internal static double GetFontSize(this NamedSize size)
        {
            // These are values pulled from the mapped sizes on Windows Phone, WinRT has no equivalent sizes, only intents.
            switch (size)
            {
                case NamedSize.Default:
                    return DefaultFontSize();
                case NamedSize.Micro:
                    return 18.667 - 3;
                case NamedSize.Small:
                    return 18.667;
                case NamedSize.Medium:
                    return 22.667;
                case NamedSize.Large:
                    return 32;
                default:
                    throw new ArgumentOutOfRangeException(nameof(size));
            }
        }

        public static void ApplyFont(this Control self, Xamarin.Forms.Font font)
        {
            self.FontSize = font.UseNamedSize ? font.NamedSize.GetFontSize() : font.FontSize;
            self.FontFamily = FontService.GetWinFontFamily(font.FontFamily);
            self.FontStyle = font.FontAttributes.HasFlag(FontAttributes.Italic) ? Windows.UI.Text.FontStyle.Italic : Windows.UI.Text.FontStyle.Normal;
            self.FontWeight = font.FontAttributes.HasFlag(FontAttributes.Bold) ? FontWeights.Bold : FontWeights.Normal;
        }

        public static void ApplyFont(this TextBlock self, Xamarin.Forms.Font font)
        {
            self.FontSize = font.UseNamedSize ? font.NamedSize.GetFontSize() : font.FontSize;
            self.FontFamily = FontService.GetWinFontFamily(font.FontFamily);
            self.FontStyle = font.FontAttributes.HasFlag(FontAttributes.Italic) ? Windows.UI.Text.FontStyle.Italic : Windows.UI.Text.FontStyle.Normal;
            self.FontWeight = font.FontAttributes.HasFlag(FontAttributes.Bold) ? FontWeights.Bold : FontWeights.Normal;
        }

        public static void ApplyFont(this TextElement self, Xamarin.Forms.Font font)
        {
            self.FontSize = font.UseNamedSize ? font.NamedSize.GetFontSize() : font.FontSize;
            self.FontFamily = FontService.GetWinFontFamily(font.FontFamily);
            self.FontStyle = font.FontAttributes.HasFlag(FontAttributes.Italic) ? Windows.UI.Text.FontStyle.Italic : Windows.UI.Text.FontStyle.Normal;
            self.FontWeight = font.FontAttributes.HasFlag(FontAttributes.Bold) ? FontWeights.Bold : FontWeights.Normal;
        }

        internal static void ApplyFont(this Control self, IFontElement element)
        {
            self.FontSize = element.FontSize;
            self.FontFamily = FontService.GetWinFontFamily(element.FontFamily);
            self.FontStyle = element.FontAttributes.HasFlag(FontAttributes.Italic) ? Windows.UI.Text.FontStyle.Italic : Windows.UI.Text.FontStyle.Normal;
            self.FontWeight = element.FontAttributes.HasFlag(FontAttributes.Bold) ? FontWeights.Bold : FontWeights.Normal;
        }


    }

    class FontCollectionLoader : CallbackBase, SharpDX.DirectWrite.FontCollectionLoader
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "will persist ")]
        readonly FontFileEnumerator _fontFileEnumerator;

        public FontCollectionLoader(FontFile fontFile)
        {
            _fontFileEnumerator = new FontFileEnumerator(fontFile);
        }

        public SharpDX.DirectWrite.FontFileEnumerator CreateEnumeratorFromKey(Factory factory, DataPointer collectionKey)
        {
            return _fontFileEnumerator;
        }
    }

    class FontFileEnumerator : CallbackBase, SharpDX.DirectWrite.FontFileEnumerator
    {
        readonly FontFile _fontFile;

        public FontFileEnumerator(FontFile fontFile)
        {
            _fontFile = fontFile;
            //CurrentFontFile = _fontFile;
        }

        public FontFile CurrentFontFile
        {
            get;
            private set;
        }

        public bool MoveNext()
        {
            if (CurrentFontFile == null)
            {
                CurrentFontFile = _fontFile;
                return true;
            }
            CurrentFontFile = null;
            return false;
        }
    }

    class FontFileLoader : CallbackBase, SharpDX.DirectWrite.FontFileLoader
    {
        private readonly Factory _factory;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Will persist the duration of app life")]
        private FontFileStream _fontStream;

        public FontFileLoader(Factory factory)
        {
            _factory = factory;
        }

        public async Task Init(string fontFamily)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(fontFamily));
            using (var fileStream = await file.OpenStreamForReadAsync())
            {
                var fontBytes = new byte[(int)fileStream.Length];
                fileStream.Read(fontBytes, 0, (int)fileStream.Length);
                var stream = new DataStream(fontBytes.Length, true, true);
                stream.Write(fontBytes, 0, fontBytes.Length);
                stream.Position = 0;

                _fontStream = new FontFileStream(stream);
            }
            // Register the 
            _factory.RegisterFontFileLoader(this);
        }

        public SharpDX.DirectWrite.FontFileStream CreateStreamFromKey(DataPointer fontFileReferenceKey)
        {
            return _fontStream;
        }
    }

    class FontFileStream : SharpDX.CallbackBase, SharpDX.DirectWrite.FontFileStream
    {
        private readonly DataStream _stream;

#pragma warning disable CC0057 // Unused parameters
        public FontFileStream(DataStream stream) => _stream = stream;
#pragma warning restore CC0057 // Unused parameters

        //public IDisposable Shadow { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public long GetFileSize()
        {
            return _stream.Length;
        }

        public long GetLastWriteTime()
        {
            return 0;
        }
        
        public void ReadFileFragment(out IntPtr fragmentStart, long fileOffset, long fragmentSize, out IntPtr fragmentContext)
        {
            lock (this)
            {
                fragmentContext = IntPtr.Zero;
                _stream.Position = fileOffset;
                fragmentStart = _stream.PositionPointer;
            }
        }

        public void ReleaseFileFragment(IntPtr fragmentContext)
        {
            //throw new NotImplementedException();
        }
    }
}
