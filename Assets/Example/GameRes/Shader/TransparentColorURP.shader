Shader "Custom/TransparentColorURP" {
    // 属性块定义了可以在材质编辑器中调整的参数
	Properties {
		_Color("Color Tint", Color) = (1,1,1,1)
		_MainTex("Main Tex", 2D) = "white" {}
		_AlphaScale("Alpha Scale", Range(0, 1)) = 0.5 // 透明度缩放，控制材质的透明度程度
	}
 
	SubShader {
		Tags {
			"RenderPipeline"="UniversalPipeline"
			"Queue"="Transparent" "IngoreProjector"="True" "RenderType"="Transparent"
		}
 
		Pass {
			ZWrite On // 开启深度写入
			ColorMask 0 // 关闭颜色通道写入
		}
 
 
		Pass {
 
			Tags {"LightMode"="UniversalForward"}
 
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha // 设置混合模式，基于源Alpha值的透明度混合
			
			HLSLPROGRAM
 
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
 
			#pragma vertex vert
			#pragma fragment frag
 
CBUFFER_START(UnityPerMaterial)
			half4 _Color;
			float4 _MainTex_ST;
			float _AlphaScale;
CBUFFER_END
 
			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);
 
			struct a2v {
				float4 vertex : POSITION;
				half3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};
 
			struct v2f {
				float4 pos : SV_POSITION;
				half3 worldNormal : TEXCOORD0;
				half3 worldPos : TEXCOORD1;
				float2 uv : TEXCOORD2;
			};
 
			v2f vert(a2v i) {
				v2f o;
				o.pos = TransformObjectToHClip(i.vertex.xyz);
				o.worldPos = mul(UNITY_MATRIX_M, i.vertex).xyz;
				o.worldNormal = TransformObjectToWorldNormal(i.normal);
				o.uv = i.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
 
				return o;
			}
 
			half4 frag(v2f i) : SV_Target {
				half3 worldNormal = normalize(i.worldNormal);
				half3 worldLightDir = normalize(_MainLightPosition.xyz);
 
				half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
				half3 albedo = texColor.rgb * _Color.rgb;
				half3 ambient = half3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w) * albedo;
				half3 diffuse = _MainLightColor.rgb * albedo * saturate(dot(worldLightDir, worldNormal));
				return half4(ambient + diffuse, texColor.a * _AlphaScale);
			}
 
			ENDHLSL
		}
	}
	FallBack "Univeral Render Pipeline/Simple Lit"
}
