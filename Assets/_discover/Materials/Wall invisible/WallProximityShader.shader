Shader "Custom/WallProximityShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
        _PlayerPosition ("Player Position", Vector) = (0,0,0,0)
        _Radius ("Radius", Float) = 5.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        sampler2D _MainTex;
        fixed4 _Color;
        half _Cutoff;
        float4 _PlayerPosition;
        float _Radius;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            float dist = distance(IN.worldPos, _PlayerPosition.xyz);
            if (dist < _Radius)
            {
                c.a = 1.0 - (dist / _Radius);
            }
            else
            {
                c.a = 0.0;
            }
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}