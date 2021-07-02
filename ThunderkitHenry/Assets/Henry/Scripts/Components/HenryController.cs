using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace ThunderHenry.Modules.Components
{
    // just a class to run some custom code for things like weapon models
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

        private void CheckWeapon()
        {
            switch (this.characterBody.skillLocator.primary.skillDef.skillNameToken)
            {
                default:
                    this.childLocator.FindChild("SwordModel").gameObject.SetActive(true);
                    this.childLocator.FindChild("BoxingGloveL").gameObject.SetActive(false);
                    this.childLocator.FindChild("BoxingGloveR").gameObject.SetActive(false);
                    this.modelAnimator.SetLayerWeight(this.modelAnimator.GetLayerIndex("Body, Alt"), 0f);
                    break;
                case ThunderHenryPlugin.developerPrefix + "_THUNDERHENRY_BODY_PRIMARY_PUNCH_NAME":
                    this.childLocator.FindChild("SwordModel").gameObject.SetActive(false);
                    this.childLocator.FindChild("BoxingGloveL").gameObject.SetActive(true);
                    this.childLocator.FindChild("BoxingGloveR").gameObject.SetActive(true);
                    this.modelAnimator.SetLayerWeight(this.modelAnimator.GetLayerIndex("Body, Alt"), 1f);
                    break;
            }
        }
    }
}