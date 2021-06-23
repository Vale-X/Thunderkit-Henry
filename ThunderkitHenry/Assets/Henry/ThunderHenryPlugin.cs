using BepInEx;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//Do a 'Find and Replace' on the ThunderHenry namespace. Make your own, please.
namespace ThunderHenry
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
    })]
    public class ThunderHenryPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.DeveloperName.MyCharacterMod";
        public const string MODNAME = "MyCharacterMod";
        public const string MODVERSION = "1.0.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "ROBVALE";

        public static ThunderHenryPlugin instance;

        public static bool debug = true;

        private void Awake()
        {
            instance = this;

            // Load/Configure assets and read Config
            Modules.Assets.Init();
            Modules.Tokens.Init();
            Modules.Prefabs.Init();
            Modules.ItemDisplays.Init();
            
            
            // Any debug stuff you need to do can go here before initialisation
            if (debug) { Modules.Helpers.AwakeDebug(); }

            //Initialize Content Pack
            Modules.ContentPackProvider.Initialize();
        }

    }
}
