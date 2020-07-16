// /*******************************************************************
//  *
//  * LabelFitPage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

using Xamarin.Forms;
//using Microsoft.VisualBasic.CompilerServices;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class UnconstrainedLabelAutoFitPage : ContentPage
    {
        static string text1 = "Żyłę;^`g <b><em>Lorem</em></b> ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";









        readonly Forms9Patch.SegmentedControl fitSelector = new Forms9Patch.SegmentedControl();

        double lastFontSize = -1;
        bool rendering = false;

        public UnconstrainedLabelAutoFitPage()
        {

            BackgroundColor = Color.Gray;
            Padding = 0;

            #region Editor
            var editor = new Editor
            {
                //Text = "Żorem ipsum dolor sit amet, consectetur adięiscing ełit",
                Text = text1,
                TextColor = Color.White,
                BackgroundColor = Color.Black,
                HeightRequest = 130,
                FontSize = 15,
            };
            #endregion


            #region Xamarin Forms label
            var xfLabel = new Label
            {
                FontSize = 15,
                TextColor = Color.White,
                BackgroundColor = Color.Black,
                Text = editor.Text
            };
            #endregion


            #region Forms9Patch Label
            var f9pLabel = new Forms9Patch.Label
            {
                //HeightRequest = 50,
                Lines = 3,
                FontSize = 15,
                TextColor = Color.White,
                AutoFit = Forms9Patch.AutoFit.None,
                BackgroundColor = Color.Black,
                Text = editor.Text
            };
            #endregion

            #region Mode
            var modeSwitch = new Switch
            {
                IsToggled = false,
                HorizontalOptions = LayoutOptions.End,
            };
            modeSwitch.Toggled += (sender, e) =>
            {
                if (modeSwitch.IsToggled)
                    f9pLabel.HtmlText = editor.Text;
                else
                    f9pLabel.Text = editor.Text;
            };
            #endregion


            #region Sync text between Editor and Labels

            editor.TextChanged += (sender, e) =>
            {
                xfLabel.Text = editor.Text;
                f9pLabel.HtmlText = editor.Text;
            };
            #endregion

            /*
			#region Frames for Labels
			var frameForF9P = new Frame
			{
				HeightRequest = 100,
				WidthRequest = 200,
				Padding = 0,
				Content = f9pLabel
			};

			var frameForXF = new Frame
			{
				HeightRequest = 100,
				WidthRequest = 200,
				Padding = 0,
				Content = xfLabel
			};
			#endregion
			*/

            #region Font Size selection
            var fontSizeSlider = new Slider
            {
                Maximum = 104,
                Minimum = 4,
                Value = 15
            };

            var fontSizeLabel = new Label
            {
                Text = "Font Size: 15"
            };
            fontSizeSlider.ValueChanged += (sender, e) =>
            {
                fontSizeLabel.Text = "Font Size: " + fontSizeSlider.Value;
                f9pLabel.FontSize = fontSizeSlider.Value;
                if (!rendering)
                {
                    rendering = true;
                    xfLabel.FontSize = fontSizeSlider.Value;
                    Device.StartTimer(TimeSpan.FromMilliseconds(50), () =>
                    {
                        if (Math.Abs(xfLabel.FontSize - lastFontSize) > double.Epsilon * 5)
                        {
                            xfLabel.FontSize = lastFontSize;
                            return true;
                        }
                        rendering = false;
                        return false;
                    });
                }
                lastFontSize = fontSizeSlider.Value;
            };
            var actualFontSizeLabel = new Label
            {
                Text = "Actual Font Size: 15"
            };

            f9pLabel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == Forms9Patch.Label.FittedFontSizeProperty.PropertyName)
                    actualFontSizeLabel.Text = "FittedFontSize: " + f9pLabel.FittedFontSize;
            };

            #endregion


            #region Lines selection
            var linesLabel = new Label
            {
                Text = "Lines: 3"
            };
            var linesSlider = new Slider
            {
                Minimum = 0,
                Maximum = 8,
                Value = 3
            };
            linesSlider.ValueChanged += (sender, e) =>
            {
                linesLabel.Text = "Lines: " + ((int)Math.Round(linesSlider.Value));
                f9pLabel.Lines = ((int)Math.Round(linesSlider.Value));
            };
            #endregion


            #region AutoFit Selection
            var fitSelector = new Forms9Patch.SegmentedControl();
            fitSelector.Segments.Add(new Forms9Patch.Segment
            {
                Text = "None",
                Command = new Command(x =>
                {
                    f9pLabel.AutoFit = Forms9Patch.AutoFit.None;
                })
            });
            var widthSegment = new Forms9Patch.Segment
            {
                Text = "Width",
                Command = new Command(x => { f9pLabel.AutoFit = Forms9Patch.AutoFit.Width; }),
                //IsEnabled = f9pLabel.HasImposedSize,
                BindingContext = f9pLabel,
                //IsEnabled = false
            };
            //widthSegment.SetBinding(Forms9Patch.Segment.IsEnabledProperty, "HasImposedSize");
            fitSelector.Segments.Add(widthSegment);
            var linesSegment = new Forms9Patch.Segment
            {
                Text = "Lines",
                Command = new Command(x => { f9pLabel.AutoFit = Forms9Patch.AutoFit.Lines; }),
                //IsEnabled = f9pLabel.HasImposedSize,
                BindingContext = f9pLabel
            };
            //linesSegment.SetBinding(Forms9Patch.Segment.IsEnabledProperty, "HasImposedSize");
            fitSelector.Segments.Add(linesSegment);
            fitSelector.SelectIndex(0);

            #endregion


            #region Alignment Selection
            var hzAlignmentSelector = new Forms9Patch.SegmentedControl();
            var vtAlignmentSelector = new Forms9Patch.SegmentedControl();
            hzAlignmentSelector.Segments.Add(
                new Forms9Patch.Segment
                {
                    Text = "Start",
                    Command = new Command(x =>
                    {
                        f9pLabel.HorizontalTextAlignment = TextAlignment.Start;
                        xfLabel.HorizontalTextAlignment = TextAlignment.Start;
                    })
                }
            );
            hzAlignmentSelector.Segments.Add(
                new Forms9Patch.Segment
                {
                    Text = "Center",
                    Command = new Command(x =>
                    {
                        f9pLabel.HorizontalTextAlignment = TextAlignment.Center;
                        xfLabel.HorizontalTextAlignment = TextAlignment.Center;
                    })
                }
            );
            hzAlignmentSelector.Segments.Add(
                new Forms9Patch.Segment
                {
                    Text = "End",
                    Command = new Command(x =>
                    {
                        f9pLabel.HorizontalTextAlignment = TextAlignment.End;
                        xfLabel.HorizontalTextAlignment = TextAlignment.End;
                    })
                }
            );
            vtAlignmentSelector.Segments.Add(
                new Forms9Patch.Segment
                {
                    Text = "Start",
                    Command = new Command(x =>
                    {
                        f9pLabel.VerticalTextAlignment = TextAlignment.Start;
                        xfLabel.VerticalTextAlignment = TextAlignment.Start;
                    })
                }
            );
            vtAlignmentSelector.Segments.Add(
                new Forms9Patch.Segment
                {
                    Text = "Center",
                    Command = new Command(x =>
                    {
                        f9pLabel.VerticalTextAlignment = TextAlignment.Center;
                        xfLabel.VerticalTextAlignment = TextAlignment.Center;
                    })
                }
            );
            vtAlignmentSelector.Segments.Add(
                new Forms9Patch.Segment
                {
                    Text = "End",
                    Command = new Command(x =>
                    {
                        f9pLabel.VerticalTextAlignment = TextAlignment.End;
                        xfLabel.VerticalTextAlignment = TextAlignment.End;
                    })
                }
            );
            hzAlignmentSelector.SelectIndex(0);
            vtAlignmentSelector.SelectIndex(0);
            #endregion


            #region BreakMode selection
            var breakModeSelector = new Forms9Patch.SegmentedControl();
            breakModeSelector.Segments.Add(
                new Forms9Patch.Segment
                {
                    Text = "NoWrap",
                    Command = new Command(x =>
                    {
                        f9pLabel.LineBreakMode = LineBreakMode.NoWrap;
                        xfLabel.LineBreakMode = LineBreakMode.NoWrap;
                    })
                }
            );
            breakModeSelector.Segments.Add(
                new Forms9Patch.Segment
                {
                    Text = "Char",
                    Command = new Command(x =>
                    {
                        f9pLabel.LineBreakMode = LineBreakMode.CharacterWrap;
                        xfLabel.LineBreakMode = LineBreakMode.CharacterWrap;
                    })
                }
            );
            breakModeSelector.Segments.Add(
                new Forms9Patch.Segment
                {
                    Text = "Word",
                    Command = new Command(x =>
                    {
                        f9pLabel.LineBreakMode = LineBreakMode.WordWrap;
                        xfLabel.LineBreakMode = LineBreakMode.WordWrap;
                    })
                }
            );
            breakModeSelector.Segments.Add(
                new Forms9Patch.Segment
                {
                    Text = "Head",
                    Command = new Command(x =>
                    {
                        f9pLabel.LineBreakMode = LineBreakMode.HeadTruncation;
                        xfLabel.LineBreakMode = LineBreakMode.HeadTruncation;
                    })
                }
            );
            breakModeSelector.Segments.Add(
                new Forms9Patch.Segment
                {
                    Text = "Mid",
                    Command = new Command(x =>
                    {
                        f9pLabel.LineBreakMode = LineBreakMode.MiddleTruncation;
                        xfLabel.LineBreakMode = LineBreakMode.MiddleTruncation;
                    })
                }
            );
            breakModeSelector.Segments.Add(
                new Forms9Patch.Segment
                {
                    Text = "Tail",
                    Command = new Command(x =>
                    {
                        f9pLabel.LineBreakMode = LineBreakMode.TailTruncation;
                        xfLabel.LineBreakMode = LineBreakMode.TailTruncation
                        ;
                    })
                }
            );
            breakModeSelector.SelectIndex(2);
            #endregion


            xfLabel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == HeightProperty.PropertyName)
                    System.Diagnostics.Debug.WriteLine("xfLabel.Height=[" + xfLabel.Height + "]\n");
            };

            #region FontSelection
            Picker fontPicker = new Picker
            {
                Title = "Default",
                //HeightRequest = 300,
                //VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand,

            };
            var fontFamilies = Forms9Patch.FontExtensions.LoadedFontFamilies();
            foreach (var fontFamily in fontFamilies)
                fontPicker.Items.Add(fontFamily);
            fontPicker.SelectedIndexChanged += (sender, e) =>
            {
                //string family = null;
                if (fontPicker.SelectedIndex > -1 && fontPicker.SelectedIndex < fontFamilies.Count)
                {
                    f9pLabel.FontFamily = fontFamilies[fontPicker.SelectedIndex];
                    xfLabel.FontFamily = fontFamilies[fontPicker.SelectedIndex];
                }
            };
            #endregion



            Content = new ScrollView
            {
                Padding = 0,
                Content = new StackLayout
                {
                    Padding = 20,
                    Children = {
                        new Label { Text = "Text:" },
                        editor,
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = {
                                new Label {
                                    Text = "HTML Formatted:",
                                    HorizontalOptions = LayoutOptions.StartAndExpand,
                                },
                                modeSwitch
                            },
                        },

                        new Label { Text = "Xamarin.Forms.Label:" },
                        xfLabel,
                        new Label { Text = "Forms9Patch.Label:" },
                        f9pLabel,
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children = {
                                new Label {
                                    Text = "Font Family:",
                                    HorizontalOptions = LayoutOptions.Start
                                },
                            fontPicker
                            }
                        },

                        fontSizeLabel,
                        fontSizeSlider,
                        actualFontSizeLabel,
                        new Label { Text = "AutoFit:" },
                        fitSelector,
                        linesLabel,
                        linesSlider,

                        new Label { Text = "Horizontal Alignment:" },
                        hzAlignmentSelector,
                        new Label { Text = "Vertical Alignment:" },
                        vtAlignmentSelector,
                        new Label { Text = "Truncation Mode:" },
                        breakModeSelector,


                    }
                }
            };
        }
    }
}


