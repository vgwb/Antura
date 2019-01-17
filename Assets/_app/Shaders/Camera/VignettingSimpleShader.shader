// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/VignettingSimple" {
	Properties {
		_Color("Color", Color) = (0,0,0,0)
	}
	
	CGINCLUDE
	
	#include "UnityCG.cginc"
	
	struct v2f {
		float4 pos : POSITION;
		half2 uv : TEXCOORD0;
	};
	
	half _Intensity;
	half _FadeOut;
	fixed4 _Color;

	half4 _MainTex_TexelSize;
		
	v2f vert( appdata_img v ) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy;

		return o;
	} 
	
	fixed4 frag(v2f i) : COLOR {
		half2 coords = i.uv;
		half2 uv = i.uv;
		
		coords = (coords - 0.5) * 2.0;		
		half coordDot = dot (coords,coords);

		half mask = coordDot * _Intensity * 0.1; 
		
		return lerp(fixed4(_Color.rgb, mask), fixed4(0,0,0,1), _FadeOut);
	}

	ENDCG 
	
Subshader {
 Pass {
	  ZTest Always 
	  ZWrite Off
	  Cull Off
	  Blend SrcAlpha OneMinusSrcAlpha
	  ColorMask RGB
	  Fog { Mode off }


      CGPROGRAM
      #pragma fragmentoption ARB_precision_hint_fastest 
      #pragma vertex vert
      #pragma fragment frag
      ENDCG
  }
}

Fallback off	
} 