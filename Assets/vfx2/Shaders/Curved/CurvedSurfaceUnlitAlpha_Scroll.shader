Shader "Curved/CurvedSurfaceUnlitAlpha_Scroll" {
	Properties {
		[HDR]_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		//_Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
		_Mask("Mask", 2D) = "white" { }
		_ScrollX("X", Range(-5,5)) = 1
		_ScrollY("Y", Range(-5,5)) = 1
		_Speed("Speed",Range(0,100)) = 10
	}
	SubShader {
		Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off

		//Cull Off
        Lighting Off

		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows vertex:vert alpha:blend noshadow
		#pragma surface surf NoLighting   vertex:vert alpha:blend noambient

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Mask;

		struct Input {
			float2 uv_MainTex;
			float4 color : Color;
			float2 uv_Mask;
		};

		//half _Glossiness;
		//half _Metallic;
		fixed4 _Color;
		float _ScrollX;
		float _ScrollY;
		float _Speed;

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
			// Albedo comes from a texture tinted by color
			_ScrollX *= _Time.y * _Speed;
			_ScrollY *= _Time.y * _Speed;

			fixed4 maintex = tex2D(_MainTex, IN.uv_MainTex + float2(_ScrollX, _ScrollY));
			fixed4 mask = tex2D(_Mask, IN.uv_Mask);

			fixed4 c = maintex * mask * _Color;
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
