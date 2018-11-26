Texture2D ScreenTexture;
Texture2D LightTexture;
float4 ColorKey;

sampler ScreenTextureSampler = sampler_state
{
	Texture = <ScreenTexture>;
};
sampler LightTextureSampler = sampler_state
{
	Texture = <LightTexture>;
};

float CalcLuminance(float3 color)
{
	return (color.r * 0.3) + (color.g * 0.59) + (color.b * 0.11);
}

//Note: Color1 not needed.
float4 PixelShaderFunction(float4 pos : SV_POSITION, float4 color1 : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET0
{
	float4 colorScreen = ScreenTexture.Sample(ScreenTextureSampler, texCoord.xy);
	float4 colorLight = LightTexture.Sample(LightTextureSampler, texCoord.xy);

	float4 baseColor = colorScreen;

	if (baseColor.r == ColorKey.r && baseColor.g == ColorKey.g && baseColor.b == ColorKey.b)
	{
		clip(-1);
	}
	else 
	{
		if (CalcLuminance(colorLight.rgb) > CalcLuminance(ColorKey.rgb)) {
			baseColor.a = CalcLuminance(colorLight.rgb);
			baseColor.r = baseColor.r * baseColor.a;
			baseColor.g = baseColor.g * baseColor.a;
			baseColor.b = baseColor.b * baseColor.a;
		}
		else {
			clip(-1);
		}
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