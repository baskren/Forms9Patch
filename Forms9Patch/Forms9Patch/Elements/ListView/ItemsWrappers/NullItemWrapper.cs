using System.ComponentModel;

namespace Forms9Patch
{
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    class NullItemWrapper : ItemWrapper
    {
        public NullItemWrapper()
        {
            RemoveBinding(CellBackgroundColorProperty);
        }
    }

}

