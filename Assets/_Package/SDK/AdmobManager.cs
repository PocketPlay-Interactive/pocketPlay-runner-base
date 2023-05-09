#if ADMOB_ENABLE
using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine;

public class AdmobManager : MonoSingleton<AdmobManager>
{
    public string AppId;
    public string OpenAdsId;

    private AppOpenAd ad;
    private bool isShowingAd = false;

    private bool IsAdAvailable
    {
        get
        {
            return ad != null;
        }
    }

    private float loadTime;
    private bool awake = false;
    private void Update()
    {
        if (awake)
            return;

        loadTime += Time.deltaTime;
        if(loadTime > 10)
        {
            awake = true;
            Manager.Instance.HideGlobalLoading();
            if (ShowAdCorotine != null)
                StopCoroutine(ShowAdCorotine);
        }
    }

    private Coroutine ShowAdCorotine;
    public IEnumerator Initialized()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => {
            Debug.Log("Admob " + initStatus);
        });
        LoadAd();
        yield return null;
        ShowAdCorotine = StartCoroutine(ShowAd());
    }

    public void LoadAd()
    {
        // We will implement this below.
        AdRequest request = new AdRequest.Builder().Build();

        // Load an app open ad for portrait orientation
        AppOpenAd.LoadAd(OpenAdsId, ScreenOrientation.Portrait, request, ((appOpenAd, error) =>
        {
            if (error != null)
            {
                // Handle the error.
                Debug.LogFormat("Failed to load the ad. (reason: {0})", error.LoadAdError.GetMessage());
                return;
            }

            Debug.Log("ad loaded");
            // App open ad is loaded.
            ad = appOpenAd;
        }));

        //https://developers.google.com/admob/unity/app-open
    }

    private IEnumerator ShowAd()
    {
        yield return new WaitUntil(() => IsAdAvailable);
        Manager.Instance.HideGlobalLoading();
        ShowAdIfAvailable();
    }    

    public void ShowAdIfAvailable()
    {
        if (!IsAdAvailable || isShowingAd)
            return;

        if (ad == null)
            return;

        ad.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
        ad.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
        ad.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
        ad.OnAdDidRecordImpression += HandleAdDidRecordImpression;
        ad.OnPaidEvent += HandlePaidEvent;

        ad.Show();
    }

    private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;
        LoadAd();
    }

    private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
    {
        Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        LoadAd();
    }

    private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
    {
        Debug.Log("Displayed app open ad");
        isShowingAd = true;
    }

    private void HandleAdDidRecordImpression(object sender, EventArgs args)
    {
        Debug.Log("Recorded ad impression");
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
        Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
                args.AdValue.CurrencyCode, args.AdValue.Value);
    }

    private void OnAppStateChanged(AppState state)
    {
        // Display the app open ad when the app is foregrounded.
        UnityEngine.Debug.Log("App State is " + state);
        if (state == AppState.Foreground)
        {
            ShowAdIfAvailable();
        }
    }
}
#endif
