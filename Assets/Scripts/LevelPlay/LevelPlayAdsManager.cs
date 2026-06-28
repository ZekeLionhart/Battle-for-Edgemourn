using System;
using TMPro;
using Unity.Services.LevelPlay;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private string appKey
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
    private string bannerAdUnitId
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
    private string interstitialAdUnitId
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
    private string rewardedAdUnitId
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

    public static Action CallInterstitial;

    private void OnEnable()
    {
        CallInterstitial += ShowInterstitialAd;
    }

    private void OnDisable()
    {
        CallInterstitial -= ShowInterstitialAd;
    }

    public void Start()
    {
        LevelPlay.ValidateIntegration();

        // Register OnInitFailed and OnInitSuccess listeners
        LevelPlay.OnInitSuccess += SdkInitializationCompletedEvent;
        LevelPlay.OnInitFailed += SdkInitializationFailedEvent;
        // SDK init
        LevelPlay.Init(appKey);
    }

    public void UpdateCoin()
    {
        if (coinsText != null)
        {
            coinsText.text = Coins.ToString();
        }
    }

    private void SdkInitializationCompletedEvent(LevelPlayConfiguration config)
    {
        CreateBannerAd();
        CreateInterstitialAd();
        CreateRewardedAd();
        Debug.Log("#BfE LevelPlay Initialized Successfully");
    }

    private void SdkInitializationFailedEvent(LevelPlayInitError error)
    {
        Debug.LogError("#BfE LevelPlay Initialization Failed: " + error);
    }

    #region bannerAds

    private void CreateBannerAd()
    {
        var adConfig = new LevelPlayBannerAd.Config.Builder()
            .SetPosition(LevelPlayBannerPosition.BottomCenter)
            .Build();

        bannerAd = new LevelPlayBannerAd(bannerAdUnitId, adConfig);

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
        bannerAd.DestroyAd();
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
        interstitialAd = new LevelPlayInterstitialAd(interstitialAdUnitId);

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

    public void ShowInterstitialAd()
    {
        if (interstitialAd.IsAdReady())
        {
            interstitialAd.ShowAd();
            Debug.Log("#BfE Interstitial Ad Showing");
        }
    }

    // Implement the events
    void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdLoadFailedEvent(LevelPlayAdError error)
    {
        LoadInterstitialAd();//
        SceneManager.LoadScene(SceneNames.Level1);
    }
    void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdDisplayFailedEvent(LevelPlayAdInfo adInfo, LevelPlayAdError error) { }
    void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        LoadInterstitialAd();//
        SceneManager.LoadScene(SceneNames.Level1);
    }
    void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }

    #endregion

    #region rewardedAds

    private void CreateRewardedAd()
    {
        rewardedAd = new LevelPlayRewardedAd(rewardedAdUnitId);

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
