using System;
using Xamarin.Forms;

namespace Forms9Patch
{
	public static class TypeExtensions
	{
		static ITypeExtensions _service;

		static ITypeExtensions Service()
		{
			_service = _service ?? DependencyService.Get<ITypeExtensions>();
			if (_service == null)
				throw new TypeLoadException("Cannot load IListViewExtensionService dependency service");
			return _service;
		}

		static bool IsValueType(this Type type)
		{
			return Service().IsValueType(type);
		}
	}
}

