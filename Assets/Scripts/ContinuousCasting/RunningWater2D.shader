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
                    // distFromRunningWater��ʾƬԪ�����ĵľ���
                    float distFromRunningWater = distance(runningWaterPos, i.uv * _ScreenParams.xy);
                    // ����_CameraSizeû�г���2��radiusSizeʵ���ǰ뾶*2�ĳ���
                    float radiusSize = _RunningWaterData[m].z * _ScreenParams.y / _CameraSize;
                    // distFromMetaball / radiusSize��ʾƬԪ�����ĵľ�����԰뾶�������������ж�ƬԪ�����λ�á�������saturate���Ա�֤distʼ����ȡ����ƬԪ�����������е���Сֵ��
                    dist *= saturate(distFromRunningWater / radiusSize);
                }
                float threshold = 0.5f;
                float outlineThreshold = threshold * (1.0f - _OutlineSize);
                // dist > threshold����0.5f����ʾƬԪ����һ����뾶��˵�������⣻dist > outlineThreshold��ʾС��һ����İ뾶���������ڲ���Ե������������λ�á�
                float4 _OutlineColorLerp = lerp(_InnerColor, _OutlineColor, (dist - outlineThreshold)/ (threshold-outlineThreshold));
                return (dist > threshold) ? tex :
                    ((dist > outlineThreshold) ? _OutlineColorLerp : _InnerColor);
            }

            ENDCG
        }
    }
}