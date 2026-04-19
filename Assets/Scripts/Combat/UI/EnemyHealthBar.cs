using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        #region Properties

        public Slider Slider { get { return _slider; } }

        #endregion

    }
}
