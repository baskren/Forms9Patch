// /*******************************************************************
//  *
//  * LabelFitPage.cs copyright 2016 ben, 42nd Parallel - ALL RIGHTS RESERVED.
//  *
//  *******************************************************************/
using System;

using Xamarin.Forms;

namespace Forms9PatchDemo
{
    [Xamarin.Forms.Internals.Preserve(AllMembers = true)]
    public class LabelAutoFitPage : ContentPage
    {
        static string text1 = "Żyłę;^`g <b><em>Lorem</em></b> ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.";


        double lastFontSize = -1;
        bool rendering = false;

        Forms9Patch.SegmentedControl hzAlignmentSelector = new Forms9Patch.SegmentedControl();
        Forms9Patch.SegmentedControl vtAlignmentSelector = new Forms9Patch.SegmentedControl();
        Label vtAlignmentSelectorLabel = new Label { Text = "Vertical Alignment:" };

        Forms9Patch.Label f9pLabel = new Forms9Patch.Label
        {
            //HeightRequest = 50,
            Lines = 5,
            FontSize = 15,
            TextColor = Color.White,
            AutoFit = Forms9Patch.AutoFit.None,
            BackgroundColor = Color.Black,
            Text = text1,
            VerticalTextAlignment = TextAlignment.End
        };

        Label xfLabel = new Label
        {
            FontSize = 15,
            TextColor = Color.White,
            BackgroundColor = Color.Black,
            Text = text1
        };
        Label fontSizeLabel = new Label
        {
            Text = "Font Size: 15"
        };
        Label fittedFontSizeLabel = new Label
        {
            Text = "FittedFontSize: 15"
        };
        Slider fontSizeSlider = new Slider
        {
            Maximum = 104,
            Minimum = 4,
            Value = 15
        };
        Slider LinesSlider = new Slider
        {
            Minimum = 0,
            Maximum = 8,
            Value = 5
        };
        Label PageWidthLabel = new Label();

        Label LabelSizeLabel = new Label();

        Slider LineHeightSlider = new Slider
        {
            Minimum = 0.01,
            Maximum = 8,
            Value = 1
        };
        Label lineHeightLabel = new Label
        {
            Text = "LineHeight: 1"
        };

        public LabelAutoFitPage()
        {

            LinesSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(1.0));

            BackgroundColor = Color.LightGray;
            Padding = 10;

            #region Editor
            var editor = new Editor
            {
                Text = text1,
                TextColor = Color.Black,
                BackgroundColor = Color.White,
                HeightRequest = 130,
                FontSize = 15,
            };

            #endregion


            #region Xamarin Forms label
            var effect = Effect.Resolve("Forms9Patch.EmbeddedResourceFontEffect");
            //xfLabel.Effects.Add(effect);
            #endregion


            #region Forms9Patch Label
            var listener = FormsGestures.Listener.For(f9pLabel);
            listener.Tapped += (object sender, FormsGestures.TapEventArgs e) =>
            {
                System.Diagnostics.Debug.WriteLine("Point=[" + e.ElementTouches[0] + "] Index=[" + f9pLabel.IndexAtPoint(e.ElementTouches[0]) + "]");
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
                if (modeSwitch.IsToggled)
                    f9pLabel.HtmlText = editor.Text;
                else
                    f9pLabel.Text = editor.Text;
            };
            #endregion


            #region Frames for Labels
            var frameForF9P = new Frame
            {
                HeightRequest = 100,
                //WidthRequest = 200,
                Padding = 0,
                Content = f9pLabel,
                CornerRadius = 0
            };

            var frameForXF = new Frame
            {
                HeightRequest = 100,
                //WidthRequest = 200,
                Padding = 0,
                Content = xfLabel,
                CornerRadius = 0
            };
            #endregion


            #region Impose Height
            var imposeHeightSwitch = new Switch { IsToggled = true };
            var heightRequestSlider = new Slider(0, 800, 100);
            heightRequestSlider.Effects.Add(new Forms9Patch.SliderStepSizeEffect(0.5));
            var imposedHeightGrid = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = GridLength.Star },
                    new RowDefinition { Height = GridLength.Star }
                },
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star },
                },
            };
            var heightRequestLabel = new Forms9Patch.Label("HeightRequest: " + 100);
            imposedHeightGrid.Children.Add(new Forms9Patch.Label("Impose Height?"), 0, 0);
            imposedHeightGrid.Children.Add(heightRequestLabel, 1, 0);
            imposedHeightGrid.Children.Add(imposeHeightSwitch, 0, 1);
            imposedHeightGrid.Children.Add(heightRequestSlider, 1, 1);

            imposeHeightSwitch.Toggled += (ihs, ihsArgs) =>
            {
                double heightRequest = imposeHeightSwitch.IsToggled ? heightRequestSlider.Value : -1;
                frameForXF.HeightRequest = heightRequest;
                frameForF9P.HeightRequest = heightRequest;
                heightRequestSlider.IsVisible = imposeHeightSwitch.IsToggled;
                heightRequestLabel.IsVisible = imposeHeightSwitch.IsToggled;
                vtAlignmentSelector.IsVisible = imposeHeightSwitch.IsToggled;
                vtAlignmentSelectorLabel.IsVisible = imposeHeightSwitch.IsToggled;
            };

            heightRequestSlider.ValueChanged += (hrs, hrsArgs) =>
            {
                double heightRequest = imposeHeightSwitch.IsToggled ? heightRequestSlider.Value : -1;
                frameForXF.HeightRequest = heightRequest;
                frameForF9P.HeightRequest = heightRequest;
                heightRequestLabel.Text = "HeightRequest: " + heightRequestSlider.Value.ToString("####.###");
            };
            #endregion


            #region Font Size selection
            fontSizeSlider.ValueChanged += OnFontSizeSliderValueChanged;
            LineHeightSlider.ValueChanged += OnLineHeightSlider_ValueChanged;

            f9pLabel.FittedFontSizeChanged += (object sender, double e) =>
            {
                fittedFontSizeLabel.Text = "FittedFontSize: " + e;
            };
            #endregion


            #region Lines selection
            var linesLabel = new Label
            {
                Text = "Lines: 5"
            };

            LinesSlider.ValueChanged += (sender, e) =>
            {
                linesLabel.Text = "Lines: " + ((int)Math.Round(LinesSlider.Value));
                f9pLabel.Lines = ((int)Math.Round(LinesSlider.Value));
            };
            #endregion


            #region AutoFit Selection
            var fitSelector = new Forms9Patch.SegmentedControl();
            fitSelector.Segments.Add(new Forms9Patch.Segment
            {
                Text = "None",
                Command = new Command(x => { f9pLabel.AutoFit = Forms9Patch.AutoFit.None; })
            });
            var widthSegment = new Forms9Patch.Segment
            {
                Text = "Width",
                Command = new Command(x => { f9pLabel.AutoFit = Forms9Patch.AutoFit.Width; }),
                //IsEnabled = f9pLabel.HasImposedSize,
                //BindingContext = f9pLabel
            };
            //widthSegment.SetBinding(Forms9Patch.Segment.IsEnabledProperty, "HasImposedSize");
            fitSelector.Segments.Add(widthSegment);
            var linesSegment = new Forms9Patch.Segment
            {
                Text = "Lines",
                Command = new Command(x => { f9pLabel.AutoFit = Forms9Patch.AutoFit.Lines; }),
                //IsEnabled = f9pLabel.HasImposedSize,
                //BindingContext = f9pLabel
            };
            //linesSegment.SetBinding(Forms9Patch.Segment.IsEnabledProperty, "HasImposedSize");
            fitSelector.Segments.Add(linesSegment);
            fitSelector.SelectIndex(0);
            #endregion


            #region Alignment Selection
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


            #region FontSelection
            Picker fontPicker = new Picker
            {
                Title = "Default",
                HorizontalOptions = LayoutOptions.EndAndExpand,

            };
            var fontFamilies = Forms9Patch.FontExtensions.LoadedFontFamilies();
            foreach (var fontFamily in fontFamilies)
                fontPicker.Items.Add(fontFamily);
            fontPicker.SelectedIndexChanged += (sender, e) =>
            {
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
                    Padding = 10,
                    Children = {
                        /*
                        new Label { Text = "Text:" },
                        editor,
                        */

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

                        /*
                        new Label { Text = "Xamarin.Forms.Label:" },
                        frameForXF,
                        */
                        new Label { Text = "Forms9Patch.Label:" },
                        frameForF9P,
                        LabelSizeLabel,
                        new StackLayout {
                            Orientation = StackOrientation.Horizontal,
                            Children = { new Label { Text = "Font:", HorizontalOptions = LayoutOptions.Start }, fontPicker, }
                        },

                        fontSizeLabel,
                        fontSizeSlider,

                        lineHeightLabel,
                        LineHeightSlider,

                        fittedFontSizeLabel,

                        new Label { Text = "AutoFit:" },
                        fitSelector,

                        linesLabel,
                        LinesSlider,

                        imposedHeightGrid,


                        vtAlignmentSelectorLabel,
                        vtAlignmentSelector,
                        new Label { Text = "Horizontal Alignment:" },
                        hzAlignmentSelector,
                        new Label { Text = "Truncation Mode:" },
                        breakModeSelector,


                    }
                }
            };

            SizeChanged += LabelAutoFitPage_SizeChanged;
            frameForF9P.SizeChanged += LabelAutoFitPage_SizeChanged;
        }

        private void LabelAutoFitPage_SizeChanged(object sender, EventArgs e)
        {
            PageWidthLabel.Text = "Page Width: " + this.Width;
            if (f9pLabel.Parent is Frame frame)
                LabelSizeLabel.Text = "LABEL: " + f9pLabel.SizeForWidthAndFontSize(frame.Width, f9pLabel.FontSize) + "\t FRAME: " + frame.Bounds.Size;// + "\t PADDING: {" + frame.Padding.HorizontalThickness + ", " + frame.Padding.VerticalThickness + "}";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            fontSizeSlider.Value = 15;
            PageWidthLabel.Text = "Page Width: " + this.Width;
        }

        private void OnLineHeightSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            lineHeightLabel.Text = "LineHeight: " + e.NewValue;
            f9pLabel.LineHeight = e.NewValue;
        }

        void OnFontSizeSliderValueChanged(object sender, ValueChangedEventArgs e)
        {

            //System.Diagnostics.Debug.WriteLine("");
            fontSizeLabel.Text = "Font Size: " + e.NewValue;
            f9pLabel.FontSize = e.NewValue;
            if (!rendering)
            {
                rendering = true;
                xfLabel.FontSize = e.NewValue;
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
            lastFontSize = e.NewValue;

        }
    }
}


