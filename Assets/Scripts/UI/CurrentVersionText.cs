using TMPro;
using UnityEngine;

namespace RPG
{
    public class CurrentVersionText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentVersionText;

        private void Awake()
        {
            _currentVersionText.text = "v" + Application.version;
        }
    }
}
