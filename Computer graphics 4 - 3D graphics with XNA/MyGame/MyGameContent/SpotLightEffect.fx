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

bool TextureEnabled = true;
bool BlinnModel = false;


float3 AmbientColor;
float3 DiffuseColor = float3(.85, .85, .85);
float3 LightPosition;
float3 LightDirection;
float ConeAngle;
float3 LightColor;
float LightFalloff;

float LightAttenuation = 20000;
float SpecularPower = 128;
float3 SpecularColor = float3(1, 1, 1);

//float a = cos(ConeAngle); //pixel shading variables
//float att = 0;

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
	float3 ViewDirection : TEXCOORD3;
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
	output.ViewDirection = worldPosition - CameraPosition;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 diffuseColor = DiffuseColor;
	float3 view = normalize(input.ViewDirection);
	float3 normal = normalize(input.Normal);
	float3 firstDot, secondDot;

	if (TextureEnabled)
		diffuseColor *= tex2D(BasicTextureSampler, input.UV).rgb;

	float3 totalLight = AmbientColor;
	float3 lightDir = normalize(LightPosition - input.WorldPosition);

	// (dot(p - lp, ld) / cos(a))^f
	float d = dot(-lightDir, normalize(LightDirection));
	float a = cos(ConeAngle);
	float att = 0;

	if (a < d)
		att = 1 - pow(clamp(a / d, 0, 1), LightFalloff);
	totalLight += saturate(dot(normal, lightDir)) * att * LightColor;

	float3 refl = reflect(lightDir, normal);
	// Add specular highlights
	if (!BlinnModel)
	{
		firstDot = refl; secondDot = view;
	}
	else
	{
		float3 mixedVectorH = (lightDir + view) / length(lightDir + view);
		firstDot = normal; secondDot = mixedVectorH;
	}
	totalLight += pow(saturate(dot(firstDot, secondDot)), SpecularPower)
		* SpecularColor;

	float3 output = saturate(totalLight) * diffuseColor;
	return float4(output, 1);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_1_1 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}