Shader "Unlit/DistanceVisualizer"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
            float _MaxDistance;
            float3 _TargetPosition;
			
			v2f vert (appdata v)
			{
				v2f o;
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

            float map(float currentVal, float tempAStart, float tempAGoal, float tempBStart, float tempBGoal){
                return ((currentVal - tempAStart) / (tempAGoal - tempAStart)) * (tempBGoal - tempBStart) + tempBStart;
            }

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = fixed4(0,0,0,1);
                float d = distance(i.worldPos, _TargetPosition);
                float t = 1 - map(clamp(d,0,_MaxDistance),0,_MaxDistance,0,1);
                col.rgb = t;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
