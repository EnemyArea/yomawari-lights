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
 
//Note: Color1 not needed.
float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET0
{
    float4 colorScreen = ScreenTexture.Sample(ScreenTextureSampler, texCoord.xy);
    float4 colorLight = LightTexture.Sample(LightTextureSampler, texCoord.xy);
    
    float4 baseColor = colorScreen * colorLight;
    //baseColor.a = colorLight / colorScreen;
    
    //if (dot(baseColor, float3(1, 1, 1)) == 0)
    //{
    //    clip(-1);
    //}
 
    return baseColor;
}
 
technique PassThrough
{
    pass Pass1
    {
        PixelShader = compile ps_4_0 PixelShaderFunction();
    }
}