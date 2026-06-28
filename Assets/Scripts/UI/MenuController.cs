using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _firstSelected;

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_firstSelected);
        }
    }
}
