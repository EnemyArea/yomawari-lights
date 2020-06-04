#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

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

float CalcLuminance(float3 color)
{
	return (color.r * 0.3) + (color.g * 0.59) + (color.b * 0.11);
}

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float4 col = tex2D(ScreenTextureSampler, input.TextureCoordinates) *input.Color; //sample the source image
	float mask = tex2D(LightTextureSampler, input.TextureCoordinates).r; //sample the R component of the mask image.

	if (mask > 0) // if the mask is >0 apply it.
		return float4(col.r,col.g,col.b,col.a*mask); // keep all colors, and use the alpha of the source.

	return float4(0, 0, 0, 0); //otherwise return a transparent (masked) pixel- without color information.
}

technique PassThrough
{
	pass Pass1
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
}