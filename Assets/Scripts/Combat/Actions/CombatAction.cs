using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public abstract class CombatAction
    {
        [SerializeField] protected string _actionName;
        [SerializeField] protected string _actionDescription;
        [SerializeField] protected List<Effect> _effects;
        [SerializeField] protected bool _lastTileNeedsToBeEmpty = true;
        protected StageEntityController _user;

        #region Properties

        public string ActionName { get { return  _actionName; }  }
        public string ActionDescription { get { return _actionDescription; } }

        public List<Effect> Effects
        {
            get {  return _effects; }
        }

        public bool LastTileNeedsToBeEmpty { get { return _lastTileNeedsToBeEmpty; } }

        #endregion

        public abstract void Init(StageEntityController user);

        public abstract IEnumerator Execute(PreviewTileInfo selectedPreviewTile);

        public abstract List<PreviewTileInfo> Preview();
    }
}
