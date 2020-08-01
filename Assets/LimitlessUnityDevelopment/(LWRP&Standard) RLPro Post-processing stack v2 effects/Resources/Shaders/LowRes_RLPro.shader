﻿Shader "RetroLookPro/LowRes_RLPro"
{
	HLSLINCLUDE
	#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
	sampler2D _MainTex;
	
		float4 Frag(VaryingsDefault i) : SV_Target
	{
		float2 pos = i.texcoord;
		float texelHeightOffset = 0.001;
		float texelWidthOffset = 0.001;
		float2 firstOffset = float2(texelWidthOffset, texelHeightOffset);
		float2 secondOffset = float2(1.0 * texelWidthOffset, 1.0 * texelHeightOffset);
		float2 thirdOffset = float2(2.0 * texelWidthOffset, 2.0 * texelHeightOffset);
		float2 fourthOffset = float2(3.0 * texelWidthOffset, 3.0 * texelHeightOffset);

		float2 centerTextureCoordinate = pos;
		float2 oneStepLeftTextureCoordinate = pos - firstOffset;
		float2 twoStepsLeftTextureCoordinate = pos - secondOffset;
		float2 threeStepsLeftTextureCoordinate = pos - thirdOffset;
		float2 fourStepsLeftTextureCoordinate = pos - fourthOffset;
		float2 oneStepRightTextureCoordinate = pos + firstOffset;
		float2 twoStepsRightTextureCoordinate = pos + secondOffset;
		float2 threeStepsRightTextureCoordinate = pos + thirdOffset;
		float2 fourStepsRightTextureCoordinate = pos + fourthOffset;

		float4 fragmentColor = tex2D(_MainTex, centerTextureCoordinate) * 0.38026;

		fragmentColor += tex2D(_MainTex, oneStepLeftTextureCoordinate) * 0.27667;
		fragmentColor += tex2D(_MainTex, oneStepRightTextureCoordinate) * 0.27667;

		fragmentColor += tex2D(_MainTex, twoStepsLeftTextureCoordinate) * 0.08074;
		fragmentColor += tex2D(_MainTex, twoStepsRightTextureCoordinate) * 0.08074;

		fragmentColor += tex2D(_MainTex, threeStepsLeftTextureCoordinate) * -0.02612;
		fragmentColor += tex2D(_MainTex, threeStepsRightTextureCoordinate) * -0.02612;

		fragmentColor += tex2D(_MainTex, fourStepsLeftTextureCoordinate) * -0.02143;
		fragmentColor += tex2D(_MainTex, fourStepsRightTextureCoordinate) * -0.02143;

		return fragmentColor;
	}
	float4 Frag1(VaryingsDefault i) : SV_Target
	{
        float2 pos = i.texcoord;
		float4 col = tex2D(_MainTex,pos);
		return col;
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
			Pass
		{
			HLSLPROGRAM

				#pragma vertex VertDefault
				#pragma fragment Frag1

			ENDHLSL
		}
	}
}