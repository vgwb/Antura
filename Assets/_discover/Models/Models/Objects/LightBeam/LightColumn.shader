Shader "Custom/LightColumn"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _Height ("Height", Float) = 5.0
        _Fade ("Fade", Range(0.1, 5.0)) = 1.0
        _PulseSpeed ("Pulse Speed", Range(0, 5)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _MainColor;
            float _Height;
            float _Fade;
            float _PulseSpeed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Vertical gradient
                float height = saturate(i.uv.y * _Height);
                float fade = pow(1.0 - height, _Fade);
                
                // Pulsing effect
                float pulse = sin(_Time.y * _PulseSpeed) * 0.2 + 0.8;
                
                // Combine
                fixed4 col = _MainColor;
                col.a *= fade * pulse;
                
                return col;
            }
            ENDCG
        }
    }
}