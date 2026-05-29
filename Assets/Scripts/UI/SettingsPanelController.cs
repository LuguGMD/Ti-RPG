using RPG.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG
{
    public class SettingsPanelController : MonoBehaviour
    {
        [Tooltip("O painel de configurações")]
        [SerializeField] private GameObject settingsPanel;

        [Tooltip("Botão de voltar — fecha o painel")]
        [SerializeField] private Button backButton;

        [Tooltip("Botão de voltar ao menu principal — troca de cena")]
        [SerializeField] private Button mainMenuButton;

        private const string MainMenuSceneName = "MainMenuScene";
        private PlayerInput _playerInput;

        private void Awake()
        {
            backButton.onClick.AddListener(ClosePanel);
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            _playerInput = GameObject.FindAnyObjectByType<PlayerInput>();
        }

        private void Start()
        {
            _playerInput.Actions.Pause.OnStart(TogglePausePanel);
        }

        private void OnDestroy()
        {
            backButton.onClick.RemoveListener(ClosePanel);
            mainMenuButton.onClick.RemoveListener(GoToMainMenu);

            if (_playerInput != null)
                _playerInput.Actions.Pause.Remove.OnStart(TogglePausePanel);
        }

        private void TogglePausePanel()
        {
            if (settingsPanel.activeSelf)
                ClosePanel();
            else
                OpenPanel();
        }

        private void OpenPanel()
        {
            settingsPanel.SetActive(true);
            Time.timeScale = 0f;
            ActionsManager.Instance?.OnPauseToggle?.Invoke(true);
        }

        private void ClosePanel()
        {
            settingsPanel.SetActive(false);
            Time.timeScale = 1f;
            ActionsManager.Instance?.OnPauseToggle?.Invoke(false);
        }

        private void GoToMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(MainMenuSceneName);
        }
    }
}