using System;
using System.Reflection;
using System.Linq;
using System.Collections.ObjectModel;

namespace Forms9Patch
{
	static class PageExtension
	{
		static readonly MethodInfo _sendAppearing;
		static readonly MethodInfo _sendDisappearing;
		static readonly MethodInfo _getInternalChildren;
		static readonly MethodInfo _onPropertyChanged;

		static PageExtension() {
			var formsType = typeof(Xamarin.Forms.Page);
			var methods = formsType.GetRuntimeMethods ().ToList();
			//_sendAppearing = methods.First (arg => arg.Name == "SendAppearing");
			//_sendDisappearing = methods.First (arg => arg.Name == "SendDisappearing");
			//_getInternalChildren = methods.First (arg => arg.Name == "get_InternalChildren");
			if ((_sendAppearing = methods.First (arg => arg.Name == "SendAppearing")) == null)
				throw new NullReferenceException (nameof (_sendAppearing));
			if ((_sendDisappearing = methods.First (arg => arg.Name == "SendDisappearing")) == null)
				throw new NullReferenceException (nameof (_sendDisappearing));
			if ((_getInternalChildren = methods.First (arg => arg.Name == "get_InternalChildren")) == null)
				throw new NullReferenceException (nameof (_getInternalChildren));
			if ((_onPropertyChanged = methods.First (arg => arg.Name == "OnPropertyChanged")) == null)
				throw new NullReferenceException (nameof (_onPropertyChanged));

			//{System.Collections.ObjectModel.ObservableCollection`1[Xamarin.Forms.Element] get_InternalChildren()}	System.Reflection.MonoMethod
 		}

		public static void OnPropertyChanged(this Xamarin.Forms.Page self, string property) {
			_onPropertyChanged.Invoke(self, new object[] { property });
		}
		/*
		public static void InternalChildrenRemove(this Xamarin.Forms.Page self, Element element) {
			if (element == null)
				return;
			var internalChildren = (ObservableCollection<Xamarin.Forms.Element>)_getInternalChildren.Invoke (self, null);
			internalChildren.Remove (element);
		}

		public static void InternalChildrenAdd(this Xamarin.Forms.Page self, Element element) {
			if (element == null)
				return;
			var internalChildren = (ObservableCollection<Xamarin.Forms.Element>)_getInternalChildren.Invoke (self, null);
			internalChildren.Add (element);
		}
		*/

		public static ObservableCollection<Xamarin.Forms.Element> InternalChildren(this Xamarin.Forms.Page self) {
			return (ObservableCollection<Xamarin.Forms.Element>)_getInternalChildren.Invoke (self, null);
		}

		public static void SendAppearing(this Xamarin.Forms.Page self) {
			_sendAppearing.Invoke (self, null);
		}

		public static void SendDisappearing(this Xamarin.Forms.Page self) {
			_sendDisappearing.Invoke (self, null);
		}

	}
}

