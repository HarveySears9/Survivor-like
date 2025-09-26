using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionDatabase", menuName = "Game/MissionDatabase")]
public class MissionDatabase : ScriptableObject
{
    [Header("Daily Missions")]
    public Mission[] dailyMissions;

    [Header("Weekly Missions")]
    public Mission[] weeklyMissions;

    [Header("Challenge Missions")]
    public Mission[] challengeMissions;
}
