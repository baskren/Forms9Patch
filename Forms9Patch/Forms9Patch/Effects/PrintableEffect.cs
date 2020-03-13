using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace Forms9Patch
{
    [DesignTimeVisible(true)]
    public class PrintableEffect : RoutingEffect
    {
        protected PrintableEffect() : base("Forms9Patch.PrintableEffect")
        {
            Settings.ConfirmInitialization();
        }

        public static bool ApplyTo(WebView webView)
        {
            if (webView.Effects.Any(e => e is PrintableEffect))
                return true;
            if (new PrintableEffect() is PrintableEffect effect)
            {
                webView.Effects.Add(effect);
                return webView.Effects.Contains(effect);
            }
            return false;
        }
    }
}
