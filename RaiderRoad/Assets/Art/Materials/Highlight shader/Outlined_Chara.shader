﻿Shader "Outlined/Chara"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor("Outline color", Color) = (0,0,0,.5)
		_OutlineWidth("Outlines width", Range(0.0, 2.0)) = 1.1
		_Active("Active", Range(0.0, 1.0)) = 1.0 //1 for hologram show
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

		//OUTLINE CURRENTLY BORKED
		/*Pass //Outline
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

		Tags{ "Queue" = "Geometry"}

		CGPROGRAM
		#pragma surface surf Lambert
		 
		struct Input {
			float2 uv_MainTex;
		};
		 
		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = lerp(_Color, c.rgb, c.a);
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}