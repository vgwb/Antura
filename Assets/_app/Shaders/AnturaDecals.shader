// HowTo:
// This shader should be used by antura decals materials (e.g. patches on it)
// Add this e.g. as second in the list of renderer materials
// This uses uv1

// To be used with the next version (two uvs) of the dog!
Shader "Antura/Decal" {
	Properties{
		_OverTex("Color (RGB) Multiply-Overwrite (A)", 2D) = "white" {}
		_Occlusion("Occlusion (A)", 2D) = "white" {}
		_OverColorR("Color R", Color) = (1,1,1,1)
		_OverColorG("Color G", Color) = (1,1,1,1)
		_OverColorB("Color B", Color) = (1,1,1,1)

		_Shininess("Shininess", Range(0.03, 1)) = 0.078125
		_Specular("Specular", Range(0,1)) = 0.5

		_Emission("Emission", Color) = (0,0,0,0)
		_SpecularColor("Specular Color", Color) = (0,0,0,0)
	}
		SubShader{
		Tags{ "RenderType" = "Transparent" "Queue"="Transparent" }
		LOD 250

		Blend SrcAlpha OneMinusSrcAlpha
		ZTest Equal

		CGPROGRAM
#pragma surface surf AnturaBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview interpolateview alpha:blend keepalpha

#include "LightningSpecular.cginc"

		sampler2D _OverTex;
		sampler2D _Occlusion;

		half _Shininess;
		half _Specular;

		fixed4 _BaseColor;

		fixed3 _OverColorR;
		fixed3 _OverColorG;
		fixed3 _OverColorB;

		fixed3 _Emission;
		fixed3 _SpecularColor;

		struct Input {
			half2 uv_OverTex;
			half2 uv_Occlusion;
		};

		void surf(Input IN, inout SurfaceOutputSpecularAntura o) 
		{
			fixed4 overmap = tex2D(_OverTex, IN.uv_OverTex);

			fixed3 overColor = saturate(overmap.r*_OverColorR + overmap.g*_OverColorG + overmap.b*_OverColorB);

			o.Albedo = overColor*tex2D(_Occlusion, IN.uv_Occlusion).a;
			o.Alpha = overmap.a;

			o.Gloss = _Specular;
			o.Specular = _Shininess;
			o.SpecularColor = _SpecularColor;
			o.Emission = _Emission;
		}
	ENDCG
	}

		FallBack "Mobile/VertexLit"
}
