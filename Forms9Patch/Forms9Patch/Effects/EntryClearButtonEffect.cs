using System.Linq;
using System.ComponentModel;
using Xamarin.Forms;

namespace Forms9Patch
{
    /// <summary>
    /// Adds a clear button to a Xamarin.Forms.Entry
    /// </summary>
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    public class EntryClearButtonEffect : RoutingEffect
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected EntryClearButtonEffect() : base("Forms9Patch.EntryClearButtonEffect")
        {
            Settings.ConfirmInitialization();
        }

        /// <summary>
        /// Applies EntryClearButtonEffect to a Xamarin.Forms.Entry
        /// </summary>
        /// <param name="entry"></param>
        /// <returns>true if the effect was applied or is already present</returns>
        public static bool ApplyTo(Entry entry)
        {
            if (entry.Effects.Any(e => e is EntryClearButtonEffect))
                return true;
            if (new EntryClearButtonEffect() is EntryClearButtonEffect effect)
            {
                entry.Effects.Add(effect);
                return entry.Effects.Contains(effect);
            }
            return false;
        }
    }
}
