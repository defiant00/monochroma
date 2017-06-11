float4x4 wpvMatrix;

float4 Color;

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	output.Position = mul(input.Position, wpvMatrix);
	output.TextureCoordinate = input.TextureCoordinate;

	return output;
}

float4 main(float2 texCoord : TEXCOORD0) : COLOR0
{
	return lerp(Color, float4(Color.x, Color.y, Color.z, 0), distance(float2(0.5, 0.5), texCoord) * 2);
}

technique Radial
{
    pass Pass1
    {
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = One;
		VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 main();
    }
}
