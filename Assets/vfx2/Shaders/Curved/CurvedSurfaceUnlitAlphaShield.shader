Shader "Curved/CurvedSurfaceUnlitAlphaShield" {
	Properties {
		[HDR]_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Mask("Mask", 2D) = "white" { }
		_Ramp("Gradient Ramp", Range(0, 1)) = 0.5
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
		
	}
	SubShader {
		Tags {"Queue"="Transparent" "RenderType"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off
		Cull off

		//Cull Off

		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows vertex:vert alpha:blend noshadow
		#pragma surface surf Lambert vertex:vert alpha


		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Mask;

		struct Input {
			float2 uv_MainTex : TEXCOORD0;
			float2 st_MainTex2 : TEXCOORD1;
			float3 normal : TEXCOORD2;
			float4 color : Color;
			float2 uv_Mask;
		};

		//half _Glossiness;
		//half _Metallic;
		fixed4 _Color;
		float _Ramp;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling

		#include "CurvedSurfaceCore.cginc"

		void vert(inout appdata_full v, out Input o) {
			float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
			o.uv_MainTex = TRANSFORM_UV(1);
			o.st_MainTex2 = float2(abs(dot(viewDir, v.normal)), .3);
			o.normal = v.normal;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			float ramp = lerp(0, 1, 1 - clamp(IN.st_MainTex2.x - (_Ramp - 0.5) * 2.0, 0, 1));
			fixed4 mask = tex2D(_Mask, IN.uv_Mask);

			half4 mTex = tex2D(_MainTex, IN.uv_MainTex) * ramp * _Color * IN.color * mask;

			o.Normal = normalize(IN.normal);
			o.Albedo = mTex.rgb;
			o.Alpha = ramp* IN.color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
