Shader "Custom/SimpleMultiMeshShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard addshadow fullforwardshadows vertex:vert
		#pragma multi_compile_instancing
		#pragma instancing_options procedural:setup

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 5.0
		#include "UnityCG.cginc"

		struct SimpleParticle
		{
			float3 position;
			float3 angles;
			float3 scale;
			float4 color;
		};

#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
		StructuredBuffer<SimpleParticle> _TestBuffer;
#endif
		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float4 color;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		struct appdata_custom {
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 texcoord3 : TEXCOORD3;
			fixed4 color : COLOR;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		float3 rotate(float3 p, float3 rotation) {
			float3 a = normalize(rotation);
			float angle = length(rotation);
			float s = sin(angle);
			float c = cos(angle);
			float r = 1.0 - c;
			float3x3 m = float3x3(
				a.x * a.x * r + c,
				a.y * a.x * r + a.z * s,
				a.z * a.x * r - a.y * s,
				a.x * a.y * r - a.z * s,
				a.y * a.y * r + c,
				a.z * a.y * r + a.x * s,
				a.x * a.z * r + a.y * s,
				a.y * a.z * r - a.x * s,
				a.z * a.z * r + c
				);
			return mul(m, p);
		}

		void setup()
		{
#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED

#endif
		}

		void vert(inout appdata_custom v, out Input IN) {
			UNITY_INITIALIZE_OUTPUT(Input, IN);
			UNITY_SETUP_INSTANCE_ID(v);
			

#ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
			SimpleParticle p = _TestBuffer[unity_InstanceID];
			v.vertex.xyz *= p.scale;
			v.vertex.xyz = rotate(v.vertex.xyz, p.angles);
			v.vertex.xyz += p.position;
			IN.color = p.color;
#endif
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = IN.color;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
