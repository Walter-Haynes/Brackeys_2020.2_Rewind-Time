Shader "RetroLookPro/BottomNoiseEffect"
{
	HLSLINCLUDE
	#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

	half tileX = 0;
	half tileY = 0;
	half _OffsetNoiseX;
	half _OffsetNoiseY;
	sampler2D _MainTex;
	float4 _MainTex_TexelSize;
	sampler2D _SecondaryTex;

	half _OffsetPosY;
	half _NoiseBottomHeight;
	half _NoiseBottomIntensity;

	float4 Frag(VaryingsDefault i) : SV_Target
	{
        float2 uv = i.texcoord;
		half4 color = tex2D(_MainTex, uv);
		float condition = saturate(floor(_NoiseBottomHeight / uv.y));
		float4 noise_bottom = tex2D(_SecondaryTex, uv - 0.5) * condition * _NoiseBottomIntensity;
		color = lerp(color, noise_bottom, -noise_bottom * ((uv.y / (_NoiseBottomHeight)) - 1.0));
		float exp = 1.0;
		return float4(pow(color.xyz, float3(exp, exp, exp)), color.a);
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