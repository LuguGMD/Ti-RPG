using Lugu.Singleton;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class CurrentVersionText : SingletonMonoPersistent<CurrentVersionText>
    {
        [SerializeField] private TextMeshProUGUI _currentVersionText;

        protected override void Awake()
        {
            base.Awake();

            _currentVersionText.text = "v" + Application.version;

#if UNITY_EDITOR
            _currentVersionText.text += "*";
#endif
        }
    }
}
