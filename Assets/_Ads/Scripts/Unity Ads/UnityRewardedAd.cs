using UnityEngine;
using UnityEngine.Advertisements;

public class UnityRewardedAd : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private string _adUnitId;

    public string AdUnitID { set { _adUnitId = value; } }

    public bool IsRewardedAdLoaded { get; private set; }

    public void LoadAd()
    {
        Advertisement.Load(_adUnitId, this);
    }

    public void ShowAd()
    {
        Advertisement.Show(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        IsRewardedAdLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {placementId}: {error} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }
    public void OnUnityAdsShowClick(string placementId) { }
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            // Grant a reward.
            AdsManager.HandleRewardedAdWatchedComplete();
        }
        Time.timeScale = 1;
    }
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }
    public void OnUnityAdsShowStart(string placementId)
    {
        IsRewardedAdLoaded = false;
        Time.timeScale = 0;
    }
}
