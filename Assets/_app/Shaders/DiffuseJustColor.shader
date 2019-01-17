Shader "Antura/JustColor/Diffuse"
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_Emission("Emission", Color) = (0,0,0,0)
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 150

		CGPROGRAM
#pragma surface surf Lambert noforwardadd

		struct Input {
			fixed3 color : COLOR;
		};

		fixed4 _Color;
		fixed3 _Emission;

		void surf(Input IN, inout SurfaceOutput o)
		{
			fixed4 c = _Color;
			o.Albedo = c.rgb * IN.color;
			o.Emission = _Emission;
			o.Alpha = c.a;
		}
		ENDCG
	}

		//FallBack "Diffuse"
}
