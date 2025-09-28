Shader "Antura/UI/GreyscaleUI"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Intensity ("Greyscale Intensity", Range(0,1)) = 1
        _Color ("Tint", Color) = (1,1,1,1)

        // --- UI masking props (match UI/Default) ---
        [PerRendererData] _StencilComp ("Stencil Comparison", Float) = 8
        [PerRendererData] _Stencil ("Stencil ID", Float) = 0
        [PerRendererData] _StencilOp ("Stencil Operation", Float) = 0
        [PerRendererData] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [PerRendererData] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [PerRendererData] _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
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
        Blend One OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            // Support RectMask2D clipping & optional alpha clip like UI/Default
            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed _Intensity;
            float4 _ClipRect; // set by Unity for RectMask2D

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float4 color  : COLOR;
            };

            struct v2f
            {
                float4 vertex        : SV_POSITION;
                float2 uv            : TEXCOORD0;
                fixed4 color         : COLOR;
                float4 worldPosition : TEXCOORD1; // for RectMask2D clip
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                o.worldPosition = v.vertex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv) * i.color;

                // Respect RectMask2D (_ClipRect): multiply alpha by clipping factor
                #ifdef UNITY_UI_CLIP_RECT
                c.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                #endif

                // Optional hard alpha clip (helps avoid soft edges at mask borders)
                #ifdef UNITY_UI_ALPHACLIP
                clip(c.a - 0.001);
                #endif

                // Greyscale: keep alpha, lerp rgb to luma
                fixed grey = dot(c.rgb, fixed3(0.299, 0.587, 0.114));
                c.rgb = lerp(c.rgb, grey.xxx, _Intensity);

                return c;
            }
            ENDCG
        }
    }
    FallBack "UI/Default"
}
