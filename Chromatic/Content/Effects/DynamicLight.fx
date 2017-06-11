// Applies a precalculated light map to the image.

sampler2D TextureSampler : register(s0)
{
	MagFilter = Point;
};
texture2D Light;
sampler2D LightSampler = sampler_state {
	Texture = <Light>;
	MagFilter = Point;
};

float4 main(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    // Multiply the main texture by the light map.
    return tex2D(TextureSampler, texCoord) * tex2D(LightSampler, texCoord) * color;
}

technique DynamicLight
{
    pass Pass1
    {
        PixelShader = compile ps_3_0 main();
    }
}
