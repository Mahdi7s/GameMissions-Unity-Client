using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class GameMissionsClient : MonoBehaviour
{
    public static GameMissionsClient Instance {get; private set;}

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        if (string.IsNullOrEmpty(PlayerData.GetStringData(PlayerData.Player_Id_Key))) {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterPlayer()
    {
        var deviceId = SystemInfo.deviceUniqueIdentifier;
        var bundleId = Application.identifier;

        
    }
    public void LevelCompleted(int level)
    {

    }
    public void Rate()
    {

    }
    public void WatchAd()
    {
        // Fetch Player
        // Open WebView
    }

    public IEnumerator HttpGet(string url, Action<UnityWebRequest.Result, string, string> callback)
    {
        using var req = UnityWebRequest.Get(url);

        yield return req.SendWebRequest();

        callback(req.result, req.error, req.downloadHandler.text);
    }
}
