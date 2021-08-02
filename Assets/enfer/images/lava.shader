Shader "Unlit/lava"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_sizeDeformationX("size deformation x", Float) = 1.0
		_sizeDeformationY("size deformation y", Float) = 1.0
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float _sizeDeformationX;
			float _sizeDeformationY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				float2 deformationUv = float2(i.uv.x + cos(i.uv.x * 3 + _Time.y)*_sizeDeformationX, i.uv.y + sin(i.uv.y * 6 + _Time.y) * _sizeDeformationY);
                fixed4 col = tex2D(_MainTex, deformationUv);
                return col;
            }
            ENDCG
        }
    }
}
