using Lugu.Singleton;
using System;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UnityConsent;

namespace RPG.Analytics
{
    public class AnalyticsManager : SingletonMonoPersistent<AnalyticsManager>
    {
        [SerializeField] private GameObject _consentPanel;
        [SerializeField] private Button _consentButton;
        [SerializeField] private Button _denyButton;

        //TO DO modificar depois para false como padrao
        private bool _hasConsented = true;

        #region Properties

        public bool HasConsented { get { return _hasConsented; } }

        #endregion

        private async void Start()
        {
            if (Instance != this) return;

            if(UnityServices.State == ServicesInitializationState.Uninitialized)
                await UnityServices.InitializeAsync();

            _consentButton?.onClick.AddListener(ConsentToAnalytics);
            _denyButton?.onClick.AddListener(DenyAnalytics);

            if(_hasConsented)
            {
                ConsentToAnalytics();
            }
            else
            {
                if (_consentPanel != null)
                    _consentPanel?.SetActive(true);
            }

        }

        private void ConsentToAnalytics()
        {
            EndUserConsent.SetConsentState(new ConsentState
            {
                AnalyticsIntent = ConsentStatus.Granted,
                AdsIntent = ConsentStatus.Denied
            });

            if(_consentPanel!= null)
                _consentPanel.SetActive(false);
        }

        private void DenyAnalytics()
        {
            if (_consentPanel != null)
                _consentPanel?.SetActive(false);
        }
        
    }
}
