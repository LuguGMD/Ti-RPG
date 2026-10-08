void AllAdditionalLightsGranular_float(float3 WorldPosition, float3 Normals, float2 ScreenPosition, float Grains, float ShadowNoiseStrength, float ShadowThreshold, float ShadowNoiseSoftness, 
                                       out float3 Color, out float Shadows)
{
    Color = 0;
    Shadows = 0;
#ifndef SHADERGRAPH_PREVIEW

    uint pixelLightCount = GetAdditionalLightsCount();

    #if USE_FORWARD_PLUS
        InputData inputData = (InputData)0;
        inputData.normalizedScreenSpaceUV = ScreenPosition;
        inputData.positionWS = WorldPosition;
    #endif

    LIGHT_LOOP_BEGIN(pixelLightCount)

        #if !USE_FORWARD_PLUS
            lightIndex = GetPerObjectLightIndex(lightIndex);
        #endif

        Light light = GetAdditionalPerObjectLight(lightIndex, WorldPosition);

        light.shadowAttenuation = AdditionalLightRealtimeShadow(lightIndex, WorldPosition, light.direction);
        float atten = light.distanceAttenuation * light.shadowAttenuation;

        float halfLambert = dot(Normals, light.direction);
        halfLambert *= 0.5f;
        halfLambert += 0.5f;
        halfLambert = saturate(halfLambert);

        float edge1 = (1 - Grains) * ShadowNoiseStrength + ShadowThreshold;
        float edge2 = edge1 + ShadowNoiseSoftness;

        float diffuse = smoothstep(edge1, edge2, halfLambert);

        Shadows += diffuse * atten;
        Color += Shadows * light.color * dot(Normals, light.direction);
    LIGHT_LOOP_END
#endif
}