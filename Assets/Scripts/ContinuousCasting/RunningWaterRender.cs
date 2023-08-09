using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RunningWaterRender : ScriptableRendererFeature
{
    [System.Serializable]
    public class RunningWaterRender2DSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;

        [Range(0f, 1f), Tooltip("Outline Size")]
        public float outlineSize = 1.0f;

        [Tooltip("Inner Color")]
        public Color innerColor = Color.white;

        [Tooltip("Outline Color")]
        public Color outlineColor = Color.black;
    }

    public RunningWaterRender2DSettings settings = new RunningWaterRender2DSettings();
    class RunningWaterRender2DPass : ScriptableRenderPass
    {
        private Material material;

        public float outlineSize;
        public Color innerColor;
        public Color outlineColor;

        private bool isFirstRender = true;

        private RenderTargetIdentifier source;
        private string profilerTag;
        //Vector4[] runningWaterDataArr = new Vector4[1000];


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
            if (isFirstRender)
            {
                cmd.SetGlobalVectorArray("_RunningWaterData", new Vector4[1000]);
                context.ExecuteCommandBuffer(cmd);
                isFirstRender = false;
                return;
            }

            List<RunningWater2D> runningWaters = RunningWaterSystem2D.Get();
            List<Vector4> runningWaterData = new List<Vector4>(runningWaters.Count);

            for (int i = 0; i < runningWaters.Count; i++)
            {
                Vector2 pos = renderingData.cameraData.camera.WorldToScreenPoint(runningWaters[i].transform.position);
                float radius = runningWaters[i].GetRadius();
                runningWaterData.Add(new Vector4(pos.x, pos.y, radius, 0.0f));
            }

            if (runningWaterData.Count > 0)
            {
                //for(int i = 0; i < runningWaterData.Count; i++)
                //{
                //    runningWaterDataArr[i] = runningWaterData[i];
                //}
                cmd.SetGlobalInt("_RunningWaterCount", runningWaters.Count);
                cmd.SetGlobalVectorArray("_RunningWaterData", runningWaterData);
                cmd.SetGlobalFloat("_OutlineSize", outlineSize);
                cmd.SetGlobalColor("_InnerColor", innerColor);
                cmd.SetGlobalColor("_OutlineColor", outlineColor);
                cmd.SetGlobalFloat("_CameraSize", renderingData.cameraData.camera.orthographicSize);

                cmd.Blit(source, source, material);

                context.ExecuteCommandBuffer(cmd);
            }

            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
            material = new Material(Shader.Find("RunningWater/RunningWater2D"));
        }

        public RunningWaterRender2DPass(string profilerTag)
        {
            this.profilerTag = profilerTag;
        }
    }

    RunningWaterRender2DPass pass;

    public override void Create()
    {
        name = "RunningWater 2D";
        pass = new RunningWaterRender2DPass("RunningWater2D");

        pass.outlineSize = settings.outlineSize;
        pass.innerColor = settings.innerColor;
        pass.outlineColor = settings.outlineColor;
        pass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        pass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(pass);
    }
}
