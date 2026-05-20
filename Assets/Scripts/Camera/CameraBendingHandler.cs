using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace RPG.Camera
{
    [ExecuteAlways]
    public class CameraBendingHandler : MonoBehaviour
    {
        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
        }


        private void OnBeginCameraRendering(ScriptableRenderContext context, UnityEngine.Camera camera)
        {
            camera.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, 0.001f, 99) * camera.worldToCameraMatrix;
        }
        private void OnEndCameraRendering(ScriptableRenderContext context, UnityEngine.Camera camera)
        {
            camera.ResetCullingMatrix();    
        }
    }
}
