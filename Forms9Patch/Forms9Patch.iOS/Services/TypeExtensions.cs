using System;
namespace Forms9Patch.iOS
{
	public class TypeExtensions : ITypeExtensions
	{
		public bool IsValueType(Type type)
		{
			return type.IsValueType;
		}
	}
}

