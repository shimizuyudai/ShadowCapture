// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Custom/BlendableUI"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
		
        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		_Alpha("Alpha",Range(0,1)) = 1
		[KeywordEnum(Normal, Add, Screen, Multiply, ColorBurn, Darken, Lighten, ColorDodge, Difference)] _BlendMode("BlendMode", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

		GrabPass{
			Name "BASE"
			Tags{ "LightMode" = "Always" }
		}

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_ALPHACLIP
#pragma multi_compile _BLENDMODE_NORMAL _BLENDMODE_ADD _BLENDMODE_SCREEN _BLENDMODE_MULTIPLY _BLENDMODE_COLORBURN _BLENDMODE_DARKEN _BLENDMODE_LIGHTEN _BLENDMODE_COLORDODGE _BLENDMODE_DIFFERENCE
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
				float4 uvgrab : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
			sampler2D _GrabTexture;
			float4 _GrabTexture_TexelSize;
			fixed _Alpha;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
				OUT.uvgrab = ComputeGrabScreenPos(OUT.vertex);
                OUT.texcoord = v.texcoord;

                OUT.color = v.color * _Color;
                return OUT;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
				half4 grabColor = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(IN.uvgrab));
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
#ifdef _BLENDMODE_NORMAL

#elif _BLENDMODE_ADD
				color += grabColor;
#elif _BLENDMODE_MULTIPLY
				color *= grabColor;
#elif _BLENDMODE_SCREEN
				color = 1 - (1 - grabColor)*(1 - color);
#elif _BLENDMODE_COLORBURN
				color = 1 - (1 - grabColor) / color;
#elif _BLENDMODE_DARKEN
				color = min(color, grabColor);
#elif _BLENDMODE_LIGHTEN
				color = max(color, grabColor);
#elif _BLENDMODE_COLORDODGE
				color /= (1 - grabColor);
#elif _BLENDMODE_DIFFERENCE
				color = abs(color - grabColor);
				
#endif
				color = saturate(color);
				color.a = _Alpha;
                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
        ENDCG
        }
    }
}
