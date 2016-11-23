using System;
namespace Forms9Patch
{
	public class DataTemplateSelector : GroupTemplate
	{
		public DataTemplateSelector() : base() { }

		public new void Add(Type itemBaseType, Type viewType)
		{
			base.Add(itemBaseType,viewType);
		}


	}
}
