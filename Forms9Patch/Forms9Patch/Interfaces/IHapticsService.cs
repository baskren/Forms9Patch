using System;

namespace Forms9Patch.Interfaces
{
    public interface IHapticsService
    {
        void Feedback(HapticEffect effect, EffectMode mode = EffectMode.Default);
    }
}
