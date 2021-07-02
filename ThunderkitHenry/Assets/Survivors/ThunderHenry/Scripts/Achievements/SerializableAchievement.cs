using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoR2;
using RoR2.Achievements;
using UnityEngine;

namespace ThunderHenry.Achievements
{
    [Serializable]
    public struct SerializableAchievement
    {
        [SerializeField]
        private string _achievementName;

        public SerializableAchievement(string achievementName)
        {
            this._achievementName = "";
            this.achievementName = achievementName;
        }

        public SerializableAchievement(Type achievementType)
        {
            this._achievementName = "";
            this.achievementType = achievementType;
        }

        public string achievementName
        {
            get
            {
                return this._achievementName;
            }
            private set
            {
                this.achievementType = Type.GetType(value);
            }
        }

        public Type achievementType
        {
            get
            {
                if (this._achievementName == null)
                {
                    return null;
                }
                Type type = Type.GetType(this._achievementName);
                if (!(type != null) || !type.IsSubclassOf(typeof(BaseAchievement)))
                {
                    return null;
                }
                return type;
            }
            set
            {
                this._achievementName = ((value != null && value.IsSubclassOf(typeof(BaseAchievement))) ? value.AssemblyQualifiedName : "");
            }
        }
    }
}
