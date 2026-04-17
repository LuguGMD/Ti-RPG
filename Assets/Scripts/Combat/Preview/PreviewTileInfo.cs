using UnityEngine;

namespace RPG.Combat.Preview
{
    public class PreviewTileInfo
    {
        private Vector2Int _relativePosition;
        private int _patternIndex = 0;
        private int _patternRepetitionCount = 1;
        private bool _isMirrored = false;
        private bool _isAttack = false;
        private bool _isMovement = false;

        #region Properties

        public Vector2Int RelativePosition { get { return _relativePosition; } }
        public int PatternCount { get { return _patternIndex; } }
        public int PatternRepetitionCount { get { return _patternRepetitionCount; } } 
        public bool IsAttack { get { return _isAttack; } }
        public bool IsMovement { get { return _isMovement; } }

        #endregion

        public PreviewTileInfo(Vector2Int relativePosition, int patternIndex, int patternRepetitionCount, bool isMirrored, bool isAttack, bool isMovement)
        {
            _relativePosition = relativePosition;
            _patternIndex = patternIndex;
            _patternRepetitionCount = patternRepetitionCount;
            _isMirrored = isMirrored;
            _isAttack = isAttack;
            _isMovement = isMovement;
        }
    }
}
