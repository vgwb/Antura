Shader "Antura/Glitter"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent+9000" }
		Cull off
		Blend SrcAlpha One
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
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
				fixed4 color : TEXCOORD2;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv)*i.color;
				
				float baseForce = abs(normalize(i.worldNormal).z);

				float force = pow(baseForce, 10);
				float colorForce = pow(baseForce, 1000);

				col.rgb = lerp(col.rgb, fixed3(1,1,1), colorForce); // fade to white

				col.a = col.a*force - 2.0*(1-force);
				return col;
			}
			ENDCG
		}
	}
}
