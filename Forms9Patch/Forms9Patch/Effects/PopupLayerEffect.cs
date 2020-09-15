using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Forms9Patch
{
    [Preserve(AllMembers = true)]
    [DesignTimeVisible(true)]
    class PopupLayerEffect : Xamarin.Forms.RoutingEffect
    {
        public PopupBase PopupBase { get; private set; }

        static PopupLayerEffect()
        {
            Settings.ConfirmInitialization();
        }

        public PopupLayerEffect() : base("Forms9Patch.PopupLayerEffect")
        {

        }

        public static bool ApplyTo(PopupBase element)
        {
            if (element?._decorativeContainerView == null)
                return false;
            foreach (var existingEffect in ((VisualElement)element._decorativeContainerView).Effects)
                if (existingEffect is PopupLayerEffect)
                    return true;
            if (new PopupLayerEffect() is PopupLayerEffect effect)
            {
                effect.PopupBase = element;
                ((VisualElement)element._decorativeContainerView).Effects.Add(effect);
                var result = ((VisualElement)element._decorativeContainerView).Effects.Contains(effect);
                return result;
            }
            return false;
        }

        public static bool RemoveFrom(PopupBase element)
        {
            if (element?._decorativeContainerView == null)
                return false;
            for (int i = 0; i < ((VisualElement)element._decorativeContainerView).Effects.Count; i++)
                if (((VisualElement)element._decorativeContainerView).Effects[i] is PopupLayerEffect effect)
                {
                    effect.PopupBase = null;
                    ((VisualElement)element._decorativeContainerView).Effects.Remove(effect);
                    return true;
                }
            return false;
        }
    }
}