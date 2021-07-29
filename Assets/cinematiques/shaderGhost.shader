Shader "Unlit/shaderGhost"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_distortionAmount("Distortion", Range(0.0,1.0)) = 0.05 
		_speed("Vitesse", Range(0.0,5.0)) = 2.0
    }
    SubShader
    {
        Tags { 
			"Queue" = "Transparent"
			"RenderType"="Transparent" }
        LOD 100
		Cull off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
     
            #include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float1 _distortionAmount;
			float1 _speed;
			//fixed4 _Color;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				float4 color : COLOR;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				i.uv.x = i.uv.x + sin(i.uv.y * 20 + _Time.y*_speed)*_distortionAmount*(1-i.uv.y);
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return col;
            }
            ENDCG
        }
    }
}
