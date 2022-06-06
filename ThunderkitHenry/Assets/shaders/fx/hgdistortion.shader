Shader "StubbedShader/fx/hgdistortion" {
	Properties {
		_BumpMap ("Bump Texture", 2D) = "bump" {}
		_MaskTex ("Mask Texture", 2D) = "white" {}
		_Magnitude ("Magnitude", Range(0, 10)) = 0.05
		_NearFadeZeroDistance ("Near-Fade Zero Distance", Float) = 0
		_NearFadeOneDistance ("Near-Fade One Distance", Float) = 5
		_FarFadeOneDistance ("Far-Fade One Distance", Float) = 200000
		_FarFadeZeroDistance ("Far-Fade Zero Distance", Float) = 250000
		[Toggle(DISTANCEMODULATION)] _DistanceModulationOn ("Apply distance modulation", Float) = 0
		_DistanceModulationMagnitude ("Distance Modulation Magnitude", Range(0, 1)) = 0.5
		_InvFade ("Soft Factor", Range(0, 2)) = 0.1
	}
		Fallback "Diffuse"
}