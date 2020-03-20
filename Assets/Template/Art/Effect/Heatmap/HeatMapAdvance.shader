
Shader "Linda/HeatMap_Advance"
{
    Properties
    {
        _MainTex	("MainTex",		2D)					=	"white"{}
        _HeatMapTex	("HeatMapTex",	2D)					=	"white"{}
        _Alpha		("Alpha",		range( 0,	1 ))	=	0.8
        _AlphaStep	("AlphaStep",	range( 1,	10))	=	1
		_Height		("Height",		range(-10,	10))	=	0
		[Enum(Off, 0, On, 1)]_TexVisible	("TexVisible",	Int)	=	0
    }

    SubShader
    {
        Tags{"Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma vertex		vert
            #pragma fragment	frag
            #include "unitycg.cginc"

			sampler2D		_MainTex;
			//_MainTex_ST		纹理名_ST 的方式来声明某个纹理的属性
			//_MainTex_ST.xy存储的是缩放值，而_MainTex_ST.zw存储的是偏移值。
			float4			_MainTex_ST;
			int				_TexVisible;
            sampler2D		_HeatMapTex;
            half			_Alpha;
			half			_AlphaStep;
			float			_Height;
            uniform int		_FactorCount;
            uniform float4	_Factors[100];
            uniform float2	_FactorsProperties;

            struct a2v
			{
				float4 pos		: POSITION;
				float2 uv		: TEXCOORD0;
			};

			struct v2f
			{
				float4 pos		: POSITION;
				fixed3 worldPos : TEXCOORD1;
				float2 uv		: TEXCOORD2;
			};

			v2f vert(a2v input)
			{
				v2f		o;
                half3	worldPos	= mul(unity_ObjectToWorld,input.pos).xyz;
                half	heat		= 0;
				for( int i = 0 ; i < _FactorCount;i++ )
				{
					half	dis			=	distance(worldPos, _Factors[i].xyz);
					float	radius		=	_FactorsProperties.x;
					float	intensity	=	_FactorsProperties.y;
					half	ratio		=	1 - saturate(dis / radius);
							heat		+=	intensity * ratio;
				}	
				o.pos		=	UnityObjectToClipPos(input.pos + half3(0,heat*_Height,0));
				o.worldPos	=	mul(unity_ObjectToWorld,input.pos).xyz;
				o.uv		=	input.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				return	o;
			}

			fixed4 frag(v2f input):COLOR
			{
				half heat = 0;
				for( int i = 0 ; i < _FactorCount;i++ )
				{
					half	dis			=	distance(input.worldPos, _Factors[i].xyz);
					float	radius		=	_FactorsProperties.x;
					float	intensity	=	_FactorsProperties.y;
					half	ratio		=	1 - saturate(dis / radius);
							heat		+=	intensity * ratio;
							heat		=	clamp(heat, 0.05, 0.95);
				}
				half4	color	=	tex2D(_HeatMapTex, fixed2(heat, 0.5));
				half	aplha	=	(heat - 0.05) / 0.9;
						color.a =	pow( aplha * _Alpha, _AlphaStep);
				half4	tex		=	tex2D(_MainTex, input.uv);
				if (_TexVisible == 0)
				{
					return	color;
				}
				else
				{
					return	fixed4(color.rgb * color.a + tex.rgb * (1 - color.a), tex.a);
				}
				
			}

            ENDCG
        }
    }

    Fallback "Diffuse"
}