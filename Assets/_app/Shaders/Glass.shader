Shader "Antura/Glass" {
	Properties{
		_Shininess("Shininess", Range(0.03, 1)) = 0.078125
		_Specular("Specular", Range(0,1)) = 0.5

		_Color("Color", Color) = (1,1,1,1)
		_Emission("Emission", Color) = (0,0,0,0)
		_SpecularColor("Specular Color", Color) = (0,0,0,0)
	}
		SubShader{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 250

		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off

		CGPROGRAM
#pragma surface surf AnturaGlass exclude_path:prepass nolightmap noforwardadd halfasview interpolateview alpha:blend keepalpha

#include "LightningSpecular.cginc"

		half _Shininess;
	half _Specular;

	fixed4 _Color;
	fixed3 _Emission;
	fixed3 _SpecularColor;

	struct Input {
		fixed3 color : COLOR;
	};

	void surf(Input IN, inout SurfaceOutputSpecularAntura o) {
		fixed4 col = _Color;
		o.Albedo = col.rgb*IN.color;
		o.Gloss = _Specular;
		o.Alpha = col.a;
		o.Specular = _Shininess;
		o.SpecularColor = _SpecularColor;
		o.Emission = _Emission;
	}
	ENDCG
	}

		//FallBack "Mobile/VertexLit"
}
