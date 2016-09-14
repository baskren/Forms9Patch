using System;
namespace Forms9Patch.Droid
{
	public class TypeExtensions : ITypeExtensions
	{
		public bool IsValueType(Type type)
		{
			return type.IsValueType;
		}
	}
}

