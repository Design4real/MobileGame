Shader "BakedVertexLighting/Diffuse" {

 

    Properties {

        _MainTex ("Texture (RGB)", 2D) = "white" {}

    }

     

    SubShader {

        Tags { "RenderType" = "Opaque" }

        LOD 200

        

        CGPROGRAM

        #pragma surface surf Lambert_BakedVertexLighting nolightmap noambient fullforwardshadows

        

        // Surface Shader

        sampler2D _MainTex;

        

        struct SurfaceOutput2 {

            fixed3 Albedo;

            fixed3 Normal;

            fixed3 Emission;

            half Specular;

            fixed Gloss;

            fixed Alpha;

            fixed3 VertexColor;

        };

        

        struct Input {

            float2 uv_MainTex; // uv coords 1

            float4 color : COLOR; // vertex color

        };

        

        void surf (Input IN, inout SurfaceOutput2 o) {

            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;

            o.VertexColor = IN.color.rgb;

        }

 

        // Custom Lighting Models //

        

        // Forward lighting mode

        inline fixed4 LightingLambert_BakedVertexLighting (SurfaceOutput2 s, fixed3 lightDir, fixed atten) {

            fixed diff = max (0, dot (s.Normal, lightDir));

    

            fixed4 c;

            c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2) + (s.VertexColor * s.Albedo);

            c.a = s.Alpha;

            return c;

        }

        

        // Deferred lighting mode

        inline fixed4 LightingLambert_BakedVertexLighting_PrePass (SurfaceOutput2 s, half4 light) {

            fixed4 c;

            c.rgb = (s.Albedo * light.rgb) + (s.VertexColor * s.Albedo);

            c.a = s.Alpha;

            return c;

        }

        

        ENDCG

    }

    Fallback "Diffuse"

}