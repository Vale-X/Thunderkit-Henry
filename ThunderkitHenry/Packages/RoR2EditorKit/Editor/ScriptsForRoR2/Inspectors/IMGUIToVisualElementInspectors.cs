﻿using RoR2.Skills;
using RoR2EditorKit.Core.Inspectors;
using UnityEditor;

namespace RoR2EditorKit.RoR2Related.Inspectors
{
    [CustomEditor(typeof(SkillFamily))]
    public sealed class SkillFamilyInspector : IMGUIToVisualElementInspector<SkillFamily> { }
}