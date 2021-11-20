# Thunderkit-Henry (EARLY RELEASE)
**This is an EARLY RELEASE of ThunderHenry, a ThunderKit based template for survivors. This template DOES NOT have WWise setup, meaning custom sounds are NOT POSSIBLE with this version of the template. A future release will have WWise support.**

See the [ThunderHenry Wiki](https://github.com/Vale-X/Thunderkit-Henry/wiki/Creating-Survivors-with-ThunderHenry) for a WORK IN PROGRESS tutorial on how to use this template, and how to create ThunderKit survivors in general.

## Changelog

__Major Update 1__
- The project now uses `ThunderKit v4.1.1` and `R2API v3.0.59`.
- The project now uses `StubbedShaderConverter v1.0.0` as a package instead of a local install.
- Added `Shaders.cs`, for using ShaderConverter and also creating a list of all materials in the proejct.
- Updated StubbedShaders, including `hgCloudRemap`. In order for cloud remap materials to work correctly, `SourceBlend` and `DestinationBlend` (set within the material) must be non-zero.
- Updated `Buffs.cs`, adding methods for Buffs and Debuffs.
- Updated `Config.cs`, adding a new `ForceUnlock` config option.
- Updated `Helpers.cs`, adding `StartDebug()` for debugging prints.
- Updated `Prefabs.cs`, adding a `bodyIndexes` list.
- Updated `Unlockables.cs`, now uses R2API's UnlockableAPI.
- Fixed an issue with `Assembly-CSharp.Public.dll`.
