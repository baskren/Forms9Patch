using System.Linq;
using System.ComponentModel;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Removed underline from a Xamarin.Forms.Entry
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class EntryNoOutlineEffect : RoutingEffect
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected EntryNoOutlineEffect() : base("Forms9Patch.EntryNoOutlineEffect")
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// Applies EntryNoUnderlineEffect to a Xamarin.Forms.Entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>true if the effect was applied or is already present</returns>
        public static bool ApplyTo(Entry entry)
        {
            if (entry.Effects.Any(e => e is EntryNoOutlineEffect))
                return true;
            if (new EntryNoOutlineEffect() is EntryNoOutlineEffect effect)
            {
                entry.Effects.Add(effect);
                return entry.Effects.Contains(effect);
            }
            return false;
        }
    }
}
