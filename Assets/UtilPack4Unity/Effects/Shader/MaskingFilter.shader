// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UtilPack4Unity/Filter/MaskingFilter"
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
			
			float4 _Color;
			sampler2D _MainTex;
			sampler2D _MaskTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 mc = tex2D(_MaskTex, i.uv);
				fixed4 col = fixed4(1,1,1,1);
				float brightness = (mc.r + mc.g + mc.b) / 3;
				if (brightness < 0.01) {
					col = _Color;
				}
				else {
					col = tex2D(_MainTex, i.uv);
				}
				return col;
			}
			ENDCG
		}
	}
}
