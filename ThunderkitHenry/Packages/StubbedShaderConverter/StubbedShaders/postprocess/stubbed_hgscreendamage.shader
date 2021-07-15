Shader "stubbed_Hopoo Games/Post Process/Screen Damage Proxy" {
	Properties {
		_Tint ("Vignette Tint", Vector) = (0.5,0.5,0.5,1)
		_NormalMap ("Normal Map Texture", 2D) = "white" {}
		[HideInInspector] _MainTex ("", any) = "" {}
		_TintStrength ("Vignette Strength", Range(0, 5)) = 1
		_DesaturationStrength ("Desaturation Strength", Range(0, 1)) = 1
		_DistortionStrength ("Distortion Strength", Range(0, 1)) = 1
	}
	
}