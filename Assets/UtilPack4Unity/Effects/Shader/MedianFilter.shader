Shader "UtilPack4Unity/Filter/MedianFilter"
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

			void bubbleSort(float values[9])
			{
				int length = 9;
				for (int i = 0; i < (length - 1); i++) {
					for (int j = (length - 1); j > i; j--) {
						if (values[j - 1] > values[j]) {
							float temp = values[j - 1];
							values[j - 1] = values[j];
							values[j] = temp;
						}
					}
				}
			}

			float sort(float table[9]) {
				int len = 9;
				float min = 0;
				int index = 0;
				for (int i = 0; i < len/2; i++) {
					min = table[i];
					index = i;
					for (int j = i; j < len/2; j++) {
						if (min > table[j]) {
							index = j;
							min = table[j];
						}
					}
					float tmp = table[i];
					table[i] = table[index];
					table[index] = tmp;
				}
				return table[4];
			}

			float getMedian(float array[9]) {
				for (int i = 0; i < 8; i++) {
					for (int j = 8; j > i; j--) {
						if (array[j - 1] > array[j]) {
							float temp = array[j - 1];
							array[j - 1] = array[j];
							array[j] = temp;
						}
					}
				}
				return array[4];
			}
			

			fixed4 median(sampler2D tex, float4 texelSize, float2 uv)
			{
				float rArray[9];
				float gArray[9];
				float bArray[9];

				float2 uvArray[9];
				uvArray[0] = float2(-1.0, -1.0);
				uvArray[1] = float2(0.0, -1.0);
				uvArray[2] = float2(1.0, -1.0);
				uvArray[3] = float2(-1.0, 0.0);
				uvArray[4] = float2(0.0, 0.0);
				uvArray[5] = float2(1.0, 0.0);
				uvArray[6] = float2(-1.0, 1.0);
				uvArray[7] = float2(0.0, 1.0);
				uvArray[8] = float2(1.0, 1.0);

				for (int i = 0; i < 9; i++) {
					fixed4 col = tex2D(tex, (uv + uvArray[i] * texelSize.xy));
					rArray[i] = col.r;
					gArray[i] = col.g;
					bArray[i] = col.b;
				}
				float r = sort(rArray);
				float g = sort(gArray);
				float b = sort(bArray);
				return fixed4(r,g,b, 1);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return median(_MainTex, _MainTex_TexelSize, i.uv);
			}
			ENDCG
		}
	}
}
