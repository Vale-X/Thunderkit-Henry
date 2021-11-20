using StubbedConverter;
using System.Collections.Generic;
using UnityEngine;

namespace ThunderHenry.Modules
{
    public class Shaders
    {
        internal static List<Material> materialStorage = new List<Material>();

        internal static void init()
        {
            ConvertBundleMaterials(Modules.Assets.mainAssetBundle);
            CreateMaterialStorage(Modules.Assets.mainAssetBundle);
        }

        // Uses StubbedShaderConverter to convert all materials within the Asset Bundle on Awake().
        // This now sorts out CloudRemap materials as well! Woo!
        private static void ConvertBundleMaterials(AssetBundle inAssetBundle)
        {
            ShaderConverter.ConvertStubbedShaders(inAssetBundle);
        }

        private static void CreateMaterialStorage(AssetBundle inAssetBundle)
        {
            Material[] tempArray = inAssetBundle.LoadAllAssets<Material>();

            materialStorage.AddRange(tempArray);
        }

        public static Material GetMaterialFromStorage(string matName)
        {
            return materialStorage.Find(x => x.name == matName);
        }
    }
}
