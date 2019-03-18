Shader "Hidden/Buffer2Texture"
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
			#pragma target 5.0

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

			StructuredBuffer<float> _DepthBuffer;
			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			float _Width, _Height;
			float2 _Offset;
			float _Coefficient;
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				int index = (i.uv.x + _Offset.x)*_Width + (1 - i.uv.y + _Offset.y)*_Height*_Width;
				col = _DepthBuffer[index];
				col *= _Coefficient;
				col.a = 1;
				return col;
			}
			ENDCG
		}
	}
}
