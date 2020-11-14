// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/TextureCompositor"
{
    Properties
    {
        // we have removed support for texture tiling/offset,
        // so make them not be displayed in material inspector
        [NoScaleOffset] _MainTexA ("TextureA", 2D) = "white" {}
        [NoScaleOffset] _MainTexB ("TextureB", 2D) = "white" {}
        _Fader("Fader",Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Pass
        {
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag

            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };
            sampler2D _MainTexA;
            sampler2D _MainTexB;
            float4 _MainTexA_ST;
            float4 _MainTexB_ST;
            float _Fader;
            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
              
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTexA);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample texture and return it
                fixed4 colA = tex2D(_MainTexA, i.uv);
                fixed4 colB = tex2D(_MainTexB, i.uv);
                return lerp(colA,colB,1.0-_Fader);
            }
            ENDCG
        }
    }
}