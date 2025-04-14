using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Game
{
    public class AdsManager : MonoBehaviour
    {
        
        private void Awake()
        {
            if (FindObjectsOfType<AdsManager>().Length > 1)
            {
                Destroy(gameObject); // Prevent duplicate AdsManager
                return;
            }
        }
        
        public void Start()
        {
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                // This callback is called once the MobileAds SDK is initialized.
            });
            LoadAd();
        }
        
        
        // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
        private string _adUnitId = "ca-app-pub-3082331033671775/4241606685";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
  private string _adUnitId = "unused";
#endif

        BannerView _bannerView;

        /// <summary>
        /// Creates a 320x50 banner view at top of the screen.
        /// </summary>
        public void CreateBannerView()
        {
            Debug.Log("Creating banner view");

            // If we already have a banner, destroy the old one.
            if (_bannerView != null)
            {
                DestroyAd();
            }

            AdSize adSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
            _bannerView = new BannerView(_adUnitId, adSize, AdPosition.Bottom);
        }
        
        
        public void LoadAd()
        {
            // create an instance of a banner view first.
            if(_bannerView == null)
            {
                CreateBannerView();
            }

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            Debug.Log("Loading banner ad.");
            _bannerView.LoadAd(adRequest);
        }
        
        public void DestroyAd()
        {
            if (_bannerView != null)
            {
                Debug.Log("Destroying banner view.");
                _bannerView.Destroy();
                _bannerView = null;
            }
        }
    }
}