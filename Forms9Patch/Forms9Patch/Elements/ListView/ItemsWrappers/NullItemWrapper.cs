using System.ComponentModel;

namespace Forms9Patch
{
    [DesignTimeVisible(true)]
    class NullItemWrapper : ItemWrapper
    {
        public NullItemWrapper()
        {
            RemoveBinding(CellBackgroundColorProperty);
        }
    }

}

