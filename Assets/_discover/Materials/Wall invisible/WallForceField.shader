Shader "Custom/WallForceField"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.3, 0.7, 1, 0.25)
        _MainTex ("Noise (RGB)", 2D) = "white" {}
        _BaseAlpha ("Base Alpha", Range(0,1)) = 0.25
        _NearBoost ("Near Alpha Boost", Range(0,1)) = 0.6
        _Radius ("Near Radius", Float) = 5.0
        _FresnelPower ("Fresnel Power", Range(0.1,8)) = 3.0
        _Emission ("Emission Intensity", Range(0,5)) = 1.5
        _Scroll ("Noise Scroll XY", Vector) = (0.1, 0.0, 0, 0)
        _PlayerPosition ("Player Position", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _BaseColor;
        half _BaseAlpha, _NearBoost, _FresnelPower, _Emission;
        float4 _Scroll;
        float4 _PlayerPosition;
        float _Radius;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.uv_MainTex + _Scroll.xy * _Time.y;
            fixed4 noise = tex2D(_MainTex, uv);

            // Fresnel rim
            float ndv = saturate(dot(normalize(o.Normal), normalize(IN.viewDir)));
            float fres = pow(1.0 - ndv, _FresnelPower);

            // Distance boost near player
            float d = distance(IN.worldPos, _PlayerPosition.xyz);
            float near = saturate(1.0 - d / max(_Radius, 0.0001));

            // Alpha
            float a = _BaseAlpha + _NearBoost * near;
            a = saturate(a);

            // Color & emission
            float emissive = (_Emission * (0.5 + 0.5 * sin(_Time.y * 3.0))) + fres * 2.0;
            fixed3 col = _BaseColor.rgb * (1.0 + noise.r * 0.25);

            o.Albedo = col;
            o.Emission = col * emissive;
            o.Alpha = a;
            o.Metallic = 0;
            o.Smoothness = 0.6;
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
