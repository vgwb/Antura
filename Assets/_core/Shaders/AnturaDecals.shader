// HowTo:
// This shader should be used by antura decals materials (e.g. patches on it)
// Add this e.g. as second in the list of renderer materials
// This uses uv1

// To be used with the next version (two uvs) of the dog!
Shader "Antura/Decal" {
	Properties{
		_MainTex("Base (RGB) Glossiness (A)", 2D) = "white" {}
		_OverTex("Color (RGB) Multiply-Overwrite (A)", 2D) = "white" {}
		_Occlusion("Occlusion (A)", 2D) = "white" {}
		_OverColorR("Color R", Color) = (1,1,1,1)
		_OverColorG("Color G", Color) = (1,1,1,1)
		_OverColorB("Color B", Color) = (1,1,1,1)
		_OverColorW("Color W", Color) = (1,1,1,1)

		_Shininess("Shininess", Range(0.03, 1)) = 0.078125
		_Specular("Specular", Range(0,1)) = 0.5

		_Emission("Emission", Color) = (0,0,0,0)
		_SpecularColor("Specular Color", Color) = (0,0,0,0)
        _Rotation ("Rotation", Float) = 2.0
		}
		SubShader{
		Tags{ "RenderType" = "Transparent" "Queue"="Transparent" }
		LOD 250

		Blend SrcAlpha OneMinusSrcAlpha
		ZTest Equal

		CGPROGRAM
#pragma surface surf AnturaBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview interpolateview alpha:blend keepalpha vertex:vert

#include "LightningSpecular.cginc"

		sampler2D _MainTex;
		sampler2D _OverTex;
		sampler2D _Occlusion;

		half _Shininess;
		half _Specular;

		fixed4 _BaseColor;

		fixed3 _OverColorR;
		fixed3 _OverColorG;
		fixed3 _OverColorB;
		fixed3 _OverColorW;

		fixed3 _Emission;
		fixed3 _SpecularColor;

		struct Input {
			half2 uv2_MainTex;
			half2 uv_OverTex;
			half2 uv_Occlusion;
		};

        float _Rotation;
        void vert (inout appdata_full v) {
            float sinX = sin ( _Rotation  );
            float cosX = cos ( _Rotation );
            float sinY = sin ( _Rotation  );
            float2x2 rotationMatrix = float2x2( cosX, -sinX, sinY, cosX);
            v.texcoord1.xy = mul ( v.texcoord1.xy, rotationMatrix );
        }

		void surf(Input IN, inout SurfaceOutputSpecularAntura o) 
		{
			fixed4 baseColor = tex2D(_MainTex, IN.uv2_MainTex)*tex2D(_Occlusion, IN.uv_Occlusion).a;
			fixed4 overmap = tex2D(_OverTex, IN.uv_OverTex);

			fixed3 overColor = baseColor*saturate(overmap.r*_OverColorR + overmap.g*_OverColorG + overmap.b*_OverColorB);

			fixed minValue = min(min(overmap.r, overmap.g), overmap.b);
			if (minValue == 1) overColor = _OverColorW; 

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
