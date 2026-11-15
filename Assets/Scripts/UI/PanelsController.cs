
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Lugu.UI
{
    public class PanelsController : MonoBehaviour
    {
        [SerializeField] private int _startCanvasIndex;
        [SerializeField] private List<GameObject> _panelsList;

        [Header("Panel Animations")]
        
        [Min(0.01f)]
        [SerializeField] private float _transitionDuration = 0.1f;
        [Range(0.8f, 1.2f)]
        [SerializeField] private float _startScale = 1.1f;


        private GameObject _currentPanel;
        private Sequence _transitionSequence;

        private readonly Dictionary<GameObject, CanvasGroup> _canvasGroups =
            new Dictionary<GameObject, CanvasGroup>();

        private readonly Dictionary<GameObject, Vector3> _originalScales =
            new Dictionary<GameObject, Vector3>();

        #region Properties

        public List<GameObject> panelsList
        {
            get { return _panelsList; }
            private set { _panelsList = value; }
        }

        #endregion

        private void Awake()
        {
            foreach (GameObject panel in _panelsList)
            {
                if (panel == null)
                    continue;

                CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();

                if (canvasGroup == null)
                    canvasGroup = panel.AddComponent<CanvasGroup>();

                _canvasGroups[panel] = canvasGroup;
                _originalScales[panel] = panel.transform.localScale;
            }

            // Start with all panels hidden.
            foreach (GameObject panel in _panelsList)
            {
                if (panel != null)
                    panel.SetActive(false);
            }

            // Open the initial panel.
            if (_startCanvasIndex >= 0 &&
                _startCanvasIndex < _panelsList.Count)
            {
                OpenPanel(_startCanvasIndex);
            }
            else
            {
                Debug.LogWarning(
                    "PanelsController: Start panel index is invalid.",
                    this);
            }
        }

        public void ChangePanel(int panelIndex)
        {
            if (panelIndex < 0 || panelIndex >= _panelsList.Count)
                return;

            GameObject nextPanel = _panelsList[panelIndex];

            if (nextPanel == null)
                return;

            if (nextPanel == _currentPanel &&
                nextPanel.activeSelf &&
                (_transitionSequence == null ||
                 !_transitionSequence.IsActive()))
            {
                return;
            }

            // Stop any previous transition.
            _transitionSequence?.Kill();
            _transitionSequence = null;

            GameObject previousPanel = _currentPanel;

            if (previousPanel == null || !previousPanel.activeSelf)
            {
                ShowPanel(nextPanel);
                return;
            }

            CanvasGroup previousGroup = _canvasGroups[previousPanel];

            _transitionSequence = DOTween.Sequence()
                .SetUpdate(true);

            _transitionSequence.Append(
                previousGroup.DOFade(0f, _transitionDuration)
                    .SetEase(Ease.InQuad)
            );

            _transitionSequence.Join(
                previousPanel.transform
                    .DOScale(
                        _originalScales[previousPanel] * _startScale,
                        _transitionDuration)
                    .SetEase(Ease.InQuad)
            );

            _transitionSequence.AppendCallback(() =>
            {
                previousPanel.SetActive(false);
                ShowPanel(nextPanel);
            });
        }

        private void ShowPanel(GameObject panel)
        {
            // Keep only the requested panel active.
            foreach (GameObject item in _panelsList)
            {
                if (item != null && item != panel)
                    item.SetActive(false);
            }

            panel.SetActive(true);
            _currentPanel = panel;

            CanvasGroup group = _canvasGroups[panel];

            // Prepare the entrance animation.
            group.alpha = 0f;
            panel.transform.localScale =
                _originalScales[panel] * _startScale;

            _transitionSequence?.Kill();

            _transitionSequence = DOTween.Sequence()
                .SetUpdate(true);

            _transitionSequence.Append(
                group.DOFade(1f, _transitionDuration)
                    .SetEase(Ease.OutQuad)
            );

            _transitionSequence.Join(
                panel.transform
                    .DOScale(_originalScales[panel], _transitionDuration)
                    .SetEase(Ease.OutBack)
            );
        }

        public void ClosePanel(int panelIndex)
        {
            if (panelIndex < 0 || panelIndex >= _panelsList.Count)
                return;

            GameObject panel = _panelsList[panelIndex];

            if (panel == null || !panel.activeSelf)
                return;

            _transitionSequence?.Kill();
            _transitionSequence = null;

            CanvasGroup group = _canvasGroups[panel];

            _transitionSequence = DOTween.Sequence()
                .SetUpdate(true);

            _transitionSequence.Append(
                group.DOFade(0f, _transitionDuration)
                    .SetEase(Ease.InQuad)
            );

            _transitionSequence.Join(
                panel.transform
                    .DOScale(
                        _originalScales[panel] * _startScale,
                        _transitionDuration)
                    .SetEase(Ease.InQuad)
            );

            _transitionSequence.OnComplete(() =>
            {
                panel.SetActive(false);

                if (_currentPanel == panel)
                    _currentPanel = null;
            });
        }

        public void OpenPanel(int panelIndex)
        {
            if (panelIndex < 0 || panelIndex >= _panelsList.Count)
                return;

            GameObject panel = _panelsList[panelIndex];

            if (panel == null)
                return;

            _transitionSequence?.Kill();
            _transitionSequence = null;

            ShowPanel(panel);
        }

        private void OnDestroy()
        {
            _transitionSequence?.Kill();

            foreach (CanvasGroup group in _canvasGroups.Values)
            {
                if (group != null)
                    group.DOKill();
            }
        }
    }
}
