Shader "Custom/DepthShader"
{
    Properties
    {
        _DepthScale ("Depth Scale", Float) = 100
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float depth : TEXCOORD0;
            };

            float _DepthScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.depth = -UnityObjectToViewPos(v.vertex).z;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Normalize depth to grayscale value
                float normalizedDepth = saturate(i.depth / _DepthScale);
                
                // Output depth as grayscale color
                return fixed4(normalizedDepth, normalizedDepth, normalizedDepth, 1);
            }
            CGPROGRAM
        }
    }
}