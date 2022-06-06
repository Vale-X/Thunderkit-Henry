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
    [BepInDependency("com.valex.ShaderConverter", BepInDependency.DependencyFlags.HardDependency)]
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
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.valex.ThunderHenry";
        public const string MODNAME = "ThunderHenry";
        public const string MODVERSION = "1.1.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "ROBVALE";

        // use this to toggle debug on stuff, make sure it's false before releasing
        public static bool debug = true;

        public static bool cancel;

        public static ThunderHenryPlugin instance;

        private void Awake()
        {
            instance = this;

            // Load/Configure assets and read Config
            Modules.Config.ReadConfig();
            Modules.Assets.Init();
            if (cancel) return;
            Modules.Tokens.Init();
            Modules.Prefabs.Init();
            Modules.Buffs.Init();
            Modules.ItemDisplays.Init();
            
            
            // Any debug stuff you need to do can go here before initialisation
            if (debug) { Modules.Helpers.AwakeDebug(); }

            //Initialize Content Pack
            Modules.ContentPackProvider.Initialize();

            Hook();
        }

        private void Start()
        {
            // If Awake isn't the right place for launch debug, you can put some in Start here.
            // Most of the time Awake will do fine though.
            if (debug) { Modules.Helpers.StartDebug(); }
        }

        private void Hook()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            // a simple stat hook, adds armor after stats are recalculated
            if (self)
            {
                Modules.Buffs.HandleBuffs(self);
                Modules.Buffs.HandleDebuffs(self);
            }
        }
    }
}
