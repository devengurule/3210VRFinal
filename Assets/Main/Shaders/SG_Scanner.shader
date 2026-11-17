
Shader "Custom/SG_Scanner"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}

        _Opacity("Object Opacity", Range(0, 1)) = 1

        _ScannerPosition("Scanner Position", Vector) = (0, 0, 0, 0)
        _Radius("Scanner Radius", Float) = 1.5
        _Softness("Scanner Softness", Float) = 0.25

        [Toggle] _UseClip("Use Opaque Cutout", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
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

                float _Opacity;

                float4 _ScannerPosition;
                float _Radius;
                float _Softness;

                float _UseClip;

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

                // Apply overall object opacity.
                color.a *= _Opacity;

                // Calculate distance from scanner.
                float distanceToScanner = distance(
                    IN.positionWS,
                    _ScannerPosition.xyz
                );

                // Calculate scanner transition.
                float innerRadius = max(
                    0.0,
                    _Radius - _Softness
                );

                float outerRadius = max(
                    _Radius + _Softness,
                    innerRadius + 0.001
                );

                // Outside = visible.
                // Inside = invisible.
                float visibility = smoothstep(
                    innerRadius,
                    outerRadius,
                    distanceToScanner
                );

                color.a *= visibility;

                // Optional opaque cutout mode.
                if (_UseClip > 0.5)
                {
                    clip(color.a - 0.5);
                    color.a = 1;
                }

                return color;
            }

            ENDHLSL
        }
    }
}
