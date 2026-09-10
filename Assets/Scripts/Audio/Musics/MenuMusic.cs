using FMODUnity;
using RPG.Audio;
using UnityEngine;

namespace RPG.Audio
{
    public class MenuMusic : MonoBehaviour
    {
        [SerializeField] private EventReference menuMusic;
        void Start()
        {
            AudioManager.Instance.PlayMusic(menuMusic);
        }

        private void OnDestroy()
        {
            AudioManager.Instance.StopMusic();
        }
    }
}
