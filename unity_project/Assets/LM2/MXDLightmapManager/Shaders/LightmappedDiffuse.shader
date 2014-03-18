Shader "LightmappingEffects/Diffuse" {

    Properties {

 		_Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
 		_MainTex ("Main Texture", 2D) = "white" {}
		_Adjust ("Brightness", Range (-1, 10)) = 0
    }

SubShader {

 
    Pass {


        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"

        

        sampler2D _MainTex;
		fixed4 _Color;
        float4 _MainTex_ST; //scale & position of _MainTex
		float	_Adjust;
        

        sampler2D unity_Lightmap;//Beast lightmap

        float4 unity_LightmapST; //scale & position of Beast lightmap

        

        

        // vertex input: position, UV0, UV1

        struct appdata {

            float4 vertex   : POSITION;

            float2 texcoord : TEXCOORD0;

            float2 texcoord1: TEXCOORD1; 

        };

        

        struct v2f {

            float4 pos  : SV_POSITION;

            float2 txuv : TEXCOORD0;

            float2 lmuv : TEXCOORD1;

        };

        

        v2f vert (appdata v) {

            v2f o;

            o.pos   = mul( UNITY_MATRIX_MVP, v.vertex );

            o.txuv  = TRANSFORM_TEX(v.texcoord.xy,_MainTex);

            o.lmuv  = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;

            return o;

        }

        

        half4 frag( v2f i ) : COLOR {

            half4 col   = tex2D(_MainTex, i.txuv.xy);

            half4 lm    = (tex2D(unity_Lightmap, i.lmuv.xy)*(_Adjust+1));
           // lm = lerp(lm,0,(2-_Adjust));
	
            col.rgb   = col.rgb * DecodeLightmap(lm) * _Color; 
			//col.rgb = newCol;
            
            return col;

        }

        ENDCG

        }

    }
 
 
   
    
    fallback "Diffuse"

}