using RoR2EditorKit.Common;
using RoR2EditorKit.Settings;
using RoR2EditorKit.Utilities;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace RoR2EditorKit.Core.Inspectors
{
    using static ThunderKit.Core.UIElements.TemplateHelpers;

    /// <summary>
    /// Data that defines a ContextMenu that's going to be handled by the ExtendedInspector
    /// </summary>
    public struct ContextMenuData
    {
        /// <summary>
        /// The menu name for this contextMenu
        /// </summary>
        public string menuName;
        /// <summary>
        /// The action that runs when the context menu is clicked
        /// </summary>
        public Action<DropdownMenuAction> menuAction;
        /// <summary>
        /// A status check to see if the menu should appear or not
        /// </summary>
        public Func<DropdownMenuAction, DropdownMenuAction.Status> actionStatusCheck;

        /// <summary>
        /// ContextMenuData Constructor
        /// </summary>
        /// <param name="name">The name of the Context Menu</param>
        /// <param name="action">An action that runs when the Context Menu is clicked</param>
        public ContextMenuData(string name, Action<DropdownMenuAction> action)
        {
            menuName = name;
            menuAction = action;
            actionStatusCheck = x => DropdownMenuAction.Status.Normal;
        }

        /// <summary>
        /// ContextMenuData Constructor
        /// </summary>
        /// <param name="name">The name of the ContextMenu</param>
        /// <param name="action">An action that runs when the ContextMenu is clicked</param>
        /// <param name="statusCheck">A function to check if the ContextMenu is clickable or not</param>
        public ContextMenuData(string name, Action<DropdownMenuAction> action, Func<DropdownMenuAction, DropdownMenuAction.Status> statusCheck)
        {
            menuName = name;
            menuAction = action;
            actionStatusCheck = statusCheck;
        }
    }

    /// <summary>
    /// Base inspector for all the RoR2EditorKit Inspectors. Uses visual elements instead of IMGUI
    /// <para>Automatically retrieves the UXML asset for the editor by looking for an UXML asset with the same name as the inheriting type</para>
    /// <para>Extended Inspectors can be enabled or disabled</para>
    /// <para>If you want to make a Scriptable Object Inspector, you'll probably want to use the <see cref="ScriptableObjectInspector{T}"/></para>
    /// <para>If you want to make an Inspector for a Component, you'll probably want to use the <see cref="ComponentInspector{T}"/></para>
    /// </summary>
    /// <typeparam name="T">The type of Object being inspected</typeparam>
    public abstract class ExtendedInspector<T> : Editor where T : Object
    {
        #region Properties
        /// <summary>
        /// Access to the main RoR2EditorKit Settings file
        /// </summary>
        public static RoR2EditorKitSettings Settings { get => RoR2EditorKitSettings.GetOrCreateSettings<RoR2EditorKitSettings>(); }

        /// <summary>
        /// The setting for this inspector
        /// </summary>
        public EditorInspectorSettings.InspectorSetting InspectorSetting
        {
            get
            {
                if (_inspectorSetting == null)
                {
                    _inspectorSetting = Settings.InspectorSettings.GetOrCreateInspectorSetting(GetType()); ;
                }
                return _inspectorSetting;
            }
            set
            {
                if (_inspectorSetting != value)
                {
                    var index = Settings.InspectorSettings.inspectorSettings.IndexOf(_inspectorSetting);
                    Settings.InspectorSettings.inspectorSettings[index] = value;
                    _inspectorSetting = value;
                }
            }
        }
        private EditorInspectorSettings.InspectorSetting _inspectorSetting;

        /// <summary>
        /// Check if the inspector is enabled
        /// <para>If you're setting the value, and the value is different from the old value, the inspector will redraw completely to accomodate the new look using either the base inspector or custom inspector</para>
        /// </summary>
        public bool InspectorEnabled
        {
            get
            {
                return InspectorSetting.isEnabled;
            }
            set
            {
                if (value != InspectorSetting.isEnabled)
                {
                    InspectorSetting.isEnabled = value;
                    OnInspectorEnabledChange();
                }
            }
        }

        /// <summary>
        /// The root visual element of the inspector, This is what gets returned by CreateInspectorGUI()
        /// <para>When the inspector is enabled, the "DrawInspectorElement" is added to this</para>
        /// <para>When the inspector is disabled, the "IMGUIContainerElement" with the default inspector is added to this.</para>
        /// </summary>
        protected VisualElement RootVisualElement
        {
            get
            {
                if (_rootVisualElement == null)
                {
                    _rootVisualElement = new VisualElement();
                    _rootVisualElement.name = "ExtendedInspector_RootElement";
                }

                return _rootVisualElement;
            }
        }
        private VisualElement _rootVisualElement;

        /// <summary>
        /// The root visual element where your custom inspector will be drawn.
        /// <para>This visual element will have the VisualTreeAsset applied.</para>
        /// </summary>
        protected VisualElement DrawInspectorElement
        {
            get
            {
                if (_drawInspectorElement == null)
                {
                    _drawInspectorElement = new VisualElement();
                    _drawInspectorElement.name = "ExtendedInspector_CustomEditor";
                }
                return _drawInspectorElement;
            }
        }
        private VisualElement _drawInspectorElement;

        /// <summary>
        /// The root visual element where the default, IMGUI inspector is drawn
        /// <para>This visual element will not have the VisualTreeAsset applied</para>
        /// <para>The IMGUIContainer that gets returned by the default inspector is added to this, it's name is "defaultInspector" if you need to Query it.</para>
        /// </summary>
        protected VisualElement IMGUIContainerElement
        {
            get
            {
                if (_imguiContianerElement == null)
                {
                    _imguiContianerElement = new VisualElement();
                    _imguiContianerElement.name = "ExtendedInspector_DefaultInspector";
                }
                return _imguiContianerElement;
            }
        }
        private VisualElement _imguiContianerElement;

        /// <summary>
        /// Wether the inspector has done its first drawing.
        /// <para>When the inspector draws for the first time, unity calls Bind() on <see cref="RootVisualElement"/>, this creates all the necesary fields for property fields, however, this runs only once.</para>
        /// <para>When HasDoneFirstDrawing is true, the ExtendedInspector will call Bind() to ensure property fields always appear.</para>
        /// </summary>
        protected bool HasDoneFirstDrawing { get => _hasDoneFirstDrawing; private set => _hasDoneFirstDrawing = value; }
        private bool _hasDoneFirstDrawing = false;

        /// <summary>
        /// Direct access to the object that's being inspected as its type.
        /// </summary>
        protected T TargetType { get => target as T; }

        /// <summary>
        /// If the editor has a visual tree asset, if set to false, RoR2EK will supress the null reference exception that appears from not having one.
        /// </summary>
        protected virtual bool HasVisualTreeAsset { get; } = true;
        #endregion Properties

        #region Fields
        private IMGUIContainer prefixContainer = null;
        private Dictionary<VisualElement, (ContextualMenuManipulator, List<ContextMenuData>)> elementToContextMenu = new Dictionary<VisualElement, (ContextualMenuManipulator, List<ContextMenuData>)>();
        #endregion Fields

        #region Methods
        /// <summary>
        /// Called when the inspector is enabled, always keep the original implementation unless you know what youre doing
        /// </summary>
        protected virtual void OnEnable()
        {
            EditorApplication.projectChanged += OnObjectNameChanged;
        }

        /// <summary>
        /// Called when the inspector is disabled, always keepp the original implementation unless you know what you're doing
        /// </summary>
        protected virtual void OnDisable()
        {
            EditorApplication.projectChanged -= OnObjectNameChanged;
        }

        private void OnObjectNameChanged()
        {
            if (this == null || serializedObject == null || serializedObject.targetObject == null)
                return;

            if (serializedObject.targetObject && Settings.InspectorSettings.enableNamingConventions && this is IObjectNameConvention objNameConvention)
            {
                if (serializedObject.targetObject.name.StartsWith(objNameConvention.Prefix))
                {
                    prefixContainer?.RemoveFromHierarchy();
                    prefixContainer = null;
                    return;
                }
                else if (prefixContainer == null)
                {
                    prefixContainer = EnsureNamingConventions(objNameConvention);
                    RootVisualElement.Add(prefixContainer);
                    prefixContainer.SendToBack();
                }
                else
                {
                    prefixContainer.RemoveFromHierarchy();
                    RootVisualElement.Add(prefixContainer);
                    prefixContainer.SendToBack();
                }
            }
        }

        private void OnInspectorEnabledChange()
        {
            void ClearElements()
            {
                DrawInspectorElement.Wipe();
                IMGUIContainerElement.Wipe();
                RootVisualElement.Wipe();
            }

            ClearElements();
            OnRootElementsCleared?.Invoke();

            try
            {
                GetTemplateInstance(GetType().Name, DrawInspectorElement, ValidateUXMLPath);
            }
            catch (Exception ex)
            {
                if (HasVisualTreeAsset)
                {
                    Debug.LogError(ex);
                }
            }

            OnVisualTreeCopy?.Invoke();

            OnObjectNameChanged();

            if (!InspectorEnabled)
            {
                var defaultImguiContainer = new IMGUIContainer(OnInspectorGUI);
                defaultImguiContainer.name = "defaultInspector";
                IMGUIContainerElement.Add(defaultImguiContainer);
                RootVisualElement.Add(IMGUIContainerElement);
                OnIMGUIContainerElementAdded?.Invoke();
            }
            else
            {
                DrawInspectorGUI();
                RootVisualElement.Add(DrawInspectorElement);
                OnDrawInspectorElementAdded?.Invoke();
                if (_hasDoneFirstDrawing)
                {
                    RootVisualElement.Bind(serializedObject);
                }
            }
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Used to validate the path of a potential UXML asset, overwrite this if youre making an inspector that isnt in the same assembly as RoR2EK.
        /// </summary>
        /// <param name="path">A potential UXML asset path</param>
        /// <returns>True if the path is for this inspector, false otherwise</returns>
        protected virtual bool ValidateUXMLPath(string path)
        {
            return path.Contains(Constants.PackageName);
        }

        /// <summary>
        /// Cannot be overwritten, creates the inspector by checking if the editor is enabled or not
        /// <para>If the editor is enabled, the custom UI from the visual tree asset is drawn, to finish the implementation of said UI, implement <see cref="DrawInspectorGUI"/></para>
        /// <para>If the editor is disabled, the default IMGUI UI is drawn.</para>
        /// </summary>
        /// <returns></returns>
        public sealed override VisualElement CreateInspectorGUI()
        {
            OnInspectorEnabledChange();
            serializedObject.ApplyModifiedProperties();
            _hasDoneFirstDrawing = true;
            return RootVisualElement;
        }

        public sealed override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }

        private IMGUIContainer EnsureNamingConventions(IObjectNameConvention objectNameConvention)
        {
            PrefixData prefixData = objectNameConvention.GetPrefixData();

            IMGUIContainer container = new IMGUIContainer(() =>
            {
                EditorGUILayout.HelpBox($"This {typeof(T).Name}'s name should start with \"{objectNameConvention.Prefix}\" so it follows naming conventions", MessageType.Info);
            });

            container.tooltip = prefixData.tooltipMessage;

            container.AddManipulator(new ContextualMenuManipulator((menuBuilder) =>
            {
                menuBuilder.menu.AppendAction("Fix naming convention", (action) =>
                {
                    prefixData.contextMenuAction();
                    OnObjectNameChanged();
                });
            }));

            return container;
        }
        #endregion Methods

        #region Delegates
        /// <summary>
        /// Invoked when the RootVisualElement, DrawInspectorElement and IMGUIContainerElement are cleared;
        /// </summary>
        protected Action OnRootElementsCleared;

        /// <summary>
        /// Invoked when the VisualTree assigned to this inspector has been copied to the "DrawInspectorElement"
        /// </summary>
        protected Action OnVisualTreeCopy;

        /// <summary>
        /// Invoked right after "IMGUIContainerElement" is added to the "RootVisualElement"
        /// </summary>
        protected Action OnIMGUIContainerElementAdded;

        /// <summary>
        /// Invoked right after the "DrawInspectorElement" is added to the "RootVisualElement"
        /// </summary>
        protected Action OnDrawInspectorElementAdded;
        #endregion

        /// <summary>
        /// Implement The code functionality of your inspector here.
        /// </summary>
        protected abstract void DrawInspectorGUI();

        #region Util Methods
        /// <summary>
        /// Creates a HelpBox and attatches it to a visualElement using IMGUIContainer
        /// </summary>
        /// <param name="message">The message that'll appear on the help box</param>
        /// <param name="messageType">The type of message</param>
        /// <param name="attachToRootIfElementToAttachIsNull">If left true, and the elementToAttach is not null, the IMGUIContainer is added to the RootVisualElement.</param>
        /// <param name="elementToAttach">Optional, if specified, the Container will be added to this element, otherwise if the "attachToRootIfElementToAttachIsNull" is true, it'll attach it to the RootVisualElement, otherwise if both those conditions fail, it returns the IMGUIContainer unattached.</param>
        /// <returns>An IMGUIContainer that's either not attached to anything, attached to the RootElement, or attached to the elementToAttach argument.</returns>
        protected IMGUIContainer CreateHelpBox(string message, MessageType messageType)
        {
            IMGUIContainer container = new IMGUIContainer();
            container.name = $"ExtendedInspector_HelpBox";
            container.onGUIHandler = () =>
            {
                EditorGUILayout.HelpBox(message, messageType);
            };

            return container;
        }

        /// <summary>
        /// Adds a ContextMenu to a visual element using RoR2EK's <see cref="ContextMenuData"/> wrapper
        /// </summary>
        /// <param name="element">The element that's going to be used for the ContextMenu</param>
        /// <param name="contextMenuData">The data for the ContextMenu</param>
        protected void AddSimpleContextMenu(VisualElement element, ContextMenuData contextMenuData)
        {
            if (!elementToContextMenu.ContainsKey(element))
            {
                var manipulator = new ContextualMenuManipulator(x => CreateMenu(element, x));
                elementToContextMenu.Add(element, (manipulator, new List<ContextMenuData>()));
                element.AddManipulator(manipulator);
            }
            var tuple = elementToContextMenu[element];
            if (!tuple.Item2.Contains(contextMenuData))
                tuple.Item2.Add(contextMenuData);
        }

        private void CreateMenu(VisualElement element, ContextualMenuPopulateEvent populateEvent)
        {
            var contextMenus = elementToContextMenu[element].Item2;

            foreach (ContextMenuData data in contextMenus)
            {
                populateEvent.menu.AppendAction(data.menuName, data.menuAction, data.actionStatusCheck);
            }
        }
        #endregion
    }
}
