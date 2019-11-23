Shader "Custom/TilingRoadShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Metallic("Metallic Map", 2D) = "white" {}
		_Smoothness("Smoothness", Range(0,1)) = 1
		_NormalMap("Normal Map",2D) = "bump" {}

		[Header(Overlay Elements)]
		_NotRoadMask("Not Road Mask (B&W)", 2D) = "white" {}
		_OverTex("Overlay Texture 1 (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		sampler2D _OverTex;

        struct Input
        {
            float2 uv_MainTex;
			float2 uv_OverTex;
        };

		sampler2D _Metallic;
		sampler2D _NormalMap;
		float _Smoothness;
        fixed4 _Color;
		sampler2D _NotRoadMask;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

       void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			//lerp between overlays based on alpha
			c = lerp(c.rgba, tex2D(_OverTex, IN.uv_OverTex).rgba, clamp(tex2D(_OverTex, IN.uv_OverTex).a - tex2D(_NotRoadMask, IN.uv_MainTex).r, 0.0, 1.0));
			o.Albedo = c.rgb;
			o.Alpha = c.a;

			o.Metallic = tex2D(_Metallic, IN.uv_MainTex).r;
			o.Smoothness = _Smoothness * tex2D(_Metallic, IN.uv_MainTex).a;
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
		}
        ENDCG
    }
    FallBack "Diffuse"
}
