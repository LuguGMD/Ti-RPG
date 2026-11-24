Shader "Lucca/Particles/AlphaBlendParticleOverlayShader"
{
    Properties
    {
        [MainColor] [HDR] _BaseColor("Tint Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Main Texture", 2D) = "white" {}
        _Gradient ("Gradient Map", 2D) = "white" {}
        _Emission ("Emission", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "PreviewType" = "Plane"}

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest Always
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_Gradient);
            SAMPLER(sampler_Gradient);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                float4 _Gradient_ST;
                float _Emission;
            CBUFFER_END

            float3 RGB_to_HSV(float3 In)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
                float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
                float D = Q.x - min(Q.w, Q.y);
                float  E = 1e-10;
                float3 Out = float3(abs(Q.z + (Q.w - Q.y)/(6.0 * D + E)), D / (Q.x + E), Q.x);
                return Out;
            }


            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.color = IN.color;
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                float4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                float3 color = tex.rgb * IN.color.rgb * _BaseColor.rgb;
                float alpha = saturate(tex.a * IN.color.a * _BaseColor.a);

                float2 gradientCoord = float2(RGB_to_HSV(color).z, 0);
                float4 gradient = SAMPLE_TEXTURE2D(_Gradient, sampler_Gradient, gradientCoord);
                return float4(gradient.rgb * _Emission, alpha);
            }
            ENDHLSL
        }
    }
}
