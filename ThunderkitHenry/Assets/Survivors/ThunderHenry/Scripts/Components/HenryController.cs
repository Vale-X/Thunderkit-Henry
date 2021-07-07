using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace ThunderHenry.Modules.Components
{
    // just a class to run some custom code for things like weapon models
    // Attached as a component onto ThunderHenryBody.
    public class HenryController : MonoBehaviour
    {
        private CharacterBody characterBody;
        private CharacterModel model;
        private ChildLocator childLocator;
        private Animator modelAnimator;

        private void Awake()
        {
            this.characterBody = this.gameObject.GetComponent<CharacterBody>();
            this.childLocator = this.gameObject.GetComponentInChildren<ChildLocator>();
            this.model = this.gameObject.GetComponentInChildren<CharacterModel>();
            this.modelAnimator = this.gameObject.GetComponentInChildren<Animator>();

            Debug.LogWarning(this.characterBody);
            Debug.LogWarning(this.childLocator);
            Debug.LogWarning(this.model);
            Debug.LogWarning(this.modelAnimator);

            Invoke("CheckWeapon", 0.2f);
        }

        // Run as an Invoke to give time for everything else to be set up and sorted.
        private void CheckWeapon()
        {
            if (this.characterBody.skillLocator.secondary.skillDef.skillNameToken == ThunderHenryPlugin.developerPrefix + "_THUNDERHENRY_BODY_SECONDARY_UZI_NAME")
            {
                this.childLocator.FindChild("GunModel").GetComponent<SkinnedMeshRenderer>().sharedMesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshUzi");
            }

        }
    }
}