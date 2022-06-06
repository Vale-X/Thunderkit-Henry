# RoR2EditorKit - Editor Utilities, Inspectors and More for Risk of Rain2

## About

RoR2EditorKit is a *Thunderkit Extension* for developing mods inside the UnityEditor, providing a myriad of Inspectors, Property Drawers, Editor Windows and more.

At it's core, RoR2EditorKit should not have any classes or systems that depend on runtime usage, RoR2EditorKit works exclusively for speeding up the modding enviroment.

## Manual Installation

To Download RoR2EditorKit to your project, it is recommended that you either add it via the ThunderKit extension store, or adding it via Unity's PackageManager (Downloading a specific tagged version is recommended).

Once installed, it is heavily reccommended to open the ThunderkitSettings window to modify certain settings that RoR2EditorKit will use while helping you develop the mod.

* RoR2EditorKitSettings: Settings of the extension itself
 * Token Prefix: A prefix for your mod, it's used to generate unique tokens.
 * Main Manifest: The manifest of your mod, used in a myriad of tools to know the assetbundle or the main DLL.

## Extending RoR2EditorKit's Functionality.

* In case you need to extend RoR2EditorKit's functionality for your own purposes (Such as a custom inspector for a mod you're working on), you can look into this wiki page that explains how to extend the editor's functionality using RoR2EditorKit's systems.

[link](https://github.com/risk-of-thunder/RoR2EditorKit/wiki/Extending-the-Editor's-Functionality-with-RoR2EditorKit's-Systems.)

## Contributing

Contributing to RoR2EditorKit is as simple as creating a fork, and cloning the project. the main folder (RoR2EditorKit) is a unity project itself. Simply opening it with the unity version ror2 uses will allow you to edit the project to your heart's content.

A more detailed Contribution guideline can be found [here](https://github.com/risk-of-thunder/RoR2EditorKit/blob/main/CONTRIBUTING.md)

## Changelog

(Old changelogs can be found [here](https://github.com/risk-of-thunder/RoR2EditorKit/blob/main/OldChangelogs.md))

### '3.2.1'

* Core Changes:
	* Cleaned up the code
	* Added XML documentation file
	* ListViewHelper now has a refresh method

* RoR2EditorScripts changes:
	* Cleaned up the code

### '3.2.0'

* Core Changes:
	* Added "GetParentProperty" extension for SerializedProperty
	* Added "SetDisplay" extension for VisualElements
	* ListViewHelper's SerializedProperty can now be changed, allowing for dynamic use of a ListView
	* ListViewHelper's created elements now have the name "elementN", a substring can be used to get the index of the serialized property
	* Improved the ExtendedEditorWindow:
		* Now works like pre 2.0.0 ExtendedEditorWindow
		* Still uses VisualElements
		* ExtendedEditorWindows can load their UI via TemplateHelpers
		* Contains a SerializedObject that points to the instance of the ExtendedEditorWindow
	* Added ObjectEditingEditorWindow
		* ObjectEditingEditorWindow's main usage is for constructing more complex editing tools for objects
		* ObjectEditingEditorWindow's SerializedObject points to the inspected/editing object

* RoR2EditorScripts changes:
	* Added an AssetCollectionInspector

### '3.1.0'

* Core Changes:
	* Added Missing XML Documentation
	* Added "HasDoneFirstDrawing" property to ExtendedInspector
	* Added "ListViewHelper" class
	* PropertyValidator now works on PropertyFields, as well as any VisualElement that implements "INotifyValueChanged"
	* Made the returning value of the PropertyValidator's Functions nullable (Returning null skips the container drawing process)
	* Removed UtilityMethods from ExtendedEditorWindow
	* Improved the look of the MaterialEditorSettings and EditorInspectorSettings inspectors & settings window

* RoR2EditorScripts changes:
	* Redid the following inspectors to use VisualElements:
		* ChildLocator
		* EntityStateConfiguration
		* ObjectScaleCurve
	* Readded Tooltip and Labeling from NetworkStateMachine feature
	* Added SerializableContentPack inspector


### '3.0.2'

* RoR2EditorScripts changes:
	*Made assembly Editor Only

### '3.0.1'

* Core Changes:
	* Fixed ScriptalbeObjectInspector drawing multiple InspectorEnabled toggles (3 inspectors = 3 toggles)
	* Added ReflectionUtils class

* RoR2EditorScripts changes:
	* Changed the method that the EntityStateDrawer and SerializableSystemTypeDrawer uses for getting the current Types in the AppDomain

### '3.0.0'

* General Changes:
	* Transformed the main repository from a __Project Repository__ to a __Package Repository__ (This change alone justifies the major version change)

* Core Changes:
	* Improvements to the Exnteded Inspector:
		* Reworked the naming convention system into the IObjectNameConvention interface
		* Made HasVisualTreeAsset virtual
		* Removed "Find" and "FindAndBind" methods
		* Added AddSimpleContextMenu, simplified version for creating context menus for VisualElements
		* Added PropertyValidator class, used for validating PropertyFields. Evaluate the states of the property fields and append helpBoxes for end users
	* Removed most USS files, added ComponentInspector.uss and ScriptableObjectInspector.uss
	* Added a ComponentInspectorBase.UXML
	* Added the following Extensions and Utilities:
		- KeyValuePair deconstructor from R2API
		- UpdateAndApply extension for ScriptableObject
		- QContainer for Foldout Elements
		- GetRootObject for GameObjects
		- Added AddressableUtils

* RoR2EditorScripts changes:
	* Updated the aspect of the following inspectors:
		- ItemDef
		- EquipmentDef
		- EliteDef
		- BuffDef
		- SkillLocator
		- NetworkStateMachine
		- CharacterBody
	* Added HurtBoxGroup inspector (Auto population of array)
	* Added CharacterModel inspector (Auto population of renderers and lights)
	* Standarized the naming conventions of certain scriptableObjects to be truly in line with Hopoo's naming conventions
	* Most buttons in various inspectors are now replaced by ContextMenus
	* Removed Tooltip and Labeling from NetworkStateMachine for now
	* Token Setting actions now take into consideration objects with Whitespaces by removing them
	* EliteDef cannow set the health and damage boost coefficient to pre SOTV tiers (T1Honor, T1 & T2)
	* CharacterBody's baseVision field can now be set to infinity
	* BaseStats can now be set to common vanilla body stats:
		* Commando
		* Lemurian
		* Golem
		* BeetleQueen
		* Mithrix