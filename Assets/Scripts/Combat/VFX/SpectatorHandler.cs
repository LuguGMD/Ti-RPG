using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class SpectatorHandler : MonoBehaviour
    {
        public enum PosesEnum
        {
            Idle,
            Cheer,
            Boo
        }

        [System.Serializable]
        private struct PoseStruct
        {
            public PosesEnum Pose;
            public GameObject Model;
            public float StartDelay;
            public float ExitTime;
        }

        private PosesEnum _currentPose = PosesEnum.Idle;

        [SerializeField] private PoseStruct[] _poseInfos;

        #region Properties

        public PosesEnum CurrentPose
        {
            get { return _currentPose; }
        }

        #endregion

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            SetActivePose(_poseInfos[(int)PosesEnum.Idle].Model);

            for(int i = 0; i < _poseInfos.Length; i++)
            {
                _poseInfos[i].ExitTime *= Random.Range(0.9f,1.1f);
                _poseInfos[i].StartDelay *= Random.Range(0.6f,1.4f);
            }
        }

        public void ChangePose(PosesEnum newPose)
        {
            StopAllCoroutines();

            switch (newPose)
            {
                case PosesEnum.Idle:
                    StartCoroutine(HandleIdlePose());
                    break;
                case PosesEnum.Cheer:
                    StartCoroutine(HandleCheerPose());
                    break;
                case PosesEnum.Boo:
                    StartCoroutine(HandleBooPose());
                    break;
            }

            _currentPose = newPose;
        }

        private void SetActivePose(GameObject poseToActivate)
        {
            for(int i = 0; i < _poseInfos.Length; i++)
            {
                _poseInfos[i].Model.SetActive(_poseInfos[i].Model == poseToActivate);
            }
        }

        private IEnumerator HandleIdlePose()
        {
            SetActivePose(_poseInfos[(int)PosesEnum.Idle].Model);
            yield return new WaitForSeconds(_poseInfos[(int)PosesEnum.Cheer].ExitTime);
        }

        private IEnumerator HandleCheerPose()
        {
            yield return new WaitForSeconds(_poseInfos[(int)PosesEnum.Cheer].StartDelay);

            GameObject model = _poseInfos[(int)PosesEnum.Cheer].Model;
            float exitTime = _poseInfos[(int)PosesEnum.Cheer].ExitTime;
            model.transform.DOKill(true);
            Vector3 originalLocalPos = model.transform.localPosition;
            SetActivePose(model);
            
            model.transform.DOShakePosition(exitTime * 0.7f).OnComplete(() =>
            {
                model.transform.localPosition = originalLocalPos;
            });
            yield return new WaitForSeconds(exitTime);
            
            ChangePose(PosesEnum.Idle);
        }

        private IEnumerator HandleBooPose()
        {
            yield return new WaitForSeconds(_poseInfos[(int)PosesEnum.Boo].StartDelay);

            GameObject model = _poseInfos[(int)PosesEnum.Boo].Model;
            float exitTime = _poseInfos[(int)PosesEnum.Cheer].ExitTime;
            model.transform.DOKill(true);
            Vector3 originalLocalPos = model.transform.localPosition;
            SetActivePose(model);
            model.transform.DOShakePosition(exitTime * 0.7f, 0.2f, 6).OnComplete(() =>
            {
                model.transform.localPosition = originalLocalPos;
            });
            yield return new WaitForSeconds(_poseInfos[(int)PosesEnum.Boo].ExitTime);
            ChangePose(PosesEnum.Idle);
        }
    }
}
