using UnityEngine;

namespace RPG
{
    public class PauseSettings : MonoBehaviour
    {
        public enum Language { Portuguese, English, Spanish }
        public enum ScreenMode { Windowed, FullScreen, Borderless }
        public enum GraphicsQuality { Low, Medium, High }
        public enum VSyncMode { Enabled, Disabled }

        private Language currentLanguage = Language.Portuguese;
        private float mouseSensitivity = 50f;
        private ScreenMode currentScreenMode = ScreenMode.Windowed;
        private GraphicsQuality currentGraphicsQuality = GraphicsQuality.Medium;
        private VSyncMode currentVSyncMode = VSyncMode.Disabled;

        #region IDIOMA
        public void SetLanguage(Language language)
        {
            currentLanguage = language;
            Debug.Log($"Idioma alterado para: {language}");
        }

        public Language GetCurrentLanguage()
        {
            return currentLanguage;
        }

        public void NextLanguage()
        {
            currentLanguage = (Language)(((int)currentLanguage + 1) % 3);
            SetLanguage(currentLanguage);
        }

        public void PreviousLanguage()
        {
            currentLanguage = (Language)(((int)currentLanguage - 1 + 3) % 3);
            SetLanguage(currentLanguage);
        }

        #endregion

        #region SENSIBILIDADE DO MOUSE

        public void SetMouseSensitivity(float sensitivity)
        {
            mouseSensitivity = Mathf.Clamp(sensitivity, 0f, 100f);
            Debug.Log($"Sensibilidade do mouse alterada para: {mouseSensitivity}");
        }

        public float GetMouseSensitivity()
        {
            return mouseSensitivity;
        }

        #endregion

        #region MODO DE TELA

        public void SetScreenMode(ScreenMode mode)
        {
            currentScreenMode = mode;
            ApplyScreenMode(mode);
            Debug.Log($"Modo de tela alterado para: {mode}");
        }

        public ScreenMode GetCurrentScreenMode()
        {
            return currentScreenMode;
        }

        public void NextScreenMode()
        {
            currentScreenMode = (ScreenMode)(((int)currentScreenMode + 1) % 3);
            SetScreenMode(currentScreenMode);
        }

        public void PreviousScreenMode()
        {
            currentScreenMode = (ScreenMode)(((int)currentScreenMode - 1 + 3) % 3);
            SetScreenMode(currentScreenMode);
        }

        private void ApplyScreenMode(ScreenMode mode)
        {
            switch (mode)
            {
                case ScreenMode.Windowed:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case ScreenMode.FullScreen:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case ScreenMode.Borderless:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
            }
        }

        #endregion

        #region GRÁFICOS
   
        public void SetGraphicsQuality(GraphicsQuality quality)
        {
            currentGraphicsQuality = quality;
            ApplyGraphicsQuality(quality);
            Debug.Log($"Qualidade gráfica alterada para: {quality}");
        }

        public GraphicsQuality GetCurrentGraphicsQuality()
        {
            return currentGraphicsQuality;
        }

        public void NextGraphicsQuality()
        {
            currentGraphicsQuality = (GraphicsQuality)(((int)currentGraphicsQuality + 1) % 3);
            SetGraphicsQuality(currentGraphicsQuality);
        }

        public void PreviousGraphicsQuality()
        {
            currentGraphicsQuality = (GraphicsQuality)(((int)currentGraphicsQuality - 1 + 3) % 3);
            SetGraphicsQuality(currentGraphicsQuality);
        }

        private void ApplyGraphicsQuality(GraphicsQuality quality)
        {
            switch (quality)
            {
                case GraphicsQuality.Low:
                    QualitySettings.SetQualityLevel(0);
                    break;
                case GraphicsQuality.Medium:
                    QualitySettings.SetQualityLevel(1);
                    break;
                case GraphicsQuality.High:
                    QualitySettings.SetQualityLevel(2);
                    break;
            }
        }

        #endregion

        #region VSYNC

        public void SetVSyncMode(VSyncMode mode)
        {
            currentVSyncMode = mode;
            QualitySettings.vSyncCount = mode == VSyncMode.Enabled ? 1 : 0;
            Debug.Log($"V-Sync: {(mode == VSyncMode.Enabled ? "Ligado" : "Desligado")}");
        }

        public VSyncMode GetCurrentVSyncMode()
        {
            return currentVSyncMode;
        }

        public void ToggleVSync()
        {
            currentVSyncMode = currentVSyncMode == VSyncMode.Enabled ? VSyncMode.Disabled : VSyncMode.Enabled;
            SetVSyncMode(currentVSyncMode);
        }

        #endregion
    }
}
