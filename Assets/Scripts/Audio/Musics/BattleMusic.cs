using FMODUnity;
using RPG.Audio;
using UnityEngine;

namespace RPG.Audio
{
    public class BattleMusic : MonoBehaviour
    {
        [SerializeField] private EventReference battleMusic;
        void Start()
        {
            AudioManager.Instance.PlayMusic(battleMusic);
        }
    }
}
