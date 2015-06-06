sampler samplerState;

float yOffsetR;
float yOffsetG;
float yOffsetB;

float xOffsetR;
float xOffsetG;
float xOffsetB;

float transparency;


struct VertexShaderOutput
{
    float2 TexCoord : TEXCOORD0;
};


float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 col = tex2D(samplerState, input.TexCoord.xy);

	return col;
}


float4 RGB(VertexShaderOutput Input) : COLOR0
{
	float4 colR = tex2D(samplerState, float2(Input.TexCoord.x+xOffsetR,Input.TexCoord.y+yOffsetR) );
	colR.gb = 0;
	colR.a = 0.1;

	float4 colG = tex2D(samplerState, float2(Input.TexCoord.x+xOffsetG,Input.TexCoord.y+yOffsetG) );
	colG.rb = 0;

	float4 colB = tex2D(samplerState, float2(Input.TexCoord.x +xOffsetB,Input.TexCoord.y+yOffsetB) );
	colB.rg = 0;

	float4 col = colR + colG + colB;

	col.a = transparency;
	return col;
}


technique Technique1
{
    pass Pass1
    {	
		AlphaBlendEnable	= true;
		SrcBlend			= SrcAlpha;
		DestBlend			= InvSrcAlpha;
        PixelShader = compile ps_2_0 RGB();
    }
}
