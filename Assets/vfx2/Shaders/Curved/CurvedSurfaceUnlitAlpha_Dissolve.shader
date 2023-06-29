Shader "Curved/CurvedSurfaceUnlitAlpha_Dissolve" {
	Properties {
		[HDR]_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0

		//Dissolve properties
		_DissolveTexture("Dissolve Texutre", 2D) = "white" {}
		_Amount("Amount", Range(0,1)) = 0
	}
	SubShader {
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent"}

		//Cull Off
        Lighting Off
		Cull Off
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows vertex:vert alpha:blend noshadow
		#pragma surface surf NoLighting   vertex:vert alpha:blend noambient

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float4 color : Color;
		};

		//half _Glossiness;
		//half _Metallic;
		fixed4 _Color;

		sampler2D _DissolveTexture;
		half _Amount;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		#include "CurvedSurfaceCore.cginc"

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
     {
         fixed4 c;
         c.rgb = s.Albedo;
         c.a = s.Alpha;
         return c;
     }

		void surf (Input IN, inout SurfaceOutput o) {
			//Dissolve function
			half dissolve_value = tex2D(_DissolveTexture, IN.uv_MainTex).r;
			clip(dissolve_value - _Amount);
			o.Emission = fixed3(1, 1, 1) * step(dissolve_value - _Amount, 0.05f);

			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb * IN.color.rgb;

			// Metallic and smoothness come from slider variables
			//o.Metallic = _Metallic;
			//o.Smoothness = _Glossiness;
			o.Alpha = c.a * IN.color.a;
		}
		ENDCG
	}
	FallBack "VertexLit"
}
