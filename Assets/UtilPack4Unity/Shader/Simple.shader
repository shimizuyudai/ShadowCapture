// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

Shader "UtilPack4Unity/Simple" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_EmissionMap("EmissionTex", 2D) = "white"{}
		_EmissionColor("EmissionColor", Color) = (0,0,0)
		_Intensity("Intensity",float) = 1
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types


			#include "UnityCG.cginc"
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target5.0
			#pragma multi_complile_instancing

			#pragma surface surf Standard addShadow 
			sampler2D _MainTex;
			sampler2D _EmissionMap;

		struct Input {
			float2 uv_MainTex;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
				UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color)
#define _Color_arr Props
				UNITY_DEFINE_INSTANCED_PROP(half3 , _EmissionColor)
#define _EmissionColor_arr Props
				UNITY_DEFINE_INSTANCED_PROP(fixed, _Intensity)
#define _Intensity_arr Props
				UNITY_DEFINE_INSTANCED_PROP(fixed, _Glossiness)
#define _Glossiness_arr Props
				UNITY_DEFINE_INSTANCED_PROP(fixed, _Metallic)
#define _Metallic_arr Props
				UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o) {
				fixed4 col = tex2D(_MainTex, IN.uv_MainTex.xy)* UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);
				o.Albedo = col;
				o.Emission = tex2D(_EmissionMap, IN.uv_MainTex)*UNITY_ACCESS_INSTANCED_PROP(_EmissionColor_arr, _EmissionColor)*UNITY_ACCESS_INSTANCED_PROP(_Intensity_arr, _Intensity);
				o.Metallic = UNITY_ACCESS_INSTANCED_PROP(_Metallic_arr, _Metallic);
				o.Smoothness = UNITY_ACCESS_INSTANCED_PROP(_Glossiness_arr, _Glossiness);
			}

			ENDCG
		}
			FallBack "Diffuse"
}
