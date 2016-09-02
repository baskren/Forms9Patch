using System;
using Xamarin.Forms;
using System.Reflection;
using System.Linq;
using System.Collections.ObjectModel;

namespace Forms9Patch
{
	static class VisualElementExtension
	{
		static readonly MethodInfo _set_Platform;
		static readonly MethodInfo _get_Platform;
		static readonly MethodInfo _get_Bounds;
		static readonly MethodInfo _set_Bounds;
		static readonly MethodInfo _get_IsNativeStateConsistent;
		static readonly MethodInfo _get_DisableLayout;
		static readonly MethodInfo _get_LogicalChildren;


		static VisualElementExtension() {
			var formsType = typeof(Xamarin.Forms.VisualElement);
			var methods = formsType.GetRuntimeMethods ().ToList();
			if ((_set_Platform = methods.First (arg => arg.Name == "set_Platform")) == null)
				throw new NullReferenceException (nameof (_set_Platform));
			if ((_get_Platform = methods.First (arg => arg.Name == "get_Platform")) == null)
				throw new NullReferenceException (nameof (_get_Platform));
			if ((_get_Bounds = methods.First (arg => arg.Name == "get_Bounds")) == null)
				throw new NullReferenceException (nameof (_get_Bounds));
			if ((_set_Bounds = methods.First (arg => arg.Name == "set_Bounds")) == null)
				throw new NullReferenceException (nameof (_set_Bounds));
			if ((_get_IsNativeStateConsistent = methods.First (arg => arg.Name == "get_IsNativeStateConsistent")) == null)
				throw new NullReferenceException (nameof (_get_IsNativeStateConsistent));
			if ((_get_DisableLayout = methods.First (arg => arg.Name == "get_DisableLayout")) == null)
				throw new NullReferenceException (nameof (_get_DisableLayout));
			if ((_get_LogicalChildren = methods.First (arg => arg.Name == "get_LogicalChildren")) == null)
				throw new NullReferenceException (nameof (_get_LogicalChildren));
		}

		internal static void setIsPlatformEnabled(this VisualElement self, bool enabled) {
			self.SetPropertyValue ("IsPlatformEnabled", enabled);
		}
		internal static void setPlatformFrom(this VisualElement dest, VisualElement source) {
			dest.setPlatform (source.getPlatform ());
		}

		//TODO: remove?
		internal static object getPlatform(this VisualElement self) {
			return _get_Platform.Invoke(self, new object[] { });
		}

		internal static void setPlatform(this VisualElement self, object platform) {
			_set_Platform.Invoke (self, new object[] { platform });
		}

		//TODO: remove?
		internal static Rectangle getBounds(this VisualElement self) {
			return (Rectangle) _get_Bounds.Invoke (self, new object[] { });
		}

		//TODO: remove?
		internal static bool getIsNativeStateConsistent(this VisualElement self) {
			return (bool)_get_IsNativeStateConsistent.Invoke (self, new object[]{ });
		}

		//TODO: remove?
		internal static bool getDisableLayout(this VisualElement self) {
			return (bool)_get_DisableLayout.Invoke (self, new object[]{ });
		}

		/*
		public static void xLayout(this VisualElement self, Rectangle value) {
			//_set_Bounds.Invoke (self, new object[] { bounds });

			if (value.X == self.X && value.Y == self.Y && value.Height == self.Height && value.Width == self.Width) {
				return;
			}
			self.BatchBegin ();
			//self.X = value.X;
			self.SetPropertyValue("X", value.X);
			//self.Y = value.Y;
			self.SetPropertyValue("Y", value.Y);
			self.SetSize (value.Width, value.Height);
			self.BatchCommit ();
		}
		*/
		/*
		public static void SetSize (this VisualElement self, double width, double height)
		{
			if (self.Width == width && self.Height == height) {
				return;
			}
			//self.Width = width;
			self.SetPropertyValue("Width",width);
			//self.Height = height;
			self.SetPropertyValue("Height",height);
			self.SizeAllocated (width, height);

			//if (self.SizeChanged != null) 
			//	self.SizeChanged (this, EventArgs.Empty);
			

			var sizeChanged = (EventHandler) self.GetFieldValue ("SizeChanged");
			if (sizeChanged != null) {
				sizeChanged (self, EventArgs.Empty);
			}
		}
		*/
		/*
		public static void SizeAllocated (this VisualElement self, double width, double height)
		{
			if (self is Layout)
				((Layout)self).OnSizeAllocated (width, height);

		}

		*/
		//TODO: remove?
		public static ReadOnlyCollection<Element> LogicalChildren(this VisualElement self) {
			var result = _get_LogicalChildren.Invoke(self, new object[]{});
			return (ReadOnlyCollection<Element>)result;
		}




		/*
		public static SizeRequest xGetSizeRequest (this VisualElement ve, double widthConstraint, double heightConstraint)
		{
			double widthRequest = ve.WidthRequest;
			double heightRequest = ve.HeightRequest;
			if (widthRequest >= 0) {
				widthConstraint = Math.Min (widthConstraint, widthRequest);
			}
			if (heightRequest >= 0) {
				heightConstraint = Math.Min (heightConstraint, heightRequest);
			}
			SizeRequest sizeRequest = ve.xOnSizeRequest (widthConstraint, heightConstraint);
			bool flag = sizeRequest.Minimum != sizeRequest.Request;
			Size request = sizeRequest.Request;
			Size minimum = sizeRequest.Minimum;
			if (heightRequest != -1) {
				request.Height = heightRequest;
				if (!flag) {
					minimum.Height = heightRequest;
				}
			}
			if (widthRequest != -1) {
				request.Width = widthRequest;
				if (!flag) {
					minimum.Width = widthRequest;
				}
			}
			double minimumHeightRequest = ve.MinimumHeightRequest;
			double minimumWidthRequest = ve.MinimumWidthRequest;
			if (minimumHeightRequest != -1) {
				minimum.Height = minimumHeightRequest;
			}
			if (minimumWidthRequest != -1) {
				minimum.Width = minimumWidthRequest;
			}
			minimum.Height = Math.Min (request.Height, minimum.Height);
			minimum.Width = Math.Min (request.Width, minimum.Width);
			return new SizeRequest (request, minimum);
		}

		public static SizeRequest xOnSizeRequest (this VisualElement ve, double widthConstraint, double heightConstraint)
		{
			//if (ve.Platform == null || !ve.IsPlatformEnabled) {
			var platform = ve.getPlatform();
			if (platform == null || !((bool)ve.GetPropertyValue("IsPlatformEnabled")) ) {
				return new SizeRequest (new Size (-1, -1));
			}
			var nativeSize = (SizeRequest)platform.CallMethod ("Xamarin.Forms.IPlatform.GetNativeSize", new object[] { ve, widthConstraint, heightConstraint });
			return nativeSize;
			//return platform.GetNativeSize (this, widthConstraint, heightConstraint);
			//return new SizeRequest(new Size(315,36));

//			return base.Platform.GetNativeSize (this, widthConstraint, heightConstraint);
		}
		*/
	}


}

