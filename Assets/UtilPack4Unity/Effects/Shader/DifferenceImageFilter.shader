Shader "UtilPack4Unity/Filter/DifferenceImageFilter"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
	{
		CGPROGRAM
	#pragma target 5.0
	#pragma vertex vert
	#pragma fragment frag
	#pragma multi_compile _ MONOTONE
#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		return o;
	}

	sampler2D _MainTex;
	sampler2D _CacheTex;

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 result = fixed4(0,0,0,1);
		fixed4 cache = tex2D(_CacheTex, i.uv);
		fixed4 col = tex2D(_MainTex, i.uv);
		
		#ifdef MONOTONE
		float diff = abs(cache.r - col.r);
		result.rgb = diff;
		#else
		float diff = distance(cache.rgb, col.rgb);
		float maxDiff = distance(float3(0,0,0), float3(1,1,1));
		result.rgb = (diff / maxDiff);
		#endif
			
			
			
			return result;
	}
		ENDCG
	}
	}
}
