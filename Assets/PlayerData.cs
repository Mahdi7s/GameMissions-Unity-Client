using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
    public const string Player_Id_Key = nameof(Player_Id_Key);

    public static string GetStringData(string dataKey, string defaultValue = null)
    {
        return PlayerPrefs.GetString(dataKey, defaultValue);
    }
    public static void SetStringData(string dataKey, string value)
    {
        PlayerPrefs.SetString(dataKey, value);
    }

    public static int GetIntData(string dataKey, int defaultValue = -1)
    {
        return PlayerPrefs.GetInt(dataKey, defaultValue);
    }
    public static void SetIntData(string dataKey, int value)
    {
        PlayerPrefs.SetInt(dataKey, value);
    }

    public static float GetFloatData(string dataKey, float defaultValue = -1.0f)
    {
        return PlayerPrefs.GetFloat(dataKey, defaultValue);
    }
    public static void SetFloatData(string dataKey, float value)
    {
        PlayerPrefs.SetFloat(dataKey, value);
    }
}
