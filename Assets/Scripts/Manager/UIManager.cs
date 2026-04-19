using UnityEngine;

namespace RPG
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private HostMotivationBarController _hostMotivationBar;
        [SerializeField] private CharacterMotivationBarController _characterMotivationBar;

        private static UIManager _instance;

        public static UIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<UIManager>();
                }
                return _instance;
            }
        }

        #region Properties

        public HostMotivationBarController HostMotivationBar { get { return _hostMotivationBar; } }
        public CharacterMotivationBarController CharacterMotivationBar { get { return _characterMotivationBar; } }

        #endregion

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InitializeUIElements();
        }

        private void InitializeUIElements()
        {
            if (_hostMotivationBar != null)
            {
                _hostMotivationBar.Initialize();
            }

            if (_characterMotivationBar != null)
            {
                _characterMotivationBar.Initialize();
            }
        }

        public void UpdateHostMotivationBar(float currentMotivation, float maxMotivation)
        {
            if (_hostMotivationBar != null)
            {
                _hostMotivationBar.UpdateMotivationBar(currentMotivation, maxMotivation);
            }
        }

        public void UpdateCharacterMotivationBar(int characterIndex, float damageValue, float maxHostMotivation)
        {
            if (_characterMotivationBar != null)
            {
                _characterMotivationBar.UpdateCharacterDamage(characterIndex, damageValue, maxHostMotivation);
            }
        }
    }
}
