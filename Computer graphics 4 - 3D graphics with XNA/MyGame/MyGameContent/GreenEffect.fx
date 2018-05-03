//float4x4 World;
//float4x4 View;
//float4x4 Projection;
//
//texture BasicTexture;
//float3 DiffuseColor = float3(1, 1, 1); 
//float3 AmbientLightColor = float3(.1, .1, .1);
//float3 LightDirection = float3(1, 1, 1);
//float3 LightColor = float3(0.9, 0.9, 0.9);
//float SpecularPower = 32;
//float3 SpecularColor = float3(1, 1, 1);
//float3 CameraPosition;
//
//sampler BasicTextureSampler = sampler_state {
//	texture = <BasicTexture>;
//	MinFilter = Anisotropic; // Minification Filter
//	MagFilter = Anisotropic; // Magnification Filter
//	MipFilter = Linear; // Mip-mapping
//	AddressU = Wrap; // Address Mode for U Coordinates
//	AddressV = Wrap; // Address Mode for V Coordinates
//};
//
//bool TextureEnabled = false;
//
//struct VertexShaderInput
//{
//	float4 Position : POSITION0;
//	float2 UV : TEXCOORD0;
//	float3 Normal : NORMAL0;
//};
//
//struct VertexShaderOutput
//{
//	float4 Position : POSITION0;
//	float2 UV : TEXCOORD0;
//	float3 Normal : TEXCOORD1;
//	float3 ViewDirection : TEXCOORD2;
//};
//
//
//VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
//{
//	VertexShaderOutput output;
//	float4 worldPosition = mul(input.Position, World);
//	float4x4 viewProjection = mul(View, Projection);
//	output.Position = mul(worldPosition, viewProjection);
//	output.UV = input.UV;
//	output.Normal = mul(input.Normal, World);
//	output.ViewDirection = worldPosition - CameraPosition;
//	return output;
//}
//
//float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
//{
//	// Start with diffuse color
//	float3 color = DiffuseColor;
//	// Texture if necessary
//	if (TextureEnabled)
//		color *= tex2D(BasicTextureSampler, input.UV);
//	// Start with ambient lighting
//	float3 lighting = AmbientLightColor;
//	float3 lightDir = normalize(LightDirection);
//	float3 normal = normalize(input.Normal);
//	// Add lambertian lighting
//	lighting += saturate(dot(lightDir, normal)) * LightColor;
//	float3 refl = reflect(lightDir, normal);
//	float3 view = normalize(input.ViewDirection);
//	// Add specular highlights
//	lighting += pow(saturate(dot(refl, view)), SpecularPower) *
//		SpecularColor;
//	// Calculate final color
//	float3 output = saturate(lighting) * color;
//	return float4(output, 1);
//}
//
//technique Technique1
//{
//	pass Pass1
//	{
//		VertexShader = compile vs_1_1 VertexShaderFunction();
//		PixelShader = compile ps_2_0 PixelShaderFunction();
//	}
//}

float4x4 World;
float4x4 View;
float4x4 Projection;

float3 CameraPosition;
texture BasicTexture;
sampler BasicTextureSampler = sampler_state {
	texture = <BasicTexture>;
	MinFilter = Anisotropic; // Minification Filter
	MagFilter = Anisotropic; // Magnification Filter
	MipFilter = Linear; // Mip-mapping
	AddressU = Wrap; // Address Mode for U Coordinates
	AddressV = Wrap; // Address Mode for V Coordinates
};

bool TextureEnabled = false;

float3 AmbientColor = float3(.15, .15, .15);
float3 DiffuseColor = float3(.85, .85, .85);

float3 LightDirection = float3(1, 1, 1);
float3 LightColor = float3(1,1,1);
float3 LightPosition = float3(0, 0, 0);
float LightAttenuation = 15000;
float LightFalloff = 2;
float SpecularPower = 1;
float3 SpecularColor = float3(1, 1, 1);
struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : NORMAL0;
};
struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : TEXCOORD1; 
	float4 WorldPosition : TEXCOORD2;
};
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.WorldPosition = worldPosition;
	output.UV = input.UV;
	output.Normal = mul(input.Normal, World);

	return output;
}
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 diffuseColor = DiffuseColor;
	if (TextureEnabled)
		diffuseColor *= tex2D(BasicTextureSampler, input.UV).rgb;
	float3 totalLight = float3(0, 0, 0);
	totalLight += AmbientColor;
	float3 lightDir = normalize(LightPosition - input.WorldPosition);
	float diffuse = saturate(dot(normalize(input.Normal), lightDir));
	float d = distance(LightPosition, input.WorldPosition);
	float att = 1 - pow(clamp(d / LightAttenuation, 0, 1),
		LightFalloff);
	totalLight += diffuse * att * LightColor;
	return float4(diffuseColor * totalLight, 1);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_1_1 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}