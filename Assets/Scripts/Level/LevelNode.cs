using Unity.Cinemachine;
using UnityEngine;

namespace RPG.Level
{
    public class LevelNode : MonoBehaviour
    {
        [Header("Level Data")]
        [SerializeField] 
        private LevelScriptable levelData;

        [Header("visualization")]
        [SerializeField]
        private GameObject visualMapsObjects;

        [Header("Camera")]
        [SerializeField]
        private CinemachineCamera levelCamera;

        #region Properties
        
        public LevelScriptable LevelData
        {
            get { return levelData; }
        }

        public CinemachineCamera LevelCamera
        {
            get { return levelCamera; }
        }

        #endregion

        #region Methods

        public void SetVisualActive(bool value)
        {
            visualMapsObjects.SetActive(value);
        }

        #endregion
    }
}
