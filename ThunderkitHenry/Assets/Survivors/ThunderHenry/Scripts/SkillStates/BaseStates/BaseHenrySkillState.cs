using EntityStates;
using ThunderHenry.Modules.Components;

namespace ThunderHenry.SkillStates
{
    public class BaseHenrySkillState : BaseSkillState
    {
        protected HenryController henryController;

        public override void OnEnter()
        {
            this.henryController = base.GetComponent<HenryController>();
            base.OnEnter();
        }
    }
}