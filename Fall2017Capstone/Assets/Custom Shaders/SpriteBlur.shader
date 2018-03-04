// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/SpriteBlur"
{
    Properties
    {
		_MainTex ("FisheyeMap", 2D) = "white" {}
        _Radius("RadiusMultiplier", Range(1, 255)) = 1
    }
 
    Category
    {
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque" }
     
        SubShader
        {
            GrabPass
            {
                Tags{ "LightMode" = "Always" }
            }
 
            Pass
            {
                Tags{ "LightModek" = "Always" }
 
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"
 
                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 texcoord: TEXCOORD0;
                };
 
                struct v2f
                {
                    float4 vertex : POSITION;
                    float4 uvgrab : TEXCOORD0;
					float2 texcoord: TEXCOORD1;
                };
 
                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    #if UNITY_UV_STARTS_AT_TOP
                    float scale = -1.0;
                    #else
                    float scale = 1.0;
                    #endif
                    o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
                    o.uvgrab.zw = o.vertex.zw;
					o.texcoord = v.texcoord;
                    return o;
                }
				
				sampler2D _MainTex;
                sampler2D _GrabTexture;
                float4 _GrabTexture_TexelSize;
                float _Radius;
 
                half4 frag(v2f i) : COLOR
                {
                    half4 sum = half4(0,0,0,0);
 
                    #define GRABXYPIXEL(kernelx, kernely) tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x + _GrabTexture_TexelSize.x * kernelx, i.uvgrab.y + _GrabTexture_TexelSize.y * kernely, i.uvgrab.z, i.uvgrab.w)))
                    sum += GRABXYPIXEL(0.0, 0.0);
					half4 mapColor = tex2D(_MainTex, i.texcoord);
					
                    int measurments = 1;
                    for (float range = 0.1f; range <= _Radius; range += 0.1f)
                    { 
						if(range <= pow(_Radius, mapColor.a))
						{
							sum += GRABXYPIXEL(range, range);
							sum += GRABXYPIXEL(range, -range);
							sum += GRABXYPIXEL(-range, range);
							sum += GRABXYPIXEL(-range, -range);
							measurments += 4;
						}
                    }
                    return sum / measurments;
                }
                ENDCG
            }
            GrabPass
            {
                Tags{ "LightMode" = "Always" }
            }
 
            Pass
            {
                Tags{ "LightMode" = "Always" }
 
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"
 
                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 texcoord: TEXCOORD0;
                };
 
                struct v2f
                {
                    float4 vertex : POSITION;
                    float4 uvgrab : TEXCOORD0;
					float2 texcoord: TEXCOORD1;
                };
 
                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    #if UNITY_UV_STARTS_AT_TOP
                    float scale = -1.0;
                    #else
                    float scale = 1.0;
                    #endif
                    o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
                    o.uvgrab.zw = o.vertex.zw;
					o.texcoord = v.texcoord;
                    return o;
                }
				
				sampler2D _MainTex;
                sampler2D _GrabTexture;
                float4 _GrabTexture_TexelSize;
                float _Radius;
 
                half4 frag(v2f i) : COLOR
                {
 
                    half4 sum = half4(0,0,0,0);
                    float radius = 1.41421356237 * _Radius;
 
                    #define GRABXYPIXEL(kernelx, kernely) tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(float4(i.uvgrab.x + _GrabTexture_TexelSize.x * kernelx, i.uvgrab.y + _GrabTexture_TexelSize.y * kernely, i.uvgrab.z, i.uvgrab.w)))

                    sum += GRABXYPIXEL(0.0, 0.0);
					
					half4 mapColor = tex2D(_MainTex, i.texcoord);

                    int measurments = 1;
 
                    for (float range = 1.41421356237f; range <= radius * 1.41; range += 1.41421356237f)
                    {
						if(range <= pow(radius * 1.41, mapColor.a))
						{
							sum += GRABXYPIXEL(range, 0);
							sum += GRABXYPIXEL(-range, 0);
							sum += GRABXYPIXEL(0, range);
							sum += GRABXYPIXEL(0, -range);
							measurments += 4;
						}
                    }
					
                    return sum / measurments;
                }
                ENDCG
            }
        }
    }
}