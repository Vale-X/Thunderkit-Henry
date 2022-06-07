using BepInEx;
using R2API.Utils;
using RoR2;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//Do a 'Find and Replace' on the ThunderHenry namespace. Make your own namespace, please.
namespace ThunderHenry
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "LanguageAPI",
        "SoundAPI",
        "UnlockableAPI"
    })]
    public class ThunderHenryPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        // please change the names to your own stuff, thanks
        public const string MODUID = "com.valex.ThunderHenry";
        public const string MODNAME = "ThunderHenry";
        public const string MODVERSION = "2.0.0";

        // a prefix for name tokens to prevent conflicts - please capitalize all name tokens for convention
        public const string developerPrefix = "ROBVALE";

        // Use this to toggle debug for development, make sure it's false before releasing
        public static bool debug = true;

        public static bool cancel;

        public static ThunderHenryPlugin instance;

        private void Awake()
        {
            // For referring to the instantiated Plugin script from anywhere.
            instance = this;

            // Load/Configure assets and read Config
            Modules.Config.ReadConfig();
            Modules.Assets.Init();

            // If something goes wrong with initialization, stop the awake script (stopping the mod from being added).
            if (cancel) return;

            // Any additional 'Module' scripts you make can be initialized here.
            Modules.Tokens.Init();
            Modules.Prefabs.Init();
            Modules.Buffs.Init();
            Modules.ItemDisplays.Init();
            
            
            // Awake() debug for whatever you need.
            if (debug) { Modules.Helpers.AwakeDebug(); }

            // Initialize the content pack, making all the content available for the game to use.
            Modules.ContentPackProvider.Initialize();

            // For hooking to the scripts (RecalculateStats, in this case).
            Hook();
        }

        private void Start()
        {
            // If Awake() isn't the right place for launch debug, you can put it in Start() here.
            if (debug) { Modules.Helpers.StartDebug(); }
        }

        private void Hook()
        {
            // Hook onto Recalculate Stats so we can handle buffs, debuffs and other stat changes.
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            // Redirects RecalculateStats to the Buffs script, for organization purposes.
            if (self)
            {
                Modules.Buffs.HandleBuffs(self);
                Modules.Buffs.HandleDebuffs(self);
            }
        }
    }
}
