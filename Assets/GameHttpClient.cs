using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class GameHttpClient : MonoBehaviour
{
    public static GameHttpClient Instance { get; private set; }


    public string BaseAddress = string.Empty;

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
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }

    public IEnumerator HttpGet<TResponse>(string route, Action<UnityWebRequest.Result, string, TResponse> callback, params object[] queryParams)
    {
        using var req = UnityWebRequest.Get(GetUrl(route, queryParams));

        yield return req.SendWebRequest();
        
        CheckConnectionState(req.result, $"{nameof(HttpGet)}: {route}");

        callback(req.result, req.error, JsonConvert.DeserializeObject<TResponse>(req.downloadHandler.text));
    }

    public IEnumerator HttpPost<TResponse>(string route, object postData, Action<UnityWebRequest.Result, string, TResponse> callback, params object[] queryParams)
    {
        using var req = UnityWebRequest.Post(GetUrl(route, queryParams), JsonConvert.SerializeObject(postData), "application/json");

        yield return req.SendWebRequest();

        CheckConnectionState(req.result, $"{nameof(HttpPost)}: {route}");

        callback(req.result, req.error, JsonConvert.DeserializeObject<TResponse>(req.downloadHandler.text));
    }

    public IEnumerator HttpPut<TResponse>(string route, object putData, Action<UnityWebRequest.Result, string, TResponse> callback, params object[] queryParams)
    {
        using var req = UnityWebRequest.Put(GetUrl(route, queryParams), JsonConvert.SerializeObject(putData));
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        CheckConnectionState(req.result, $"{nameof(HttpPut)}: {route}");

        callback(req.result, req.error, JsonConvert.DeserializeObject<TResponse>(req.downloadHandler.text));
    }

    public IEnumerator HttpDelete(string route, Action<UnityWebRequest.Result, string> callback, params object[] queryParams)
    {
        using var req = UnityWebRequest.Delete(GetUrl(route, queryParams));

        yield return req.SendWebRequest();

        CheckConnectionState(req.result, $"{nameof(HttpDelete)}: {route}");

        callback(req.result, req.error);
    }

    private void CheckConnectionState(UnityWebRequest.Result result, string operationName)
    {
        if (result == UnityWebRequest.Result.ConnectionError)
        {
            // inform player about connection error ...
        }
    }

    private string GetUrl(string route, object[] queryParams = null)
    {
        if (queryParams.Any())
        {
            for (var i = 0; i <= queryParams.Length; i++)
            {
                var param = $"p{i + 1}";
                
                if (!route.Contains(param))
                    throw new ArgumentException("queryParams does not match the route.");

                route = route.Replace(param, Convert.ToString(queryParams[i]));
            }
        }

        return Path.Combine(BaseAddress, route);
    }
}
