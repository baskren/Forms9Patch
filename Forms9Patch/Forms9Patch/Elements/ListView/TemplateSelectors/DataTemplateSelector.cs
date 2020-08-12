using System;
using System.ComponentModel;

namespace Forms9Patch
{
    /// <summary>
    /// The Forms9Patch Data template selector.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class DataTemplateSelector : TemplateSelectorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Forms9Patch.DataTemplateSelector"/> class.
        /// </summary>
        public DataTemplateSelector() : base() { }

        /// <summary>
        /// Add the specified itemBaseType and viewType.
        /// </summary>
        /// <param name="itemBaseType">Item base type.</param>
        /// <param name="viewType">View type.</param>
        public new void Add(Type itemBaseType, Type viewType)
        {
            base.Add(itemBaseType, viewType);
        }


    }
}
