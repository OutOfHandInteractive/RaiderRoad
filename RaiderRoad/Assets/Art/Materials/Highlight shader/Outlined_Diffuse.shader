Shader "Outlined/Uniform"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor("Outline color", Color) = (0,0,0,.5)
		_OutlineWidth("Outlines width", Range(0.0, 2.0)) = 1.1
		_Active("Active", Range(0.0, 1.0)) = 1.0 //1 for hologram show
		_Metallic ("Metallic Map", 2D) = "white" {}
		_Smoothness ("Smoothness", Range(0,1)) = 1
		_NormalMap ("Normal Map",2D) = "bump" {}
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
	};

	struct v2f
	{
		float4 pos : POSITION;
	};

	uniform float _OutlineWidth;
	uniform float4 _OutlineColor;
	uniform sampler2D _MainTex;
	uniform float4 _Color;
	uniform float _Active;

	ENDCG

	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" }

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
				v.vertex.z += 0.005 * normalize(v.vertex.z) * _Active;

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;

			}

			half4 frag(v2f i) : COLOR
			{
				return _OutlineColor;
			}

			ENDCG
		}

		Tags{ "Queue" = "Geometry"}

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		 
		sampler2D _Metallic;
		sampler2D _NormalMap;
		float _Smoothness;

		struct Input {
			float2 uv_MainTex;
		};
		 
		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;

			o.Metallic = tex2D(_Metallic, IN.uv_MainTex).r;
			o.Smoothness = _Smoothness * tex2D(_Metallic, IN.uv_MainTex).a;
			o.Normal = tex2D(_NormalMap, IN.uv_MainTex).rgba;
		}
		ENDCG
	}
	Fallback "Diffuse"
}