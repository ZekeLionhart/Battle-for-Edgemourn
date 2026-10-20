using System;
using TMPro;
using Unity.Services.LevelPlay;
using UnityEngine;

public class LevelPlayAdsManager : MonoBehaviour
{
    [Header("App key")]
    [SerializeField] private string androidAppKey;
    [SerializeField] private string iosAppKey;

    [Header("Banner Ad Unit ID")]
    [SerializeField] private string androidBannerAdUnitId;
    [SerializeField] private string iosBannerAdUnitId;

    [Header("Interstitial Ad Unit ID")]
    [SerializeField] private string androidInterstitialAdUnitId;
    [SerializeField] private string iosInterstitialAdUnitId;

    [Header("Rewarded Ad Unit ID")]
    [SerializeField] private string androidRewardedAdUnitId;
    [SerializeField] private string iosRewardedAdUnitId;

    [Header("Coins UI")]
    [SerializeField] private TMP_Text coinsText;

    private LevelPlayBannerAd bannerAd;
    private LevelPlayInterstitialAd interstitialAd;
    private LevelPlayRewardedAd rewardedAd;
    private Action onInterstitialFinished;
    private bool interstitialRequestActive;

    private string AppKey
    {
        get
        {
#if UNITY_ANDROID
            return androidAppKey;
#elif UNITY_IOS
            return iosAppKey;
#else
            return string.Empty;
#endif
        }
    }
    private string BannerAdUnitId
    {
        get
        {
#if UNITY_ANDROID
            return androidBannerAdUnitId;
#elif UNITY_IOS
            return iosBannerAdUnitId;
#else
            return string.Empty;
#endif
        }
    }
    private string InterstitialAdUnitId
    {
        get
        {
#if UNITY_ANDROID
            return androidInterstitialAdUnitId;
#elif UNITY_IOS
            return iosInterstitialAdUnitId;
#else
            return string.Empty;
#endif
        }
    }
    private string RewardedAdUnitId
    {
        get
        {
#if UNITY_ANDROID
            return androidRewardedAdUnitId;
#elif UNITY_IOS
            return iosRewardedAdUnitId;
#else
            return string.Empty;
#endif
        }
    }

    public int Coins
    {
        get => PlayerPrefs.GetInt("PLAYER_COINS", 0);
        set
        {
            PlayerPrefs.SetInt("PLAYER_COINS", value);
            PlayerPrefs.Save();

            UpdateCoin();
        }
    }

#if UNITY_ANDROID || UNITY_IOS
    private enum InitializationState
    {
        NotStarted,
        Initializing,
        Initialized,
        Failed
    }

    private static InitializationState initializationState;
    private static bool initializationTrackingRegistered;
#endif

    public void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (!initializationTrackingRegistered)
        {
            LevelPlay.OnInitSuccess += TrackInitializationSuccess;
            LevelPlay.OnInitFailed += TrackInitializationFailure;

            initializationTrackingRegistered = true;
        }

        if (initializationState == InitializationState.Initialized)
        {
            CreateSceneAds();
            return;
        }

        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;

        if (initializationState == InitializationState.Initializing)
            return;

        initializationState = InitializationState.Initializing;

        LevelPlay.ValidateIntegration();
        LevelPlay.Init(AppKey);
#endif
    }

    private void OnDestroy()
    {
        StopWaitingForInitialization();

        onInterstitialFinished = null;
        interstitialRequestActive = false;

        DestroyBanner();
        DestroyInterstitial();
        DestroyRewarded();
    }

#if UNITY_ANDROID || UNITY_IOS
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetInitializationState()
    {
        LevelPlay.OnInitSuccess -= TrackInitializationSuccess;
        LevelPlay.OnInitFailed -= TrackInitializationFailure;

        initializationState = InitializationState.NotStarted;
        initializationTrackingRegistered = false;
    }

    private static void TrackInitializationSuccess(LevelPlayConfiguration config)
    {
        initializationState = InitializationState.Initialized;
        Debug.Log("#BfE LevelPlay Initialized Successfully");
    }

    private static void TrackInitializationFailure(LevelPlayInitError error)
    {
        initializationState = InitializationState.Failed;
        Debug.LogError("#BfE LevelPlay Initialization Failed: " + error);
    }
#endif

    public void UpdateCoin()
    {
        if (coinsText != null)
        {
            coinsText.text = Coins.ToString();
        }
    }

    private void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
    {
        StopWaitingForInitialization();
        CreateSceneAds();
    }

    private void SdkInitializationFailedEvent(LevelPlayInitError error)
    {
        StopWaitingForInitialization();
    }

    private void StopWaitingForInitialization()
    {
        LevelPlay.OnInitSuccess -= SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed -= SdkInitializationFailedEvent;
    }

    private void CreateSceneAds()
    {
        CreateBannerAd();
        CreateInterstitialAd();
        CreateRewardedAd();
    }

    #region bannerAds

    private void CreateBannerAd()
    {
        var adConfig = new LevelPlayBannerAd.Config.Builder()
            .SetPosition(LevelPlayBannerPosition.BottomCenter)
            .Build();

        bannerAd = new LevelPlayBannerAd(BannerAdUnitId, adConfig);

        // Register to the events 
        bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
        bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
        bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
        bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
        bannerAd.OnAdClicked += BannerOnAdClickedEvent;
        bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
        bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
        bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;
    }

    public void ShowBanner()
    {
        bannerAd.LoadAd();
    }

    public void DestroyBanner()
    {
        if (bannerAd == null) return;

        bannerAd.OnAdLoaded -= BannerOnAdLoadedEvent;
        bannerAd.OnAdLoadFailed -= BannerOnAdLoadFailedEvent;
        bannerAd.OnAdDisplayed -= BannerOnAdDisplayedEvent;
        bannerAd.OnAdDisplayFailed -= BannerOnAdDisplayFailedEvent;
        bannerAd.OnAdClicked -= BannerOnAdClickedEvent;
        bannerAd.OnAdCollapsed -= BannerOnAdCollapsedEvent;
        bannerAd.OnAdLeftApplication -= BannerOnAdLeftApplicationEvent;
        bannerAd.OnAdExpanded -= BannerOnAdExpandedEvent;

        bannerAd.DestroyAd();
        bannerAd = null;
    }

    // Implement the events
    void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdLoadFailedEvent(LevelPlayAdError ironSourceError) { }
    void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo)
    {
        Debug.Log("#BfE Click Banner Ads");
    }
    void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) { }
    void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo) { }

    #endregion

    #region interstitialAds

    private void CreateInterstitialAd()
    {
        interstitialAd = new LevelPlayInterstitialAd(InterstitialAdUnitId);

        // Register to interstitial events
        interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;

        LoadInterstitialAd();//
    }

    public void LoadInterstitialAd()
    {
        interstitialAd.LoadAd();
        Debug.Log("#BfE Interstitial Ad Loaded");
    }

    public void ShowInterstitialAd(Action onFinished)
    {
        if (interstitialRequestActive) return;

        interstitialRequestActive = true;
        onInterstitialFinished = onFinished;

#if UNITY_ANDROID
        if (interstitialAd != null && interstitialAd.IsAdReady())
        {
            interstitialAd.ShowAd();
            return;
        }
#endif

        FinishInterstitialRequest();
    }

    private void FinishInterstitialRequest()
    {
        if (!interstitialRequestActive) return;

        Action callback = onInterstitialFinished;

        onInterstitialFinished = null;
        interstitialRequestActive = false;

        callback?.Invoke();
    }
    private void DestroyInterstitial()
    {
        if (interstitialAd == null) return;

        interstitialAd.OnAdLoaded -= InterstitialOnAdLoadedEvent;
        interstitialAd.OnAdLoadFailed -= InterstitialOnAdLoadFailedEvent;
        interstitialAd.OnAdDisplayed -= InterstitialOnAdDisplayedEvent;
        interstitialAd.OnAdDisplayFailed -= InterstitialOnAdDisplayFailedEvent;
        interstitialAd.OnAdClicked -= InterstitialOnAdClickedEvent;
        interstitialAd.OnAdClosed -= InterstitialOnAdClosedEvent;
        interstitialAd.OnAdInfoChanged -= InterstitialOnAdInfoChangedEvent;

        interstitialAd.DestroyAd();
        interstitialAd = null;
    }

    // Implement the events
    void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        LoadInterstitialAd();
    }
    void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error)
    {
        FinishInterstitialRequest();
    }
    void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        FinishInterstitialRequest();
    }
    void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }

    #endregion

    #region rewardedAds

    private void CreateRewardedAd()
    {
        rewardedAd = new LevelPlayRewardedAd(RewardedAdUnitId);

        // Register to Rewarded events
        rewardedAd.OnAdLoaded += RewardedOnAdLoadedEvent;
        rewardedAd.OnAdLoadFailed += RewardedOnAdLoadFailedEvent;
        rewardedAd.OnAdDisplayed += RewardedOnAdDisplayedEvent;
        rewardedAd.OnAdDisplayFailed += RewardedOnAdDisplayFailedEvent;
        rewardedAd.OnAdRewarded += RewardedOnAdRewardedEvent;
        rewardedAd.OnAdClosed += RewardedOnAdClosedEvent;
        // Optional
        rewardedAd.OnAdClicked += RewardedOnAdClickedEvent;
        rewardedAd.OnAdInfoChanged += RewardedOnAdInfoChangedEvent;

        //LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        rewardedAd.LoadAd();
        Debug.Log("#BfE Rewarded Ad Loaded");
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd.IsAdReady())
        {
            rewardedAd.ShowAd();
            Debug.Log("#BfE Rewarded Ad Showing");
        }
    }

    private void DestroyRewarded()
    {
        if (rewardedAd == null) return;

        rewardedAd.OnAdLoaded -= RewardedOnAdLoadedEvent;
        rewardedAd.OnAdLoadFailed -= RewardedOnAdLoadFailedEvent;
        rewardedAd.OnAdDisplayed -= RewardedOnAdDisplayedEvent;
        rewardedAd.OnAdDisplayFailed -= RewardedOnAdDisplayFailedEvent;
        rewardedAd.OnAdRewarded -= RewardedOnAdRewardedEvent;
        rewardedAd.OnAdClosed -= RewardedOnAdClosedEvent;
        rewardedAd.OnAdClicked -= RewardedOnAdClickedEvent;
        rewardedAd.OnAdInfoChanged -= RewardedOnAdInfoChangedEvent;

        rewardedAd.DestroyAd();
        rewardedAd = null;
    }

    // Implement the events
    void RewardedOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        //LoadRewardedAd();
    }
    void RewardedOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) { }
    void RewardedOnAdRewardedEvent(LevelPlayAdInfo adInfo, LevelPlayReward adReward)
    {
        string rewardName = adReward.Name;
        int rewardAmount = adReward.Amount;

        Coins += rewardAmount;
        Debug.Log($"#BfE Get Reward: Reward Name: {rewardName}, Reward Amount: {rewardAmount}");
    }
    void RewardedOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        //LoadRewardedAd();
    }
    void RewardedOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }

    #endregion
}
