Shader "UtilPack4Unity/Filter/ColorDistanceThresholdImageFilter"
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

			#include "UnityCG.cginc"

		struct ColorThresholdInfo
		{
			float4 color;
			float DistanceThreshold;
		};

		StructuredBuffer<ColorThresholdInfo> _Buffer;
		int _Length;

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

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 result = fixed4(0,0,0,1);
				fixed4 col = tex2D(_MainTex, i.uv);
				for (int j = 0; j < _Length; j++) {
					if (distance(col, _Buffer[j].color) < _Buffer[j].DistanceThreshold) {
						result = fixed4(1,1,1,1);
						break;
					}
				}
				return result;
			}
		ENDCG
	}
	}
}
