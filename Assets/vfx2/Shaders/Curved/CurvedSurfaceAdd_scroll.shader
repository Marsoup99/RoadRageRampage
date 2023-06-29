Shader "Curved/CurvedSurfaceAdditiveScroll" {
    Properties {
        [HDR]_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" { }
        [NoScaleOffset] _FlowMap("Flow (RG)", 2D) = "black" {}
        _ScrollX("X", Range(-5,5)) = 1
        _ScrollY("Y", Range(-5,5)) = 1
        _Speed("Speed",Range(0,100)) = 10
    }

    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }

        Blend SrcAlpha One

        Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		LOD 200

        CGPROGRAM

        //#pragma exclude_renderers flash
        #pragma surface surf Lambert vertex:vert noforwardadd

        sampler2D _MainTex;
        sampler2D _FlowMap;

        struct Input {
            float2 uv_MainTex;
            float4 color : Color;
        };

        float3 _Curvature;
        float _ScrollX;
        float _ScrollY;
        float _Speed;

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input,o);
            o.color = v.color;

            // for Curved
            float4 vPos = mul(unity_ObjectToWorld, v.vertex);

            float dist = vPos.z - _WorldSpaceCameraPos.z;
            float addY = dist * dist;
            vPos.y -= addY * _Curvature.y;

            dist = vPos.x - _WorldSpaceCameraPos.x;
            float addHY = dist * dist;
            vPos.y -= addHY * _Curvature.x;

            // for corner
            vPos.x += addY * _Curvature.z;

            vPos = mul(unity_WorldToObject, vPos);
            v.vertex = vPos;
        }

        fixed4 _Color;

        void surf(Input IN, inout SurfaceOutput o) {
            // Albedo comes from a texture tinted by color
            _ScrollX *= _Time.x * _Speed;
            _ScrollY *= _Time.y * _Speed;
            float3 sur = (tex2D(_MainTex, IN.uv_MainTex + float2(_ScrollX, _ScrollY))).rgb;
            
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb * IN.color.rgb * sur;
            o.Alpha = c.a * IN.color.a;

        }
        ENDCG

    }
    FallBack "Diffuse"
}