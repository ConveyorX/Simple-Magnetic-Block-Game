using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAds : Ads, IUnityAdsInitializationListener
{
    [SerializeField] AdsDataSO _data;

    UnityBannerAd _banner;
    UnityInterstitialAd _interstitial;
    UnityRewardedAd _rewarded;

    static bool IsInitComplete = false;

    public UnityRewardedAd GetRewardedAd => _rewarded;

    void Awake()
    {
        _interstitial = GetComponent<UnityInterstitialAd>();
        _rewarded = GetComponent<UnityRewardedAd>();
        _banner = GetComponent<UnityBannerAd>();

        if (_interstitial) _interstitial.AdUnitID = _data.InterstitialID;
        if (_rewarded) _rewarded.AdUnitID = _data.RewardedID;
        if (_banner) _banner.AdUnitID = _data.BannerID;

        InitializeAds();
    }

    public void InitializeAds()
    {
        if (!IsInitComplete)
        {
            Advertisement.Initialize(_data.GameID, _data.TestMode, this);
        }
    }

    public override void LoadAds()
    {
        if (!IsInitComplete) return;

        if (_interstitial) _interstitial.LoadAd();
        if (_rewarded) _rewarded.LoadAd();
        if (_banner) _banner.LoadBanner();
    }

    public override bool IsRewardedAdLoaded()
    {
        return GetRewardedAd.IsRewardedAdLoaded;
    }

    public override void ShowBanner()
    {
        if (!_data.BannerEnabled) return;

        if (_banner) _banner.ShowBannerAd();
    }

    public override void ShowInterstitial()
    {
        if (!_data.InterstitialEnabled) return;

        if (_interstitial) _interstitial.ShowAd();
    }

    public override void ShowRewarded()
    {
        if (!_data.RewardedEnabled) return;

        if (_rewarded)
        {
            _rewarded.ShowAd();
        }
    }

    public void OnInitializationComplete()
    {
        IsInitComplete = true;
        LoadAds();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
    }
}
