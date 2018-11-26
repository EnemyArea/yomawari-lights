Texture2D ScreenTexture;
Texture2D LightTexture;
 
sampler ScreenTextureSampler = sampler_state
{
    Texture = <ScreenTexture>;
};
sampler LightTextureSampler = sampler_state
{
    Texture = <LightTexture>;
};

float CalcLuminance(float r, float g, float b)
{
    return (r * 0.3) + (g * 0.59) + (b * 0.11);
}

//Note: Color1 not needed.
float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET0
{
    float4 colorScreen = ScreenTexture.Sample(ScreenTextureSampler, texCoord.xy);
    float4 colorLight = LightTexture.Sample(LightTextureSampler, texCoord.xy);
    
    float4 baseColor = colorScreen;
    baseColor.a = CalcLuminance(colorLight.r, colorLight.g, colorLight.b);
    baseColor.r = baseColor.r * baseColor.a;
    baseColor.g = baseColor.g * baseColor.a;
    baseColor.b = baseColor.b * baseColor.a;
    
    if (dot(baseColor, float3(1, 1, 1)) == 0)
    {
        clip(-1);
    }
 
    return baseColor;
}
 
technique PassThrough
{
    pass Pass1
    {
        PixelShader = compile ps_4_0 PixelShaderFunction();
    }
}