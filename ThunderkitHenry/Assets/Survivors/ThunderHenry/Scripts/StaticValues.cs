using System;

namespace ThunderHenry.Modules
{
    // StaticValues is used more heavily in code-based projects, but for ThunderKit based projects
    // most of the values are handled by EntityStateConfigurations (see ThunderHenry/Definitions/Skills/StateConfigs in Assets)
    // In ThunderHenry's case, it's used for buff info and Token values.
    internal static class StaticValues
    {
        // Armor Buff
        internal const float armorBuffDuration = 3f;
        internal const float armorBuffValue = 300f;

        // Speed Buff
        internal const float speedBuffDuration = 5f;
        internal const int speedBuffMaxStacks = 20;
        internal const float speedBuffCoefficient = 0.035f;

        // Token values
        internal const float swordDamageCoefficient = 2.8f;
        internal const float gunDamageCoefficient = 4.2f;
        internal const float bombDamageCoefficient = 16f;
        internal const float uziDamageCoefficient = 0.75f;
    }
}