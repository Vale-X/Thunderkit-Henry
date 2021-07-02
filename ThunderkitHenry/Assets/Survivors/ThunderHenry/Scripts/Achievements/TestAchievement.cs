using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoR2.Achievements;
using RoR2.Networking;
using UnityEngine;

namespace ThunderHenry.Achievements
{
    public class TestHenryAchievement : BaseAchievement
    {
        public override void OnInstall()
        {
            base.OnInstall();

            GameNetworkManager.onServerSceneChangedGlobal += TestCheck;
        }

        private void TestCheck(string obj)
        {
            if (obj == "bazaar")
            {
                base.Grant();
                Debug.LogWarning("Bazaar!!!");
            }
            else Debug.LogWarning("NotInBazaar");
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            GameNetworkManager.onServerSceneChangedGlobal -= TestCheck;
        }
    }
}
