using System;

namespace ThunderHenry.Modules
{
    // StaticValues is a good place to put in any variables you might want to change at a moment's notice
    // good for easily making balance changes. Usually you'd have the body values (health, movement speed, etc) be here too,
    // but that's in the CharacterBody component instead.
    internal static class StaticValues
    {
        internal const float swordDamageCoefficient = 2.8f;

        internal const float gunDamageCoefficient = 4.2f;

        internal const float bombDamageCoefficient = 16f;

        internal const float uziDamageCoefficient = 0.75f;
        internal const float uziProcCoefficient = 0.75f;
    }
}