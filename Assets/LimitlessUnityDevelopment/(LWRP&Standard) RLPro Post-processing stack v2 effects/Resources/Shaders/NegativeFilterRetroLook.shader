Shader "RetroLookPro/NegativeFilterRetroLook"
{
	HLSLINCLUDE

	#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
	uniform sampler2D _MainTex;
	uniform float Luminosity;
	uniform float Vignette;
	uniform float Negative;

	float3 linearLight(float3 s, float3 d)
	{
		return 2.0 * s + d - 1.0 * Luminosity;
	}
	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float2 uv = i.texcoord;
		float3 col = tex2D(_MainTex, uv).rgb;
		col = lerp(col,1 - col,Negative);
		col *= pow(abs(16.0 * uv.x * (1.0 - uv.x) * uv.y * (1.0 - uv.y)), 0.4) * 1 + Vignette;
		col = dot(float3(0.2126, 0.7152, 0.0722), col);
		col = linearLight(float3(0.5,0.5,0.5),col);
		return float4(col, tex2D(_MainTex,uv).a);
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