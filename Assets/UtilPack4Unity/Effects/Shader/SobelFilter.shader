Shader "UtilPack4Unity/Filter/SobelFilter"
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


			fixed4 sobel(sampler2D tex, float4 texelSize, float2 uv) {
				fixed4 hr = fixed4(0, 0, 0, 0);
				fixed4 vt = fixed4(0, 0, 0, 0);

				hr += tex2D(tex, (uv + float2(-1.0, -1.0) * texelSize.xy)) *  1.0;
				//hr += tex2D(tex, (uv + float2(0.0, -1.0) * texelSize.xy)) *  0.0;
				hr += tex2D(tex, (uv + float2(1.0, -1.0) * texelSize.xy)) * -1.0;
				hr += tex2D(tex, (uv + float2(-1.0, 0.0) * texelSize.xy)) *  2.0;
				//hr += tex2D(tex, (uv + float2(0.0, 0.0) * texelSize.xy)) *  0.0;
				hr += tex2D(tex, (uv + float2(1.0, 0.0) * texelSize.xy)) * -2.0;
				hr += tex2D(tex, (uv + float2(-1.0, 1.0) * texelSize.xy)) *  1.0;
				//hr += tex2D(tex, (uv + float2(0.0, 1.0) * texelSize.xy)) *  0.0;
				hr += tex2D(tex, (uv + float2(1.0, 1.0) * texelSize.xy)) * -1.0;

				vt += tex2D(tex, (uv + float2(-1.0, -1.0) * texelSize.xy)) *  1.0;
				vt += tex2D(tex, (uv + float2(0.0, -1.0) * texelSize.xy)) *  2.0;
				vt += tex2D(tex, (uv + float2(1.0, -1.0) * texelSize.xy)) *  1.0;
				//vt += tex2D(tex, (uv + float2(-1.0, 0.0) * texelSize.xy)) *  0.0;
				//vt += tex2D(tex, (uv + float2(0.0, 0.0) * texelSize.xy)) *  0.0;
				//vt += tex2D(tex, (uv + float2(1.0, 0.0) * texelSize.xy)) *  0.0;
				vt += tex2D(tex, (uv + float2(-1.0, 1.0) * texelSize.xy)) * -1.0;
				vt += tex2D(tex, (uv + float2(0.0, 1.0) * texelSize.xy)) * -2.0;
				vt += tex2D(tex, (uv + float2(1.0, 1.0) * texelSize.xy)) * -1.0;

				return sqrt(hr * hr + vt * vt);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return sobel(_MainTex, _MainTex_TexelSize, i.uv);
			}
			ENDCG
		}
	}
}
