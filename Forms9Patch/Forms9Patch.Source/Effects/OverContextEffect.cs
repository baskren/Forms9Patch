using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
    class OverContextEffect : Xamarin.Forms.RoutingEffect
    {
        static OverContextEffect()
        {
            Settings.ConfirmInitialization();
        }

        public OverContextEffect() : base("Forms9Patch.OverContextEffect")
        {

        }

        public static bool ApplyTo(VisualElement element)
        {
            foreach (var existingEffect in element.Effects)
                if (existingEffect is OverContextEffect)
                    return true;
            if (new OverContextEffect() is OverContextEffect effect)
            {
                element.Effects.Add(effect);
                var result = element.Effects.Contains(effect);
                return result;
            }
            return false;
        }
    }
}