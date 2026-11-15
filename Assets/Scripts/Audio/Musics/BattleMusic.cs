using FMODUnity;
using RPG.Audio;
using UnityEngine;

namespace RPG.Audio
{
    public class BattleMusic : MonoBehaviour
    {
        [SerializeField] private EventReference battleMusic;
        [SerializeField] private EventReference bossMusic;
        void Start()
        {
            if (GameManager.SelectedLevel == GameManager.BossFightLevel)
            {
                AudioManager.Instance.PlayMusic(bossMusic);
            }
            else
            {
                AudioManager.Instance.PlayMusic(battleMusic);
            }
        }

        private void OnDestroy()
        {
            AudioManager.Instance.StopMusic();
        }
    }
}
