// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/EnlargingTube" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_OpeningAnimation ("Opening", Range(0,1)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert noforwardadd 

		struct Input {
			fixed4 color;
		};

		fixed4 _Color;
		half _OpeningAnimation;

		void vert(inout appdata_full v) {
			//half3 localV = mul(unity_ObjectToWorld, v.vertex);
			half4 localV = v.vertex;

			half3 offset = (1 - smoothstep(0, 0.5, localV.y))*_OpeningAnimation*0.1f*sin(_Time.y * 20)*normalize(localV);

			localV += half4(offset,0);

			//v.vertex = mul(unity_WorldToObject, localV);
			v.vertex = localV;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
		//FallBack "Diffuse"
}
