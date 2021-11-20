# Stubbed Shader Converter

This is a [ThunderKit created](https://github.com/risk-of-thunder/R2Wiki/wiki/Creating-Mods-with-Thunderkit) external tool for converting stubbed shaders into Hopoo equivelents, and is primarily a tool for other mod developers.  
Using StubbedShaderConverter within your mod will require a `Soft/HardDependency`, as StubbedShaderConverter must load before your mod loads.  

Information on how to use this as a mod developer can be found within [the Github](https://github.com/Vale-X/StubbedShaderConverter).

# Credits
- **KomradeSpectre**: For providing the stubbed shaders and MaterialControllerComponent and massive help with setting this project up and debugging.
- **Kevin**: For providing the CloudFix component, which is a variant on KomradeSpectre's MaterialControllerComponent.
- **PassivePicasso (Twiner)**: For ThunderKit related support and creation of [ThunderKit](https://github.com/risk-of-thunder/R2Wiki/wiki/Creating-Mods-with-Thunderkit).
- **Nebby**: For inspiring me to create this project, and helping out with setting up.

# Changelog

`0.1.0`

- __StubbedShaderConverter's `ShaderConverter` has been reworked.__
    - The namespace has been renamed from `ShaderConvert` to `ShaderConverter`.
	- `ShaderConvert` is kept in this release for compatibility, but will be removed in the future. Please update to use `ShaderConverter` instead!
    - The cloud remap issues, which most of the methods in this tool were used to solve, has been figured out! All of StubbedShaderConverter's main converter methods have been removed.
	- The new primary method is `ShaderConverter.ConvertStubbedShaders()` which accepts AssetBundles, GameObjects, Renderers and Materials as input. This can be called in awake and correctly handles cloud remap shaders.
	- Please check out [the Github](https://github.com/Vale-X/StubbedShaderConverter) for information on cloud remap materials.
- __Added a new `AddMaterialController` component.__
    - Used for adding KomradeSpectre's Material Controller to any prefab contained within your asset bundle on start.
- __KomradeSpectre's Material Controller has been updated.__
    - Now supports all Renderer types.
	- Now supports `HGSnowTopped` shaders.

`0.0.2`

- `ConvertAssetBundleShaders` now returns an AssetBundle.

`0.0.1`

- Initial Release.