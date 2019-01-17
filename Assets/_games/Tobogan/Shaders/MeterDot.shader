// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/MeterDot"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("color", Color) = (1,1,1,1)
		_Base ("Base", Float) = 0
		_Height("Height", Float) = 10
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent+100" }

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off

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
				float worldY : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			float _Base;
			float _Height;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldY = mul(unity_ObjectToWorld, v.vertex).y;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv)*_Color;

				col.a *= smoothstep(_Base, _Base + 0.1, i.worldY);
				col.a *= (1 - smoothstep(_Base + _Height - 0.1, _Base + _Height, i.worldY));

				return col;
			}
			ENDCG
		}
	}
}
