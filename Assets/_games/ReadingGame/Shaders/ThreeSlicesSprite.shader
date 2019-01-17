// Upgrade NOTE: upgraded instancing buffer 'PerDrawSprite' to new syntax.

Shader "Antura/ReadingGame/ThreeSlices"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_DoneColor("Done Tint", Color) = (1,1,1,1)
		_StartSlice("Start slice", Float) = 0.2
		_EndSlice("End slice", Float) = 0.8
		//_ScreenLeftOffset("Screen Left Offset", Float) = 0
		//_ScreenRightOffset("Screen Right Offset", Float) = 0

		_Done("Done", Range(0,1)) = 0

		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent-10"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile _ PIXELSNAP_ON
#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
#include "UnityCG.cginc"

#ifdef UNITY_INSTANCING_ENABLED

    UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
        // SpriteRenderer.Color while Non-Batched/Instanced.
        fixed4 unity_SpriteRendererColorArray[UNITY_INSTANCED_ARRAY_SIZE];
        // this could be smaller but that's how bit each entry is regardless of type
        float4 unity_SpriteFlipArray[UNITY_INSTANCED_ARRAY_SIZE];
    UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

    #define _RendererColor unity_SpriteRendererColorArray[unity_InstanceID]
    #define _Flip unity_SpriteFlipArray[unity_InstanceID]

#endif // instancing

CBUFFER_START(UnityPerDrawSprite)
#ifndef UNITY_INSTANCING_ENABLED
    fixed4 _RendererColor;
    float4 _Flip;
#endif
    float _EnableExternalAlpha;
CBUFFER_END

		struct appdata_t
		{
			half4 vertex   : POSITION;
			fixed4 color    : COLOR;
			half2 texcoord : TEXCOORD0;
		};

		struct v2f
		{
			half4 vertex   : SV_POSITION;
			fixed4 color : COLOR;
			half2 texcoord  : TEXCOORD0;
			half4 screencoord : TEXCOORD1;
		};

		half4 _MainTex_TexelSize;
		fixed4 _Color;
		fixed4 _DoneColor;

		half _StartSlice;
		half _EndSlice;
		half _ScreenLeftOffset;
		half _ScreenRightOffset;

		half _Done;

		v2f vert(appdata_t IN)
		{
			v2f OUT;
			OUT.vertex = UnityObjectToClipPos(IN.vertex);
			OUT.texcoord = IN.texcoord;
			OUT.color = IN.color;
	#ifdef PIXELSNAP_ON
			OUT.vertex = UnityPixelSnap(OUT.vertex);
	#endif
			OUT.screencoord = ComputeScreenPos(OUT.vertex);

			return OUT;
		}

		sampler2D _MainTex;
		sampler2D _AlphaTex;

	fixed4 SampleSpriteTexture(float2 uv)
	{
		fixed4 color = tex2D(_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
    fixed4 alpha = tex2D (_AlphaTex, uv);
    color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif

		return color;
	}

		fixed4 frag(v2f IN) : SV_Target
		{
			half2 uv = IN.texcoord;

			half screenX = (IN.screencoord.x / IN.screencoord.w) * _ScreenParams.x;
			half screenLeftX = screenX - _ScreenLeftOffset;
			half screenRightX = _ScreenRightOffset - screenX;
			half screenSizeX = screenRightX - screenLeftX;

			half screenUVLeftX = screenLeftX * _MainTex_TexelSize.x;
			half screenUVRightX = screenRightX * _MainTex_TexelSize.x;

			if (screenUVLeftX < _StartSlice)
				uv.x = screenUVLeftX;
			else if (screenUVRightX < 1 - _EndSlice)
				uv.x = 1 - screenUVRightX;
			else
			{
				float l = _ScreenLeftOffset + _StartSlice*_MainTex_TexelSize.z;
				float r = _ScreenRightOffset - (1 - _EndSlice)*_MainTex_TexelSize.z;

				uv.x = lerp(screenUVLeftX, 1 - screenUVRightX, (screenX - l) / (r - l));
			}

			fixed4 c = SampleSpriteTexture(uv) * IN.color * lerp(_Color, _DoneColor, 1 - smoothstep(_Done - 0.01, _Done, IN.texcoord.x*0.95));

			c.rgb *= c.a;
		
			return c;
		}
		ENDCG
	}
	}
}
