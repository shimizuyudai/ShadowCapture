Shader "Hidden/DifferenceImageFilter"
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
#pragma multi_compile _ IsRefresh

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
	sampler2D _PreFrameTex;
	sampler2D _CacheTex;

	float _Threshold;

	fixed4 frag(v2f i) : SV_Target
	{
		#ifdef IsRefresh
			fixed4 result = fixed4(0,0,0,1);
			fixed4 prev = tex2D(_PreFrameTex, i.uv);
			fixed4 col = tex2D(_MainTex, i.uv);
			if (distance(prev, col) > _Threshold) {
				result = col;
			}
			return result;
		#else
			return tex2D(_CacheTex, i.uv);
		#endif
	}
		ENDCG
	}

	/*Pass
	{
		CGPROGRAM
		#pragma target 5.0
		#pragma vertex vert
		#pragma fragment frag

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

	float _Threshold;

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.uv);;
		return col;
	}
		ENDCG
	}*/
	}
}
