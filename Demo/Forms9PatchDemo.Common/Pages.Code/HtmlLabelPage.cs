using Xamarin.Forms;
using System.Reflection;
//using Forms9Patch;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class HtmlLabelPage : ContentPage
    {
        public HtmlLabelPage()
        {
            var aTagLabel = new Forms9Patch.Label
            {
                HtmlText = "This is a test of the &lt;a&gt; tag.  Tap <a id=\"the id attribute\" href=\"the href attribute\">here</a> to try it.",
            };

            aTagLabel.ActionTagTapped += (sender, e) =>
            {
                Forms9Patch.Toast.Create("&lt;a&gt; tagged label", "<b>id:</b> " + e.Id + ";\n<b>href:</b>" + e.Href + ";");
            };

            var assembly = GetType().GetTypeInfo().Assembly;
            var resources = assembly.GetManifestResourceNames();
            //var resourceInfo = assembly.GetManifestResourceInfo("Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf");
            //var resourceStream = assembly.GetManifestResourceStream("Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf");

            //var canRead = resourceStream.CanRead;

            //var x = resourceStream.Length;
            BackgroundColor = Color.AliceBlue;
            Padding = new Thickness(5, 20, 5, 5);
            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Children = {

                        new Label { Text = "Hello HtmlLabelPage" },

                        aTagLabel,

                        new Forms9Patch.Label { HtmlText =  "<b>\nEMBEDDED (resource) CUSTOM FONT:</b>"},
                        new Forms9Patch.Label {
                            Text = "",
                            FontFamily = "Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf",
                        },

                        new Forms9Patch.Label("ColorFonts: ☺⛄☂♨⛅"),


                        new Forms9Patch.Label { HtmlText =  "<b>\nSUPPORTED HTML TAGS:</b>" },
                        new Forms9Patch.Label { HtmlText =  "plain: no tags"},
                        new Forms9Patch.Label { Text =  "non-html and very long.  Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?"},
                        new Forms9Patch.Label { HtmlText =  "&lt;b&gt;: <b>Bold</b> plain"},


                        new Forms9Patch.Label {
                            HtmlText =  "&lt;Bold&gt;: <b>Bold</b> w/ serif italic <div style=\"color:red\">s</div><div style=\"color:green\">e</div>rif base font",
                            FontFamily = "Serif",
                            FontAttributes = FontAttributes.Italic,
                            TextColor = Color.Blue,
                        },

                        new Forms9Patch.Label { HtmlText =  "&lt;strong&gt;: <strong>strong</strong> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;i&gt;: <i>Italic</i> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;em&gt;: <em>emphesis</em> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;b&gt;&lt;i&gt;: <b><i>Bold+Italic</i></b> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;sub&gt;: H<sub>2</sub>0 H₂0 plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;sup&gt;: 42<sup>2</sup> 42² plain"},
                        new Forms9Patch.Label { HtmlText =  "1- <num>1234567890</num>/<den>1234567890</den>"},
                        new Forms9Patch.Label { HtmlText =  "&lt;u&gt;: <u>underlined</u> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;ins&gt;: <ins>inserted</ins> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font color=&quot;blue&quot;&gt;: <font color=\"blue\">Blue</font> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font color=&quot;#FF0000&quot;&gt;: <font color=\"#FF0000\">Red</font> plain"},
                        new Forms9Patch.Label { HtmlText =  "\twith underline: <u><font color=\"#FF0000\">Red</font></u> plain"},
                        new Forms9Patch.Label { HtmlText =  "\twith underline: <font color=\"#FF0000\"><u>Red</u></font> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font color=&quot;rgb(0,255,0)&quot;&gt;: <font color=\"rgb(0,255,0)\">Green</font> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font size=&quot;1&quot;&gt;: <font size=\"1\">size 1</font> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font size=&quot;2&quot;&gt;: <font size=\"2\">size 2</font> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font size=&quot;3&quot;&gt;: <font size=\"3\">size 3</font> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font size=&quot;4&quot;&gt;: <font size=\"4\">size 4</font> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font size=&quot;5&quot;&gt;: <font size=\"5\">size 5</font> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font size=&quot;6&quot;&gt;: <font size=\"6\">size 6</font> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font size=&quot;7&quot;&gt;: <font size=\"7\">size 7</font> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font face=&quot;Monospace&quot;&gt;: <font face=\"Monospace\">Monspace</font> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font face=&quot;Serif&quot;&gt;: <font face=\"Serif\">Serif</font> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font face=&quot;Sans-serif&quot;&gt;: <font face=\"Sans-serif\">Sans-serif</font> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;font face=(resource)&gt;: <font face=\"Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\"></font> plain"},


                        new Forms9Patch.Label { HtmlText =  "&lt;pre&gt;: <pre>preformatted  text</pre> plain  \ttext"},
                        new Forms9Patch.Label { HtmlText =  "&lt;tt&gt;: <tt>teletype</tt> plain"},


                        new Forms9Patch.Label {
                            HtmlText = @"<font color=""red""><u><tt>This is a <i>very</i> <b>long block</b> of text which should wrap to the next line.</tt></b></font>",
                            BackgroundColor = Color.FromRgba(128,128,128,50),
                                            VerticalOptions = LayoutOptions.Center
                        },

                        new Forms9Patch.Label {
                            HtmlText = @"<font color=""red""><u><i><tt>AG Lynch refuses to answer questions over 74 times...</tt></i></u></font>",
                            BackgroundColor = Color.FromRgba(128,128,128,50),
                                            VerticalOptions = LayoutOptions.Center
                        },

                        new Forms9Patch.Label { HtmlText =  "&lt;strike&gt;: <strike>strikethrough</strike> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;s&gt;: <s>strikethrough</s> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;del&gt;: <del>deleted text</del> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;big&gt;: <big>BIG text</big> plain"},
                        new Forms9Patch.Label { HtmlText =  "&lt;small&gt;: <small>SMALL text</small> plain"},

                        new Forms9Patch.Label { HtmlText =  "\n<b>SUPPORTTED HTML ATTRIBUTES:</b>"},
                        new Forms9Patch.Label { HtmlText =  "background-color: <div style=\"background-color:LightGrey\">LightGrey background</div> plain"},
                        new Forms9Patch.Label { HtmlText =  "color: <div style=\"color:Sienna\">Sienna Text</div> plain"},

                        new Forms9Patch.Label { HtmlText =  "font-family (resource): <div style=\"font-family:Forms9PatchDemo.Resources.Fonts.MaterialIcons-Regular.ttf\"></div> plain"},

                        new Forms9Patch.Label { HtmlText =  "font-size: <div style=\"font-size:50%\">50% Text</div> plain"},
                        new Forms9Patch.Label { HtmlText =  "font-size: <div style=\"font-size:8.5px\">8.5px Text</div> plain"},
                        new Forms9Patch.Label { HtmlText =  "font-size: <div style=\"font-size:100%\">100% Text</div> plain"},
                        new Forms9Patch.Label { HtmlText =  "font-size: <div style=\"font-size:17px\">17px Text</div> plain"},
                        new Forms9Patch.Label { HtmlText =  "font-size: <div style=\"font-size:200%\">200% Text</div> plain"},
                        new Forms9Patch.Label { HtmlText =  "font-size: <div style=\"font-size:34px\">34px Text</div> plain"},
                        new Forms9Patch.Label { HtmlText =  "font-weight: <div style=\"font-weight:Bold;font-style:Italic\">Bold+Italic Text</div> plain"},


                    }
                }
            };
        }
    }
}


