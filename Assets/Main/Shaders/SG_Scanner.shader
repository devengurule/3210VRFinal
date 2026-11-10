
Shader "Custom/SG_Scanner"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}

        _ScannerPosition("Scanner Position", Vector) = (0, 0, 0, 0)
        _Radius("Scanner Radius", Float) = 1.5
        _Softness("Scanner Softness", Float) = 0.25
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)

                half4 _BaseColor;
                float4 _BaseMap_ST;

                float4 _ScannerPosition;
                float _Radius;
                float _Softness;

            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                OUT.positionHCS =
                    TransformObjectToHClip(IN.positionOS.xyz);

                OUT.positionWS =
                    TransformObjectToWorld(IN.positionOS.xyz);

                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(
                    _BaseMap,
                    sampler_BaseMap,
                    IN.uv
                ) * _BaseColor;

                // Calculate world-space distance from scanner.
                float distanceToScanner = distance(
                    IN.positionWS,
                    _ScannerPosition.xyz
                );

                // Calculate inner and outer radius.
                float innerRadius = max(
                    0.0,
                    _Radius - _Softness
                );

                float outerRadius = _Radius + _Softness;

                // Inside the scanner radius = invisible.
                // Outside the scanner radius = visible.
                float visibility = smoothstep(
                    innerRadius,
                    max(outerRadius, innerRadius + 0.001),
                    distanceToScanner
                );

                // Apply visibility to alpha.
                color.a *= visibility;

                return color;
            }

            ENDHLSL
        }
    }
}
