using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class GameMissionsClient : MonoBehaviour
{
    private WebViewManager webViewManager;
    
    public static GameMissionsClient Instance {get; private set;}

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        webViewManager = GetComponentInChildren<WebViewManager>();
        webViewManager.OnRate = RateThisGame;
        webViewManager.OnWatchRewarded = ShowRewarded;

        webViewManager.OnGotoLevel = ClaimLevelCompletion;
        webViewManager.OnInstallGame = InstallGame;
        webViewManager.OnReferalGame = ReferGame;
    }

    void Start()
    {
        if (!PlayerData.PlayerId.HasValue) {
            RegisterPlayer();
        }
    }   

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterPlayer()
    {
        GameHttpClient.Instance.HttpPost<CreatePlayerResponse>(CreatePlayerRequest.Route, new CreatePlayerRequest
        {
            DeviceId = SystemInfo.deviceUniqueIdentifier,
            GamePackageName = Application.identifier
        }, 
        (state, err, resp) => {
            if (state == UnityWebRequest.Result.Success)
            {
                PlayerData.PlayerId = resp.Id;
            }
            else
            {
                // log error
            }
        });
    }

    private void InstallGame(int missionId, bool isClaimAction)
    {
        if (isClaimAction)
        {

        }
        else
        {
            if (GameUtils.IsAppInstalled("packagename"))
            {

            }
        }
    }

    private void ReferGame(int missionId, bool isClaimAction)
    {

    }

    private void ClaimLevelCompletion(int missionId)
    {

    }
    public void SendLevelCompletion(int level)
    {
        FetchPlayer(player =>
        {
            if(player != null)
            {
                player.Level = level;

                UpdatePlayer(player, updatedPlayer => { });
            }
        });
    }
    private void RateThisGame()
    {
        FetchPlayer(player =>
        {
            if (player != null)
            {
                player.Rated = true;

                UpdatePlayer(player, updatedPlayer => { });
            }
        });
    }

    private void ShowRewarded()
    {

    }
    public void OnRewardedVideoShown()
    {
        FetchPlayer(player =>
        {
            if (player != null)
            {
                player.LastAdWatch = DateTime.Now; // we can get the time from server ...

                UpdatePlayer(player, updatedPlayer => { });
            }
        });
        // Fetch Player
        // Open WebView
    }
    private void FetchPlayer(Action<Player> callback)
    {
        GameHttpClient.Instance.HttpGet<GetPlayerByIdResponse>(GetPlayerByIdRequest.Route, (state, err, resp) =>
        {
            if (state == UnityWebRequest.Result.Success)
            {
                PlayerData.Player = resp.PlayerRecord;

                callback(resp.PlayerRecord);
            } else
            {
                // log error

                callback(PlayerData.Player); // use last synced player
            }
        }, PlayerData.PlayerId);
    }

    private void UpdatePlayer(Player playerToUpdate, Action<Player> callback)
    {
        GameHttpClient.Instance.HttpPut<UpdatePlayerResponse>(UpdatePlayerRequest.Route, new UpdatePlayerRequest
        {
            Id = playerToUpdate.Id,
            DeviceId = playerToUpdate.DeviceId,
            LastConnectedIP = playerToUpdate.LastConnectedIP ?? GameHttpClient.GetLocalIPAddress(),
            LocaleCode = playerToUpdate.LocaleCode,
            GameId = playerToUpdate.GameId,
            Rated = playerToUpdate.Rated,
            Level = playerToUpdate.Level,
            LastAdWatch = playerToUpdate.LastAdWatch
        }, (state, err, resp) =>
        {
            if(state == UnityWebRequest.Result.Success)
            {
                PlayerData.Player = resp.PlayerRecord;

                callback(resp.PlayerRecord);
            }
            else
            {
                // log error

                callback(null);
            }
        });
    }
}
