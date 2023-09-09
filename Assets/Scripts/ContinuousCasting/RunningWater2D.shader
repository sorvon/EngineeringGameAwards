Shader "RunningWater/RunningWater2D"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
        SubShader
    {
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}

        Pass{
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;

            int _RunningWaterCount;
            float3 _RunningWaterData[1000];
            float _OutlineSize;
            float4 _InnerColor;
            float4 _OutlineColor;
            float _CameraSize;

            float4 frag(v2f_img i) : SV_Target
            {
                float4 tex = tex2D(_MainTex, i.uv); 
                //return tex;

                float dist = 1.0f;

                for (int m = 0; m < _RunningWaterCount; ++m)
                {
                    float2 runningWaterPos = _RunningWaterData[m].xy;
                    // distFromRunningWater表示片元与球心的距离
                    float distFromRunningWater = distance(runningWaterPos, i.uv * _ScreenParams.xy);
                    // 由于_CameraSize没有乘以2，radiusSize实际是半径*2的长度
                    float radiusSize = _RunningWaterData[m].z * _ScreenParams.y / _CameraSize;
                    // distFromMetaball / radiusSize表示片元到球心的距离除以半径的两倍，用以判断片元与球的位置。这里用saturate可以保证dist始终能取到该片元到所有球心中的最小值。
                    dist *= saturate(distFromRunningWater / radiusSize);
                }
                float threshold = 0.5f;
                float outlineThreshold = threshold * (1.0f - _OutlineSize);
                // dist > threshold（即0.5f）表示片元大于一个球半径，说明在球外；dist > outlineThreshold表示小于一个球的半径，大于球内部边缘，处于轮廓线位置。
                float4 _OutlineColorLerp = lerp(_InnerColor, _OutlineColor, (dist - outlineThreshold)/ (threshold-outlineThreshold));
                return (dist > threshold) ? tex :
                    ((dist > outlineThreshold) ? _OutlineColorLerp : _InnerColor);
            }

            ENDCG
        }
    }
}