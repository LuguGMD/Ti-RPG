using Lugu.UI;
using RPG.Input;
using RPG.Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Management.Minigames
{
    public class MinigameUIController : MonoBehaviour
    {
        private PanelsController _panelsController;

        [SerializeField] private TextMeshProUGUI _currentComboText;
        [SerializeField] private TextMeshProUGUI _currentPerfectComboText;

        [SerializeField] private TextMeshProUGUI _challengeNameText;
        [SerializeField] private Slider _challengeProgressBar;
        [SerializeField] private TextMeshProUGUI _challengeProgressText;

        private void Awake()
        {
            _panelsController = GetComponent<PanelsController>();
        }

        private void Start()
        {
            UpdateTexts();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnMinigameValuesUpdated += UpdateTexts;
            ActionsManager.Instance.OnMinigameChallengeUpdated += UpdateChallengePanel;
            ActionsManager.Instance.OnPauseToggle += TogglePause;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnMinigameValuesUpdated -= UpdateTexts;
            ActionsManager.Instance.OnMinigameChallengeUpdated -= UpdateChallengePanel;
            ActionsManager.Instance.OnPauseToggle -= TogglePause;
        }

        private void UpdateTexts()
        {
            _currentComboText.text = MinigameManager.CurrentCombo.ToString();
            _currentPerfectComboText.text = MinigameManager.CurrentPerfectCombo.ToString();
        }

        private void UpdateChallengePanel(MinigameChallenge challenge)
        {
            string challengeName = "";

            switch (challenge.ChallengeType)
            {
                case MinigameChallengeEnum.CurrentCombo:
                    challengeName = $"Combo";
                    break;
                case MinigameChallengeEnum.CurrentPerfectCombo:
                    challengeName = $"Combo Perfeito";
                    break;
                case MinigameChallengeEnum.TotalPerfects:
                    challengeName = $"Total de Acertos Perfeitos";
                    break;
                case MinigameChallengeEnum.TotalHits:
                    challengeName = $"Total de Acertos";
                    break;
                case MinigameChallengeEnum.Duration:
                    challengeName = $"Não erre por {challenge.ChallengeGoal} segundos";
                    break;
            }

            _challengeNameText.text = challengeName + $" #{MinigameManager.CurrentChallengeIndex+1}";
            _challengeProgressBar.value = (float)challenge.ChallengeProgress / (float)challenge.ChallengeGoal;
            _challengeProgressText.text = $"{challenge.ChallengeProgress} / {challenge.ChallengeGoal}";
        }

        private void TogglePause(bool isPaused)
        {
            if(isPaused)
            {
                PauseMinigame(); 
            }
            else
            {
                UnPauseMinigame();
            }
        }

        public void PauseMinigame()
        {
            MinigameManager.Instance.TogglePause(true);
        }

        public void UnPauseMinigame()
        {
            MinigameManager.Instance.TogglePause(false);
            _panelsController.ChangePanel(0);
        }

        public void ExitMinigame()
        {
            MinigameManager.Instance.EndMinigame();
        }
    }
}
