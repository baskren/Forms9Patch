

using Xamarin.Forms;

namespace TestProject
{
	public class HtmlLabelPage : ContentPage
	{
		public HtmlLabelPage ()
		{
			Padding = new Thickness (5, 20, 5, 5);
			Content = new ScrollView {
				Content = new StackLayout {
					Children = {
						
						new Label { Text = "Hello HtmlLabelPage" },

						new Forms9Patch.Label { HtmlText =  "<b>\nEMBEDDED (resource) CUSTOM FONT:</b>"},
						new Forms9Patch.Label { 
							Text = "",
							FontFamily = "TestProject.Resources.Fonts.MaterialIcons-Regular.ttf",
						},

						new Forms9Patch.Label { HtmlText =  "<b>\nSUPPORTED HTML TAGS:</b>" },
						new Forms9Patch.Label { HtmlText =  "plain: no tags"},
						new Forms9Patch.Label { Text =  "non-html and very long.  Just trying to see what happens when a wrap is called for.  Let's see!"},
						new Forms9Patch.Label { HtmlText =  "&lt;b&gt;: <b>Bold</b> plain"},

						new Forms9Patch.Label { 
							HtmlText =  "&lt;Bold&gt;: <b>Bold</b> w/ serif italic serif base font",
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
						new Forms9Patch.Label { HtmlText =  "&lt;font face=(resource)&gt;: <font face=\"TestProject.Resources.Fonts.MaterialIcons-Regular.ttf\"></font> plain"},
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

						new Forms9Patch.Label { HtmlText =  "font-family (resource): <div style=\"font-family:TestProject.Resources.Fonts.MaterialIcons-Regular.ttf\"></div> plain"},

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


