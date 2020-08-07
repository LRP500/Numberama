Shader "Unlit/ColorPalette"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PrimaryColor("Primary Color", Color) = (1, 1, 1, 1)
        _SecondaryColor("Secondary Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags
		{
			"RenderType" = "Opaque"
		}

        Pass
        {
            CGPROGRAM
        
			#pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _PrimaryColor;
            fixed4 _SecondaryColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float blend = i.uv.x > i.uv.y ? 0 : 1; 
                return lerp(_PrimaryColor, _SecondaryColor, blend);
            }
            
			ENDCG
        }
    }
}
