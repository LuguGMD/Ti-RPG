using TMPro;
using UnityEngine;

namespace RPG.Management.Progression
{
    public class UpgradeConfirmPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _confirmName;
        [SerializeField] private TextMeshProUGUI _confirmPrice;

        public void Init(UpgradeNode node)
        {
            _confirmName.text = node.Data.upgradeName;
            _confirmPrice.text = $"Preço: {node.Data.priceUpgrade}";
        }
    }
}
