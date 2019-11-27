Shader "Antura/Diffuse" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Emission("Emission", Color) = (0,0,0,0)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 150

		CGPROGRAM
		#pragma surface surf Lambert noforwardadd

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			fixed3 color : COLOR;
		};

		fixed4 _Color;
		fixed3 _Emission;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb * IN.color;
			o.Emission = _Emission;
			o.Alpha = c.a;
		}
		ENDCG
	}

			//FallBack "Diffuse"
}
