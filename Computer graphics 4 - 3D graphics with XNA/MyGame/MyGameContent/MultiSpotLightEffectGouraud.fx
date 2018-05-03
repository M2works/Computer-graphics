float4x4 World;
float4x4 View;
float4x4 Projection;

float3 CameraPosition;
texture BasicTexture;

#define NUMLIGHTS 2

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
float3 LightPosition[NUMLIGHTS];
float3 LightDirection[NUMLIGHTS];
float ConeAngle[NUMLIGHTS];
float3 LightColor[NUMLIGHTS];
float LightFalloff[NUMLIGHTS];

float LightAttenuation = 30000;
float SpecularPower = 128;
float3 SpecularColor = float3(1, 1, 1);

float FogStart = 5000;
float FogEnd = 25000;
float3 FogColor = float3(105 / 255, 105 / 255, 105 / 255);

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
	float3 Lightning : TEXCOORD1;
	float3 ViewDirection : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.UV = input.UV;
	float3 normal = normalize(mul(input.Normal, World));
	float3 view = normalize(worldPosition - CameraPosition);
	float3 diffuseColor = DiffuseColor;
	float3 firstDot, secondDot;

	float3 totalLight = AmbientColor;

	for (int i = 0; i < NUMLIGHTS; i++)
	{
		float3 lightDir = normalize(LightPosition[i] - worldPosition);

		// (dot(p - lp, ld) / cos(a))^f
		float d = dot(-lightDir, normalize(LightDirection[i]));
		float a = cos(ConeAngle[i]);
		float att = 0;

		if (a < d)
			att = 1 - pow(clamp(a / d, 0, 1), LightFalloff[i]);
		totalLight += saturate(dot(normal, lightDir)) * att * LightColor[i];

		float3 refl = reflect(lightDir, normal);

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
	}
	output.ViewDirection = worldPosition - CameraPosition;
	output.Lightning = saturate(totalLight) * diffuseColor;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 output = input.Lightning;

	if (TextureEnabled)
		output *= tex2D(BasicTextureSampler, input.UV).rgb;
	
	float dist = length(input.ViewDirection);
	float fog = clamp((dist - FogStart) / (FogEnd - FogStart), 0, 1);

	return float4(lerp(output, FogColor, fog), 1);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_3_0 VertexShaderFunction();
		PixelShader = compile ps_3_0 PixelShaderFunction();
	}
}