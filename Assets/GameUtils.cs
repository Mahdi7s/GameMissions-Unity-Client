using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    public static bool IsAppInstalled(string packageName)
    {
#if UNITY_ANDROID
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        Debug.Log(" ********LaunchOtherApp ");
        AndroidJavaObject launchIntent = null;
        //if the app is installed, no errors. Else, doesn't get past next line
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName);
            //        
            //        ca.Call("startActivity",launchIntent);
        }
        catch (Exception ex)
        {
            Debug.Log("exception" + ex.Message);
        }
        if (launchIntent == null)
            return false;
        return true;
#else // TODO: add ios plugin to check
         return false;
#endif
    }

    public static void OpenRate()
    {
#if UNITY_ANDROID
        // Create Refernece of AndroidJavaClass class for intent
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        // Create Refernece of AndroidJavaObject class intent
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        // Set action for intent
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<AndroidJavaObject>("ACTION_VIEW"));
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        //Uri.parse("bazaar://details?id=" + PACKAGE_NAME)")
        intentObject.Call<AndroidJavaObject>("setData", uriClass.CallStatic<AndroidJavaObject>("parse", "market://details?id=" + Application.identifier));
        intentObject.Call<AndroidJavaObject>("setPackage", "com.android.vending");

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        // Invoke android activity for passing intent to share data
        currentActivity.Call("startActivity", intentObject);
#endif
    }
}
