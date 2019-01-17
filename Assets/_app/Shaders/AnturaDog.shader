// HowTo:
// This shader should be used by antura base materials (e.g. robotic, wool, etc.)
// Add this e.g. as first in the list of renderer materials
// This uses uv2

// To be used with the next version (two uvs) of the dog!
Shader "Antura/Dog" {
	Properties{
		_MainTex("Base (RGB) Glossiness (A)", 2D) = "white" {}
		_Occlusion("Occlusion (A)", 2D) = "white" {}
		_BaseColor("Base Color", Color) = (1,1,1,1)

		_Shininess("Shininess", Range(0.03, 1)) = 0.078125
		_Specular("Specular", Range(0,1)) = 0.5

		_Emission("Emission", Color) = (0,0,0,0)
		_SpecularColor("Specular Color", Color) = (0,0,0,0)

		_HueOffset("Hue Offset", Range(0,1)) = 0
		_SaturationOffset("Saturation Offset", Range(-1,1)) = 0
		_BrightnessOffset("Brightness Offset", Range(-1,1)) = 0
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }
		LOD 250


		CGPROGRAM
#pragma surface surf AnturaBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview interpolateview

#include "LightningSpecular.cginc"

		sampler2D _MainTex;
		sampler2D _Occlusion;

		half _Shininess;
		half _Specular;

		fixed4 _BaseColor;

		fixed3 _Emission;
		fixed3 _SpecularColor;

		float _HueOffset;
		float _SaturationOffset;
		float _BrightnessOffset;

		struct Input {
			half2 uv2_MainTex;
			half2 uv_Occlusion;
		};

		half3 RGBToHSB(fixed3 color)
		{
			half3 ret = half3(0, 0, 0);

			half r = color.r;
			half g = color.g;
			half b = color.b;

			half maxValue = max(max(r, g), b);

			if (maxValue <= 0)
				return ret;

			half minValue = min(min(r, g), b);
			half dif = maxValue - minValue;

			if (maxValue > minValue)
			{
				if (g == maxValue)
				{
					ret.x = (b - r) / dif * 60.0 + 120.0;
				}
				else if (b == maxValue)
				{
					ret.x = (r - g) / dif * 60.0 + 240.0;
				}
				else if (b > g)
				{
					ret.x = (g - b) / dif * 60.0 + 360.0;
				}
				else
				{
					ret.x = (g - b) / dif * 60.0;
				}
				if (ret.x < 0)
				{
					ret.x = ret.x + 360.0;
				}
			}
			else
			{
				ret.x = 0;
			}

			ret.x *= (1 / 360.0);
			ret.y = (dif / maxValue);
			ret.z = maxValue;

			return ret;
		}

		fixed3 HSBToRGB(half3 hsbColor)
		{
			half r = hsbColor.z;
			half g = hsbColor.z;
			half b = hsbColor.z;

			if (hsbColor.y != 0)
			{
				half max = hsbColor.z;
				half dif = hsbColor.z * hsbColor.y;
				half min = hsbColor.z - dif;

				half h = hsbColor.x * 360.0;

				if (h < 60.0)
				{
					r = max;
					g = h * dif / 60.0 + min;
					b = min;
				}
				else if (h < 120.0)
				{
					r = -(h - 120.0) * dif / 60.0 + min;
					g = max;
					b = min;
				}
				else if (h < 180.0)
				{
					r = min;
					g = max;
					b = (h - 120.0) * dif / 60.0 + min;
				}
				else if (h < 240)
				{
					r = min;
					g = -(h - 240.0) * dif / 60.0 + min;
					b = max;
				}
				else if (h < 300.0)
				{
					r = (h - 240) * dif / 60 + min;
					g = min;
					b = max;
				}
				else if (h <= 360.0)
				{
					r = max;
					g = min;
					b = -(h - 360.0) * dif / 60.0 + min;
				}
				else
				{
					r = 0;
					g = 0;
					b = 0;
				}
			}

			return fixed3(r, g, b);
		}

		void surf(Input IN, inout SurfaceOutputSpecularAntura o) {
			fixed4 baseColor = tex2D(_MainTex, IN.uv2_MainTex)*tex2D(_Occlusion, IN.uv_Occlusion).a;
			
			half3 hsb = RGBToHSB(baseColor.rgb);

			hsb.x = frac(hsb.x + _HueOffset); 
			hsb.y += _SaturationOffset;
			hsb.z += _BrightnessOffset;

			baseColor.rgb = HSBToRGB(hsb) * _BaseColor;

			o.Albedo = baseColor.rgb;
			o.Gloss = _Specular*baseColor.a;
			o.Alpha = 1;
			o.Specular = _Shininess;
			o.SpecularColor = _SpecularColor;
			o.Emission = _Emission;
		}
	ENDCG
	}

	FallBack "Mobile/VertexLit"
}
