using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxManager : MonoSingleton<MaxManager>
{
    public string MaxSDK;
    public string MaxReward;
    public string MaxInter;
    public string MaxBanner;

    public bool _initialized = false;

    private Action Interstitial_Callback;
    private Action Reward_Success_Callback;
    private Action Reward_Fail_Callback;

    public GameObject LoadingAd;

    public IEnumerator Initialized()
    {
#if APPLOVIN_ENABLE
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            // AppLovin SDK is initialized, start loading ads
        };

        MaxSdk.SetSdkKey(MaxSDK);
        MaxSdk.InitializeSdk();
#endif
        yield return null;
        InitializeBannerAds();
        InitializeInterstitialAds();
        InitializeRewardedAds();
        yield return WaitForSecondCache.WAIT_TIME_ONE;

        _initialized = true;

    }

    public void InitializeBannerAds()
    {
#if APPLOVIN_ENABLE
        // Banners are automatically sized to 320?50 on phones and 728?90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(MaxBanner, MaxSdkBase.BannerPosition.TopCenter);

        // Set background or background color for banners to be fully functional
        //MaxSdk.SetBannerBackgroundColor(MaxBanner, Color.white);

        //Start auto-refresh for a banner ad with the following code:
        MaxSdk.StartBannerAutoRefresh(MaxBanner);

        //There may be cases when you would like to stop auto-refresh, for instance, if you want to manually refresh banner ads. To stop auto-refresh for a banner ad, use the following code:
        //MaxSdk.StopBannerAutoRefresh(MaxBanner);

        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
        MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;

        //Load banner
        LoadBanner();
        ShowBanner();
#endif
    }

    public void LoadBanner()
    {
#if APPLOVIN_ENABLE
        MaxSdk.LoadBanner(MaxBanner);
#endif
    }

    public void ShowBanner()
    {
#if APPLOVIN_ENABLE
        MaxSdk.ShowBanner(MaxBanner);
#endif
    }

    public void HideBanner()
    {
#if APPLOVIN_ENABLE
        MaxSdk.HideBanner(MaxBanner);
#endif
    }

#if APPLOVIN_ENABLE
    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { Debug.Log("OnBannerAdLoadedEvent"); }

    private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) { Debug.Log("OnBannerAdLoadFailedEvent"); }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { Debug.Log("OnBannerAdClickedEvent"); }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { Debug.Log("OnBannerAdRevenuePaidEvent"); }

    private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { Debug.Log("OnBannerAdExpandedEvent"); }

    private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { Debug.Log("OnBannerAdCollapsedEvent"); }
#endif


    private int retryAttemptInter;
    public void InitializeInterstitialAds()
    {
#if APPLOVIN_ENABLE
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
#endif
        // Load the first interstitial
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
#if APPLOVIN_ENABLE
        if (!MaxSdk.IsInterstitialReady(MaxInter))
            MaxSdk.LoadInterstitial(MaxInter);
#endif
    }

#if APPLOVIN_ENABLE
    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'
        // Reset retry attempt
        retryAttemptInter = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttemptInter++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttemptInter));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { Debug.Log("OnInterstitialDisplayedEvent"); }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
        Interstitial_Callback?.Invoke();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { Debug.Log("OnInterstitialClickedEvent"); }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
        Interstitial_Callback?.Invoke();
    }
#endif

    public void ShowInter(Action callback)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            callback?.Invoke();
        }
        else {
            LoadingAd.SetActive(true);
            InterShowing = true;
            InterStep = 0;
            InterAsync = StartCoroutine(ShowInterAsync(callback));
        } 
    }

    public bool InterShowing = false;
    public float InterStep = 0;
    private Coroutine InterAsync;
    private IEnumerator ShowInterAsync(Action callback)
    {
        LoadInterstitial();
#if APPLOVIN_ENABLE
        yield return new WaitUntil(() => MaxSdk.IsInterstitialReady(MaxInter));
#endif
        yield return WaitForSecondCache.WAIT_TIME_ONE;
        LoadingAd.SetActive(false);
        InterShowing = false;
        Interstitial_Callback = callback;
#if APPLOVIN_ENABLE
        MaxSdk.ShowInterstitial(MaxInter);
#endif
    }


    private int retryAttemptReward;
    public void InitializeRewardedAds()
    {
#if APPLOVIN_ENABLE
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
#endif
    }

    private void LoadRewardedAd()
    {
#if APPLOVIN_ENABLE
        if (!MaxSdk.IsRewardedAdReady(MaxReward))
            MaxSdk.LoadRewardedAd(MaxReward);
#endif
    }

#if APPLOVIN_ENABLE
    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.

        // Reset retry attempt
        retryAttemptReward = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttemptReward++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttemptReward));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { Debug.Log("OnRewardedAdDisplayedEvent"); }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
        Reward_Fail_Callback?.Invoke();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { Debug.Log("OnRewardedAdClickedEvent"); }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
        Reward_Success_Callback?.Invoke();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }
#endif

    public void ShowReward(Action callback_success, Action callback_fail)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            callback_fail?.Invoke();
        }
        else
        {
            LoadingAd.SetActive(true);
            RewardShowing = true;
            RewardStep = 0;
            RewardAsync = StartCoroutine(ShowRewardAsync(callback_success, callback_fail));
        }
    }

    public bool RewardShowing = false;
    public float RewardStep = 0;
    private Coroutine RewardAsync;
    private IEnumerator ShowRewardAsync(Action callback_success, Action callback_fail)
    {
        LoadRewardedAd();
#if APPLOVIN_ENABLE
        yield return new WaitUntil(() => MaxSdk.IsRewardedAdReady(MaxReward));
#endif
        yield return WaitForSecondCache.WAIT_TIME_ONE;
        LoadingAd.SetActive(false);
        RewardShowing = false;
        Reward_Success_Callback = callback_success;
        Reward_Fail_Callback = callback_fail;
#if APPLOVIN_ENABLE
        MaxSdk.ShowRewardedAd(MaxReward);
#endif
    }

    private void Update()
    {
        if(InterShowing)
        {
            InterStep += Time.deltaTime;
            if(InterStep > 20)
            {
                InterShowing = false;
                LoadingAd.SetActive(false);
                if (InterAsync != null)
                    StopCoroutine(InterAsync);
            }
        }

        if (RewardShowing)
        {
            RewardStep += Time.deltaTime;
            if (RewardStep > 20)
            {
                RewardShowing = false;
                LoadingAd.SetActive(false);
                if (RewardAsync != null)
                    StopCoroutine(RewardAsync);
            }
        }
    }
}
