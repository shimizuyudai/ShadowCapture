Shader "UtilPack4Unity/Filter/RBump2NormalImageFilter"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float _BumpRate;
			float _NormalRate;
			fixed4 _Color;

			float3 bump2Normal(sampler2D tex, float2 uv, float2 step, float bumpRate, float normalRate)
			{
				float2 ruv = float2(uv.x + step.x, uv.y);
				float2 luv = float2(uv.x - step.x, uv.y);
				float2 tuv = float2(uv.x, uv.y + step.y);
				float2 buv = float2(uv.x, uv.y - step.y);
				float4 r = tex2D(_MainTex, ruv)*bumpRate;
				float4 l = tex2D(_MainTex, luv)*bumpRate;
				float4 t = tex2D(_MainTex, tuv)*bumpRate;
				float4 b = tex2D(_MainTex, buv)*bumpRate;
				float rb = r.r;
				float lb = l.r;
				float tb = t.r;
				float bb = b.r;
				rb *= bumpRate;
				lb *= bumpRate;
				tb *= bumpRate;
				bb *= bumpRate;

				float3 du = { 1, 0, normalRate * (rb - lb) };
				float3 dv = { 0, 1, normalRate * (tb - bb) };
				float3 normal = normalize(cross(du, dv)) * 0.5 + 0.5;
				return normal;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = float4(bump2Normal(_MainTex, i.uv, _MainTex_TexelSize.xy, _BumpRate, _NormalRate),1);
				return col;
			}
			ENDCG
		}
	}
}
