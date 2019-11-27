Shader "Antura/Confetti" {
	Properties{
		_Reflections("Reflections", CUBE) = "" {}
		_Shininess("Shininess", Range(0.03, 1)) = 0.078125
		_Specular("Specular", Range(0,1)) = 0.5

		_Color("Color", Color) = (1,1,1,1)
		_Emission("Emission", Color) = (0,0,0,0)
		_SpecularColor("Specular Color", Color) = (0,0,0,0)
	}
		SubShader{
		Tags{ "RenderType" = "Transparent" "Queue"="Transparent+5000" }
		LOD 250
		//Cull off

		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf AnturaBlinnPhong alpha exclude_path:prepass nolightmap noforwardadd halfasview interpolateview

#include "LightningSpecular.cginc"

	half _Shininess;
	half _Specular;

	fixed4 _Color;
	fixed3 _Emission;
	fixed3 _SpecularColor;
	samplerCUBE _Reflections;

	struct Input {
		half2 uv_MainTex;
		fixed4 color : COLOR;
		float3 worldRefl;
	};
	
	void surf(Input IN, inout SurfaceOutputSpecularAntura o) {
		fixed4 tex = _Color*IN.color;
		o.Albedo = tex.rgb;
		o.Gloss = _Specular;
		o.Alpha = tex.a;
		o.Specular = _Shininess;
		o.SpecularColor = texCUBE(_Reflections, IN.worldRefl).rgb;
		o.Emission = _Emission*IN.color;
	}

	ENDCG
	}

		//FallBack "Mobile/VertexLit"
}
