Shader "RetroLookPro/Glitch2RetroLook"
{
	HLSLINCLUDE
	#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
	sampler2D _MainTex;
	sampler2D _NoiseTex;
	sampler2D _TrashTex;
	float _Intensity;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
    float2 uvs = i.texcoord;
    float4 glitch = tex2D(_NoiseTex, uvs);
	float thresh = 1.001 - _Intensity * 1.001;
	float w_d = step(thresh, pow(abs(glitch.z), 2.5)); 
	float w_f = step(thresh, pow(abs(glitch.w), 2.5)); 
	float w_c = step(thresh, pow(abs(glitch.z), 3.5)); 
	float2 uv = frac(uvs + glitch.xy * w_d);
	float4 source = tex2D(_MainTex, uv);
	float3 color = lerp(source, tex2D(_TrashTex, uv), w_f).rgb;
	float3 neg = saturate(color.grb + (1 - dot(color, 1)) * 0.1);
	color = lerp(color, neg, w_c);

	return float4(color, source.a);
	}

		ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment Frag

			ENDHLSL
		}
	}
}