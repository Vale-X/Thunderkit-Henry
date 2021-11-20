using BepInEx.Configuration;

namespace ThunderHenry.Modules
{
    class Config
    {
        //Config entry variables go here. Use these to get the config settings in Mod Manager.
        public static ConfigEntry<bool> characterEnabled;
        public static ConfigEntry<bool> forceUnlock;

        public static void ReadConfig()
        {
            //Template
            //ThunderHenryPlugin.instance.Config.Bind<bool>(new ConfigDefinition("section", "name"), false, new ConfigDescription("description"));

            //General
            characterEnabled = ThunderHenryPlugin.instance.Config.Bind<bool>(new ConfigDefinition("General", "Character Enabled"), true, new ConfigDescription("Set to false to disable this Survivor."));
            forceUnlock = ThunderHenryPlugin.instance.Config.Bind<bool>(new ConfigDefinition("General", "Force Unlock"), false, new ConfigDescription("Set to true to force this Survivor's content to be unlocked."));
        }
    }
}
