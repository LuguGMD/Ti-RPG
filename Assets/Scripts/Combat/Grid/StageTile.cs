using RPG.Input;
using RPG.Management.Progression;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Combat.Grid
{
    [RequireComponent(typeof(CursorTarget))]
    public class StageTile : MonoBehaviour
    {
        private CursorTarget _cursorTarget;
        [SerializeField] private Vector2Int _position;
        private Tile _tile;

        [SerializeField] private MeshRenderer _renderer;

        private void Awake()
        {
            _cursorTarget = GetComponent<CursorTarget>();
        }

        private void Start()
        {
            _cursorTarget.Actions.Hover.OnStart(OnHover);
            _tile = MapManager.Map.GetTile(_position);
            SetUpgrade(_tile.TileUpgrade);
        }

        private void OnDestroy()
        {
            _cursorTarget.Actions.Hover.Stop();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnMapChanged += UpdatePosition;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnMapChanged -= UpdatePosition;
        }

        private void OnHover()
        {
            ActionsManager.Instance.OnTileHovered?.Invoke(_position);
        }

        private void UpdatePosition()
        {
            if(_tile != null)
                _position = _tile.Position;
        }

        [ContextMenu("Set Upgrade Test")]
        public void SetUpgradeTest()
        {
            SetUpgrade(UpgradeConstants.UpgradeKey.TileDamageReduction1);
        }

        public void SetUpgrade(UpgradeConstants.UpgradeKey upgrade)
        {
            Texture texture = null;

            switch (upgrade)
            {
                case UpgradeConstants.UpgradeKey.TileDamageReduction1:
                case UpgradeConstants.UpgradeKey.TileDamageReduction2:
                    texture = CombatManager.TileResistenceImage;
                    break;
                case UpgradeConstants.UpgradeKey.TileDamageIncrease1:
                case UpgradeConstants.UpgradeKey.TileDamageIncrease2:
                    texture = CombatManager.TileWeaknessImage;
                    break;
                case UpgradeConstants.UpgradeKey.TilePushBlock1:
                case UpgradeConstants.UpgradeKey.TilePushBlock2:
                    texture = CombatManager.TilePushImage;
                    break;
            }

            if (texture == null) return;

            _renderer.material = new Material(CombatManager.TileMaterial);
            _renderer.material.SetTexture("_Diffuse_Map", texture);
        }
    }
}
