// Upgrade NOTE: upgraded instancing buffer 'PerDrawSprite' to new syntax.

Shader "Antura/ReadingGame/MagnifyingGlass"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_MaskTex("Glass Mask", 2D) = "white" {}
		_BackTex("Back", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_BackOffset("Back Offset", Vector) = (1,1,1,1)
		_BackScale("Back Scale", Vector) = (1,1,1,1)

		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent+100"
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
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		float2 texcoord  : TEXCOORD0;
		float4 screencoord : TEXCOORD1;
	};

	fixed4 _Color;
	half2 _BackOffset;
	half2 _BackScale;

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = UnityObjectToClipPos(IN.vertex);
		OUT.texcoord = IN.texcoord;
		OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif
		OUT.screencoord = ComputeScreenPos(OUT.vertex);

		return OUT;
	}

	sampler2D _MainTex;
	sampler2D _AlphaTex;
	sampler2D _MaskTex;
	sampler2D _BackTex;

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
		fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
		
		float mask = tex2D(_MaskTex, IN.texcoord).r;

		// Apply distortion
		
		float2 backUV = IN.screencoord.xy / IN.screencoord.w;
		backUV = (backUV - _BackOffset) / _BackScale;
		backUV += (1 - mask)*0.025*normalize((IN.texcoord - half2(0.5,0.5)));

		float4 back = tex2D(_BackTex, backUV).rgba;
		back.rgb = lerp(half3(1, 1, 1), half3(0, 0, 0), 1 - back.r);

		half inside = smoothstep(0.0, 0.7, mask);

		c.rgb = lerp(c.rgb, lerp(back, c.rgb, c.a), inside);
		c.a = lerp(c.a, IN.color.a, inside);
		c.rgb *= c.a;

		return c;
	}
		ENDCG
	}
	}
}
