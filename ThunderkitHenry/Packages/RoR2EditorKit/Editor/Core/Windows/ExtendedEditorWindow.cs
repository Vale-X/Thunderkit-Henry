using RoR2EditorKit.Common;
using RoR2EditorKit.Settings;
using UnityEditor;
using UnityEditor.UIElements;

namespace RoR2EditorKit.Core.EditorWindows
{
    using static ThunderKit.Core.UIElements.TemplateHelpers;

    /// <summary>
    /// Base EditorWindow for all the RoR2EditorKit Editor Windows. Uses VisualElements instead of IMGUI
    /// <para>Automatically retrieves the UXML asset for the editor by looking for an asset with the same name as the inheriting type</para>
    /// <para>If you want to create an EditorWindow for editing an object, you'll probably want to use the <see cref="ObjectEditingEditorWindow{TObject}"/></para>
    /// </summary>
    public abstract class ExtendedEditorWindow : EditorWindow
    {
        /// <summary>
        /// RoR2EK's main settings file
        /// </summary>
        public static RoR2EditorKitSettings Settings { get => RoR2EditorKitSettings.GetOrCreateSettings<RoR2EditorKitSettings>(); }

        /// <summary>
        /// The serialized object for this window
        /// </summary>
        protected SerializedObject SerializedObject { get; set; }

        /// <summary>
        /// Opens an ExtendedEditorWindow and sets it's <see cref="SerializedObject"/> to the new ExtendedEditorWindow instance
        /// </summary>
        /// <typeparam name="TEditorWindow">The type of ExtendedEditorWindow to open</typeparam>
        /// <param name="windowName">The name for this window, leaving this null nicifies the <typeparamref name="TEditorWindow"/>'s type name</param>
        public static void OpenEditorWindow<TEditorWindow>(string windowName = null) where TEditorWindow : ExtendedEditorWindow
        {
            TEditorWindow window = GetWindow<TEditorWindow>(windowName == null ? ObjectNames.NicifyVariableName(typeof(TEditorWindow).Name) : windowName);
            window.SerializedObject = new SerializedObject(window);
            window.OnWindowOpened();
        }

        /// <summary>
        /// Finish any initialization here
        /// Keep base implementation unless you know what you're doing.
        /// <para>OnWindowOpened binds the root visual element to the <see cref="SerializedObject"/></para>
        /// <para>Execution order: OnEnable -> CreateGUI -> OnWindowOpened</para>
        /// </summary>
        protected virtual void OnWindowOpened()
        {
            rootVisualElement.Bind(SerializedObject);
        }

        /// <summary>
        /// Create or finalize your VisualElement UI here.
        /// Keep base implementation unless you know what you're doing.
        /// <para>RoR2EditorKit copies the VisualTreeAsset to the rootVisualElement in this method.</para>
        /// <para>Execution order: OnEnable -> CreateGUI -> OnWindowOpened</para>
        /// </summary>
        protected virtual void CreateGUI()
        {
            base.rootVisualElement.Clear();
            GetTemplateInstance(GetType().Name, rootVisualElement, ValidateUXMLPath);
        }

        /// <summary>
        /// Used to validate the path of a potential UXML asset, overwrite this if youre making a window that isnt in the same assembly as RoR2EK.
        /// </summary>
        /// <param name="path">A potential UXML asset path</param>
        /// <returns>True if the path is for this editor window, false otherwise</returns>
        protected virtual bool ValidateUXMLPath(string path)
        {
            return path.StartsWith(Constants.PackageFolderPath);
        }
    }
}