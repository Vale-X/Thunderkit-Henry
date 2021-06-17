Shader "stubbed_Hopoo Games/FX/Damage Number Proxy" {
	Properties {
		[HDR] _TintColor ("Tint", Vector) = (1,1,1,1)
		_CritColor ("Crit Color", Vector) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_CharacterLimit ("Character Limit", Float) = 3
	}
	Fallback "Diffuse"
}