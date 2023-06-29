Shader "Curved/CurvedSurfaceAdditiveDistortion" {
    Properties {
        [HDR]_Color("Main Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" { }
        _Mask("Mask", 2D) = "white" { }
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
        sampler2D _Mask;

        struct Input {
            float2 uv_MainTex;
            float4 color : Color;
            float2 uv_Mask;
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
            _ScrollX *= _Time.y * _Speed;
            _ScrollY *= _Time.y * _Speed;

            fixed4 maintex = tex2D(_MainTex, IN.uv_MainTex + float2(_ScrollX, _ScrollY));
            fixed4 mask = tex2D(_Mask, IN.uv_Mask);
            
            fixed4 c = maintex * mask * _Color;
            o.Albedo = c.rgb * IN.color.rgb;
            o.Alpha = c.a * IN.color.a;

        }
        ENDCG

    }
    FallBack "Diffuse"
}