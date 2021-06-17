#if UNITY_EDITOR
using System.Collections.Generic;

using EntityStates;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(SerializableEntityStateType), true)]
public class EntityStateEditor : PropertyDrawer
{
    private int selection = -2;
    private int Selection
    {
        get => selection;
        set
        {
            if (value == selection || value < 0 || value >= Options.Count)
            {
                return;
            }
            selection = value;
        }
    }
    private static List<Node> options;
    private static List<Node> Options { get => options ?? (options = GetOptions()); }
    private static GUIContent[] optionNames;
    private static GUIContent[] OptionNames { get => optionNames ?? (optionNames = Options.Select(el => new GUIContent(el.label)).ToArray()); }
    private static List<Node> GetOptions()
    {
        var nodes = new List<Node>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                nodes.AddRange(
                    assembly
                    .GetTypes()
                    .Where(type => type.IsSubclassOf(typeof(EntityState)) && !type.IsAbstract)
                    .Select(el => 
                        new Node 
                        {
                            label = $"{assembly.GetName().Name}/{el.FullName.Replace('.', '/')}",
                            fullName = $"{el.FullName}, {assembly.GetName().Name}"
                        }
                    ));
            }
            catch { }
        }
        return nodes.OrderBy(node => node.label).ToList();
    }
    private static readonly Dictionary<string, int> AssemblyHashes = new Dictionary<string, int>();

    public EntityStateEditor()
    {
        UpdateHashes();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        var typeNameProperty = property.FindPropertyRelative("_typeName");
        if (Selection == -2)
        {
            if (!string.IsNullOrWhiteSpace(typeNameProperty.stringValue))
            {
                Selection = Options.FindIndex(el => el.fullName == typeNameProperty.stringValue);
            }
            else
            {
                Selection = -1;
            }
        }
        var tempSelection = EditorGUI.Popup(position, new GUIContent(label.text, typeNameProperty.stringValue), Selection, OptionNames);
        if (tempSelection != Selection && (Selection = tempSelection) == tempSelection)
        {
            typeNameProperty.stringValue = Options[tempSelection].fullName;
        }
        EditorGUI.EndProperty();
    }

    private static void UpdateHashes()
    {
        var needUpdate = false;
        var tempAssemlyHashes = new Dictionary<string, int>();
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (!assembly.GetReferencedAssemblies().Any(el => el.Name == "Assembly-CSharp"))
            {
                continue;
            }
            AssemblyHashes[assembly.FullName] = assembly.GetHashCode();
            if (!AssemblyHashes.TryGetValue(assembly.FullName, out var hash) || hash != assembly.GetHashCode())
            {
                needUpdate = true;
            }
        }
        if (needUpdate)
        {
            AssemblyHashes.Clear();
            foreach (var row in tempAssemlyHashes)
            {
                AssemblyHashes[row.Key] = row.Value;
            }
            options = null;
            optionNames = null;
            _ = Options;
        }
    }

    private class Node
    {
        public string label;
        public string fullName;
    }
}
#endif