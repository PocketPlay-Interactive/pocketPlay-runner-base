using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NameScene
{
    Home,
    BallSort
}

public class Manager : MonoSingletonGlobal<Manager>
{
    private bool session_1;
    protected override void Awake()
    {
        base.Awake();
        RuntimeStorageData.ReadData();
        session_1 = PrefManager.GetBool("session_1", false);
        if (!session_1)
            PrefManager.SetBool("session_1", true);

        ShowGlobalLoading();
    }

    public bool IsOpen = true;
    public bool IsInitializedComplete = false;

    private IEnumerator Start()
    {
#if FIREBASE_ENABLE
        yield return FirebaseManager.Instance.InitializedFirebase();
        yield return FirebaseManager.Instance.InitializedFirebaseMessaging();
#endif
#if ADMOB_ENABLE
        yield return AdmobManager.Instance.Initialized();
#endif
#if APPLOVIN_ENABLE
        yield return MaxManager.Instance.Initialized();
#endif
        IsInitializedComplete = true;

        //caculate day;
        var _caculateDayNow = PrefManager.GetInt("retentionTime");
        if(_caculateDayNow != StaticTimerHelper.CurrentTimeDayInDay())
        {
            Debug.Log($"{_caculateDayNow} - {StaticTimerHelper.CurrentTimeDayInDay()} is next day");
            _caculateDayNow = StaticTimerHelper.CurrentTimeDayInDay();
            PrefManager.SetInt("retentionTime", _caculateDayNow);
            var _caculateRetention = PrefManager.GetInt("retention", 0);
#if FIREBASE_ENABLE
            FirebaseManager.Instance.CVRetention(_caculateRetention.ToString());
#endif
            _caculateRetention += 1;
            PrefManager.SetInt("retention", _caculateRetention);
        }
        yield return null;

#if UNITY_EDITOR
        for (int i = 0; i < 10000; i++)
        {
            RandomXacXuat();
        }
#endif
    }

    private float timerCheckpoint = 0;
    private int index = 0;

    private float timerCV = 0;
    private bool cv300 = false;
    private bool cv480 = false;
    private bool cv780 = false;

    private int min1 = 1000, max1 = 0, min2 = 1000, max2 = 0;

    private void Update()
    {
        if (!IsInitializedComplete)
            return;
#if FIREBASE_ENABLE
        if (session_1)
        {
            timerCheckpoint += Time.deltaTime;
            if (timerCheckpoint > 30)
            {
                timerCheckpoint = 0;
                index += 1;

                FirebaseManager.Instance.TimeCheckPoint(index < 10 ? $"0{index}" : $"{index}");
        if (index >= 20)
                    session_1 = false;
            }
        }

        timerCV += Time.deltaTime;
        if(!cv300 && timerCV >= 300)
        {
            cv300 = true;
            FirebaseManager.Instance.CVPlayTime("05");
        }
        if(!cv480 && timerCV > 480)
        {
            cv480 = true;
            FirebaseManager.Instance.CVPlayTime("08");
        }
        if (!cv780 && timerCV > 780)
        {
            cv780 = true;
            FirebaseManager.Instance.CVPlayTime("13");
        }
#endif

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RandomXacXuat();
        }

        if (_duration)
            _timer += Time.deltaTime;
    }

    private void RandomXacXuat()
    {
//#if UNITY_EDITOR
//        var arr = new int[] { 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 };
//        int value0 = 0;
//        int value1 = 0;
//        for (int i = 0; i < 1000; i++)
//        {
//            var value = arr[Random.Range(0, arr.Length)];
//            if (value == 1) value1 += 1;
//            else value0 += 1;
//        }

//        if (min1 > value1) min1 = value1;
//        if (max1 < value1) max1 = value1;
//        if (min2 > value0) min2 = value0;
//        if (max2 < value0) max2 = value0;

//        Debug.Log($"So lan ra xanh la {value1} ra do la {value0} min xanh {min1} max xanh {max1} min do {min2} max do {max2}");
//#endif
    }

    private bool _duration = false;
    private float _timer;
    public void GameDurationStart()
    {
        _duration = true;
        _timer = 0;
    }

    public float GameDurationEnd()
    {
        _duration = false;
        return _timer;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            RuntimeStorageData.SaveAllData();
    }

    private void OnApplicationQuit()
    {
        RuntimeStorageData.SaveAllData();
    }

    public NameScene GetNameScene()
    {
        int indexScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        return (NameScene)indexScene;
    }

    [SerializeField] GameObject LoadingGlobal;
    public void ShowGlobalLoading()
    {
        if (LoadingGlobal != null)
            LoadingGlobal.SetActive(true);
    }

    public void HideGlobalLoading()
    {
        if (LoadingGlobal != null)
            LoadingGlobal.SetActive(false);
    }
}
