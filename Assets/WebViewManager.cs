using Newtonsoft.Json.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[RequireComponent(typeof(WebViewObject))]
public class WebViewManager : MonoBehaviour
{
    /// <summary>
    /// MissionId , IsClaimAction?!
    /// </summary>
    public UnityAction<int, bool> OnInstallGame;
    /// <summary>
    /// MissionId , IsClaimAction?!
    /// </summary>
    public UnityAction<int, bool> OnReferalGame;
    /// <summary>
    /// MissionId
    /// </summary>
    public UnityAction<int> OnGotoLevel;
    public UnityAction OnRate;
    public UnityAction OnWatchRewarded;

    private enum JsCallbackActions
    {
        GoBack = 1,
        GoForward = 2,

        WatchRewardedAd = 3,
        RateThisGame = 4,

        GotoLevel = 5,

        InstallGame = 6,
        ReferThisGame = 7,

        // Claim the reward of AdWatch, Rate, GotoLevel, Install or Referal
    }

    private WebViewObject webViewObject = null;
    public string HomePage = string.Empty;

    IEnumerator Start()
    {
        webViewObject = GetComponent<WebViewObject>();

        webViewObject.Init(
          cb: (msg) =>
          {
              Debug.Log(string.Format("CallFromJS[{0}]", msg));
              
              var actionParameters = ExtractJsCallbackInfo(msg);
              var actionData = actionParameters.Item2;
              
              switch (actionParameters.Item1)
              {
                  case JsCallbackActions.GoBack:
                      webViewObject.GoBack();

                      break;
                  case JsCallbackActions.GoForward:
                      webViewObject.GoForward();

                      break;
                  case JsCallbackActions.WatchRewardedAd:
                      OnWatchRewarded?.Invoke();

                      break;
                 case JsCallbackActions.RateThisGame: 
                      OnRate?.Invoke();

                      break;
                 case JsCallbackActions.GotoLevel:
                      // always is a claim act so it's unnecessary to check

                      var missionId = int.Parse(actionData["MissionId"]);

                      OnGotoLevel(missionId);

                      break;
                 case JsCallbackActions.InstallGame:
                      missionId = int.Parse(actionData["MissionId"]);
                      var isClaimAct = bool.Parse(actionParameters.Item2["Claim"]);
                      
                      OnInstallGame.Invoke(missionId, isClaimAct);

                      break;
                 case JsCallbackActions.ReferThisGame:
                      missionId = int.Parse(actionData["MissionId"]);
                      isClaimAct = bool.Parse(actionParameters.Item2["Claim"]);

                      OnReferalGame.Invoke(missionId, isClaimAct);

                      break;
              }

              webViewObject.SetVisibility(false);
          },
          err: (msg) =>
          {
              Debug.Log(string.Format("CallOnError[{0}]", msg));

          },
          httpErr: (msg) =>
          {
              Debug.Log(string.Format("CallOnHttpError[{0}]", msg));

          },
          started: (msg) =>
          {
              Debug.Log(string.Format("CallOnStarted[{0}]", msg));
          },
          hooked: (msg) =>
          {
              Debug.Log(string.Format("CallOnHooked[{0}]", msg));
          },
          ld: (msg) =>
          {
              Debug.Log(string.Format("CallOnLoaded[{0}]", msg));
#if UNITY_EDITOR_OSX || (!UNITY_ANDROID && !UNITY_WEBPLAYER && !UNITY_WEBGL)
                // NOTE: depending on the situation, you might prefer
                // the 'iframe' approach.
                // cf. https://github.com/gree/unity-webview/issues/189
#if true
                webViewObject.EvaluateJS(@"
                  if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                    window.Unity = {
                      call: function(msg) {
                        window.webkit.messageHandlers.unityControl.postMessage(msg);
                      }
                    }
                  } else {
                    window.Unity = {
                      call: function(msg) {
                        window.location = 'unity:' + msg;
                      }
                    }
                  }
                ");
#else
                webViewObject.EvaluateJS(@"
                  if (window && window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.unityControl) {
                    window.Unity = {
                      call: function(msg) {
                        window.webkit.messageHandlers.unityControl.postMessage(msg);
                      }
                    }
                  } else {
                    window.Unity = {
                      call: function(msg) {
                        var iframe = document.createElement('IFRAME');
                        iframe.setAttribute('src', 'unity:' + msg);
                        document.documentElement.appendChild(iframe);
                        iframe.parentNode.removeChild(iframe);
                        iframe = null;
                      }
                    }
                  }
                ");
#endif
#elif UNITY_WEBPLAYER || UNITY_WEBGL
                webViewObject.EvaluateJS(
                    "window.Unity = {" +
                    "   call:function(msg) {" +
                    "       parent.unityWebView.sendMessage('WebViewObject', msg)" +
                    "   }" +
                    "};");
#endif
              webViewObject.EvaluateJS(@"Unity.call('ua=' + navigator.userAgent)");
          }
          //transparent: false,
          //zoom: true,
          //ua: "custom user agent string",
          //// android
          //androidForceDarkMode: 0,  // 0: follow system setting, 1: force dark off, 2: force dark on
          //// ios
          //enableWKWebView: true,
          //wkContentMode: 0,  // 0: recommended, 1: mobile, 2: desktop
          //wkAllowsLinkPreview: true,
          //// editor
          //separated: false
          );
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        webViewObject.bitmapRefreshCycle = 1;
#endif
        // cf. https://github.com/gree/unity-webview/pull/512
        // Added alertDialogEnabled flag to enable/disable alert/confirm/prompt dialogs. by KojiNakamaru · Pull Request #512 · gree/unity-webview
        //webViewObject.SetAlertDialogEnabled(false);

        // cf. https://github.com/gree/unity-webview/pull/728
        //webViewObject.SetCameraAccess(true);
        //webViewObject.SetMicrophoneAccess(true);

        // cf. https://github.com/gree/unity-webview/pull/550
        // introduced SetURLPattern(..., hookPattern). by KojiNakamaru · Pull Request #550 · gree/unity-webview
        //webViewObject.SetURLPattern("", "^https://.*youtube.com", "^https://.*google.com");

        // cf. https://github.com/gree/unity-webview/pull/570
        // Add BASIC authentication feature (Android and iOS with WKWebView only) by takeh1k0 · Pull Request #570 · gree/unity-webview
        //webViewObject.SetBasicAuthInfo("id", "password");

        //webViewObject.SetScrollbarsVisibility(true);

        webViewObject.SetMargins(5, 100, 5, Screen.height / 4);
        webViewObject.SetTextZoom(100);  // android only. cf. https://stackoverflow.com/questions/21647641/android-webview-set-font-size-system-default/47017410#47017410
        webViewObject.SetVisibility(true);

#if !UNITY_WEBPLAYER && !UNITY_WEBGL
        if (HomePage.StartsWith("http"))
        {
            webViewObject.LoadURL(HomePage.Replace(" ", "%20"));
        }
        else
        {
            var exts = new string[]{
                ".jpg",
                ".js",
                ".html"  // should be last
            };
            foreach (var ext in exts)
            {
                var url = HomePage.Replace(".html", ext);
                var src = System.IO.Path.Combine(Application.streamingAssetsPath, url);
                var dst = System.IO.Path.Combine(Application.persistentDataPath, url);
                byte[] result = null;
                if (src.Contains("://"))
                {  // for Android
#if UNITY_2018_4_OR_NEWER
                    // NOTE: a more complete code that utilizes UnityWebRequest can be found in https://github.com/gree/unity-webview/commit/2a07e82f760a8495aa3a77a23453f384869caba7#diff-4379160fa4c2a287f414c07eb10ee36d
                    var unityWebRequest = UnityWebRequest.Get(src);
                    yield return unityWebRequest.SendWebRequest();
                    result = unityWebRequest.downloadHandler.data;
#else
                    var www = new WWW(src);
                    yield return www;
                    result = www.bytes;
#endif
                }
                else
                {
                    result = System.IO.File.ReadAllBytes(src);
                }
                System.IO.File.WriteAllBytes(dst, result);
                if (ext == ".html")
                {
                    webViewObject.LoadURL("file://" + dst.Replace(" ", "%20"));
                    break;
                }
            }
        }
#else
        if (Url.StartsWith("http")) {
            webViewObject.LoadURL(Url.Replace(" ", "%20"));
        } else {
            webViewObject.LoadURL("StreamingAssets/" + Url.Replace(" ", "%20"));
        }
#endif
        yield break;
    }

    /// <summary>
    /// Smaples of jsCallbackMsg: 
    /// InstallGame=GamePackage:Game.Package.Name,GameId:5,Claim:true
    /// ReferThisGame=ReferalCode:4345,Claim:false
    /// WatchAd
    /// </summary>
    /// <param name="jsCallbackMsg"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private (JsCallbackActions, Dictionary<string, string> data) ExtractJsCallbackInfo(string jsCallbackMsg)
    {
        if(string.IsNullOrEmpty(jsCallbackMsg))
        {
            throw new ArgumentNullException($"{nameof(jsCallbackMsg)} is null");
        }

        if (jsCallbackMsg.Contains("=")) // the call has parameters
        {
            var slices = jsCallbackMsg.Split('=', StringSplitOptions.RemoveEmptyEntries);
            var action = Enum.Parse<JsCallbackActions>(slices[0]);
            var data = slices[1].Split(',', StringSplitOptions.RemoveEmptyEntries)
                .ToDictionary(p => p.Split(':')[0], p => p.Split(':')[1]);

            return (action, data);
        }
        else
        {
            var action = Enum.Parse<JsCallbackActions>(jsCallbackMsg);

            return (action, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
