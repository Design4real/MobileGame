Shader "OceanShader/Ocean" {
	Properties {
		_WaterTex ("Normal Map (RGB), Foam (A)", 2D) = "white" {}
		_WaterTex2 ("Normal Map (RGB), Foam (B)", 2D) = "white" {}
		_ShoreLineTex ("ShoreLine Foam", 2D) = "white" {}

		_Tiling ("Wave Scale", Range(0.00025, 0.01)) = 0.25
		_WaveSpeed("Wave Speed", Float) = 0.4
		_ShoreLineIntensity("ShoreLine Intensity", Float) = 2
		_ReflectionIntensity("Reflection Intensity", Range(0, 1)) = 0.1
		
		_SpecularRatio ("Specular Ratio", Range(10,500)) = 200

		_BottomColor("Bottom Color",Color) = (0,0,0,0)
		_TopColor("Top Color",Color) = (0,0,0,0)
		
		
//		_ReflectionTex ("Reflection", 2D) = "white" { TexGen ObjectLinear }
	}
	SubShader {
		LOD 200	
		Tags {
			"Queue"="Geometry"
			 "RenderType"="Opaque" 
			"IgnoreProjector" = "True"
		}
		Lighting On
		Pass{
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#include "UnityCG.cginc"
				
			uniform float _Tiling;
			uniform float _WaveSpeed;
			uniform float _SpecularRatio;
			uniform float _ShoreLineIntensity;
			uniform float _ReflectionIntensity;
	
			uniform sampler2D _WaterTex;
			uniform sampler2D _WaterTex2;
			sampler2D _ReflectionTex;
			uniform sampler2D _ShoreLineTex;
		
			float4 _ShoreLineTex_ST;
			
			
			float4x4 _ProjMatrix;
			uniform float4 _LightColor0; 
			
			float4 _BottomColor;
			float4 _TopColor;
			
			struct VS_OUT
			{
			
				float4 position  : POSITION;
				float3 worldPos  : TEXCOORD0;	
				float4 texCoordProj :TEXCOORD1;
				float3 tilingAndOffset:TEXCOORD2;
				float2 uv:TEXCOORD3;
				float4 color : COLOR;
			};
			
	
		  
			VS_OUT Vert(appdata_full v)
			{
				VS_OUT o;
				o.worldPos = mul(_Object2World, v.vertex);
				o.position = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tilingAndOffset.z =frac( _Time.x * _WaveSpeed);
				o.texCoordProj = mul(_ProjMatrix,  v.vertex);
				o.tilingAndOffset.xy = o.worldPos.xz*_Tiling;
				o.color = v.color;
				o.uv =TRANSFORM_TEX(v.texcoord.xy,_ShoreLineTex);
				return o;
			}
				
	
			float4 Frag(VS_OUT IN):COLOR
			{
				fixed3 lightColor=_LightColor0.xyz*2;
				
				float3 worldView = -normalize(IN.worldPos - _WorldSpaceCameraPos);

				half2 tiling = IN.tilingAndOffset.xy;

				half4 nmap1 = tex2D(_WaterTex, tiling.yx +float2(IN.tilingAndOffset.z,0));
				
				half4 nmap2 = tex2D(_WaterTex2, tiling.yx -float2(IN.tilingAndOffset.z,0));
				
				float3 worldNormal  = normalize((nmap1.xyz+nmap2.xyz)*2-2);
				
				fixed dotLightWorldNomal = dot(worldNormal, float3(0,1,0));
				
				float3 light = _WorldSpaceLightPos0.xyz;
//				float3 reflLight =  reflect(-light, worldNormal);
				float3 specularReflection = float3(0,0,0) ;
				
				 if (dotLightWorldNomal < 0.0) {
	               // light source on the wrong side?
	               specularReflection = float3(0.0, 0.0, 0.0); 
	            }
	            else{
	            	
	            
//	            	fixed dotSpecular = dot(reflLight,  float3( worldView.x,-worldView.y,worldView.z));
					fixed dotSpecular = dot(worldNormal,  normalize( worldView+light));
					specularReflection = pow(max(0.0, dotSpecular), _SpecularRatio);
	            }
				
			
				float4 col;
				float fresnel = 0.5*dotLightWorldNomal+0.5;
					
				col.rgb  = lerp(_BottomColor.xyz, _TopColor.xyz, fresnel);
				
				col.a = 1;

				float4 newTecCoord = IN.texCoordProj;
				newTecCoord.xz+=(worldNormal.xz)*2;
				
				half4 reflectCol = tex2Dproj(_ReflectionTex, newTecCoord);
				col.rgb = col.rgb*(1-_ReflectionIntensity)+reflectCol.rgb*_ReflectionIntensity;
				
				
				col.rgb+=specularReflection;

				half4 shoreLineCol = tex2D(_ShoreLineTex, IN.uv+(worldNormal.xz)*0.05+float2(IN.tilingAndOffset.z,0));
				
				
				col.rgb += _ShoreLineIntensity*(1-IN.color.r)*shoreLineCol.rgb;	
				
				col.rgb*=lightColor;
				
				return col;

			}
		ENDCG	
		}  
		
		
		
	}
	
	SubShader {
		LOD 100	
		Tags {
			"Queue"="Geometry"
			 "RenderType"="Opaque" 
			"IgnoreProjector" = "True"
		}
		Lighting On
		Pass{
			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex Vert
			#pragma fragment Frag
			#include "UnityCG.cginc"
				
			uniform float _Tiling;
			uniform float _WaveSpeed;
			uniform float _SpecularRatio;
			uniform float _ShoreLineIntensity;
			uniform float _ReflectionIntensity;
	
			uniform sampler2D _WaterTex;
			uniform sampler2D _WaterTex2;
			sampler2D _ReflectionTex;
			uniform sampler2D _ShoreLineTex;
		
			float4 _ShoreLineTex_ST;
			
			
			float4x4 _ProjMatrix;
			uniform float4 _LightColor0; 
			
			float4 _BottomColor;
			float4 _TopColor;
			
			struct VS_OUT
			{
			
				float4 position  : POSITION;
				float3 worldPos  : TEXCOORD0;	
				float4 texCoordProj :TEXCOORD1;
				float3 tilingAndOffset:TEXCOORD2;
				float2 uv:TEXCOORD3;
				float4 color : COLOR;
			};
			
	
		  
			VS_OUT Vert(appdata_full v)
			{
				VS_OUT o;
				o.worldPos = mul(_Object2World, v.vertex);
				o.position = mul(UNITY_MATRIX_MVP, v.vertex);
				o.tilingAndOffset.z =frac( _Time.x * _WaveSpeed);
				o.texCoordProj = mul(_ProjMatrix,  v.vertex);
				o.tilingAndOffset.xy = o.worldPos.xz*_Tiling;
				o.color = v.color;
				o.uv =TRANSFORM_TEX(v.texcoord.xy,_ShoreLineTex);
				return o;
			}
				
	
			float4 Frag(VS_OUT IN):COLOR
			{
				fixed3 lightColor=_LightColor0.xyz*2;
				
				float3 worldView = -normalize(IN.worldPos - _WorldSpaceCameraPos);

				half2 tiling = IN.tilingAndOffset.xy;

				half4 nmap1 = tex2D(_WaterTex, tiling.yx +float2(IN.tilingAndOffset.z,0));
				
				half4 nmap2 = tex2D(_WaterTex2, tiling.yx -float2(IN.tilingAndOffset.z,0));
				
				float3 worldNormal  = normalize((nmap1.xyz+nmap2.xyz)*2-2);
				
				fixed dotLightWorldNomal = dot(worldNormal, float3(0,1,0));
				
				float3 light = _WorldSpaceLightPos0.xyz;
//				float3 reflLight =  reflect(-light, worldNormal);
				float3 specularReflection = float3(0,0,0) ;
				
				 if (dotLightWorldNomal < 0.0) {
	               // light source on the wrong side?
	               specularReflection = float3(0.0, 0.0, 0.0); 
	            }
	            else{
	            	
	            
//	            	fixed dotSpecular = dot(reflLight,  float3( worldView.x,-worldView.y,worldView.z));
					fixed dotSpecular = dot(worldNormal,  normalize( worldView+light));
					specularReflection = pow(max(0.0, dotSpecular), _SpecularRatio);
	            }
				
			
				float4 col;
				float fresnel = 0.5*dotLightWorldNomal+0.5;
					
				col.rgb  = lerp(_BottomColor.xyz, _TopColor.xyz, fresnel);
				
				col.a = 1;

				float4 newTecCoord = IN.texCoordProj;
				newTecCoord.xz+=(worldNormal.xz)*2;
				
				col.rgb+=specularReflection;

				half4 shoreLineCol = tex2D(_ShoreLineTex, IN.uv+(worldNormal.xz)*0.05+float2(IN.tilingAndOffset.z,0));
				
				
				col.rgb += _ShoreLineIntensity*(1-IN.color.r)*shoreLineCol.rgb;	
				
				col.rgb*=lightColor;
				
				return col;

			}
		ENDCG	
		}  
		
		
		
	}
	
	
	FallBack "Diffuse"
}
