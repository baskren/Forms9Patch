using System.ComponentModel;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System;

[assembly: ExportRenderer(typeof(Forms9Patch.BubbleLayout), typeof(Forms9Patch.iOS.BubbleLayoutRenderer))]
namespace Forms9Patch.iOS
{
	/// <summary>
	/// FormsPopups BubbleLayout renderer.
	/// </summary>
	internal class BubbleLayoutRenderer : VisualElementRenderer<BubbleLayout>
	{
		protected override void OnElementChanged (ElementChangedEventArgs<BubbleLayout> e) {
			base.OnElementChanged (e);
			if (e.NewElement != null)
				SetNeedsDisplay ();
			base.BackgroundColor = Color.Transparent.ToUIColor ();
		}

		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e) {
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == RoundedBoxBase.OutlineColorProperty.PropertyName ||
				e.PropertyName == BubbleLayout.HasShadowProperty.PropertyName ||
				e.PropertyName == RoundedBoxBase.OutlineWidthProperty.PropertyName ||
				e.PropertyName == RoundedBoxBase.OutlineRadiusProperty.PropertyName ||
				e.PropertyName == BubbleLayout.BackgroundColorProperty.PropertyName ||
				e.PropertyName == BubbleLayout.PaddingProperty.PropertyName ||
				e.PropertyName == BubbleLayout.PointerLengthProperty.PropertyName ||
				e.PropertyName == BubbleLayout.PointerTipRadiusProperty.PropertyName ||
				e.PropertyName == BubbleLayout.PointerCornerRadiusProperty.PropertyName ||
				e.PropertyName == BubbleLayout.PointerAxialPositionProperty.PropertyName ||
				//e.PropertyName == BubbleLayout.PointerDirectionProperty.PropertyName ||
				e.PropertyName == VisualElement.WidthProperty.PropertyName ||
				e.PropertyName == RoundedBoxBase.ShadowInvertedProperty.PropertyName
			) {
				SetNeedsDisplay ();
			}
			base.BackgroundColor = Color.Transparent.ToUIColor ();
		}


		/// <summary>
		/// Draw in the specified rect.
		/// </summary>
		/// <param name="rect">Rect.</param>
		public override void Draw (CGRect rect)
		{
			ContentMode = UIViewContentMode.Redraw;

			var backgroundColor = (Color) Element.GetValue (VisualElement.BackgroundColorProperty);
			var outlineColor = (Color) Element.GetValue (RoundedBoxBase.OutlineColorProperty);
			var outlineWidth = (nfloat)(Math.Max (0, (float)Element.GetValue (RoundedBoxBase.OutlineWidthProperty) ) );

			if (backgroundColor.A < 0.01 && (outlineColor.A < 0.01 || outlineWidth < 0.01 )) {
				base.Draw (rect);
				return;
			}

			var outlineRadius = Math.Max (0, (float)Element.GetValue (RoundedBoxBase.OutlineRadiusProperty));
			var hasShadow = (bool)Element.GetValue (BubbleLayout.HasShadowProperty);
			var shadowInverted = ((bool)Element.GetValue (RoundedBoxBase.ShadowInvertedProperty));

			var shadowX = (nfloat)Forms9Patch.Settings.ShadowOffset.X;//* Display.Scale);
			var shadowY = (nfloat)Forms9Patch.Settings.ShadowOffset.Y;// * Display.Scale);
			var shadowR = (nfloat)Forms9Patch.Settings.ShadowRadius;// * Display.Scale);

			using (var g = UIGraphics.GetCurrentContext ()) {
				g.SaveState ();
				var shadowColor = Color.FromRgba (0.0, 0.0, 0.0, 0.55).ToCGColor ();
				CGRect perimeter = rect;
				if (hasShadow) {

					// what additional padding was allocated to cast the button's shadow?
					var shadowPad = BubbleLayout.ShadowPadding(Element);
					// shrink the button's perimeter by that extra padding (so it is the size it was originally intended to be)
					perimeter = new CGRect (rect.Left + shadowPad.Left, rect.Top + shadowPad.Top, rect.Width - shadowPad.HorizontalThickness, rect.Height - shadowPad.VerticalThickness);
					// setup for a "raised" shadow
					if (!shadowInverted && hasShadow && backgroundColor.A > 0) {
						g.SetShadow (new CGSize (shadowX, shadowY), shadowR, shadowColor);
					}
				}


				// generate background
				CGPath perimeterPath;

				if (backgroundColor.A > 0) {
					g.SetFillColor (backgroundColor.ToCGColor ());
					//g.SetFillColor (Color.Aqua.ToCGColor ());
					perimeterPath = PerimeterPath (Element, perimeter, (float)(outlineRadius + (outlineColor.A > 0 ? outlineWidth / 2.0 : 0) ));
						//Console.WriteLine ("periPath: [" + perimeter.Left + ", " + perimeter.Top + ", " + perimeter.Width + ", " + perimeter.Height + "]");
					g.AddPath (perimeterPath);
					g.FillPath ();
				}

				// shrink the perimeter by the outlinewidth so it's not clipped by the view's bounds
				if (outlineWidth > 0) {
					perimeter = perimeter.Inset ((nfloat)(outlineWidth / 2.0), (nfloat)(outlineWidth / 2.0));
					//Console.WriteLine ("perimeter:["+perimeter.Left+", "+perimeter.Top+", "+perimeter.Width+", "+perimeter.Height+"]");
				}


				// outline
				if (outlineWidth > 0 && outlineColor.A > 0) {
					g.SetShadow (new CGSize (0, 0), 0, Color.Transparent.ToCGColor ());
					g.SetLineWidth (outlineWidth);
					g.SetStrokeColor (outlineColor.ToCGColor ());
					var outlinePath = PerimeterPath (Element, perimeter, outlineRadius);
					g.AddPath (outlinePath);
					g.StrokePath ();
				}


				// interior shadow
				if (hasShadow && backgroundColor.A > 0 && shadowInverted) {
					var insetShadowPath = CGPath.FromRect (rect.Inset (-40, -40));
					perimeterPath = PerimeterPath(Element, perimeter, outlineRadius);
					insetShadowPath.AddPath (perimeterPath);
					insetShadowPath.CloseSubpath ();
					g.AddPath (perimeterPath);
					g.Clip ();
					shadowColor = Color.FromRgba (0.0, 0.0, 0.0, 0.75).ToCGColor ();
					g.SetShadow (new CGSize (shadowX, shadowY), shadowR, shadowColor);
					g.SetFillColor (shadowColor);
					g.SaveState ();
					g.AddPath (insetShadowPath);
					g.EOFillPath ();
					g.RestoreState ();
				}

				g.RestoreState ();
			}
			base.Draw (rect);

		}

		static CGRect RectInset (CGRect rect, double left, double top, double right, double bottom) {
			return new CGRect (rect.X + left, rect.Y + top, rect.Width - left - right, rect.Height - top - bottom);
		}


		static CGPath PerimeterPath(BubbleLayout element, CGRect rect, float radius) {

			if (element.PointerDirection == PointerDirection.None)
				return LayoutRenderer<Frame>.PerimeterPath (element, rect, radius);
			
			var length = element.PointerLength;

			if (radius * 2 > rect.Height - (element.PointerDirection.IsVertical() ? length : 0))
				radius = (float)((rect.Height - (element.PointerDirection.IsVertical() ? length : 0)) / 2.0);
			if (radius * 2 > rect.Width - (element.PointerDirection.IsHorizontal() ? length : 0))
				radius = (float)((rect.Width - (element.PointerDirection.IsHorizontal() ? length : 0)) / 2.0);

			var filetRadius = element.PointerCornerRadius;
			var tipRadius = element.PointerTipRadius;

			if (filetRadius / 2.0 + tipRadius / 2.0 > length) {
				filetRadius = (float)(2 * (length - tipRadius / 2.0));
				if (filetRadius < 0) {
					filetRadius = 0;
					tipRadius = 2 * length;
				}
			}

			//System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "L-Fr/2=["+(length-filetRadius/2.0)+"]  Tr/2=[" + (tipRadius/2.0) + "] length=["+length+"]");
			if (length - filetRadius / 2.0 < tipRadius / 2.0) {
				tipRadius = (float)(2 * (length - filetRadius / 2.0));
			}
			//System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "L-Fr/2=["+(length-filetRadius/2.0)+"]  Tr=/2[" + (tipRadius/2.0) + "] length=["+length+"]");



			var result = new CGPath ();
			var position = element.PointerAxialPosition;
			if (position <= 1.0) {
				if (element.PointerDirection == PointerDirection.Down || element.PointerDirection == PointerDirection.Up)
					position = (float)(rect.Width * position);
				else
					position = (float)(rect.Height * position);
			}
			var lft = rect.Left;
			var rht = rect.Right;
			var top = rect.Top;
			var bot = rect.Bottom;

			const float sqrt3 = (float)1.732050807568877;
			const float sqrt3d2 = (float)0.86602540378444;
			const float PId3 = (float)(Math.PI / 3.0);
			const float PId2 = (float)(Math.PI / 2.0);
			const float PId6 = (float)(Math.PI / 6.0);

			float tipCornerHalfWidth = tipRadius * sqrt3d2;
			float pointerToCornerIntercept = (float)Math.Sqrt((2 * radius * Math.Sin (Math.PI / 12.0)) * (2 * radius * Math.Sin (Math.PI / 12.0)) - (radius*radius/4.0));

			float pointerAtLimitSansTipHalfWidth = (float)( pointerToCornerIntercept + radius/ (2.0*sqrt3) + (length - tipRadius/2.0)/sqrt3 );
			float pointerAtLimitHalfWidth = pointerAtLimitSansTipHalfWidth + tipRadius * sqrt3d2;

			float pointerSansFiletHalfWidth = (float)(tipCornerHalfWidth + (length - filetRadius / 2.0 - tipRadius / 2.0) / sqrt3);
			float pointerFiletWidth = filetRadius * sqrt3d2;
			float pointerAndFiletHalfWidth = pointerSansFiletHalfWidth + pointerFiletWidth;

			if (element.PointerDirection.IsHorizontal ()) {
				int dir = 1;
				float start = (float)lft;
				float end = (float)rht;
				if (element.PointerDirection == PointerDirection.Right) {
					dir = -1;
					start = (float)rht;
					end = (float)lft;
				}
				float tip = position;
				if (tip > rect.Height - pointerAtLimitHalfWidth) 
					tip = (float)(rect.Height - pointerAtLimitHalfWidth);
				if (tip < pointerAtLimitHalfWidth) 
					tip = pointerAtLimitHalfWidth;
				if (rect.Height <= 2 * pointerAtLimitHalfWidth) 
					tip = (float)(rect.Height / 2.0);
				result.MoveToPoint (start + dir * (length + radius), top);
				result.AddLineToPoint (end - dir * radius, top);
				result.AddRelativeArc (end - dir * radius, top + radius, radius, -PId2, dir * PId2);
				result.AddLineToPoint (end, bot - radius);
				result.AddRelativeArc (end - dir * radius, bot - radius, radius, PId2 - dir * PId2, dir * PId2);
				result.AddLineToPoint (start + dir * (length + radius), bot);
				// bottom half
				if (tip >= rect.Height - pointerAndFiletHalfWidth - radius) {
					double endRatio = (rect.Height - tip) / (pointerAndFiletHalfWidth + radius);
					//System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "A endRatio=[" + endRatio + "]");
					result.AddCurveToPoint (
						new CGPoint (start + dir * (length + radius - endRatio * 4 * radius / 3.0), bot),
						new CGPoint (start + dir * (length - filetRadius / 2.0 + filetRadius * sqrt3d2), top + tip + pointerSansFiletHalfWidth + filetRadius / 2.0),
						new CGPoint (start + dir * (length - filetRadius / 2.0), top + tip + pointerSansFiletHalfWidth));
				} else {
					//System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "B");
					result.AddRelativeArc (
						start + dir * (length + radius), 
						bot - radius, 
						radius, PId2, dir * PId2);
					result.AddLineToPoint (
						start + dir * length,
						top + tip + pointerAndFiletHalfWidth
					);
					result.AddRelativeArc (
						start + dir * (length - filetRadius),
						top + tip + pointerAndFiletHalfWidth, 
						filetRadius, PId2 - dir * PId2, dir * -PId3);
				}

				//tip
				result.AddLineToPoint (
					(nfloat)(start + dir * tipRadius / 2.0),
					top + tip + tipCornerHalfWidth
				);
				result.AddRelativeArc (
					start + dir * tipRadius, 
					top + tip, 
					tipRadius, 
					PId2 + dir * PId6,
					dir * 2 * PId3);
				result.AddLineToPoint (
					(nfloat)(start + dir * (length - filetRadius / 2.0)),
					top + tip - pointerSansFiletHalfWidth
				);

				// top half
				if (tip <= pointerAndFiletHalfWidth + radius) { 
					double startRatio = tip / (pointerAndFiletHalfWidth + radius);
					//System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "C startRatio=[" + startRatio + "]");
					result.AddCurveToPoint (
						new CGPoint (start + dir * (length - filetRadius / 2.0 + filetRadius * sqrt3d2), top + tip - pointerSansFiletHalfWidth - filetRadius / 2.0),
						new CGPoint (start + dir * (length + radius - startRatio * 4 * radius / 3.0), top),
						new CGPoint (start + dir * (length + radius), top)
					);
				} else {
					//System.Diagnostics.Debug.WriteLineIf (element.PointerDirection == PointerDirection.Left, "D");
					result.AddRelativeArc (
						start + dir * (length - filetRadius),
						top + tip - pointerAndFiletHalfWidth, 
						filetRadius, 
						PId2 - dir * PId6,
						dir * -PId3);
					result.AddLineToPoint (
						start + dir * length,
						top + radius
					);
					result.AddRelativeArc (
						start + dir * (length + radius), 
						top + radius, 
						radius, 
						PId2 + dir * PId2,
						dir * PId2);
				}
			} else {
				int dir = 1;
				float start = (float)top;
				float end = (float)bot;
				if (element.PointerDirection == PointerDirection.Down) {
					dir = -1;
					start = (float)bot;
					end = (float)top;
				}
				float tip = position;
				if (tip > rect.Width - pointerAtLimitHalfWidth) 
					tip = (float)(rect.Width - pointerAtLimitHalfWidth);
				if (tip < pointerAtLimitHalfWidth) 
					tip = pointerAtLimitHalfWidth;
				if (rect.Width <= 2 * pointerAtLimitHalfWidth) 
					tip = (float)(rect.Width / 2.0);
				result.MoveToPoint (lft, start + dir * (length + radius));
				result.AddLineToPoint (lft, end - dir * radius);
				result.AddRelativeArc (lft + radius, end - dir * radius, radius, (nfloat)(Math.PI), dir * -PId2);
				result.AddLineToPoint (rht - radius, end);
				result.AddRelativeArc (rht - radius, end - dir * radius, radius, dir * PId2, dir * -PId2);
				result.AddLineToPoint (rht, start + dir * (radius + length));					
				// right half
				if (tip > rect.Width - pointerAndFiletHalfWidth - radius) { 
					double endRatio = (rect.Width - tip) / (pointerAndFiletHalfWidth + radius);
					result.AddCurveToPoint (
						new CGPoint ( rht, start + dir * (length + radius - endRatio * 4 * radius / 3.0) ),
						new CGPoint ( lft + tip + pointerSansFiletHalfWidth + filetRadius / 2.0, start + dir * (length - filetRadius / 2.0 + filetRadius * sqrt3d2) ),
						new CGPoint ( lft + tip + pointerSansFiletHalfWidth, start + dir * (length - filetRadius / 2.0) )
					);
				} else {
					result.AddRelativeArc (
						rht - radius, 
						start + dir * (length + radius), 
						radius, 0, dir * -PId2);
					result.AddLineToPoint (
						lft + tip + pointerAndFiletHalfWidth,
						start + dir * length
					);
					result.AddRelativeArc (
						lft + tip + pointerAndFiletHalfWidth, 
						start + dir * (length - filetRadius),
						filetRadius, dir * PId2, dir * PId3);
				}
				//tip
				result.AddLineToPoint (
					lft + tip + tipCornerHalfWidth,
					(nfloat)(start + dir * tipRadius / 2.0)
				);
				result.AddRelativeArc (
					lft + tip, 
					start + dir * tipRadius, 
					tipRadius, 
					dir * - PId6,
					dir * - 2 * PId3);
				result.AddLineToPoint (
					lft + tip - pointerSansFiletHalfWidth,
					(nfloat)(start + dir * (length - filetRadius / 2.0))
				);

				// left half
				if (tip < pointerAndFiletHalfWidth + radius) { 
					double startRatio = tip / (pointerAndFiletHalfWidth + radius);
					result.AddCurveToPoint (
						new CGPoint (
							lft + tip - pointerSansFiletHalfWidth - filetRadius / 2.0,
							start + dir * (length - filetRadius / 2.0 + filetRadius * sqrt3d2)
						),
						new CGPoint (
							lft,
							start + dir * (length + radius - startRatio * 4 * radius / 3.0)
						),
						new CGPoint (
							lft,
							start + dir * (length + radius)
						)
					);
				} else {
					result.AddRelativeArc (
						lft + tip - pointerAndFiletHalfWidth, 
						start + dir * (length - filetRadius),
						filetRadius, 
						dir * PId6,
						dir * PId3);
					result.AddLineToPoint (
						lft + radius,
						start + dir * length
					);
					result.AddRelativeArc (
						lft + radius, 
						start + dir * (length + radius), 
						radius, 
						dir * -PId2,
						dir * -PId2);
				}

			}

			return result;
		}
			
	}
}