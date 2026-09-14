using UnityEngine;
using Unity.Services.LevelPlay;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    [Header("LevelPlay")]
    [SerializeField] private string appKey;

    [Header("Rewarded Ad")]
    [SerializeField] private string rewardedAdUnitId;

    private LevelPlayRewardedAd rewardedAd;

    private Action pendingReward;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitialiseLevelPlay();
    }

    private void InitialiseLevelPlay()
    {
        LevelPlay.OnInitSuccess += OnInitSuccess;
        LevelPlay.OnInitFailed += OnInitFailed;

        LevelPlay.Init(appKey);
    }

    private void OnInitSuccess(LevelPlayConfiguration configuration)
    {
        Debug.Log("LevelPlay initialised successfully.");

        CreateRewardedAd();
    }

    private void OnInitFailed(LevelPlayInitError error)
    {
        Debug.LogError("LevelPlay failed to initialise: " + error);
    }

    private void CreateRewardedAd()
    {
        rewardedAd = new LevelPlayRewardedAd(rewardedAdUnitId);

        rewardedAd.OnAdLoaded += OnRewardedAdLoaded;
        rewardedAd.OnAdLoadFailed += OnRewardedAdLoadFailed;
        rewardedAd.OnAdDisplayed += OnRewardedAdDisplayed;
        rewardedAd.OnAdDisplayFailed += OnRewardedAdDisplayFailed;
        rewardedAd.OnAdRewarded += OnRewardedAdRewarded;
        rewardedAd.OnAdClosed += OnRewardedAdClosed;

        rewardedAd.LoadAd();
    }

    private void OnRewardedAdLoaded(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Rewarded ad loaded.");
    }

    private void OnRewardedAdLoadFailed(LevelPlayAdError error)
    {
        Debug.LogWarning("Rewarded ad failed to load: " + error);
    }

    private void OnRewardedAdDisplayed(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Rewarded ad displayed.");
    }

    private void OnRewardedAdDisplayFailed(
        LevelPlayAdInfo adInfo,
        LevelPlayAdError error)
    {
        Debug.LogWarning("Rewarded ad failed to display: " + error);

        pendingReward = null;

        rewardedAd.LoadAd();
    }

    private void OnRewardedAdRewarded(
        LevelPlayAdInfo adInfo,
        LevelPlayReward reward)
    {
        Debug.Log(
            $"Player rewarded: {reward.Name} - {reward.Amount}"
        );

        // IMPORTANT:
        // This is where we know the player actually earned
        // the reward for watching the ad.

        pendingReward?.Invoke();
        pendingReward = null;
    }

    private void OnRewardedAdClosed(LevelPlayAdInfo adInfo)
    {
        Debug.Log("Rewarded ad closed.");

        // Load the next ad so another one is ready.
        rewardedAd.LoadAd();
    }

    public bool IsRewardedAdReady()
    {
        return rewardedAd != null && rewardedAd.IsAdReady();
    }

    public bool ShowRewardedAd(Action rewardCallback)
    {
        if (rewardedAd == null)
        {
            Debug.LogWarning("Rewarded ad has not been created yet.");
            return false;
        }

        if (!rewardedAd.IsAdReady())
        {
            Debug.LogWarning("Rewarded ad is not ready.");
            return false;
        }

        pendingReward = rewardCallback;

        rewardedAd.ShowAd();

        return true;
    }
}