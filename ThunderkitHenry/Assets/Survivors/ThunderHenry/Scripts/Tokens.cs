using R2API;
using System;

namespace ThunderHenry.Modules
{
    internal static class Tokens
    {
        internal static void Init()
        {
            #region Henry
            string prefix = ThunderHenryPlugin.developerPrefix + "_THUNDERHENRY_BODY_";

            string desc = "Thunder Henry is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a new identity.";
            string outroFailure = "..and so he vanished, forever a blank slate.";

            LanguageAPI.Add(prefix + "NAME", "Thunder Henry");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Chosen One");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");

            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Thunder Henry passive");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            LanguageAPI.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>.");
            LanguageAPI.Add(prefix + "PRIMARY_PUNCH_NAME", "Boxing Gloves");
            LanguageAPI.Add(prefix + "PRIMARY_PUNCH_DESCRIPTION", Helpers.agilePrefix + $"Punch rapidly for <style=cIsDamage>{100f * 2.4f}% damage</style>. <style=cIsUtility>Ignores armor.</style>");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "UNLOCK_ACHIEVEMENT_NAME", "Thunderous Prelude");
            LanguageAPI.Add(prefix + "UNLOCK_ACHIEVEMENT_DESC", "Enter Distant Roost.");
            LanguageAPI.Add(prefix + "UNLOCK_UNLOCKABLE_NAME", "Thunderous Prelude");
            LanguageAPI.Add(prefix + "UNLOCK_MASTERY_ACHIEVEMENT_NAME", "Thunder Henry: Mastery");
            LanguageAPI.Add(prefix + "UNLOCK_MASTERY_ACHIEVEMENT_DESC", "As Thunder Henry, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "UNLOCK_MASTERY_UNLOCKABLE_NAME", "Thunder Henry: Mastery");

            LanguageAPI.Add(prefix + "UNLOCK_TEST_NAME", "WOOOO");
            LanguageAPI.Add(prefix + "UNLOCK_TEST_DESC", "FUCK YEAAAAH");
            LanguageAPI.Add(prefix + "UNLOCK_UNLOCK_NAME", "WOOOO");
            #endregion
            #endregion
        }
    }
}