### '2.2.1'

* General Changes:
	* Updated AssemblyDefinitions to reference via name instead of GUIDs (Thanks Passive Picasso!)

### '2.2.0'

* General Changes
* Updated to use TK 5.0
* Updated to use RoR2 1.2.3
* The RoR2 scripts are now in the "RoR2EditorScripts" assembly

* Core Changes:
	* Started to generalize the look of inspectors in RoR2EK, not all inspectors are updated to show this change.
	* Fixed an issue where the RoR2EK AsmDef wouldnt recognize the AsmDef com.Multiplayer.Hlapi-runtime.
	* Fixed an issue where the system to make the RoR2EK assets inedtable wouldnt work properly
	* Reimplemented XML documentation
	* Improvements to the ExtendedInspector system
		* added a bool to define if the inspector being created has a visual tree asset or not
		* Fixed an issue where "VisualElementPropertyDrawers" would draw multiple times when inspecting an object
		* Having a null TokenPrefix no longer stops the inspector from rendering.
	* Improved the IMGUToVisualElementInspector so it no longer throws errors.
	* Removed unfinished "WeaveAssemblies" job
	* Removed PropertyDarawer wrappers

* RoR2EditorScripts changes:
	* Added an ArtifactCompoundDef Inspector
	* Added an ItemDef Inspector
	* Reimplemented the SkillFamilyVariant property drawer
	* Made all the classes in the "RoR2EditorScripts" assembly sealed
	* Removed the HGButton Inspector, this removes the unstated dependency on unity's UI package, Deleting the UIPackage's button editor is a good and simple workaround to make HGButton workable.

### '2.1.0'

* Actually added ValidateUXMLPath to the expended inspector.
* Added IMGUToVisualElementInspector editor. Used to transform an IMGUI inspector into a VisualElement inspector.
* Fixed StageLanguageFiles not working properly
* Fixed StageLanguageFiles not copying the results to the manifest's staging paths.
* Improved StageLanguageFiles' logging capabilities.
* RoR2EK assets can no longer be edited if the package is installed under the "Packages" folder.
* Split Utils.CS into 5 classes
	* Added AssetDatabaseUtils
	* Added ExtensionUtils
	* Added IOUtils
	* Added MarkdownUtils
	* Added ScriptableObjectUtils
* Removed SkillFamilyVariant property drawer

### '2.0.2'

* Fixed an issue where ExtendedInspectors would not display properly due to incorrect USS paths.
* Added ValidateUXMLPath to ExtendedInspector, used to validate the UXML's file path, override this if youre making an ExtendedInspector for a package that depends on RoR2EK's systems.
* Added ValidateUXMLPath to ExtendedEditorWindow
* Hopefully fixed the issue where RoR2EK assets can be edited.

### '2.0.1'

* Fixed an issue where ExtendedInspectors would not work due to incorrect path management.

### '2.0.0'

* Updated to unity version 2019.4.26f1
* Updated to Survivors of The Void
* Added a plethora of Util Methods to Util.CS, including Extensions
* Removed UnlockableDef creation as it's been fixed
* Added "VisualElementPropertyDrawer"
* Renamed "ExtendedPropertyDrawer" to "IMGUIPropertyDrawer"
* Rewrote ExtendedInspector sistem to use VisualElements
* Rewrote CharacterBody inspector
* Rewrote BuffDef inspector
* Rewrote ExtendedEditorWindow to use VisualElements
* Added EliteDef inspector
* Added EquipmentDef inspector
* Added NetworkStateMachine inspector
* Added SkillLocator inspector
* Removed Entirety of AssetCreator systems
* Removed SerializableContentPack window

### 1.0.0

* First Risk of Thunder release
* Rewrote readme a bit
* Added missing XML documentation to methods
* Added a property drawer for PrefabReference (Used on anything that uses RendererInfos)
* Added the MaterialEditor
    * The material editor is used for making modifying and working with HG shaders easier.
    * Works with both stubbed and non stubbed shaders
    * Entire system can be disabled on settings
* Properly added an Extended Property Drawer
* Added Inspector for CharacterBody
* Added Inspector for Child Locator
* Added Inspector for Object Scale Curve
* Added Inspector for BuffDef
* Fixed the enum mask drawer not working with uint based enum flags

### 0.2.4

* Made sure the Assembly Definition is Editor Only.

### 0.2.3

* Added the ability for the EntityStateConfiguration inspector to ignore fields with HideInInspector attribute.

### 0.2.2

* Added 2 new Extended Inspector inheriting classes
    * Component Inspector: Used for creating inspectors for components.
    * ScriptableObject Inspector: Used for creating inspectors for Scriptable Objects.
* Modified the existing inspectors to inherit from these new inspectors.
* Added an inspector for HGButton
* Moved old changelogs to new file

### 0.2.1

* Renamed UnlockableDefCreator to ScriptableCreators
* All the uncreatable skilldefs in the namespace RoR2.Skills can now be created thanks to the ScriptableCreator
* Added an EditorGUILayoutProperyDrawer
    * Extends from property drawer.
    * Should only be used for extremely simple property drawer work.
    * It's not intended as a proper extension to the PropertyDrawer system.
* Added Utility methods to the ExtendedInspector

### 0.2.0

* Added CreateRoR2PrefabWindow, used for creating prefabs.
* Added a window for creating an Interactable prefab.
* Fixed an issue where the Serializable System Type Drawer wouldn't work properly if the inspected type had mode than 1 field.
* Added a fallback on the Serializable System Type Drawer
* Added a property drawer for EnumMasks, allowing proper usage of Flags on RoR2 Enums with the Flags attribute.

### 0.1.4

* Separated the Enabled and Disabled inspector settings to its own setting file. allowing projects to git ignore it.
* The Toggle for enabling and disabling the inspector is now on its header GUI for a more pleasant experience.

### 0.1.2

* Fixed no assembly definition being packaged with the toolkit, whoops.

### 0.1.1

- RoR2EditorKitSettings:
    * Removed the "EditorWindowsEnabled" setting.
    * Added an EnabledInspectors setting.
        * Lets the user choose what inspectors to enable/disable.
    * Added a MainManifest setting.
        * Lets RoR2EditorKit know the main manifest it'll work off, used in the SerializableContentPackWindow.

- Inspectors:
    * Added InspectorSetting property
        * Automatically Gets the inspector's settings, or creates one if none are found.
    * Inspectors can now be toggled on or off at the top of the inspector window.
    
- Editor Windows: 
    * Cleaned up and documented the Extended Editor Window class.
    * Updated the SerializableContentPack editor window:
        * Restored function for Drag and Dropping multiple files
        * Added a button to each array to auto-populate the arrays using the main manifest of the project.

### 0.1.0

- Reorganized CreateAsset Menu
- Added EntityStateConfiguration creator, select state type and hit create. Optional checkbox for setting the asset name to the state's name.
- Added SurvivorDef creator, currently halfway implemented.
- Added BuffDef creator, can automatically create a networked sound event for the start sfx.
- Removed EntityStateConfiguration editor window.
- Implemented a new EntityStateConfiguration inspector
- Internal Changes

### 0.0.1

- Initial Release