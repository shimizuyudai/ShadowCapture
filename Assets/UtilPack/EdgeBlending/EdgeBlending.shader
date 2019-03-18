Shader "Unlit/EdgeBlending"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Top("Top",Range(0,1)) = 0
		_Right("Right",Range(0,1)) = 0
		_Bottom("Bottom",Range(0,1)) = 0
		_Left("Left",Range(0,1)) = 0
		_Gamma("Gamma",Range(0.1,5)) = 1
		_Color("Color",Color) = (1,1,1,1)
		_Temp("Temp",Range(0,2)) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" "Queue" = "Transparent"}
			LOD 100
			Blend SrcAlpha OneMinusSrcAlpha

			Blend One One //DEBUG BLENDMODE ADD

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

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

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _Top;
				float _Right;
				float _Bottom;
				float _Left;
				float _Gamma;
				fixed4 _Color;
				float _Temp;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				float map(float currentVal, float tempAStart, float tempAGoal, float tempBStart, float tempBGoal)
				{

					return ((currentVal - tempAStart) / (tempAGoal - tempAStart)) * (tempBGoal - tempBStart) + tempBStart;

				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = tex2D(_MainTex, i.uv)*_Color;
					if (i.uv.y > 1 - _Top * 2) {
						float val = map(i.uv.y, 1 - _Top * 2, 1, 1, 0);
						if (val < 0.5) {
							col.a *= _Temp * pow(2 * val, _Gamma);
						}
						else {
							col.a *= 1 - (1 - _Temp) * (pow(2 * (1 - val), _Gamma));
						}

					}
					if (i.uv.y < _Bottom * 2) {
						float val = map(i.uv.y, 0, _Bottom * 2, 0, 1);
						if (val < 0.5) {
							col.a *= _Temp * pow(2 * val, _Gamma);
						}
						else {
							col.a *= 1 - (1 - _Temp) * (pow(2 * (1 - val), _Gamma));
						}
					}
					if (i.uv.x > 1 - _Right * 2) {
						float val = map(i.uv.x, 1 - _Right * 2, 1, 1, 0);
						if (val < 0.5) {
							col.a *= _Temp * pow(2 * val, _Gamma);
						}
						else {
							col.a *= 1 - (1 - _Temp) * (pow(2 * (1 - val), _Gamma));
						}
					}
					if (i.uv.x < _Left * 2) {
						float val = map(i.uv.x, 0, _Left * 2, 0, 1);
						if (val < 0.5) {
							col.a *= _Temp * pow(2 * val, _Gamma);
						}
						else {
							col.a *= 1 - (1 - _Temp) * (pow(2 * (1 - val), _Gamma));
						}
					}

					/*col.rgb *= col.a;
					col.a = 1;*/
					col.rgb *= col.a;
					return col;
				}
				ENDCG
			}
		}
}
