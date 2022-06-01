//czarno-biały

//sampler2D tex;
//
//float4 PixelShaderFunction(float4 Position : POSITION0, float2 UV : TEXCOORD0) : COLOR0
//{
//    float4 color = tex2D(tex, UV);
//    float intensity = 0.3f * color.r + 0.59f * color.g + 0.11f * color.b;
//    return float4(intensity, intensity, intensity, color.a);
//}
//
//technique Technique1
//{
//    pass Pass1
//    {
//        PixelShader = compile ps_2_0 PixelShaderFunction();
//    }
//}


sampler2D tex;

float treshold = 0.05;
float pixelx = 0.001;
float pixely = 0.002;

float Translate(float2 uv) {
	float3 color = tex2D(tex, uv);
	return (color.r + color.g + color.b) / 3;
}

float4 PixelShaderFunction(float4 Position : POSITION0, float2 UV : TEXCOORD0) : COLOR0
{

	float2 delta = float2(0.001, 0.002);
	 
	float4 hr = float4(0, 0, 0, 0);
	float4 vt = float4(0, 0, 0, 0);

	hr += tex2D(tex, (UV + float2(-1.0, -1.0) * delta)) * 1.0;
	hr += tex2D(tex, (UV + float2(0.0, -1.0) * delta)) * 0.0;
	hr += tex2D(tex, (UV + float2(1.0, -1.0) * delta)) * -1.0;
	hr += tex2D(tex, (UV + float2(-1.0,  0.0) * delta)) * 2.0;
	hr += tex2D(tex, (UV + float2(0.0,  0.0) * delta)) * 0.0;
	hr += tex2D(tex, (UV + float2(1.0,  0.0) * delta)) * -2.0;
	hr += tex2D(tex, (UV + float2(-1.0,  1.0) * delta)) * 1.0;
	hr += tex2D(tex, (UV + float2(0.0,  1.0) * delta)) * 0.0;
	hr += tex2D(tex, (UV + float2(1.0,  1.0) * delta)) * -1.0;

	vt += tex2D(tex, (UV + float2(-1.0, -1.0) * delta)) * 1.0;
	vt += tex2D(tex, (UV + float2(0.0, -1.0) * delta)) * 2.0;
	vt += tex2D(tex, (UV + float2(1.0, -1.0) * delta)) * 1.0;
	vt += tex2D(tex, (UV + float2(-1.0,  0.0) * delta)) * 0.0;
	vt += tex2D(tex, (UV + float2(0.0,  0.0) * delta)) * 0.0;
	vt += tex2D(tex, (UV + float2(1.0,  0.0) * delta)) * 0.0;
	vt += tex2D(tex, (UV + float2(-1.0,  1.0) * delta)) * -1.0;
	vt += tex2D(tex, (UV + float2(0.0,  1.0) * delta)) * -2.0;
	vt += tex2D(tex, (UV + float2(1.0,  1.0) * delta)) * -1.0;

	if (Translate(vt * vt + hr * hr) < treshold) return Translate(tex2D(tex, UV));
	return sqrt(hr * hr + vt * vt)/3;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
