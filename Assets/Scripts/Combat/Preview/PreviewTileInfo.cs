using RPG.Combat.Actions;
using RPG.Combat.Grid;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Preview
{
    public class PreviewTileInfo
    {
        private Vector2Int _relativePosition;
        private DirectionEnum _direction;
        private PreviewTileInfo _child;
        private PreviewTileInfo _parent;
        private List<Effect> _effects = new List<Effect>();
        private bool _needsToBeEmpty = false;
        private bool _doShowParent = false;
        private bool _doShowSelf = false;

        #region Properties

        public Vector2Int RelativePosition { get { return _relativePosition; } }
        public PreviewTileInfo Child { get { return _child; } }
        public PreviewTileInfo Parent { get { return _parent; } }
        public List<Effect> Effects { get { return _effects; } }
        public DirectionEnum Direction { get { return _direction; } }
        public bool NeedsToBeEmpty { get { return _needsToBeEmpty; } }
        public bool DoShowParent { get { return _doShowParent; } }
        public bool DoShowSelf { get { return _doShowSelf; } }

        #endregion

        public PreviewTileInfo(Vector2Int relativePosition, DirectionEnum direction, bool needsToBeEmpty, bool doShowParent = true, bool doShowSelf = true)
        {
            _relativePosition = relativePosition;
            _direction = direction;
            _needsToBeEmpty = needsToBeEmpty;
            _doShowParent = doShowParent;
            _doShowSelf = doShowSelf;
        }

        public void SetParent(PreviewTileInfo parent)
        {
            _parent = parent;
        }

        public PreviewTileInfo CreateChild(Vector2Int relativePosition, DirectionEnum direction, bool needsToBeEmpty, bool doShowParent = true, bool doShowSelf = true)
        {
            PreviewTileInfo child = new PreviewTileInfo(relativePosition, direction, needsToBeEmpty, doShowParent, doShowSelf);
            _child = child;
            child.SetParent(this);

            return child;
        }

        public static PreviewTileInfo GetRoot(PreviewTileInfo previewTileInfo)
        {
            if (previewTileInfo.Parent == null)
            {
                return previewTileInfo;
            }

            return GetRoot(previewTileInfo.Parent);
        }

        public static PreviewTileInfo GetLeaf(PreviewTileInfo previewTileInfo)
        {
            if(previewTileInfo.Child == null) 
            {
                return previewTileInfo;
            }

            return GetLeaf(previewTileInfo.Child);
        }
    }
}
