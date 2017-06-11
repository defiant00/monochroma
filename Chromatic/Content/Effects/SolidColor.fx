float4 Color;

float4 main() : COLOR0
{
	return Color;
}

technique SolidColor
{
    pass Pass1
    {
		AlphaBlendEnable = true;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;
        PixelShader = compile ps_3_0 main();
    }
}
