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
    public class EditorNoUnderlineEffect : RoutingEffect
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected EditorNoUnderlineEffect() : base("Forms9Patch.EditorNoUnderlineEffect")
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// Applies EntryNoUnderlineEffect to a Xamarin.Forms.Entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>true if the effect was applied or is already present</returns>
        public static bool ApplyTo(Editor entry)
        {
            if (entry.Effects.Any(e => e is EditorNoUnderlineEffect))
                return true;
            if (new EditorNoUnderlineEffect() is EditorNoUnderlineEffect effect)
            {
                entry.Effects.Add(effect);
                return entry.Effects.Contains(effect);
            }
            return false;
        }
    }
}
