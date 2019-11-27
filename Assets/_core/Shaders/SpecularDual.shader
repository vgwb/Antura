Shader "Antura/Specular Both Sides" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Shininess("Shininess", Range(0.03, 1)) = 0.078125
		_Specular("Specular", Range(0,1)) = 0.5

		_Color("Color", Color) = (1,1,1,1)
		_Emission("Emission", Color) = (0,0,0,0)
		_SpecularColor("Specular Color", Color) = (0,0,0,0)
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 250
		Cull off

		CGPROGRAM
#pragma surface surf AnturaBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview interpolateview

#include "LightningSpecular.cginc"

	sampler2D _MainTex;
	half _Shininess;
	half _Specular;

	fixed4 _Color;
	fixed3 _Emission;
	fixed3 _SpecularColor;

	struct Input {
		half2 uv_MainTex;
		fixed3 color : COLOR;
	};

	void surf(Input IN, inout SurfaceOutputSpecularAntura o) {
		fixed4 tex = tex2D(_MainTex, IN.uv_MainTex)*_Color;
		o.Albedo = tex.rgb * IN.color;
		o.Gloss = _Specular;
		o.Alpha = tex.a;
		o.Specular = _Shininess;
		o.SpecularColor = _SpecularColor;
		o.Emission = _Emission;
	}
	ENDCG
	}

	//FallBack "Mobile/VertexLit"
}
