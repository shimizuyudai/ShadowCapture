Shader "UtilPack4Unity/Filter/LaplacianFilter"
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
			int _Repeat;
			float4 _MainTex_TexelSize;


			fixed4 laplacian(sampler2D tex, float4 texelSize, float2 uv) {
				
				fixed4 col = tex2D(tex, (uv + float2(0.0, 0.0) * texelSize.xy)) *  -8.0;
				col += tex2D(tex, (uv + float2(-1.0, -1.0) * texelSize.xy));
				col += tex2D(tex, (uv + float2(0.0, -1.0) * texelSize.xy));
				col += tex2D(tex, (uv + float2(1.0, -1.0) * texelSize.xy));
				col += tex2D(tex, (uv + float2(-1.0, 0.0) * texelSize.xy));
				
				col += tex2D(tex, (uv + float2(1.0, 0.0) * texelSize.xy));
				col += tex2D(tex, (uv + float2(-1.0, 1.0) * texelSize.xy));
				col += tex2D(tex, (uv + float2(0.0, 1.0) * texelSize.xy));
				col += tex2D(tex, (uv + float2(1.0, 1.0) * texelSize.xy));

				return col;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return laplacian(_MainTex, _MainTex_TexelSize, i.uv);
			}
			ENDCG
		}
	}
}
