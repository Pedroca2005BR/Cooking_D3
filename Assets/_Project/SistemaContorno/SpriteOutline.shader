Shader "Custom/SpriteOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1, 1, 0, 1) // Amarelo por padrão
        _Thickness ("Outline Thickness", Range(0, 10)) = 0 // Começa em zero (desligado)
    }
    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
            };

            fixed4 _Color;
            fixed4 _OutlineColor;
            float _Thickness;
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.texcoord);
                
                // Se o pixel for transparente e a espessura for maior que zero, checa os vizinhos
                if (c.a < 0.1 && _Thickness > 0)
                {
                    float t = _Thickness;
                    // Procura pixels em volta
                    float2 up = float2(0, _MainTex_TexelSize.y) * t;
                    float2 down = float2(0, -_MainTex_TexelSize.y) * t;
                    float2 right = float2(_MainTex_TexelSize.x, 0) * t;
                    float2 left = float2(-_MainTex_TexelSize.x, 0) * t;

                    float alpha = tex2D(_MainTex, IN.texcoord + up).a +
                                  tex2D(_MainTex, IN.texcoord + down).a +
                                  tex2D(_MainTex, IN.texcoord + right).a +
                                  tex2D(_MainTex, IN.texcoord + left).a;

                    // Se encontrou um pedaço do sprite por perto, desenha a borda
                    if (alpha > 0)
                    {
                        return fixed4(_OutlineColor.rgb, 1.0) * IN.color; 
                    }
                }

                // Multiplica a cor original do sprite
                c.rgb *= c.a;
                return c * IN.color;
            }
            ENDCG
        }
    }
}