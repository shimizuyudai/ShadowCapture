// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UtilPack4Unity/Filter/DilateImageFilter"
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
			int _Repeat;
			float4 _MainTex_TexelSize;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0,0,0,1);
				fixed4 color = tex2D(_MainTex, i.uv);
				float brightness = (color.r + color.g + color.b) / 3;
				if (brightness > 0) {
					col = fixed4(1,1,1,1);
				}

				for (int y = -1; y <= 1; y++) {
					for (int x = -1; x <= 1; x++) {
						float2 uv = float2(i.uv.x + (float)x*_MainTex_TexelSize.x, i.uv.y + (float)y*_MainTex_TexelSize.y);
						fixed4 c = tex2D(_MainTex, uv);
						float b = (c.r + c.g + c.b) / 3;
						if (b > 0) {
							col = fixed4(1,1,1,1);
						}
					}
				}	
				return col;
			}
			ENDCG
		}
	}
}
