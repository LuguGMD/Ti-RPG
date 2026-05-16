using Lugu.UI;
using RPG.Input;
using UnityEngine;

namespace RPG.Management.Minigames
{
    public class MinigameUIController : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private PanelsController _panelsController;

        private void Awake()
        {
            _playerInput = GameObject.FindAnyObjectByType<PlayerInput>();
            _panelsController = GetComponent<PanelsController>();
        }

        private void Start()
        {
            _playerInput.Actions.Pause.OnStart(TogglePause);
        }

        private void TogglePause()
        {
            if(MinigameManager.IsPaused)
            {
                UnPauseMinigame();
            }
            else
            {
                PauseMinigame();
            }
        }

        private void PauseMinigame()
        {
            MinigameManager.Instance.TogglePause(true);
            _panelsController.ChangePanel(1);
        }

        private void UnPauseMinigame()
        {
            MinigameManager.Instance.TogglePause(false);
            _panelsController.ChangePanel(0);
        }
    }
}
