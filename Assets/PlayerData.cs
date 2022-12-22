using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public static int? PlayerId 
    { 
        get 
        { 
            return GetIntData(nameof(PlayerId));
        }
        set
        {
            SetIntData(nameof(PlayerId), value.Value);
        }
    }

    public static Player Player
    {
        get
        {
            return JsonConvert.DeserializeObject<Player>(GetStringData(nameof(Player)));
        }
        set
        {
            SetStringData(nameof(Player), JsonConvert.SerializeObject(value));
        }
    }

    public static string GetStringData(string dataKey)
    {
        return PlayerPrefs.GetString(dataKey, null);
    }
    public static void SetStringData(string dataKey, string value)
    {
        PlayerPrefs.SetString(dataKey, value);
    }

    public static int? GetIntData(string dataKey)
    {
        return PlayerPrefs.HasKey(dataKey) ? PlayerPrefs.GetInt(dataKey) : null;
    }
    public static void SetIntData(string dataKey, int value)
    {
        PlayerPrefs.SetInt(dataKey, value);
    }

    public static float? GetFloatData(string dataKey)
    {
        return PlayerPrefs.HasKey(dataKey) ? PlayerPrefs.GetFloat(dataKey) : null;
    }
    public static void SetFloatData(string dataKey, float value)
    {
        PlayerPrefs.SetFloat(dataKey, value);
    }
}
