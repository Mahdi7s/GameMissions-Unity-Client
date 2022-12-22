using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionType
{
    /// <summary>
    /// Install another game or app and get a reward
    /// </summary>
    Install = 1,
    /// <summary>
    /// 
    /// </summary>
    GotoLevel = 2,
    /// <summary>
    /// Invite a friend to play this game 
    /// Get all [Referral]s of current game by order
    /// </summary>
    Referral = 3
}
