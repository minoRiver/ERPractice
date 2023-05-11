Shader "Unlit/NewUnlitShader"
{
	Properties
	{
		_Albedo("Albedo", Color) = (1,1,1,1)
		_Shades("Shades", Range(1, 20)) = 3
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
		// make fog work
		#pragma multi_compile_fog

		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
		};

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex + v.normal * 0.01f);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			return fixed4(1.0, 0.0, 0.0, 1.0);
		}
		ENDCG
			}


	}
}
