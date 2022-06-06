using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.AddressableAssets;

namespace RoR2EditorKit.Utilities
{
    /// <summary>
    /// Utilities for loading an asset via addressables.
    /// </summary>
    public static class AddressablesUtils
    {
        /// <summary>
        /// Returns true if ThunderKit has loaded the AddressableCatalog
        /// <para>Basically when the ScriptingDefine symbols contain "TK_ADDRESSABLE"</para>
        /// </summary>
        public static bool AddressableCatalogExists => ContainsDefine("TK_ADDRESSABLE");

        /// <summary>
        /// Loads an asset of type <typeparamref name="T"/> from the AddressableCatalog, while the loading process is running, it shows a progress bar popup.
        /// </summary>
        /// <typeparam name="T">The type of asset to load, must be an Unity Object</typeparam>
        /// <param name="address">The address of the aset</param>
        /// <returns>A Task that can be awaited for obtaining the loaded object</returns>
        /// <exception cref="InvalidOperationException">Thrown when the ScriptingDefains do not contain the "TK_ADDRESSABLE" defaine.</exception>
        public static async Task<T> LoadAssetFromCatalog<T>(object address) where T : UnityEngine.Object
        {
            if (AddressableCatalogExists)
                throw new InvalidOperationException($"Cannot load asset because ThunderKit has not imported the addressable catalog!");

            using (var pb = new ThunderKit.Common.Logging.ProgressBar("Loading Object"))
            {
                var op = Addressables.LoadAssetAsync<T>(address);
                while (!op.IsDone)
                {
                    await Task.Delay(100);
                    pb.Update($"Loading asset from address {address}, this may take a while", null, op.PercentComplete);
                }
                return op.Result;
            }
        }

        private static bool ContainsDefine(string define)
        {
            foreach (BuildTargetGroup targetGroup in System.Enum.GetValues(typeof(BuildTargetGroup)))
            {
                if (targetGroup == BuildTargetGroup.Unknown || IsObsolete(targetGroup))
                    continue;

                string defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

                if (!defineSymbols.Contains(define))
                    return false;
            }

            return true;


            bool IsObsolete(BuildTargetGroup group)
            {
                var attrs = typeof(BuildTargetGroup).GetField(group.ToString()).GetCustomAttributes(typeof(ObsoleteAttribute), false);
                return attrs.Length > 0;
            }
        }
    }
}