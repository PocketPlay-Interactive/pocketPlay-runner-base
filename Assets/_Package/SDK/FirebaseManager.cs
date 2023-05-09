using System.Collections;
using System.Collections.Generic;
#if FIREBASE_ENABLE
using Firebase;
using Firebase.Analytics;
#endif
using UnityEngine;

public class FirebaseManager : MonoSingleton<FirebaseManager>
{
    private bool IsFirebaseInitialized = false;
#if FIREBASE_ENABLE
    public IEnumerator InitializedFirebase()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Initializer maybe
                var app = FirebaseApp.DefaultInstance;
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

                // Finish initializer
                Debug.Log("Firebase initialized");
                IsFirebaseInitialized = true;
            }
            else
            {
                IsFirebaseInitialized = true;
                Debug.LogError(string.Format("Dependency error: {0}", dependencyStatus)); // Firebase Unity SDK is not safe to use here.
            }
        });
        yield return null;
    }

    public IEnumerator InitializedFirebaseMessaging()
    {
        yield return new WaitUntil(() => IsFirebaseInitialized);
        Debug.Log("Firebase Messaging initialized");
        Firebase.Messaging.FirebaseMessaging.TokenReceived += FirebaseMessaging_TokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += FirebaseMessaging_MessageReceived;
    }

    private void FirebaseMessaging_MessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }

    private void FirebaseMessaging_TokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + e.Token);
    }

    public void AdsInterClick()
    {

    }

    public void AdsInterShow(string position)
    {
        if (!IsFirebaseInitialized)
            return;

        Parameter[] parameters = {
            new Firebase.Analytics.Parameter("placement", position),
        };
        FirebaseAnalytics.LogEvent("ads_inter_show", parameters);
    }

    public void AdsInterClick(string position)
    {
        if (!IsFirebaseInitialized)
            return;

        Parameter[] parameters = {
            new Firebase.Analytics.Parameter("placement", position),
        };
        FirebaseAnalytics.LogEvent("ads_inter_click", parameters);
    }

    public void AdsRewardShow(string position)
    {
        if (!IsFirebaseInitialized)
            return;

        Parameter[] parameters = {
            new Firebase.Analytics.Parameter("placement", position),
        };
        FirebaseAnalytics.LogEvent("ads_reward_show", parameters);
    }

    public void AdsRewardClick(string position)
    {
        if (!IsFirebaseInitialized)
            return;

        Parameter[] parameters = {
            new Firebase.Analytics.Parameter("placement", position),
        };
        FirebaseAnalytics.LogEvent("ads_reward_click", parameters);
    }

    public void AdsPopupOffer(params Parameter[] parameters)
    {
        if (!IsFirebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent("ads_popup_offer", parameters);
    }

    public void AdsPopupClick(params Parameter[] parameters)
    {
        if (!IsFirebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent("ads_popup_click", parameters);
    }

    public void AdsPopupFinish(params Parameter[] parameters)
    {
        if (!IsFirebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent("ads_popup_finish", parameters);
    }

    public void TimeCheckPoint(string index)
    {
        if (!IsFirebaseInitialized)
            return;

        Parameter[] parameters = {
            new Firebase.Analytics.Parameter("scene", "game"),
        };

        FirebaseAnalytics.LogEvent($"checkpoint_{index}", parameters);
    }

    public void LevelStartCheckPoint(int level)
    {
        if (!IsFirebaseInitialized)
            return;

        var _strLevel = level < 10 ? $"0{level}" : $"{level}";
        FirebaseAnalytics.LogEvent($"checkpoint_level_{_strLevel}_start");
    }

    public void LevelStart(int play_time, string theme)
    {
        if (!IsFirebaseInitialized)
            return;

        Parameter[] parameters = {
          new Parameter("play_time", play_time),
          new Parameter("theme", theme),
        };

        FirebaseAnalytics.LogEvent("game_start", parameters);
    }    

    public void LevelEndCheckPoint(int level)
    {
        if (!IsFirebaseInitialized)
            return;

        var _strLevel = level < 10 ? $"0{level}" : $"{level}";
        FirebaseAnalytics.LogEvent($"checkpoint_level_{_strLevel}_end");
    }

    public void LevelEnd(int play_time, float duration, int skin, int hair, int eyeshadow, int eyes, int eyebrow, int eyelash, int lipstick, int top, int bottom, int dress, int necklace, int earings, int bag, int shoes, int access, int glasses)
    {
        if (!IsFirebaseInitialized)
            return;

        Parameter[] parameters = {
          new Parameter("play_time", play_time),
          new Parameter("duration", duration),
          new Parameter("skin", skin),
          new Parameter("hair", hair),
          new Parameter("eyeshadow", eyeshadow),
          new Parameter("eyes", eyes),
          new Parameter("eyebrow", eyebrow),
          new Parameter("eyelash", eyelash),
          new Parameter("lipstick", lipstick),
          new Parameter("top", top),
          new Parameter("bottom", bottom),
          new Parameter("dress", dress),
          new Parameter("necklace", necklace),
          new Parameter("earings", earings),
          new Parameter("bag", bag),
          new Parameter("shoes", shoes),
          new Parameter("access", access),
          new Parameter("glasses", glasses),
        };

        FirebaseAnalytics.LogEvent("game_end", parameters);
    }    

    public void CVPlayTime(string time)
    {
        if (!IsFirebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent($"cv_play_{time}min");
    }

    public void CVRetention(string day)
    {
        if (!IsFirebaseInitialized)
            return;

        FirebaseAnalytics.LogEvent($"cv_retention_d{day}");
    }

    public void ShopPurchaseItem(string item_id, int cost)
    {
        if (!IsFirebaseInitialized)
            return;

        Parameter[] parameters = {
          new Parameter("item_id", item_id),
          new Parameter("cost", cost),
        };

        FirebaseAnalytics.LogEvent("shop_purchase_item", parameters);
    }

    public void ShopPurchasePack(string pack_id, int cost)
    {
        if (!IsFirebaseInitialized)
            return;

        Parameter[] parameters = {
          new Parameter("pack_id", pack_id),
          new Parameter("cost", cost),
        };

        FirebaseAnalytics.LogEvent("shop_purchase_pack", parameters);
    }
#endif
}
