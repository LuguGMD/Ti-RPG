void MainLight_float(float3 WorldPosition, float3 Normals, out float HalfLambert, out float3 Color, out float ShadowAtten)
{

#ifdef SHADERGRAPH_PREVIEW
    Color = float3(1,1,1);
    ShadowAtten = 1;
    HalfLambert = 0;
#else
    #if defined(UNIVERSAL_PIPELINE_CORE_INCLUDED)
        float4 shadowCoord = TransformWorldToShadowCoord(WorldPosition);
        Light mainLight = GetMainLight(shadowCoord);
        Color = mainLight.color;
        ShadowAtten = mainLight.shadowAttenuation;

        HalfLambert = dot(mainLight.direction, Normals);
        HalfLambert *= 0.5f;
        HalfLambert += 0.5f;

	if (dot(mainLight.color, float3(1,1,1)) <= 0.0001)
	{
	    HalfLambert = 0;
	}
    #else
        Color = float3(1, 1, 1);
        ShadowAtten = 0;
        HalfLambert = 0;
    #endif
#endif

}


