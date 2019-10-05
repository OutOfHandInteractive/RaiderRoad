// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Outlined/Chara"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor("Outline color", Color) = (0,0,0,.5)
		_OutlineWidth("Outlines width", Range(0.0, 2.0)) = 1.1
		_Active("Active", Range(0.0, 1.0)) = 1.0 //1 for hologram show
	}

	/*SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" }

		//OUTLINE CURRENTLY BORKED
		Pass //Outline
		{
			ZWrite Off
			//ZTest Always
			Cull Back
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			v2f vert(appdata v)
			{
				appdata original = v;
				v.vertex.xyz += _OutlineWidth * normalize(v.vertex.xyz) * _Active;

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;

			}

			half4 frag(v2f i) : COLOR
			{
				return _OutlineColor;
			}

			ENDCG
		}*/

	SubShader{
		Tags {"RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True"}
		LOD 200

		Pass {
			ZWrite On
			ColorMask 0

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			float4 frag(v2f i) : COLOR
			{
				return float4 (0,0,0,0);
			}
			ENDCG
		}

		CGPROGRAM
		#pragma surface surf Lambert alpha
		#pragma debug

		uniform float _OutlineWidth;
		uniform float4 _OutlineColor;
		uniform float4 _Color;
		uniform float _Active;

		sampler2D _MainTex;
		//fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = lerp(_Color, c.rgb, c.a);
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}