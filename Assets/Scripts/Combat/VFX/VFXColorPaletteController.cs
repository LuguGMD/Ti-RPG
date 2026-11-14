using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG
{
    public class VFXColorPaletteController : MonoBehaviour
    {
        [SerializeField] Texture2D debugColorPalette;

        public void SetColorPalette(Texture2D colorPalette)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.material.SetTexture("_Gradient", colorPalette);
            }
        }

        [ContextMenu("Debug Set Color Palette")]
        public void DebugSetColorPalette()
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                renderer.sharedMaterial?.SetTexture("_Gradient", debugColorPalette);
            }
        }
    }
}
